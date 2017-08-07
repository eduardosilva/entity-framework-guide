using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;
using entity_framework_guide.Core.Infrastructure.DataAccess.Configurations;

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
            CheckAudit();
            return base.SaveChanges();
        }

        private void CheckAudit()
        {
            foreach (var itemChanged in ChangeTracker.Entries().Where(e => e.Entity is Auditable))
            {
                if (!(itemChanged.State == EntityState.Added || itemChanged.State == EntityState.Modified))
                    continue;


                var item = itemChanged.Entity as Auditable;
                item.ModifiedDate = DateTime.Now;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var configurationTypes = typeof(DataContext).Assembly.GetTypes()
                                                                 .Where(t => t.IsAbstract == false &&
                                                                             t.BaseType != null &&
                                                                             t.BaseType.IsGenericType &&
                                                                             (t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) ||
                                                                             t.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)))
                                                                 .ToArray();

            foreach (var configurationType in configurationTypes)
            {
                dynamic configurationTypeInstance = Activator.CreateInstance(configurationType);
                modelBuilder.Configurations.Add(configurationTypeInstance);
            }

            var conventionTypes = typeof(DataContext).Assembly.GetTypes()
                                                              .Where(t => t.IsAbstract == false &&
                                                                          t.BaseType != null &&
                                                                          t.BaseType == typeof(Convention))
                                                              .ToArray();

            foreach (var conventionType in conventionTypes)
            {
                dynamic configurationTypeInstance = Activator.CreateInstance(conventionType);
                modelBuilder.Conventions.Add(configurationTypeInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
