using Microsoft.AspNetCore.Mvc;
// please remove unused references 
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
            // move this as optional parameter for action method and don't read this from appsettings
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
            // I suggest to return BadRequest when id is null
            // this second check for whole ProcudtTable is not necessary here. You select all records so request performance is to long for product details
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
            // please move this two ViewBags before return view
            // it also make request performance longer

            // SupplierList List<SelectListItem>
            ViewBag.CompanyName = await (from suppliers in _context.Suppliers
                                                 select suppliers.CompanyName).ToListAsync();

            // CategoryList List<SelectListItem>
            ViewBag.CategoryName = await (from categories in _context.Categories
                                                select categories.CategoryName).ToListAsync();

            // this id is not nullable so this condidtion never will be fulfilled
            if (id == null)
            {
                return NotFound();
            }

            // again for edit one product we don't need to check whole table 
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
                    // move CreateProductFromProductTableViewModel method from method input and assign it to separate value it will be more helpfully for future debug of code for others devs
                    _context.Update(CreateProductFromProductTableViewModel(product));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // just throw exception with message without checking all products
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
            // same here return BadRequest when id is null without checking whole context
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
            // please keep the same behaviour for all endpoint actions 
            // one you check id and whole context here only context
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
            // this condition in Any is enough for check if product exists
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
