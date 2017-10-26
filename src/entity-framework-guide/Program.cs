using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;
using entity_framework_guide.Core.Infrastructure.DataAccess;

namespace entity_framework_guide
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer<DataContext>(null);

            using (var context = new DataContext())
            {
                var e = context.Employees.First();
                e.NationalIDNumber = "295847284";

                context.SaveChanges();
            }
        }
    }
}
