using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ResourceModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.MVC;

namespace Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetAll();
        public Task<PaginatedList<Product>> Filter(string sortOrder = null,
                                                string keyword = null,
                                                int? pageIndex = null,
                                                int? pageSize = null);
        public Task CreateOne(Product product);
        public Task<Product> GetOneById(int? id);
        public Task<SelectList> GetSelectListCategory(int? categoryId = null);
        public bool ProductExists(int id);
        public bool UpdateOne(Product product);
        public bool DeleteOneById(int id);
        public bool DeleteManyById(List<int> listProductId);
    }
}
