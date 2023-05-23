using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Filters;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.ViewModels;

namespace WebDotNetMentoringProgram.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        // GET: Categories
        [ApiExplorerSettings(IgnoreApi = true)]
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public IActionResult Index()
        {
            var _categories = _categoryRepository.GetCategories();

            var _categoryViewModel = (from _category in _categories
                select CategoryToCategoryViewModel(_category)).ToList();

            return View(_categoryViewModel);
        }

        [Route("Images/{id?}")]
        [Route("Categories/Image/{id?}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public IActionResult Image(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var _categories = _categoryRepository.GetCategories();

            var _categoryPicture = (from _category in _categories
                                    where _category.CategoryId == id
                                    select _category.Picture).FirstOrDefault();

            _categoryPicture = RemoveGarbageBytes(_categoryPicture);
            string imageBase64String = GetImageBase64String(_categoryPicture);

            ViewBag.Id = id;
            ViewBag.Image = imageBase64String;

            var image = ByteArrayToImage(_categoryPicture);
            return File(_categoryPicture, "image/bmp");
        }

        [Route("Categories/ChangeImage/{id?}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public IActionResult ChangeImage(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var _categories = _categoryRepository.GetCategories();

            var _categoryPicture = (from _category in _categories
                                    where _category.CategoryId == id
                                        select _category.Picture).FirstOrDefault();

            _categoryPicture = RemoveGarbageBytes(_categoryPicture);
            string imageBase64String = GetImageBase64String(_categoryPicture);

            ViewBag.Id = id;
            ViewBag.Image = imageBase64String;

            return View();
        }

        [Route("Categories/Image/{id?}")]
        [HttpPost, ActionName("NewImage")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public IActionResult NewImage(int? id, ImageFileUpload imageFileUpload)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if ((imageFileUpload == null) || (imageFileUpload.ImageFile == null))
            {
                return Problem($"Entity set {nameof(ImageFileUpload)} or {nameof(ImageFileUpload.ImageFile)} is null.");
            }

            var imageFile = imageFileUpload.ImageFile;

            //Getting file meta data
            var fileName = Path.GetFullPath(imageFile.FileName);
            var contentType = imageFile.ContentType;

            var _categories = _categoryRepository.GetCategories();

            var _categoryToUpdate = (from _category in _categories
                                        where _category.CategoryId == id
                                        select _category).FirstOrDefault();

            byte[] bitmapImage;
            using (var ms = new MemoryStream())
            {
                imageFile.CopyTo(ms);
                ms.Position = 0;
                bitmapImage = ms.ToArray();
            }

            _categoryToUpdate.Picture = bitmapImage;
            _categoryRepository.UpdateCategoryById(_categoryToUpdate);

            string imageBase64String = GetImageBase64String(bitmapImage);

            ViewBag.Id = id;
            ViewBag.Image = imageBase64String;

            return View();
        }

        private CategoryViewModel CategoryToCategoryViewModel(Category category)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();

            categoryViewModel.CategoryId = category.CategoryId;
            categoryViewModel.CategoryName = category.CategoryName;
            categoryViewModel.Description = category.Description;
            categoryViewModel.Image = GetImageBase64String(category.Picture);

            return categoryViewModel;
        }

        private byte[] RemoveGarbageBytes(byte[] bytes)
        {
            // the original Northwind database images have 10746 bytes
            return (bytes != null && bytes.Length == 10746)
                ? bytes.Skip(78).ToArray()
                : bytes;
        }

        private string GetImageBase64String(byte[] categoryPicture)
        {
            return Convert.ToBase64String(categoryPicture).ToString();
        }

        private Image ByteArrayToImage(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (Image img = SixLabors.ImageSharp.Image.Load(stream))
                {
                    return img;
                }
            }
        }
    }
}
