using entity_framework_guide.Core.Entities;
using entity_framework_guide.Core.Infrastructure.DataAccess;
using entity_framework_guide.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace entity_framework_guide_test.Services
{
	[TestClass]
	public class EmployeeServiceTest
	{
		private DataContext context;
		private Employee employee;

		[TestInitialize]
		public void Initialize()
		{
			employee = new Employee { Id = 1, NationalIDNumber = "11111" };

			var employeeMockDbSet = new List<Employee>()
			{
				employee
			}
			.AsQueryable().BuildMockDbSet();

			var mockContext = new Mock<DataContext>();
			mockContext.Setup(c => c.Employees).Returns(employeeMockDbSet.Object);

			context = mockContext.Object;
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AddExistingEmployee()
		{
			var service = new EmployeeService(this.context);
			service.AddNewEmployee(new NewEmployeeModel { NationalIDNumber = "11111" });
		}

		[TestMethod]
		public void AddNewEmployee()
		{
			var service = new EmployeeService(this.context);
			var result = service.AddNewEmployee(new NewEmployeeModel { NationalIDNumber = "2222" });

			Assert.IsNotNull(result);
		}
	}
}
