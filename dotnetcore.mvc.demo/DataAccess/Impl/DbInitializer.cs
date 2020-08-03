using System;
using dotnetcore.mvc.demo.DataAccess.Entity;
using System.Linq;
using dotnetcore.mvc.demo.DataAccess.Base;

namespace dotnetcore.mvc.demo.DataAccess.Impl
{
    public static class DbInitializer
    {
        public static void InitializeLife(LifeContext context)
        {
            context.Database.EnsureCreated();

            // Look for any Girls.
            if (context.Girls.Any())
            {
                // DB has been seeded
                return;
            }

            var girls = new Girl[]
            {
                new Girl {Id = Guid.NewGuid().ToString("D"), Name = "Li", Age = 18, PhoneNumber = "18888888888"},
            };

            foreach (Girl s in girls)
            {
                context.Girls.Add(s);
            }

            context.SaveChanges();
        }
    }
}