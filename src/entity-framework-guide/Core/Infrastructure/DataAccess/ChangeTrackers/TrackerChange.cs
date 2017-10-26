using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.ChangeTrackers
{
    public abstract class ChangeTracker<T> : IChangeTracker<T>
        where T: class
    {
        public DataContext Context { get; set; }

        public abstract void Added(T entity);
        public abstract void Deleted(T entity);
        public abstract void Updated(T entity);

        public void ApplyTo(DbEntityEntry entry)
        {
            var entity = entry.Entity as T;

            if (entry.State == EntityState.Added)
            {
                Added(entity);
            }
            else if (entry.State == EntityState.Modified)
            {
                Updated(entity);
            }
            else if (entry.State == EntityState.Deleted) {
                Deleted(entity);
            }
        }

        public bool CanApplyTo(object type)
        {
            return type is T;
        }
    }

    public interface IChangeTracker<T> : IChangeTracker
        where T : class

    {
        void Added(T entities);
        void Updated(T entities);
        void Deleted(T entities);
    }

    public interface IChangeTracker
    {
        DataContext Context { get; set; }
        bool CanApplyTo(object type);
        void ApplyTo(DbEntityEntry entry);
    }
}
