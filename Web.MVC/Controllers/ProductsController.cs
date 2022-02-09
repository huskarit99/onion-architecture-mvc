using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Services.Interfaces;

namespace Web.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> Index(
            string sort_order,
            string keyword,
            int? page = 1,
            int? page_size = 5)
        {
            ViewData["CurrentSort"] = sort_order;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sort_order) || sort_order == "name-asc" ? "name-desc" : "name-asc";
            ViewData["PhoneSortParm"] = sort_order == "phone-asc" ? "phone-desc" : "phone-asc";
            ViewData["EmailSortParm"] = sort_order == "email-asc" ? "email-desc" : "email-asc";
            ViewData["CurrencySortParm"] = sort_order == "currency-asc" ? "currency-desc" : "currency-asc";
            ViewData["TextSortParm"] = sort_order == "text-asc" ? "text-desc" : "text-asc";
            ViewData["CategoryIdSortParm"] = sort_order == "category-id-asc" ? "category-id-desc" : "category-id-asc";

            if (ViewData.ContainsKey("CurrentFilter") && (string)ViewData["CurrentFilter"] != keyword)
            {
                page = 1;
            }
            else
            {
                ViewData["CurrentFilter"] = keyword;
            }
            int pageSize = (int)page_size;
            if (page_size != 5)
            {
                ViewData["CurrentPageSize"] = pageSize;
            }

            return View(await _productService.Filter(sort_order, keyword, page ?? 1, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var product = await _productService.GetOneById(id);
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [HttpGet("products/create")]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = await _productService.GetSelectListCategory();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("products/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Email,Text,Currency,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                await  _productService.CreateOne(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = await _productService.GetSelectListCategory(product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [HttpGet("products/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _productService.GetOneById(id);
            if (product is null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = await _productService.GetSelectListCategory(product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("products/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Phone,Email,Text,Currency,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (_productService.UpdateOne(product))
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            ViewData["CategoryId"] = await _productService.GetSelectListCategory(product.CategoryId);
            return View(product);
        }
        [HttpGet("products/delete")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _productService.GetOneById(id);
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost("products/delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _productService.DeleteOneById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}