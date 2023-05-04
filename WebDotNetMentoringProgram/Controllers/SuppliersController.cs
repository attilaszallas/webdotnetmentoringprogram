using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Filters;
using WebDotNetMentoringProgram.Repositories;

namespace WebDotNetMentoringProgram.Controllers
{

    public class SuppliersController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        }

        // GET: Suppliers
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public async Task<IActionResult> Index()
        {
            // tenatary condition will be look better
            if (_supplierRepository != null)
            {
                return View(_supplierRepository.GetSuppliers());
            }
            else
            {
                return Problem($"Entity set '{nameof(SupplierRepository)}' is null.");
            }
        }
    }
}
