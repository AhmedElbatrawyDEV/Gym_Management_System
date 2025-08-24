using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Infrastructure;


public class UnitOfWork : IUnitOfWork {
    public readonly DbConnection Connection;

    public UnitOfWork(DbConnection connection, IMediator mediator, CancellationToken cancellationToken) {
        Connection = connection ?? throw new ArgumentException("The database connection cannot be null.", nameof(connection));
        _cancellationToken = cancellationToken;
        _mediator = mediator;
        _transaction = null;
    }

    #region Private Variables

    private bool _dbConnectionOpenedLocally;
    private DbTransaction _transaction;
    private readonly SemaphoreSlim _transactionSemaphore = new SemaphoreSlim(1, 1);
    private int _dbTransactionNestLevel = -1;
    private readonly HashSet<DbContext> _dbContexts = new();
    private readonly CancellationToken _cancellationToken;
    private readonly IMediator _mediator;
    private const int MaxRecursionDepth = 5; // Don't Change this number before consulting with the team leader

    #endregion

    #region IUnitOfWork Implementation


    public Task BeginAsync() {
        return BeginAsync(IsolationLevel.ReadCommitted);
    }

    public async Task BeginAsync(IsolationLevel isolationLevel) {
        await _transactionSemaphore.WaitAsync(_cancellationToken).ConfigureAwait(false);

        try
        {
            if (_transaction == null)
            {
                var dbTransactionIsolationLevel = isolationLevel;
                await OpenConnection().ConfigureAwait(false);
                _transaction = await Connection.BeginTransactionAsync(dbTransactionIsolationLevel, _cancellationToken).ConfigureAwait(false);
                foreach (var dbContext in _dbContexts)
                {
                    await dbContext.Database.UseTransactionAsync(_transaction, _cancellationToken).ConfigureAwait(false);
                }
            }
            _dbTransactionNestLevel++;
        } finally
        {
            _transactionSemaphore.Release();
        }
    }

    public async Task Commit() {
        await _transactionSemaphore.WaitAsync(_cancellationToken).ConfigureAwait(false);

        if (_dbTransactionNestLevel > 0)
        {
            _dbTransactionNestLevel--;
            return;
        }

        try
        {
            await PublishDomainEvents().ConfigureAwait(false);

            await _transaction.CommitAsync(_cancellationToken).ConfigureAwait(false);

        } catch (Exception)
        {
            await Rollback().ConfigureAwait(false);
            throw;
        } finally
        {
            Reset();
            _transactionSemaphore.Release();
        }
    }

    public async Task Rollback() {
        await _transactionSemaphore.WaitAsync(_cancellationToken).ConfigureAwait(false);

        try
        {
            if (_dbTransactionNestLevel == -1)
            {
                return;
            }

            await _transaction.RollbackAsync(_cancellationToken).ConfigureAwait(false);
        } finally
        {
            Reset();
        }
    }

    #endregion

    #region Helpers

    private void Reset() {
        _transaction.Dispose();
        _transaction = null;
        _dbTransactionNestLevel = -1;
        CloseConnection();
    }

    private void CloseConnection() {
        if (Connection.State == ConnectionState.Open && _dbConnectionOpenedLocally)
        {
            Connection.Close();
            _dbConnectionOpenedLocally = false;
        }
    }

    private async Task OpenConnection() {
        if (Connection.State == ConnectionState.Open) return;

        await Connection.OpenAsync(_cancellationToken).ConfigureAwait(false);
        _dbConnectionOpenedLocally = true;
    }

    public void RegisterContext(DbContext context) {
        _transactionSemaphore.Wait(_cancellationToken);

        try
        {
            _dbContexts.Add(context);
            if (_transaction != null)
            {

                context.Database.UseTransaction(_transaction);
            }
        } finally
        {
            _transactionSemaphore.Release();
        }
    }


    private async Task PublishDomainEvents() {
        var eventsToPublish = new List<INotification>();
        CollectDomainEvents(eventsToPublish);

        var recursionDepth = 0;

        while (eventsToPublish.Count > 0 && recursionDepth < MaxRecursionDepth)
        {
            recursionDepth++;
            foreach (var domainEvent in eventsToPublish)
            {
                await _mediator.Publish(domainEvent).ConfigureAwait(false);
            }

            eventsToPublish.Clear();
            CollectDomainEvents(eventsToPublish);
        }

        if (recursionDepth >= MaxRecursionDepth)
        {
            throw new InvalidOperationException("Maximum recursion depth reached while publishing domain events.");
        }
    }

    private void CollectDomainEvents(List<INotification> eventsToPublish) {
        foreach (var context in _dbContexts)
        {
            var aggregateRoots = context.ChangeTracker.Entries<IAggregateRoot>()
                .Where(e => e.Entity.UncommittedEvents.Any())
                .Select(e => e.Entity);

            foreach (var aggregateRoot in aggregateRoots)
            {
                var domainEvents = aggregateRoot.UncommittedEvents.ToList();
                aggregateRoot.ClearDomainEvents();
                eventsToPublish.AddRange(domainEvents);
            }
        }
    }

    public void Begin() {
        throw new NotImplementedException();
    }

    public void Begin(IsolationLevel isolationLevel) {
        throw new NotImplementedException();
    }

    #endregion
}
