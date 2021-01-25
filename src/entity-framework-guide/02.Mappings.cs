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
	/// <summary>
	/// Mappings
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		// 1 - Let's see the Employee.cs
	//		// 2 - Let's see OnModelCreating method;
	//		// 3 - Let's see the configuration structure;
	//		// 4 - Let's see the EmployeeConfiguration.cs;
	//		// 5 - Let's see the EmployeeDepartmentConfiguration.cs;
	//		// 6 - Let's see custom conventions;

	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Configuration.ProxyCreationEnabled = false;
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
	//			}
	//		}

	//		Console.Read();
	//	}
	//}
}
