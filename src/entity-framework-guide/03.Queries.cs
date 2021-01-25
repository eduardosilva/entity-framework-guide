using entity_framework_guide.Core.Entities;
using entity_framework_guide.Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide
{
	/// <summary>
	/// Test basic execution queries
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Configuration.LazyLoadingEnabled = false;
	//			context.Configuration.ProxyCreationEnabled = false;

	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			var employess = context.Employees;
	//			foreach (var employe in employess)
	//			{
	//				Console.WriteLine(employe.Name);
	//			}
	//		}

	//		Console.Read();
	//	}
	//}

	/// <summary>
	/// Chain queries example
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Configuration.LazyLoadingEnabled = false;
	//			context.Configuration.ProxyCreationEnabled = false;

	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			// BAD WAY
	//			//var name = "a";
	//			//var departmentName = "a";
	//			////var name = "";
	//			////var departmentName = "";
	//			//var query = context.Employees.AsQueryable();

	//			//query = query.Where(e => String.IsNullOrEmpty(name) || (e.Name.FirstName.Contains(name) ||
	//			//						 e.Name.MiddleName.Contains(name) ||
	//			//						 e.Name.LastName.Contains(name)));

	//			//query = query.Where(e => e.HistoryDepartments.Any(h => h.EndDate == null && (String.IsNullOrEmpty(departmentName) || h.Department.Name.Contains(departmentName))));

	//			//var employess = query.ToArray();
	//			//foreach (var employe in employess)
	//			//{
	//			//	Console.WriteLine(employe.Id + ": " + employe.Name);
	//			//}

	//			// GOOD WAY
	//			//var name = "a";
	//			//var departmentName = "a";
	//			////var name = "";
	//			////var departmentName = "";
	//			//var query = context.Employees.AsQueryable();

	//			//if (!String.IsNullOrWhiteSpace(name))
	//			//	query = query.Where(e => e.Name.FirstName.Contains(name) ||
	//			//							 e.Name.MiddleName.Contains(name) ||
	//			//							 e.Name.LastName.Contains(name));

	//			//if (!String.IsNullOrWhiteSpace(departmentName))
	//			//	query = query.Where(e => e.HistoryDepartments.Any(h => h.EndDate == null && h.Department.Name.Contains(departmentName)));

	//			//var employess = query.ToArray();
	//			//foreach (var employe in employess)
	//			//{
	//			//	Console.WriteLine(employe.Id + ": " + employe.Name);
	//			//}
	//		}

	//		Console.Read();
	//	}
	//}

	/// <summary>
	/// Using SelectMany
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Configuration.LazyLoadingEnabled = false;
	//			context.Configuration.ProxyCreationEnabled = false;

	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			var nationalIdNumber = "885055826";

	//			// BAD WAY 1
	//			var employee = context.Employees.FirstOrDefault(e => e.NationalIDNumber == nationalIdNumber);
	//			var jobCandidates = employee.JobCandidates.Where(j => j.ModifiedDate < DateTime.Today).ToArray();

	//			foreach (var jobCandidate in jobCandidates)
	//			{
	//				Console.WriteLine("Resume: " + jobCandidate.Resume);
	//			}

	//			// BAD WAY 2
	//			//var employee = context.Employees.Include(e => e.JobCandidates).FirstOrDefault(e => e.NationalIDNumber == nationalIdNumber);
	//			//var jobCandidates = employee.JobCandidates.Where(j => j.ModifiedDate < DateTime.Today).ToArray();

	//			//foreach (var jobCandidate in jobCandidates)
	//			//{
	//			//	Console.WriteLine("Resume: " + jobCandidate.Resume);
	//			//}

	//			// GOOD WAY
	//			//var jobCandidates = context.Employees.Where(e => e.NationalIDNumber == nationalIdNumber)
	//			//									 .SelectMany(e => e.JobCandidates)
	//			//									 .Where(j => j.ModifiedDate < DateTime.Today).ToArray();

	//			//foreach (var jobCandidate in jobCandidates)
	//			//{
	//			//	Console.WriteLine("Resume: " + jobCandidate.Resume);
	//			//}
	//		}

	//		Console.Read();
	//	}
	//}

	/// <summary>
	/// 1st level cache & DbSet.Local & AsNoTracking
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Configuration.LazyLoadingEnabled = false;
	//			context.Configuration.ProxyCreationEnabled = false;

	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			Console.WriteLine("Query result =========================================");
	//			var employess = context.Employees.Where(e => e.Id > 200).ToArray();
	//			//var employess = context.Employees.Where(e => e.Id >= 200).ToArray();
	//			//var employess = context.Employees.Where(e => e.Id >= 200).AsNoTracking().ToArray();
	//			foreach (var employe in employess)
	//			{
	//				Console.WriteLine(employe.Id + ": " + employe.Name);
	//			}

	//			Console.WriteLine("Find result =========================================");
	//			var specificEmployee = context.Employees.Find(200);
	//			Console.WriteLine(specificEmployee.Id + ": " + specificEmployee.Name);

	//			Console.WriteLine("Local result =========================================");
	//			foreach (var item in context.Employees.Local)
	//			{
	//				Console.WriteLine(item.Id + ": " + item.Name);
	//			}
	//		}

	//		Console.Read();
	//	}
	//}

	/// <summary>
	/// Projections
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Configuration.LazyLoadingEnabled = false;
	//			context.Configuration.ProxyCreationEnabled = false;

	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};


	//			// see the differences bettwen these queries
	//			var departments = context.Departments.ToArray();

	//			//var departments = context.Departments.Select(d => new
	//			//{
	//			//	d.Id,
	//			//	d.Name
	//			//}).ToArray();

	//			//var departments = context.Departments.Select(DepartmentViewModel.Projection).ToArray();

	//			foreach (var department in departments)
	//			{
	//				Console.WriteLine(department.Id + ": " + department.Name);
	//			}
	//		}

	//		Console.Read();
	//	}
	//	public class DepartmentViewModel
	//	{
	//		public int Id { get; set; }
	//		public string Name { get; set; }

	//		public static Expression<Func<Department, DepartmentViewModel>> Projection
	//		{
	//			get
	//			{
	//				return d => new DepartmentViewModel
	//				{
	//					Id = d.Id,
	//					Name = d.Name
	//				};
	//			}
	//		}
	//	}
	//}

	/// <summary>
	/// Using Include, simple or in many levels
	/// </summary>
	//partial class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Database.SetInitializer<DataContext>(null);

	//		using (var context = new DataContext())
	//		{
	//			context.Configuration.ProxyCreationEnabled = false;

	//			context.Database.Log = (l) =>
	//			{
	//				Console.WriteLine(l);
	//				Debug.WriteLine(l);
	//			};

	//			var employess = context.Employees.Include(e => e.HistoryDepartments)
	//											 .Include(e => e.HistoryDepartments.Select(h => h.Department))
	//											 .ToArray();
	//			foreach (var employe in employess)
	//			{
	//				Console.WriteLine("Employe name: " + employe.Name);
	//				foreach (var historicalDepartment in employe.HistoryDepartments)
	//				{
	//					Console.WriteLine("\t Department: " + historicalDepartment.Department.Name);
	//				}

	//			}
	//		}

	//		Console.Read();
	//	}
	//}
}
