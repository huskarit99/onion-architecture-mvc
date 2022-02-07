using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Enums
{
    public class MappingSortOrder : Dictionary<string, Func<IQueryable<Product>, IOrderedQueryable<Product>>>
    {
        public MappingSortOrder()
        {
            this.Add("name-desc", query => query.OrderByDescending(p => p.Name));
            this.Add("phone-asc", query => query.OrderBy(p => p.Phone));
            this.Add("phone-desc", query => query.OrderByDescending(p => p.Phone));
            this.Add("email-asc", query => query.OrderBy(p => p.Email));
            this.Add("email-desc", query => query.OrderByDescending(p => p.Email));
            this.Add("currency-asc", query => query.OrderBy(p => p.Currency));
            this.Add("currency-desc", query => query.OrderByDescending(p => p.Currency));
            this.Add("text-asc", query => query.OrderBy(p => p.Text));
            this.Add("text-desc", query => query.OrderByDescending(p => p.Text));
            this.Add("category-id-asc", query => query.OrderBy(p => p.CategoryId));
            this.Add("category-id-desc", query => query.OrderByDescending(p => p.CategoryId));
        }
    }
}
