# Entity Framework tips and tricks

## Introduction

The purpose of this guide is to provide guidance on building applications using Entity Framework by showing tips and tricks about it.
All examples were made using [AdventureWorks](https://docs.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver15&tabs=ssms) database.

## Table of Contents

1. [Structure](#structure)
1. [Log](#log)
1. [Mappings and Configurations](#mappings-and-configurations)
1. [Migrations](#migrations)
1. [Queries](#queries)
1. [Write](#write)
1. [Tests](#tests)

## Structure

* We tried to do this guide using DDD (Domain-driven design). If you already know DDD so you'll be familiarized with folders structure.

``` c#
 ├── Entities
 |   ├── Department.cs
 |   └── Employee.cs
 |   └── ...
 ├── Infrastructure
 |   ├── DataAccess
 |   |   ├── Configurations
 |   |   |   ├── DepartmentConfiguration.cs
 |   |   |   ├── EmployeeConfiguration.cs
 |   |   |   ├── ...
 |   |   ├── Conventions
 |   |   |   ├── EntityConvention.cs
 |   |   |   ├── MakeAllStringsNonUnicodeConvention.cs
 |   |   |   ├── ...
 |   |   ├── Migrations
 |   |   ├── DataContext.cs
```

> See more about [Domain-driven design](https://en.wikipedia.org/wiki/Domain-driven_design)

* Don't use repository or unit of work patterns, to try to resolve performance problems using these patterns is hard. To not repeat others guys who also recommends not use these patterns I'll show you some goods articles about this subject:

  * [Repository is the new singleton](https://ayende.com/blog/3955/repository-is-the-new-singleton)
  * [Night of the living repositories](https://ayende.com/blog/3973/night-of-the-living-repositories)
  * [The false myth of encapsulating data access in the DAL](https://ayende.com/blog/4567/the-false-myth-of-encapsulating-data-access-in-the-dal)

**[Back to top](#table-of-contents)**

## Log

* Use `Database.Log` to visualize sql instructions in the context:

```c#
// Visualize sql instructions in a console app
context.Database.Log = Console.WriteLine

//Visualize sql instructions in Visual Studio Output Window
Database.Log = (l) => Debug.WriteLine(l);
```

   >See more about [Debug.WriteLine on Visual Studio Output Window](https://msdn.microsoft.com/pt-br/library/windows/desktop/ms698739(v=vs.100).aspx)

**[Back to top](#table-of-contents)**

## Mappings and Configurations

* Define as null database initialization when you don't use migrations (good to production environment).

**Example without database strategy initialization**

```bash
Opened connection at 26/10/2017 17:23:38 -02:00


SELECT Count(*)
FROM INFORMATION_SCHEMA.TABLES AS t
WHERE t.TABLE_SCHEMA + '.' + t.TABLE_NAME IN ('HumanResources.Department','Person.Person','HumanResources.EmployeeDepartmentHistory','HumanResources.Shift','HumanResources.JobCandidate','Person.Password','dbo.ErrorLog','HumanResources.Employee')
    OR t.TABLE_NAME = 'EdmMetadata'


-- Executing at 26/10/2017 17:23:38 -02:00

-- Completed in 157 ms with result: 8



Closed connection at 26/10/2017 17:23:38 -02:00

'entity-framework-guide.vshost.exe' (CLR v4.0.30319: entity-framework-guide.vshost.exe): Loaded 'C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Runtime.Serialization\v4.0_4.0.0.0__b77a5c561934e089\System.Runtime.Serialization.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
Opened connection at 26/10/2017 17:23:38 -02:00

SELECT 
    [GroupBy1].[A1] AS [C1]
    FROM ( SELECT 
        COUNT(1) AS [A1]
        FROM [dbo].[__MigrationHistory] AS [Extent1]
        WHERE [Extent1].[ContextKey] = @p__linq__0
    )  AS [GroupBy1]


-- p__linq__0: 'entity_framework_guide.Core.Infrastructure.DataAccess.DataContext' (Type = String, Size = 4000)

-- Executing at 26/10/2017 17:23:38 -02:00

-- Failed in 38 ms with error: Invalid object name 'dbo.__MigrationHistory'.



Closed connection at 26/10/2017 17:23:38 -02:00

Opened connection at 26/10/2017 17:23:38 -02:00

SELECT 
    [GroupBy1].[A1] AS [C1]
    FROM ( SELECT 
        COUNT(1) AS [A1]
        FROM [dbo].[__MigrationHistory] AS [Extent1]
    )  AS [GroupBy1]


-- Executing at 26/10/2017 17:23:38 -02:00

-- Failed in 1 ms with error: Invalid object name 'dbo.__MigrationHistory'.



Closed connection at 26/10/2017 17:23:38 -02:00

'entity-framework-guide.vshost.exe' (CLR v4.0.30319: entity-framework-guide.vshost.exe): Loaded 'EntityFrameworkDynamicProxies-EntityFramework'. 
Opened connection at 26/10/2017 17:23:38 -02:00

'entity-framework-guide.vshost.exe' (CLR v4.0.30319: entity-framework-guide.vshost.exe): Loaded 'EntityFrameworkDynamicProxies-entity-framework-guide'. 
SELECT 
    [Limit1].[C1] AS [C1], 
    [Limit1].[BusinessEntityID] AS [BusinessEntityID], 
    [Limit1].[Title] AS [Title], 
    [Limit1].[FirstName] AS [FirstName], 
    [Limit1].[MiddleName] AS [MiddleName], 
    [Limit1].[LastName] AS [LastName], 
    [Limit1].[ModifiedDate] AS [ModifiedDate], 
    [Limit1].[NationalIDNumber] AS [NationalIDNumber]
    FROM ( SELECT TOP (1) 
        [Extent1].[BusinessEntityID] AS [BusinessEntityID], 
        [Extent1].[NationalIDNumber] AS [NationalIDNumber], 
        [Extent2].[Title] AS [Title], 
        [Extent2].[FirstName] AS [FirstName], 
        [Extent2].[MiddleName] AS [MiddleName], 
        [Extent2].[LastName] AS [LastName], 
        [Extent2].[ModifiedDate] AS [ModifiedDate], 
        '1X0X' AS [C1]
        FROM  [HumanResources].[Employee] AS [Extent1]
        INNER JOIN [Person].[Person] AS [Extent2] ON [Extent1].[BusinessEntityID] = [Extent2].[BusinessEntityID]
    )  AS [Limit1]


-- Executing at 26/10/2017 17:23:39 -02:00

-- Completed in 1560 ms with result: SqlDataReader



Closed connection at 26/10/2017 17:23:40 -02:00
```
**Example with null database initialization**

```bash

SELECT 
    [Limit1].[C1] AS [C1], 
    [Limit1].[BusinessEntityID] AS [BusinessEntityID], 
    [Limit1].[Title] AS [Title], 
    [Limit1].[FirstName] AS [FirstName], 
    [Limit1].[MiddleName] AS [MiddleName], 
    [Limit1].[LastName] AS [LastName], 
    [Limit1].[ModifiedDate] AS [ModifiedDate], 
    [Limit1].[NationalIDNumber] AS [NationalIDNumber]
    FROM ( SELECT TOP (1) 
        [Extent1].[BusinessEntityID] AS [BusinessEntityID], 
        [Extent1].[NationalIDNumber] AS [NationalIDNumber], 
        [Extent2].[Title] AS [Title], 
        [Extent2].[FirstName] AS [FirstName], 
        [Extent2].[MiddleName] AS [MiddleName], 
        [Extent2].[LastName] AS [LastName], 
        [Extent2].[ModifiedDate] AS [ModifiedDate], 
        '1X0X' AS [C1]
        FROM  [HumanResources].[Employee] AS [Extent1]
        INNER JOIN [Person].[Person] AS [Extent2] ON [Extent1].[BusinessEntityID] = [Extent2].[BusinessEntityID]
    )  AS [Limit1]


-- Executing at 26/10/2017 17:25:23 -02:00

-- Completed in 0 ms with result: SqlDataReader


```

* Don't use database initialization strategy inside the context, because this way is difficult to change the strategy in a different environment. For example, in integrated tests we can create a SQL compact database to perform tests, but to do this it is necessary to create the database for all tests, so in this case, The application can use `Database.SetInitializer<DataContext>(null)` and to tests can use `Database.SetInitializer<DataContext>(new DropCreateDatabaseAlways<DataContext>())`

```c#
//wrong
public DataContext()
{
    //Very wrong
    Database.SetInitializer<DataContext>(null);

    Configuration.LazyLoadingEnabled = false;
    Configuration.ProxyCreationEnabled = false;


    Database.Log = (l) =>
    {
        Console.WriteLine(l); //ONLY CONSOLE APP
        Debug.WriteLine(l);
    };
}

...

//Good
protected void Application_Start()
{
  Database.SetInitializer<DataContext>(null);

  AreaRegistration.RegisterAllAreas();
  RegisterGlobalFilters(GlobalFilters.Filters);
  RegisterRoutes(RouteTable.Routes);
  ...
}

```


* Use the `EntityTypeConfiguration` class to mapping your classes instead of inline code in the OnModelCreating method. When we have a large number of domain classes to configure, each class in OnModelCreating method may become unmanageable.

```c#
// Entity class ErrorLog.cs
public class ErrorLog
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public string UserName { get; set; }
    public int Number { get; set; }
    public int? Severity { get; set; }
    public int? State { get; set; }
    public string Procedure { get; set; }
    public int Line { get; set; }
    public string Message { get; set; }
}

// Configuration class ErrorLogConfiguration.cs
public class ErrorLogConfiguration : EntityTypeConfiguration<ErrorLog>
{
    public ErrorLogConfiguration()
    {
        this.ToTable("ErrorLog");

        this.HasKey(t => t.Id);

        this.Property(t => t.Id)
            .HasColumnName("ErrorLogID");

        this.Property(t => t.Line)
            .HasColumnName("ErrorLine");

        this.Property(t => t.Message)
            .HasColumnName("ErrorMessage")
            .HasMaxLength(4000);

        this.Property(t => t.Number)
            .HasColumnName("ErrorNumber")
            .IsRequired();

        this.Property(t => t.Procedure)
            .HasColumnName("ErrorProcedure")
            .HasMaxLength(126);

        this.Property(t => t.Procedure)
            .HasColumnName("ErrorProcedure")
            .HasMaxLength(126);

        this.Property(t => t.Severity)
            .HasColumnName("ErrorSeverity");

        this.Property(t => t.State)
            .HasColumnName("ErrorState");

        this.Property(t => t.Time)
            .HasColumnName("ErrorTime")
            .IsRequired();

        this.Property(t => t.UserName)
            .HasColumnName("ErrorName")
            .HasMaxLength(128)
            .IsRequired();
    }
}
```

> To see others [mappings models](https://msdn.microsoft.com/en-us/library/jj591617(v=vs.113).aspx) (e.g. Inheritance, Entity Splitting, Table Splitting)

* Use explicit mapping for all properties and relations. Even convention mapping being a productive resource, the explicit mapping gives us data validation before to send it to the database. So you can prevent errors like:

  * String or binary data would be truncated. The statement has been terminated.
  * The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value.

* Use conventions to global types.

```c#

// Using varchar instead of nvarchar for all string types
public class UnicodeConvention : Convention
{
    public UnicodeConvention()
    {
        Properties<string>().Configure(t => t.IsUnicode(false));
    }
}
```

* Use conventions to avoid repeated code.

```c#
// Convention to mapping Entities
public class EntityConvention : Convention
{
    public EntityConvention()
    {
        Types().Where(t => t.IsAbstract == false && 
                            (
                                (t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(Entity<>)) ||
                                t.BaseType == typeof(Entity)
                            )
                        )
                .Configure(t =>
                {
                    t.Property("Id").IsKey().HasColumnName(t.ClrType.Name + "ID");
                });
    }
}

//Convention to mapping auditable entities
public class AuditableConvention : Convention
{
    public AuditableConvention()
    {
        Types().Where(t => typeof(Auditable).IsAssignableFrom(t))
                .Configure(t =>
                {
                    t.Property("ModifiedDate").IsRequired();
                });
    }
}

//People-mapping convention (e.g Employee)
public class PersonConvention : Convention
{
    public PersonConvention()
    {
        var personType = typeof(Person);

        Types().Where(t => t == personType || t.BaseType == personType)
                .Configure(t =>
                {
                    t.Property("Id").IsKey().HasColumnName("BusinessEntityID");
                });
    }
}
```

* Create DbSet properties in your context only on classes which you'll really need.

* There are some classes which you never will write operations (e.g. Views). In these cases, you should use read-only DbQuery to expose them.

```c#
public virtual DbQuery<IndividualCustomer> IndividualCustomers { get { return Set<IndividualCustomer>().AsNoTracking(); } }
```

* Load automatically conventions and configurations in the modelBuilder method:

```c#
 protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
    var assembly = typeof(AppDbContext).Assembly;

    modelBuilder.Configurations.AddFromAssembly(assembly);
    modelBuilder.Conventions.AddFromAssembly(assembly);
}
```

* When to use complex type you should initialize it in the constructor method, so you avoid problems either inserting a new record or using the attach method.

```c#
//Complex type
public class Name
{
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public string FullName()
    {
        return String.Format("{0} {1}, {2} {3}", Title, LastName, FirstName, MiddleName);
    }
}

//Using a complex type
public abstract class Person : Entity
{
    public Person()
    {
        //Starting the complex type
        Name = new Name();
    }

    public Name Name { get; set; }
    ...
}
```

**[Back to top](#table-of-contents)**

## Migrations

* Put Migrations class together with yours data access classes:

```bash
Enable-Migrations -MigrationsDirectory "Core\Infrastructure\DataAccess\Migrations"

```

* Gerente scripts file when necessary

```bash
Update-Database -Script -SourceMigration: $InitialDatabase -TargetMigration: AddPostAbstract
```

Reference https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/migrations/

### Migrations in a Existing Database

```bash
Add-Migration InitialCreate –IgnoreChanges 
```

Reference https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/migrations/existing-database


**[Back to top](#table-of-contents)**

## Queries

* Turn [Proxy and Lazy loading](https://msdn.microsoft.com/en-us/data/jj574232.aspx) off, with this you'll have to manually handle each related property loading:

```c#
public DataContext()
{
    Configuration.ProxyCreationEnabled = false;
    Configuration.LazyLoadingEnabled = false;
}
```

* Use `Include` method to load complex properties when you need:

```c#
using System.Data.Entity; // needed to use lambda expression with Include method

...

var employees = context.Employees.Include(e => e.HistoryDepartments)
                                 .Include(e => e.HistoryDepartments.Select(h => h.Department))
                                 .ToArray();
```

* The `Find` method realizes query using the mapping key and always look for the data on the local cache before the database.

```c#
// b is loaded from database
var a = context.Employees.Where(t => t.Id < 5).ToArray().First();
var b = context.Employees.First(1);

Console.WriteLine("A name: {0}", a.Name.FirstName);
Console.WriteLine("B name {0}", b.Name.FirstName);

```

* To access the local cache use `Local` DbSet property.

```c#
var employees = context.Employees.Where(t => t.Id < 5).ToArray();
var employee = context.Employees.Local.FirstOrDefault();
```

* Use `AsNoTracking` method to read-only situations. When you use it the context doesn't cache, in other words, the objects don't being available to access in the DbSets `Local` property.

```c#
var employees = context.Employees.AsNoTracking().ToArray();
```

* Use Projections Queries to load only required data.

```c#
context.Employees.Select(e => new { e.Id, e.Name });
```

> When you use projections queries you don't need to use `AsNoTracking` method.

* Use `Set` method to perform queries on classes does not expose in the context.

```c#
var resumes = context.Set<JobCandidate>().Where(j => j.Id > 2)
                                         .Select(j => j.Resume)
                                         .ToArray();
```

* Use `SelectMany` method to group collection properties:

```c#
var jobCandidates = context.Employees.SelectMany(e => e.JobCandidates) -- JobCandidates is a collection
                                      .Where(j => j.ModifiedDate < DateTime.Today).ToArray();
```

* Chain queries to avoid unnecessary joins:

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

> Entity Framework only performs queries after to call methods like `Single`, `SingleOrDefault`, `First`, `FirstOrDefault`, `ToList` or `ToArray`.

* Use default null result queries where use Max or Min to avoid problems when there aren't results.

```c#
var minStartDate = context.Employees.SelectMany(e => e.HistoryDepartments)
                       		        .Min(h => (DateTime?)h.StartDate) ?? DateTime.Today;
```

* Use paged queries with one or two calls to improve performance.

```c#
// two calls in the database
var query = context.Employees.Where(p => p.Id > 0);
var total = query.Count();

var people = query.OrderBy(p => p.Name.FirstName)
                  .Skip(0) // page
                  .Take(10) // records by page
                  .ToArray();

// one call in the database
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

> WARNING: Complex paged queries with one call may not work.

* Reuse your queries with property projections

```c#
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Expression<Func<Product, ProductListViewModel>> Projection
        {
            get
            {
                return p => new ProductListViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                };

            }
        }
    }

    ...

    var result = context.Products.Select(PhotoListItemViewModel.Projection)
                        .ToArray();
```

or

```c#
    public class ProductProjetions
    {
        public static Expression<Func<Product, dynamic>> ProductListViewModel
        {
            get
            {
                return p => new
                {
                    Id = p.Id,
                    Name = p.Name
                };

            }
        }
    }

    ...

    var result = context.Products.Select(ProductProjetions.ProductListViewModel)
                                 .ToArray();
```

**[Back to top](#table-of-contents)**

## Write

* Use `IValidatableObject` interface to implement custom validations where they are executed during `SaveChanges`

```c#
public class Department : Entity<short>, IValidatableObject
{
    public string Name { get; set; }
    public string GroupName { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var result = new List<ValidationResult>();

        if (Name == GroupName)
            result.Add(new ValidationResult("Name and group name cannot be equals"));

        return result;
    }
}
```

* Disable `ValidateOnSaveEnabled` when you need performance in write process:

```c#
context.Configuration.ValidateOnSaveEnabled = false;
```

* Disable `AutoDetectChangesEnabled` when you need performance in write process:

```c#
context.Configuration.AutoDetectChangesEnabled = false;
```

* Get only required data to write process.

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

> Using [`DbSet Extension`](https://gist.github.com/eduardosilva/58d1f672335a6788b9cbb2c2f4e747d3) you can use `GetOrAttach` method

```c#
...

// from this
var department = new Department { Id = departmentId };
context.Departments.Attach(department);
context.Entry(department).State = EntityState.Unchanged;

//to this
var department = context.Departments.GetLocalOrAttach(d => d.Id == departmentId, () => new Department { Id = departmentId });

...

```

* Override `SaveChanges` method to add operations before send data to database.

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

...

// See the same action using ChangeTracker class
// for more details see the source code
public class AudityChangeTracker : ChangeTracker<Auditable>
{
    public override void Added(Auditable entity)
    {
        entity.ModifiedDate = DateTime.Now;
    }

    public override void Deleted(Auditable entity)
    {

    }

    public override void Updated(Auditable entity)
    {
        entity.ModifiedDate = DateTime.Now;
    }
}

...

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

```

* Use `GetValidationErrors` method to get validation errors before execute `SaveChanges`

```c#
var newDepartment = new Department { };
context.Departments.Add(newDepartment);

//Get all errors
var errors = context.GetValidationErrors();

if (!errors.Any())
    context.SaveChanges();

foreach (var error in errors)
{
    Console.WriteLine("Entity Name: " + error.Entry.Entity.GetType().Name);
    foreach (var entityError in error.ValidationErrors)
    {
        Console.WriteLine("Property: {0} | Message {1}", entityError.PropertyName, entityError.ErrorMessage);
    }
}
```

* Use `GetValidationResult` method to get errors from a specific class

```c#
var newDepartment = new Department { Name = "A", GroupName = "A" };
context.Departments.Add(newDepartment);

var entityErrors = context.Entry(newDepartment).GetValidationResult();

if (entityErrors.IsValid)
    context.SaveChanges();

Console.WriteLine("Entity Name: " + entityErrors.Entry.Entity.GetType().Name);
foreach (var error in entityErrors.ValidationErrors)
{
    Console.WriteLine("Property: {0} | Message {1}", error.PropertyName, error.ErrorMessage);
}

Console.ReadKey();
```

**[Back to top](#table-of-contents)**

## Tests

* You can write tests using:
  * In-memory provider
  * Fake `Context` and `DbSet`
  * Frameworks like Moq

```c#
// Test example using Moq Framework

[TestMethod]
public void Get_departments()
{
    var data = new List<Department>
    {
        new Department { Name = "BBB" },
        new Department { Name = "ZZZ" },
        new Department { Name = "AAA" },
    }.AsQueryable();

    var mockSet = new Mock<DbSet<Department>>();
    mockSet.As<IQueryable<Department>>().Setup(m => m.Provider).Returns(data.Provider);
    mockSet.As<IQueryable<Department>>().Setup(m => m.Expression).Returns(data.Expression);
    mockSet.As<IQueryable<Department>>().Setup(m => m.ElementType).Returns(data.ElementType);
    mockSet.As<IQueryable<Department>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

    var mockContext = new Mock<DataContext>();
    mockContext.Setup(c => c.Departments).Returns(mockSet.Object);

    var context = mockContext.Object;

    var blogs = context.Departments.ToList();

    Assert.AreEqual(3, blogs.Count);
    Assert.AreEqual("BBB", blogs[0].Name);
    Assert.AreEqual("ZZZ", blogs[1].Name);
    Assert.AreEqual("AAA", blogs[2].Name);
}

```

> See more about [tests](https://msdn.microsoft.com/en-us/library/dn314431(v=vs.113).aspx).
>
> IMPORTANT: Don't write tests to Entity framework methods, write tests for your methods, use de ways above to achieve this.

**[Back to top](#table-of-contents)**