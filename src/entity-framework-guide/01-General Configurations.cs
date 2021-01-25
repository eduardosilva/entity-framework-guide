using entity_framework_guide.Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide
{
	#region Logs Examples

	/// <summary>
	/// Without log
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		using (var context = new DataContext())
	//		{
	//			var employess = context.Employees.ToArray();
	//			foreach (var employee in employess)
	//			{
	//				Console.WriteLine(employee.Name);
	//			}
	//		}

	//		Console.Read();
	//	}
	//}


	/// <summary>
	/// With log
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		using (var context = new DataContext())
	//		{
	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			var employess = context.Employees.ToArray();
	//			foreach (var employee in employess)
	//			{
	//				Console.WriteLine(employee.Name);
	//			}
	//		}

	//		Console.Read();
	//	}
	//}

	#endregion

	#region Database Initializer
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{

	//		// https://www.entityframeworktutorial.net/code-first/database-initialization-strategy-in-code-first.aspx
	//		//1 - CreateDatabaseIfNotExists: This is the default initializer.As the name suggests, it will create the database if none exists as per the configuration. 
	//		//		However, if you change the model class and then run the application with this initializer, then it will throw an exception.

	//		//2 - DropCreateDatabaseIfModelChanges: This initializer drops an existing database and creates a new database, if your model classes(entity classes) have been changed.

	//		//3- DropCreateDatabaseAlways: This initializer drops an existing database every time you run the application. (Like in tests)

	//		//4- Custom DB Initializer.


	//		//WARNING: Be careful where you configure your database initializer.
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			var employess = context.Employees.ToArray();
	//			foreach (var employee in employess)
	//			{
	//				Console.WriteLine(employee.Name);
	//			}
	//		}

	//		Console.Read();
	//	}
	//}

	#endregion

	#region Lazy Loading & Proxies
	/// <summary>
	/// Without lazy loading and proxy configuration
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			var employess = context.Employees.ToArray();
	//			foreach (var employe in employess)
	//			{
	//				Console.WriteLine(employe.Name);
	//				foreach (var historicalDepartment in employe.HistoryDepartments)
	//				{
	//					Console.WriteLine("\t" + historicalDepartment.Department.Name);
	//				}

	//			}
	//		}

	//		Console.Read();
	//	}
	//}

	/// <summary>
	/// disabling lazy loading
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			// THE FAMOUS SELECT+1
	//			context.Configuration.LazyLoadingEnabled = false;

	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			var employess = context.Employees.ToArray();
	//			foreach (var employe in employess)
	//			{
	//				Console.WriteLine(employe.Name);
	//				foreach (var historicalDepartment in employe.HistoryDepartments)
	//				{
	//					Console.WriteLine("\t" + historicalDepartment.Department.Name);
	//				}

	//			}
	//		}

	//		Console.Read();
	//	}
	//}

	/// <summary>
	/// disabling proxy
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			// without lazy loading we don't need proxies
	//			// WARNING: proxy enable require virtual properties
	//			// context.Configuration.ProxyCreationEnabled = false;

	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			var employess = context.Employees.ToArray();
	//			foreach (var employe in employess)
	//			{
	//				Console.WriteLine(employe.Name);

	//				// Let's see with more details 
	//				Console.WriteLine(employe.GetType() + ": " + employe.Name);
	//			}
	//		}

	//		Console.Read();
	//	}
	//}
	#endregion
}
