using entity_framework_guide.Core.Entities;
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
	/// Update a entity First or Find
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

	//			// Let's change de Engineering Departament
	//			var department = context.Departments.FirstOrDefault(d => d.Id == 1);
	//			//var department = context.Departments.Find(1);


	//			department.Name += "s";
	//			context.SaveChanges();

	//			Console.Read();
	//		}
	//	}
	//}

	/// <summary>
	/// Update using Attach
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

	//			// Let's change de Engineering Departament
	//			var department = context.Departments.Attach(new Department { Id = 1 });


	//			department.Name += "s";

	//			// context.Configuration.ValidateOnSaveEnabled = false;
	//			context.SaveChanges();

	//			Console.Read();
	//		}
	//	}
	//}

	/// <summary>
	/// Getting errors and creating custom errors
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

	//			// Let's change de Engineering Departament
	//			var department = context.Departments.Attach(new Department { Id = 1 });
	//			department.Name += "s";

	//			// getting error by GetValidationErrors
	//			var errors = context.GetValidationErrors();
	//			foreach (var error in errors)
	//			{
	//				Console.WriteLine(error.Entry.Entity);
	//				error.ValidationErrors.ToList().ForEach(d => Console.WriteLine("\t" + d.PropertyName + ": " + d.ErrorMessage));
	//			}

	//			// getting error to a specific entity class
	//			//var error = context.Entry(department).GetValidationResult();
	//			//error.ValidationErrors.ToList().ForEach(d => Console.WriteLine(d.PropertyName + ": " + d.ErrorMessage));

	//			// context.Configuration.ValidateOnSaveEnabled = false;
	//			// context.SaveChanges();

	//			Console.Read();
	//		}
	//	}
	//}

	#region Advanced Reading & Writing

	/// <summary>
	/// why we need analyze each case
	/// Change employee's department (basic)
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

	//			// PAY ATTENTION: THIS IS A UNIQUE TRANSACTION OR A UNIT OF WORK PROCESS

	//			int employeeId = 1;
	//			short departmentId = 1;
	//			byte shiftId = 1;

	//			// WARNING: Find doesn't work with Include
	//			var employee = context.Employees.Include(e => e.HistoryDepartments)
	//											.Include(e => e.HistoryDepartments.Select(h => h.Department))
	//											.FirstOrDefault(e => e.Id == employeeId);
	//			// get current department to close
	//			var oldDepartment = employee.HistoryDepartments.FirstOrDefault(h => h.EndDate == null);
	//			oldDepartment.EndDate = DateTime.Now;

	//			var department = context.Departments.Find(departmentId);
	//			var shift = context.Shifts.Find(shiftId);

	//			employee.HistoryDepartments.Add(new EmployeeDepartment
	//			{
	//				Department = department,
	//				StartDate = DateTime.Now,
	//				Shift = shift
	//			});

	//			//context.SaveChanges();
	//			Console.Read();
	//		}
	//	}
	//}


	/// <summary>
	/// Change employee's department (advanced)
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

	//			int employeeId = 1;
	//			short departmentId = 1;
	//			byte shiftId = 1;

	//			var employee = new Employee { Id = employeeId };
	//			context.Employees.Attach(employee);

	//			// get current department to close
	//			var oldDepartment = context.Entry(employee)
	//									   .Collection(e => e.HistoryDepartments)
	//									   .Query().FirstOrDefault(h => h.EndDate == null);

	//			oldDepartment.EndDate = DateTime.Now;

	//			var department = new Department { Id = departmentId };
	//			context.Departments.Attach(department);

	//			var shift = new Shift { Id = shiftId };

	//			employee.HistoryDepartments.Add(new EmployeeDepartment
	//			{
	//				Department = department,
	//				StartDate = DateTime.Now,
	//				Shift = shift
	//			});

	//			// context.SaveChanges();
	//			Console.Read();
	//		}
	//	}
	//}


	/// <summary>
	/// override save changes
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

	//			// Let's see savechanges
	//		}
	//	}
	//}
	#endregion
}