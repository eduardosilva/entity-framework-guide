# Entity Framework Lab

### Log
Use Database.Log for view all sql instructions realized for the context

    // view sql instructions in a console app
    context.Database.Log = Console.WriteLine


### Mappings
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
Readonly DbSet in the context
```c#
public DbQuery<Department> Departments { get { return Set<Department>().AsNoTracking(); } }
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
### Queries
Disable proxy and lazyloading by default
```c#
public DataContext()
{
    Configuration.ProxyCreationEnabled = false;
    Configuration.LazyLoadingEnabled = false;
}
```
Use Include method to load referencial properties
```c#
var employees = context.Employees.Include(c => c.HistoryDepartments).ToArray();
```


Find method always use Local store before the database

```c#
// b is loaded from database
var a = context.Employees.Where(t => t.Id < 5).First();
var b = context.Employees.First(1);

Console.WriteLine("A name: {0}", a.Name.FirstName);
Console.WriteLine("B name {0}", b.Name.FirstName);
```

```c#
// b is loaded from memory cache
var a = context.Employees.Where(t => t.Id < 5).First();
var b = context.Employees.Find(1);

Console.WriteLine("A name: {0}", a.Name.FirstName);
Console.WriteLine("B name {0}", b.Name.FirstName);
```

Use TransactionScope for read uncommited data (NOLOCK)

```c#
var transactionOptions = new System.Transactions.TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted };
using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, transactionOptions))
{
	using (var context = new DataContext())
	{
		context.Database.Log = Console.WriteLine;
		var d = context.Departments.ToArray();

		transactionScope.Complete();
	}
}

```
