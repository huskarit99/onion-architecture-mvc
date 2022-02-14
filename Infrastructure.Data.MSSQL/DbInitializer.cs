using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Data.MSSQL
{
    public static class DbInitializer
    {
		static private List<Boolean> isCategoryChildren = new List<Boolean>();
		public static void Initialize(TestDBContext context)
        {
           context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
				return;
            }
			List<Category> categories = context.Categories.ToList();
			
			for (int i = 0; i < categories.Count; i++) isCategoryChildren.Add(false);

			/// <summary>
			/// Random ParentId for Category Table.
			/// Each Category only has 1 Parent Category.
			/// Child Category can not be Parent Category Of other Category.
			/// </summary>
			for (int i = 0; i < categories.Count; i++)
            {
				if (i == 0) continue;
				int indexOfParent = (new Random()).Next(i) + 1;
				if (isCategoryChildren[indexOfParent] == false)
                {
					isCategoryChildren[i] = true;
					categories[i].ParentId = indexOfParent;
				}
            }

			/// <summary>
			/// Random CategoryId for Product Table.
			/// </summary>
			List<Product> products = context.Products.ToList();
			foreach (var product in products)
            {
				product.CategoryId = (new Random()).Next(500) + 1;
			}
			context.SaveChanges(); 
		}
    } 
}