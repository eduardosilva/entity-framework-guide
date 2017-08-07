using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using entity_framework_guide.Core.Entities;
using entity_framework_guide.Core.Infrastructure.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace entity_framework_guide_test
{
    [TestClass]
    public class DepartmentTests
    {
        [TestMethod]
        public void Add_new_department()
        {
            var mockSet = new Mock<DbSet<Department>>();

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(m => m.Departments).Returns(mockSet.Object);

            var context = mockContext.Object;

            context.Departments.Add(new Department { Name = "Development", GroupName = "Department" });
            context.SaveChanges();

            mockSet.Verify(m => m.Add(It.IsAny<Department>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

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
    }
}
