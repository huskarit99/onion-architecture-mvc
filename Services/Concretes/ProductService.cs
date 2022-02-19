using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.Enums;
using Services.Interfaces;
using Services.ResourceModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.MVC;

namespace Services.Concretes
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Product>> GetAll()
        {
            Func<IQueryable<Product>, IQueryable<Product>> include = query => query.Include(p => p.Category);
            return (List<Product>)await _unitOfWork.Repository<Product>().GetAsync(include: include, asNoTracking: true);
        }
        public async Task<PaginatedList<Product>> Filter(
            string sortOrder = null,
            string keyword = null,
            int? pageIndex = null,
            int? pageSize = null)
        {
            Func<IQueryable<Product>, IQueryable<Product>> include = query => query.Include(p => p.Category);
            Expression<Func<Product, bool>> filter = keyword is null ? null : p => p.Name.Contains(keyword);
            MappingSortOrder mappingSortOrder = new();
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy =
                sortOrder is null || mappingSortOrder.ContainsKey(sortOrder) == false
                ? query => query.OrderBy(p => p.Name)
                : mappingSortOrder[sortOrder];

            List<Product> products = (List<Product>)await _unitOfWork.Repository<Product>().GetAsync(
                include: include,
                filter: filter,
                orderBy: orderBy,
                asNoTracking: true);

            int totalPages = (int)Math.Ceiling((int)products.Count() / (double)pageSize);
            if (pageIndex > totalPages)
                pageIndex = totalPages;

            return new PaginatedList<Product>(
                (List<Product>)await _unitOfWork.Repository<Product>().GetAsync(
                    include: include,
                    filter: filter,
                    orderBy: orderBy,
                    asNoTracking: true,
                    numberOfSkip: ((int)pageIndex - 1) * (int)pageSize,
                    numberOfTake: pageSize),
                products.Count(),
                (int)pageIndex,
                (int)pageSize);
        }
        public async Task<Product> GetOneById(int? id)
        {
            if (id is null)
            {
                return null;
            }
            Expression<Func<Product, bool>> filter = p => p.Id == id;
            Func<IQueryable<Product>, IQueryable<Product>> include = query => query.Include(p => p.Category);
            var product = await _unitOfWork.Repository<Product>().GetOneAsync(filter: filter, include: include, asNoTracking: true);
            if (product is null)
            {
                return null;
            }
            return product;
        }
        public async Task<SelectList> GetSelectListCategory(int? categoryId = null)
        {
            return categoryId is null
                ?
                new SelectList(await _unitOfWork.Repository<Category>().GetAsync(asNoTracking: true), "Id", "Name")
                :
                new SelectList(await _unitOfWork.Repository<Category>().GetAsync(asNoTracking: true), "Id", "Name", categoryId);
        }
        public async Task CreateOne(Product product)
        {
            await _unitOfWork.Repository<Product>().CreateOne(product);
            _unitOfWork.SaveChanges();
        }
        public bool ProductExists(int id)
        {
            Expression<Func<Product, bool>> filter = p => p.Id == id;
            return _unitOfWork.Repository<Product>().CheckExist(filter);
        }
        public bool UpdateOne(Product product)
        {
            try
            {
                _unitOfWork.Repository<Product>().UpdateOne(product);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
        public bool DeleteOneById(int id)
        {
            try
            {
                _unitOfWork.Repository<Product>().DeleteOne(id);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool DeleteManyById(List<int> listProductId)
        {
            try
            {
                foreach (var id in listProductId)
                {
                    DeleteOneById(id);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
