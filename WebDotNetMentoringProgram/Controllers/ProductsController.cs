using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.ViewModels;

namespace WebDotNetMentoringProgram.Controllers
{
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private ISupplierRepository _supplierRepository;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, ISupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index(int _numberOfProductsToShow = 10)
        {
            if (_numberOfProductsToShow == 0)
                _numberOfProductsToShow = _productRepository.GetProductCount();

            var _productTableViewModel = new List<ProductTableViewModel>();

            var _productsToShow = _productRepository.GetProductsWithLimit(_numberOfProductsToShow);

            foreach (var product in _productsToShow)
            {
                _productTableViewModel.Add(CreateProductTableViewModelFromProduct(product));
            }

            return View(_productTableViewModel);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var product = _productRepository.GetProductById(id);

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
                _productRepository.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var _product = _productRepository.GetProductById(id);

            if (_product == null)
            {
                return NotFound();
            }

            var _productTableViewModel = CreateProductTableViewModelFromProduct(_product);

            // SupplierList List<SelectListItem>
            ViewBag.CompanyNames = _supplierRepository.GetSupplierNames();

            // CategoryList List<SelectListItem>
            ViewBag.CategoryNames = _categoryRepository.GetCategoryNames();

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

                _productRepository.Update(product);
                return RedirectToAction(nameof(Index));
            }

            return View(productTableViewModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var product = _productRepository.GetProductById(id);
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

            var product = _productRepository.GetProductById(id);
            if (product != null)
            {
                _productRepository.Remove(product);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private ProductTableViewModel CreateProductTableViewModelFromProduct(Product product)
        {
            var supplierSelected = _supplierRepository.GetSupplierById(product.ProductID);
            var categorySelected = _categoryRepository.GetCategoryById(product.CategoryID);

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
            var supplierSelected = _supplierRepository.GetSupplierByName(productTableViewModel.CompanyName);
            var categorySelected = _categoryRepository.GetCategoryByName(productTableViewModel.CategoryName);

            return new Product(
                productTableViewModel.ProductID,
                productTableViewModel.ProductName,
                supplierSelected.SupplierID,
                categorySelected.CategoryId,
                productTableViewModel.QuantityPerUnit,
                productTableViewModel.UnitPrice,
                productTableViewModel.UnitsInStock,
                productTableViewModel.UnitsOnOrder,
                productTableViewModel.ReorderLevel,
                productTableViewModel.Discontinued
                ) {};
        }
    }
}
