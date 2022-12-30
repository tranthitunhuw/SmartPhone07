using mvc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    public class SeedData
    {
		public static void Initialize(ApplicationDbContext context)
		{
			context.Database.EnsureCreated();

			// Look for any data
			if (context.Product.Any())
			{
				return;   // DB has been seeded
			}

			var products = new Product[]
			{
                
            };

			context.Product.AddRange(products);
			context.SaveChanges();
		}
	}
}
