using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string Currency { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
