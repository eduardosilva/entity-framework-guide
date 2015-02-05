# Entity Framework Lab

### Mappings
Readonly DbSet in the context
```c#
public DbQuery<Department> Departments { get { return Set<Department>().AsNoTracking(); } }
```
Entity implementation with convention mapping
```c#
//Entities
public class Entity<T> : IAuditable
    where T : struct
{
    public T Id { get; set; }
    public DateTime ModifiedDate { get; set; }
}

public interface IAuditable
{
    DateTime ModifiedDate { get; set; }
}

//Infrastrucutre > DataAccess > Conventions
public class EntityConvention : Convention
{
    public EntityConvention()
    {
        Types().Where(t => t.BaseType.GetGenericTypeDefinition() == typeof(Entity<>))
               .Configure(t =>
               {
                   t.Property("Id").IsKey().HasColumnName(t.ClrType.Name + "ID");
                   t.Property("ModifiedDate").IsRequired();
               });
    }
}
```
Unicode convention
```c#
public class UnicodeConvention : Convention
{
    public UnicodeConvention()
    {
        Properties<string>().Configure(t => t.IsUnicode(false));
    }
}
```
Automatic load all conventions and configurations
```c#
 protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var assemblyTypes = typeof(DataContext).Assembly.GetTypes();

            var configurationTypes = assemblyTypes.Where(t => t.IsAbstract == false &&
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

            var conventionTypes = assemblyTypes.Where(t => t.IsAbstract == false &&
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
```
