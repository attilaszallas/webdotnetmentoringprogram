using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebDotNetMentoringProgram.Filters;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.ViewModels;

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
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public async Task<IActionResult> Index()
        {
            if (_categoryRepository != null)
            {
                var _categories = _categoryRepository.GetCategories();

                var _categoryViewModel = (from _category in _categories
                 select CategoryToCategoryViewModel(_category)).ToList();

                return View(_categoryViewModel);
            }
            else
            {
                return Problem($"Entity set '{nameof(CategoryRepository)}' is null.");
            }
        }

        [Route("Images/{id?}")]
        [Route("Categories/Image/{id?}")]
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public async Task<IActionResult> Image(int? id)
        {
            if (_categoryRepository != null)
            {
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
            else
            {
                return Problem($"Entity set '{nameof(CategoryRepository)}' is null.");
            }
        }

        [Route("Categories/ChangeImage/{id?}")]
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public async Task<IActionResult> ChangeImage(int? id)
        {
            if (_categoryRepository != null)
            {
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
            else
            {
                return Problem($"Entity set '{nameof(CategoryRepository)}' is null.");
            }
        }

        [Route("Categories/Image/{id?}")]
        [HttpPost, ActionName("NewImage")]
        [ServiceFilter(typeof(LoggingResponseHeaderFilterService))]
        public async Task<IActionResult> NewImage(int? id, ImageFileUpload imageFileUpload)
        {
            if ((imageFileUpload != null) && (imageFileUpload.ImageFile != null))
            {
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
            else
            { 
                return Problem($"Entity set {nameof(ImageFileUpload)} or {nameof(ImageFileUpload.ImageFile)} is null.");
            }
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
            if (bytes != null && bytes.Length == 10746) // the original Northwind database images have 10746 bytes
            {
                return bytes.Skip(78).ToArray();
            }
            else
            {
                return bytes;
            }
        }

        private string GetImageBase64String(byte[] categoryPicture)
        {
            return Convert.ToBase64String(categoryPicture).ToString();
        }

        private Image ByteArrayToImage(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                Image img = System.Drawing.Image.FromStream(stream);
                return img;
            }
        }
    }
}
