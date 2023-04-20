using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Controllers
{
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var _categories = _categoryRepository;

            if (_categories != null)
            {
                return View(_categories.GetCategories());
            }
            else
            {
                return Problem($"Entity set '{nameof(CategoryRepository)}' is null.");
            }
        }
    }
}
