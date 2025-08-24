using KellermanSoftware.CompareNetObjects;

namespace WorkoutAPI.Domain.Common;

public interface IEntity<TId> {
    TId Id { get; set; }
}

public abstract class Entity<TEntity, TId> : IEntity<TId> where TEntity : Entity<TEntity, TId> {
    private static readonly CompareLogic Compare = new CompareLogic(new ComparisonConfig {
        CompareChildren = true
    });

    private int? _hashCode;

    public TId Id { get; set; }

    public static bool operator !=(Entity<TEntity, TId> entity1, Entity<TEntity, TId> entity2) {
        return !(entity1 == entity2);
    }

    public static bool operator ==(Entity<TEntity, TId> entity1, Entity<TEntity, TId> entity2) {
        if ((object)entity1 == entity2)
        {
            return true;
        }

        if ((object)entity1 == null)
        {
            return false;
        }

        if ((object)entity2 == null)
        {
            return false;
        }

        return entity1.Equals(entity2);
    }

    public override bool Equals(object obj) {
        if (obj is TEntity val)
        {
            bool num = Id.Equals(default(TId));
            bool flag = val.Id.Equals(default(TId));
            if (num && flag)
            {
                if ((object)this != val)
                {
                    return DeepEquals(this, val);
                }

                return true;
            }

            return Id.Equals(val.Id);
        }

        return false;
    }

    public override int GetHashCode() {
        if (_hashCode.HasValue)
        {
            return _hashCode.Value;
        }

        if (Id.Equals(default(TId)))
        {
            _hashCode = base.GetHashCode();
            return _hashCode.Value;
        }

        return Id.GetHashCode();
    }

    private bool DeepEquals<T>(T thisObject, T otherObj) {
        return Compare.Compare(thisObject, otherObj).AreEqual;
    }
}
