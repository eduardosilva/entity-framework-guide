﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;
using entity_framework_guide.Core.Infrastructure.DataAccess;

namespace entity_framework_guide
{
	partial class Program
	{
		static void Main(string[] args)
		{
			//Database.SetInitializer<DataContext>(null);

			using (var context = new DataContext())
			{
				var e = context.Employees.ToArray();
				foreach (var i in e)
				{
					Console.WriteLine(i.Name);
				}
			}

			Console.Read();
		}
	}

	// Migration Example
	//class Program
	//{
	//    static void Main(string[] args)
	//    {
	//        Database.SetInitializer<DataContext>(null);

	//        using (var context = new DataContext())
	//        {
	//            var e = context.Employees.First();
	//            e.NationalIDNumber = "295847284";

	//            context.SaveChanges();
	//        }
	//    }

	//    public class Supplier : Entity
	//    {
	//        public string Name { get; set; }
	//    }

	//    public class SupplierConfiguration : EntityTypeConfiguration<Supplier>
	//    {
	//        public SupplierConfiguration()
	//        {
	//            ToTable(tableName: "Supplier", schemaName: "Bootcamp");

	//            Property(t => t.Name).HasMaxLength(250);
	//        }
	//    }
	//}
}
