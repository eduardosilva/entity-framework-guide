using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using entity_framework_guide.Core.Entities;
using entity_framework_guide.Core.Infrastructure.DataAccess.ChangeTrackers;

namespace entity_framework_guide.Core.Infrastructure.DataAccess
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<ErrorLog> Logs { get; set; }

        public DataContext()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;


            Database.Log = (l) =>
            {
                Console.WriteLine(l); //ONLY CONSOLE APP
                Debug.WriteLine(l);
            };
        }

        public override int SaveChanges()
        {
            CheckChanges();
            return base.SaveChanges();

        }

        private void CheckChanges()
        {
            var trackersChange = typeof(DataContext).Assembly.GetTypes()
                                                         .Where(t => t.IsAbstract == false &&
                                                                     typeof(IChangeTracker).IsAssignableFrom(t))
                                                         .Select(t =>
                                                         {
                                                             dynamic trackerChangeInstance = Activator.CreateInstance(t);
                                                             return trackerChangeInstance;
                                                         })
                                                         .Cast<IChangeTracker>()
                                                         .Select(t =>
                                                         {
                                                             t.Context = this;
                                                             return t;
                                                         })
                                                         .ToArray();

            var trackedEntities = ChangeTracker.Entries()
                                               .Where(e => trackersChange.Any(tc => tc.CanApplyTo(e.Entity)))
                                               .ToArray();

            foreach (var trackEntity in trackedEntities)
            {
                var trackers = trackersChange.Where(t => t.CanApplyTo(trackEntity.Entity)).ToList();

                trackers.ForEach(t => t.ApplyTo(trackEntity));
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var assemblyTypes = typeof(DataContext).Assembly.GetTypes();

            //add entities and complex types configurations
            assemblyTypes.Where(t => t.IsAbstract == false &&
                                     t.BaseType != null &&
                                     t.BaseType.IsGenericType &&
                                     (t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) ||
                                     t.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)))
                         .ToList()
                         .ForEach(t =>
                         {
                             dynamic instance = Activator.CreateInstance(t);
                             modelBuilder.Configurations.Add(instance);
                         });

            // add conventions
            assemblyTypes.Where(t => t.IsAbstract == false &&
                                     t.BaseType != null &&
                                     t.BaseType == typeof(Convention))
                         .ToList().ForEach(t => 
                         {
                             dynamic instance = Activator.CreateInstance(t);
                             modelBuilder.Conventions.Add(instance);
                         });

            base.OnModelCreating(modelBuilder);
        }
    }
}