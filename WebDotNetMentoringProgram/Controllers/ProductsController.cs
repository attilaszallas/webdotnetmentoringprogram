﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebDotNetMentoringProgram.Data;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.ViewModels;

namespace WebDotNetMentoringProgram.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebDotNetMentoringProgramContext _context;

        public ProductsController(WebDotNetMentoringProgramContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            int _numberOfProductsToShow = 0;

            if (Int32.TryParse(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("CustomSettings")["NumberOfProductsToShow"], out _numberOfProductsToShow))
            {
                if (_numberOfProductsToShow == 0)
                    _numberOfProductsToShow = await _context.Products.CountAsync();

                var _productTableViewModel = await ProductTableViewModel().Take(_numberOfProductsToShow).ToListAsync();

                return View(_productTableViewModel);
            }
            else
            {
                throw new Exception("Undefined or non existing 'NumberOfProductsToShow' parameter!");
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || ProductTableViewModel() == null)
            {
                return NotFound();
            }

            var product = await ProductTableViewModel()
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,SupplierID,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // SupplierList List<SelectListItem>
            ViewBag.CompanyName = await (from suppliers in _context.Suppliers
                                                 select suppliers.CompanyName).ToListAsync();

            // CategoryList List<SelectListItem>
            ViewBag.CategoryName = await (from categories in _context.Categories
                                                select categories.CategoryName).ToListAsync();

            if (id == null)
            {
                return NotFound();
            }

            var _productTableViewModel = await ProductTableViewModel().ToListAsync();

            if (_productTableViewModel == null)
            {
                return NotFound();
            }

            var _product = (from product in _productTableViewModel
                            where product.ProductID == id
                            select product).FirstOrDefault();
            if (_product == null)
            {
                return NotFound();
            }

            return View(_product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductTableViewModel product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(CreateProductFromProductTableViewModel(product));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'WebDotNetMentoringProgramContext.Product'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
     
        private IQueryable<ProductTableViewModel> ProductTableViewModel()
        {
            return (from product in _context.Products
                                               join supplier in _context.Suppliers on product.SupplierID equals supplier.SupplierID
                                               join category in _context.Categories on product.CategoryID equals category.CategoryId
                                               select new ProductTableViewModel()
                                               {
                                                   ProductID = product.ProductID,
                                                   ProductName = product.ProductName,
                                                   CompanyName = supplier.CompanyName,
                                                   CategoryName = category.CategoryName,
                                                   QuantityPerUnit = product.QuantityPerUnit,
                                                   UnitPrice = product.UnitPrice,
                                                   UnitsInStock = product.UnitsInStock,
                                                   UnitsOnOrder = product.UnitsOnOrder,
                                                   ReorderLevel = product.ReorderLevel,
                                                   Discontinued = product.Discontinued
                                               });
        }

        private Product CreateProductFromProductTableViewModel(ProductTableViewModel productTableViewModel)
        {
            var supplierSelected = (from supplier in _context.Suppliers 
                                    where supplier.CompanyName == productTableViewModel.CompanyName
                                    select supplier).FirstOrDefault();

            var categorySelected = (from category in _context.Categories
                                    where category.CategoryName == productTableViewModel.CategoryName
                                    select category).FirstOrDefault();

            return new Product()
            {
                ProductID = productTableViewModel.ProductID,
                ProductName = productTableViewModel.ProductName,
                SupplierID = supplierSelected.SupplierID,
                CategoryID = categorySelected.CategoryId,
                QuantityPerUnit = productTableViewModel.QuantityPerUnit,
                UnitPrice = productTableViewModel.UnitPrice,
                UnitsInStock = productTableViewModel.UnitsInStock,
                UnitsOnOrder = productTableViewModel.UnitsOnOrder,
                ReorderLevel = productTableViewModel.ReorderLevel,
                Discontinued = productTableViewModel.Discontinued
            };
        }
    }
}