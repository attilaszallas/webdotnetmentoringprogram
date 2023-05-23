using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Abstractions;

namespace WebDotNetMentoringProgram.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesApiController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            var _categories = _categoryRepository.GetCategories();

            IEnumerable<string> categoryNames =
                from category in _categories
                select category.CategoryName;

            return Ok(categoryNames);
        }

        [HttpGet("GetImageById")]
        public IActionResult GetImageById(int id)
        {
            var _categories = _categoryRepository.GetCategories();

            if (id == 0 || id > _categories.Count())
            {
                return BadRequest();
            }

            var _category = _categoryRepository.GetCategoryById(id);

            return Ok(_category.Picture);
        }
        
        [HttpPost("UpdateImage")]
        public IActionResult UpdateImage(int id, byte[] image)
        {
            var _category = _categoryRepository.GetCategoryById(id);

            _category.Picture = image;

            _categoryRepository.UpdateCategoryById(_category);

            return Ok();
        }
    }
}
