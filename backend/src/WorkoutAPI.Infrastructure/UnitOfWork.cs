using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        public DbConnection Connection { get; }

        public UnitOfWork(DbConnection connection, IMediator mediator, CancellationToken cancellationToken)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection), "The database connection cannot be null.");
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _cancellationToken = cancellationToken;
        }

        #region Private State

        private bool _dbConnectionOpenedLocally;
        private DbTransaction? _transaction;
        private readonly SemaphoreSlim _transactionSemaphore = new(1, 1);
        private int _dbTransactionNestLevel = 0; // 0 = no txn, 1 = outermost, >1 = nested
        private readonly HashSet<DbContext> _dbContexts = new(); // de-duplicates DbContext registrations
        private readonly CancellationToken _cancellationToken;
        private readonly IMediator _mediator;
        private const int MaxRecursionDepth = 5; // consult team before changing

        #endregion

        #region IUnitOfWork - Async

        public Task BeginAsync() => BeginAsync(IsolationLevel.ReadCommitted);

        public async Task BeginAsync(IsolationLevel isolationLevel)
        {
            await _transactionSemaphore.WaitAsync(_cancellationToken).ConfigureAwait(false);
            try
            {
                if (_transaction == null)
                {
                    await OpenConnection().ConfigureAwait(false);
                    _transaction = await Connection.BeginTransactionAsync(isolationLevel, _cancellationToken).ConfigureAwait(false);

                    // Attach existing contexts to the new transaction
                    foreach (var dbContext in _dbContexts)
                    {
                        await dbContext.Database.UseTransactionAsync(_transaction, _cancellationToken).ConfigureAwait(false);
                    }

                    _dbTransactionNestLevel = 1; // outermost
                }
                else
                {
                    _dbTransactionNestLevel++; // nested
                }
            }
            finally
            {
                _transactionSemaphore.Release();
            }
        }

        public async Task Commit()
        {
            await _transactionSemaphore.WaitAsync(_cancellationToken).ConfigureAwait(false);
            try
            {
                if (_dbTransactionNestLevel == 0)
                {
                    // No active transaction: nothing to commit
                    return;
                }

                if (_dbTransactionNestLevel > 1)
                {
                    // Nested scope: just decrease the level
                    _dbTransactionNestLevel--;
                    return;
                }

                // Outermost scope: actually commit
                await PublishDomainEvents().ConfigureAwait(false);

                try
                {
                    if (_transaction != null)
                    {
                        await _transaction.CommitAsync(_cancellationToken).ConfigureAwait(false);
                    }
                }
                catch
                {
                    await Rollback().ConfigureAwait(false);
                    throw;
                }
                finally
                {
                    Reset(); // dispose txn, close connection if we opened it
                }
            }
            finally
            {
                _transactionSemaphore.Release();
            }
        }

        public async Task Rollback()
        {
            await _transactionSemaphore.WaitAsync(_cancellationToken).ConfigureAwait(false);
            try
            {
                if (_dbTransactionNestLevel == 0)
                {
                    // Nothing to rollback
                    return;
                }

                // On rollback we always rollback the actual database transaction,
                // regardless of the nesting level, to guarantee atomicity.
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync(_cancellationToken).ConfigureAwait(false);
                }
                Reset();
            }
            finally
            {
                _transactionSemaphore.Release();
            }
        }

        #endregion

        #region IUnitOfWork - Sync (wrappers for legacy code)

        // If you prefer to drop sync APIs, delete these two and the interface members.
        public void Begin() => Begin(IsolationLevel.ReadCommitted);

        public void Begin(IsolationLevel isolationLevel)
        {
            // Use async core to avoid duplicated logic; block safely
            BeginAsync(isolationLevel).GetAwaiter().GetResult();
        }

        #endregion

        #region Context Registration

        // Keeps the original sync signature to match your interface.
        public void RegisterContext(DbContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            _transactionSemaphore.Wait(_cancellationToken);
            try
            {
                // HashSet.Add returns false if already present
                var added = _dbContexts.Add(context);

                // If a transaction is already active, enlist this context
                if (_transaction != null)
                {
                    // Use sync enlistment to respect the sync signature
                    context.Database.UseTransaction(_transaction);
                }
            }
            finally
            {
                _transactionSemaphore.Release();
            }
        }

        #endregion

        #region Helpers

        private void Reset()
        {
            // Dispose transaction if present
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            _dbTransactionNestLevel = 0;
            CloseConnection(); // close only if we opened it locally
        }

        private void CloseConnection()
        {
            if (Connection.State == ConnectionState.Open && _dbConnectionOpenedLocally)
            {
                Connection.Close();
                _dbConnectionOpenedLocally = false;
            }
        }

        private async Task OpenConnection()
        {
            if (Connection.State == ConnectionState.Open) return;

            await Connection.OpenAsync(_cancellationToken).ConfigureAwait(false);
            _dbConnectionOpenedLocally = true;
        }

        private async Task PublishDomainEvents()
        {
            // Collect current uncommitted events from all registered aggregate roots
            var eventsToPublish = new List<INotification>();
            CollectDomainEvents(eventsToPublish);

            var depth = 0;

            while (eventsToPublish.Count > 0 && depth < MaxRecursionDepth)
            {
                depth++;

                // Publish all currently collected events
                foreach (var domainEvent in eventsToPublish)
                {
                    // Cancellation is honored through the token provided in ctor
                    await _mediator.Publish(domainEvent, _cancellationToken).ConfigureAwait(false);
                }

                // Clear the list and collect again in case handlers queued new domain events
                eventsToPublish.Clear();
                CollectDomainEvents(eventsToPublish);
            }

            if (depth >= MaxRecursionDepth && eventsToPublish.Count > 0)
            {
                throw new InvalidOperationException("Maximum recursion depth reached while publishing domain events.");
            }
        }

        private void CollectDomainEvents(List<INotification> sink)
        {
            foreach (var context in _dbContexts)
            {
                // Find all tracked aggregate roots that have uncommitted events
                var aggregateRoots = context.ChangeTracker.Entries<IAggregateRoot>()
                    .Where(e => e.Entity.UncommittedEvents.Any())
                    .Select(e => e.Entity);

                foreach (var aggregateRoot in aggregateRoots)
                {
                    // Copy out and clear to avoid double publishing
                    var domainEvents = aggregateRoot.UncommittedEvents.ToList();
                    aggregateRoot.ClearDomainEvents();

                    // IDomainEvent should implement INotification in your domain
                    foreach (var evt in domainEvents)
                    {
                        if (evt is INotification notification)
                        {
                            sink.Add(notification);
                        }
                        else
                        {
                            throw new InvalidOperationException(
                                $"Domain event type '{evt.GetType().FullName}' does not implement INotification.");
                        }
                    }
                }
            }
        }

        #endregion

        #region Disposal

        public void Dispose()
        {
            // Best-effort sync cleanup
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
            CloseConnection();
            _transactionSemaphore.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync().ConfigureAwait(false);
                _transaction = null;
            }
            CloseConnection();
            _transactionSemaphore.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
