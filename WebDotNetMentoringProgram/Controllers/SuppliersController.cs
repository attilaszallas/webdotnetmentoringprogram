using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Controllers
{

    public class SuppliersController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var _suppliers = _supplierRepository;

            if (_suppliers != null)
            {
                return View(_supplierRepository.GetSuppliers());
            }
            else
            {
                return Problem($"Entity set '{nameof(SupplierRepository)}'  is null.");
            }
        }
    }
}
