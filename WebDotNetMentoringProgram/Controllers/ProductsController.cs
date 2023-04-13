using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index(int _numberOfProductsToShow = 10)
        {
            if (_numberOfProductsToShow == 0)
                _numberOfProductsToShow = await _context.Products.CountAsync();

            var _productTableViewModel = new List<ProductTableViewModel>();

            var _productsToShow = await _context.Products.Take(_numberOfProductsToShow).ToListAsync();

            foreach (var product in _productsToShow)
            {
                _productTableViewModel.Add(CreateProductTableViewModelFromProduct(product));
            }

            return View(_productTableViewModel);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            var productTableView = CreateProductTableViewModelFromProduct(product);

            return View(productTableView);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var _product = await (from product in _context.Products
                            where product.ProductID == id
                            select product).FirstOrDefaultAsync();

            if (_product == null)
            {
                return NotFound();
            }

            var _productTableViewModel = CreateProductTableViewModelFromProduct(_product);

            // SupplierList List<SelectListItem>
            ViewBag.CompanyName = await (from suppliers in _context.Suppliers
                                         select suppliers.CompanyName).ToListAsync();

            // CategoryList List<SelectListItem>
            ViewBag.CategoryName = await (from categories in _context.Categories
                                          select categories.CategoryName).ToListAsync();

            return View(_productTableViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductTableViewModel productTableViewModel)
        {
            if (id != productTableViewModel.ProductID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var product = CreateProductFromProductTableViewModel(productTableViewModel);
                _context.Update(product);
                await _context.SaveChangesAsync();
                 
                return RedirectToAction(nameof(Index));
            }

            return View(productTableViewModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // same here return BadRequest when id is null without checking whole context
            if (id == null)
            {
                return BadRequest();
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
            // please keep the same behaviour for all endpoint actions 
            // one you check id and whole context here only context
            if (id == null)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private ProductTableViewModel CreateProductTableViewModelFromProduct(Product product)
        {
            var supplierSelected = (from supplier in _context.Suppliers
                                    where supplier.SupplierID == product.SupplierID
                                    select supplier).FirstOrDefault();

            var categorySelected = (from category in _context.Categories
                                    where category.CategoryId == product.CategoryID
                                    select category).FirstOrDefault();

            return new ProductTableViewModel()
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                CompanyName = supplierSelected.CompanyName,
                CategoryName = categorySelected.CategoryName,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued
            };
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
