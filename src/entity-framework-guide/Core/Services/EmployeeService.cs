using entity_framework_guide.Core.Entities;
using entity_framework_guide.Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Services
{
	public class EmployeeService
	{
		private DataContext context;

		public EmployeeService(DataContext context)
		{
			this.context = context;
		}

		public Employee AddNewEmployee(NewEmployeeModel model)
		{
			if (context.Employees.Any(e => e.NationalIDNumber == model.NationalIDNumber)) {
				throw new InvalidOperationException("Employee already exists");
			}

			// Code to add new 
			var employee = new Employee { NationalIDNumber = model.NationalIDNumber };

			// context.SaveChanges();

			return employee;
		}
	}

	public class NewEmployeeModel
	{
		public string NationalIDNumber { get; set; }
	}
}
