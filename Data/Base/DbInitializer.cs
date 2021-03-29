using System;
using System.Linq;
using MvcDemo.Data.Girl;

namespace MvcDemo.Data.Base
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

            var girls = new GirlEntity[]
            {
                new GirlEntity {Id = Guid.NewGuid().ToString("D"), Name = "Li", Age = 18, PhoneNumber = "18888888888"},
            };

            foreach (GirlEntity s in girls)
            {
                context.Girls.Add(s);
            }

            context.SaveChanges();
        }
    }
}