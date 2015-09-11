# Entity Framework Lab

[![Join the chat at https://gitter.im/ITLab-Academy/EntityFrameworkLab](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/ITLab-Academy/EntityFrameworkLab?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

### Introdução
Abaixo se encontra uma série de dicas sobre Entity Framework (versão 6 ou superior), todos utilizando como base de dados [AdventureWorks](http://msftdbprodsamples.codeplex.com/).

### Log
Use `Database.Log` para visualizar todas instruções SQL realizadas pelo contexto:
```c#
// Visualizando as instruções SQL em um console app
context.Database.Log = Console.WriteLine
```

### Mapeamento
Exemplo de implementação de entidade com Convention Mapping:
```c#
// Entities
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

// Infrastrucutre > DataAccess > Conventions
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

Exemplo de aplicação de Unicode convention tornando todas as propriedades do tipo `string` para `VARCHAR` ao invés de `NVARCHAR`:
```c#
public class UnicodeConvention : Convention
{
    public UnicodeConvention()
    {
        Properties<string>().Configure(t => t.IsUnicode(false));
    }
}
```

Exemplo de como criar um readonly `DbSet` impedindo operações de escrita para alguns mappings.: (Ex.: Status, Tipos, etc...):
```c#
public DbQuery<Department> Departments { get { return Set<Department>().AsNoTracking(); } }
```

Exemplo de como carregar todos os conventions e configurations automaticamente no modelBuilder:
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

Se utilizar ComplexType sempre o deixe inicializado no construtor da classe para que você não tenha problemas com manipulações de instâncias para inserir um novo registro ou utilizando Attach ao invés de dar um Get
```c#
/// <summary>
/// Classe utilizada como ComplexType
/// </summary>
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

public abstract class Person : Entity
{
    public Person()
    {
        //Inicializando o ComplexType
        Name = new Name();
    }

    public Name Name { get; set; }
    public PersonPassword PasswordConfiguration { get; set; }
}
```

### Consultas
Em C# podemos escrever nossas consultas utilizando as seguintes sintaxes:

1. **Query Expression Syntax**.
	```c#
	from employee in context.Employees
	where employee.Id = 5
	select employee.Name.FirstName;
	```
	Parecida com T-SQL, foi criada para facilitar o entendimento da consulta.
	Algumas diferenças estruturais, como por exmeplo a clausula select estar no fim da instrução, se fizeram necessárias para manter o intellisense.

2. **Dot Notation Syntax**. 
	```c#
	context.Employees.Where(t => t.Id == 5).Name.FirstName;
	```
	Partindo das coleções de dados, é possível agregar operações utilizando os métodos disponibilizados por extensão. E após a consulta ser resolvida navegar nas propriedades dos objetos retornados.

O exemplo abaixo demonstra como uma mesma consulta pode ser realizada utilizando ambas as sintaxes.
```c#
// Exemplo Query Expression
var a = from employee in context.Employees
	    where employee.Id = 5
	    select employee.Name.FirstName;
	
//Exemplo Dot Notation
var b = context.Employees.Where(t => t.Id == 5).FirstOrDefault().Name.FirstName;

Console.WriteLine("A name: {0}", a);
Console.WriteLine("B name {0}", b);
```

Desabilitar [Proxy e LazyLoading](https://msdn.microsoft.com/en-us/data/jj574232.aspx) por padrão:
```c#
public DataContext()
{
    Configuration.ProxyCreationEnabled = false;
    Configuration.LazyLoadingEnabled = false;
}
```

Use o método Include para carregar propriedades complexas quando necessário:
```c#
using System.Data.Entity;

...

var employees = context.Employees.Include(e => e.HistoryDepartments)
                         .Include(e => e.HistoryDepartments.Select(h => h.Department))
                         .ToArray();
```

O método `Find` realiza consulta pela chave do mapeamento e sempre que possível, utiliza o cache local antes de realizar uma consulta no banco de dados:
```c#
// b is loaded from database
var a = context.Employees.Where(t => t.Id < 5).ToArray().First();
var b = context.Employees.First(1);

Console.WriteLine("A name: {0}", a.Name.FirstName);
Console.WriteLine("B name {0}", b.Name.FirstName);
```

```c#
// b is loaded from memory cache
var a = context.Employees.Where(t => t.Id < 5).ToArray().First();
var b = context.Employees.Find(1);

Console.WriteLine("A name: {0}", a.Name.FirstName);
Console.WriteLine("B name {0}", b.Name.FirstName);
```

Para acessar o cache local utilizar a propridade .Local do Dbset
```c#
var employees = context.Employees.Where(t => t.Id < 5).ToArray();
var employee = context.Employees.Local.FirstOrDefault();
```


Use `TransactionScope` para ler linhas que estão em transação (NOLOCK):

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
Exemplo de consulta paginada:
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

Utilize `SelectMany` para agrupar propriedades do tipo coleção:
```c#
var jobCandidates = context.Employees.SelectMany(e => e.JobCandidates)
                                     .Where(j => j.ModifiedDate < DateTime.Today).ToArray();
```

Concatene consulta para evitar joins desnecessários:
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

Utilize default nullables para consultas que retornam Max ou Min afim de evitar problemas quando não retornam nenhum resultado.

```c#
var minStartDate = context.Employees.SelectMany(e => e.HistoryDepartments)
                       		    .Min(h => (DateTime?)h.StartDate) ?? DateTime.Today;
```

### Escrita
Exemplo de como realizar um simples update:
```c#
var employee = new Employee { Id = 1 };
context.Employees.Attach(employee);
context.Entry(employee).State = EntityState.Unchanged;

employee.Name.FirstName = "Jones";
employee.Name.LastName = "Stwart";

context.SaveChanges();
```

Desabilite `ValidateOnSaveEnabled` para updates simples:
```c#
context.Configuration.ValidateOnSaveEnabled = false;
```

Obtenha somente os dados necessários para processos de escrita:
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

Sobreescreva o método `SaveChanges` para evitar código duplicado (Ex.: facilitar o preenchimento de campos de auditoria)
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

Desabilite `AutoDetectChangesEnabled` para maior performance em processo de escrita em lote:
```c#
context.Configuration.AutoDetectChangesEnabled = false;
```
