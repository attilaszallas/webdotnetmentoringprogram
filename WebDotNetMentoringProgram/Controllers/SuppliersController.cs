using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDotNetMentoringProgram.Data;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Controllers
{
    // here is very similiar situation to improve like in ProductsController
    // please change this controller with comments form there

    public class SuppliersController : Controller
    {
        private readonly WebDotNetMentoringProgramContext _context;

        public SuppliersController(WebDotNetMentoringProgramContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            // same situation to improvment like in CategoriesController
            var _suppliers = _context.Suppliers;

            if (_suppliers != null)
            {
                return View(await _context.Suppliers.ToListAsync());
            }
            else
            {
                return Problem("Entity set 'WebDotNetMentoringProgramContext.Supplier'  is null.");
            }
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Throw Test Exception
            // throw new Exception("Test exception");

            if (id == null)
            {
                return BadRequest();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierID,CompanyName")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,CompanyName")] Supplier supplier)
        {
            if (id != supplier.SupplierID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(supplier);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
