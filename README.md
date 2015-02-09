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
var employees = context.Employees.Include(e => e.HistoryDepartments)
                         .Include(e => e.HistoryDepartments.Select(h => h.Department))
                         .ToArray();
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

Access local cache use .Local method
```c#
var employees = context.Employees.Where(t => t.Id < 5);
var employee = context.Employees.Local.FirstOrDefault();
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
Paged query
```c#
// two call in database
var query = context.Employees.Where(p => p.Id > 0);
var total = query.Count();

var people = query.OrderBy(p => p.Name.FirstName)
                  .Skip(0) // page
                  .Take(10) // records by page
                  .ToArray();

// one call in database
var query = context.Employees.Where(p => p.Id > 0);

var page = query.OrderBy(p => p.Name.FirstName)
                .Skip(0) // page
                .Take(10) // records by page
                .Select(p => new { Name = p.Name })
                .GroupBy(p => new { Total = query.Count() })
                .First();

int total = page.Key.Total;
var people = page.Select(p => p);
```
Use SelectMany to group array properties
```c#
var jobCandidates = context.Employees.SelectMany(e => e.JobCandidates)
                                     .Where(j => j.ModifiedDate < DateTime.Today).ToArray();
```
Concatenate queries to avoid unnecessary joins
```c#
var query = context.Employees.AsQueryable();

if (!String.IsNullOrWhiteSpace(name))
    query = query.Where(e => e.Name.FirstName.Contains(name) ||
                             e.Name.MiddleName.Contains(name) ||
                             e.Name.LastName.Contains(name));

if (!String.IsNullOrWhiteSpace(departmentName))
    query = query.Where(e => e.HistoryDepartments.Any(h => h.EndDate == null && h.Department.Name.Contains(departmentName)));

var result = query.ToArray();
```

### Writing
Use simple update
```c#
var employee = new Employee { Id = 1 };
context.Employees.Attach(employee);
context.Entry(employee).State = EntityState.Unchanged;

employee.Name.FirstName = "Jones";
employee.Name.LastName = "Stwart";

context.SaveChanges();
```

Disable ValidateOnSaveEnabled to simple update
```c#
context.Configuration.ValidateOnSaveEnabled = false;
```

Retrive only required data in writing process
```c#
int employeeId = 1;
short departmentId = 1;
byte shiftId = 1;

var employee = new Employee { Id = employeeId };
context.Employees.Attach(employee);
context.Entry(employee).State = EntityState.Unchanged;

// get current department to close
var oldDepartment = context.Entry(employee)
                            .Collection(e => e.HistoryDepartments)
                            .Query().FirstOrDefault(h => h.EndDate == null);

oldDepartment.EndDate = DateTime.Now;

var department = new Department { Id = departmentId };
context.Departments.Attach(department);
context.Entry(department).State = EntityState.Unchanged;

var shift = new Shift { Id = shiftId };
context.Entry(shift).State = EntityState.Unchanged;

employee.HistoryDepartments.Add(new EmployeeDepartment
{
    Department = department,
    StartDate = DateTime.Now,
    Shift = shift
});

context.SaveChanges();
```
Override to SaveChanges method to  avoid repeated code
```c#
public override int SaveChanges()
{
    CheckAudit();
    return base.SaveChanges();
}

private void CheckAudit()
{
    foreach (var itemChanged in ChangeTracker.Entries())
    {
        if (!(itemChanged.State == EntityState.Added || itemChanged.State == EntityState.Modified))
            continue;


        var item = itemChanged.Entity as Auditable;
        item.ModifiedDate = DateTime.Now;
    }
}
```

Disable AutoDetectChangesEnabled to block write
```c#
context.Configuration.AutoDetectChangesEnabled = false;
```
