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
            // reading all catogories here is unnecessary 
            // if you not find image for proper id just return not found 404 
            var _category = _categoryRepository.GetCategoryById(id);

            return (_category != null) ? Ok(_category.Picture) : NotFound();
        }
        
        [HttpPost("UpdateImage")]
        public IActionResult UpdateImage(int id, string image)
        {
            // same situation here
            if (image == string.Empty)
            {
                return BadRequest("Image string is empty");
            }

            byte[] bitmapImage;

            try
            {
                bitmapImage = Convert.FromBase64String(image);
            }
            catch
            {
                return BadRequest("Image format problem");
            }

            var _category = _categoryRepository.GetCategoryById(id);

            if (_category == null)
            {
                return NotFound();
            }

            _category.Picture = bitmapImage;

            _categoryRepository.UpdateCategoryById(_category);

            return Ok();
        }
    }
}
