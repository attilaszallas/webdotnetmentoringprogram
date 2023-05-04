using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SixLabors.ImageSharp.Formats.Bmp;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Controllers;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.ViewModels;
using Assert = Xunit.Assert;
using Image = SixLabors.ImageSharp.Image;

namespace WebDotNetMentoringProgramTest
{
    [TestFixture]
    public class CategoryTests
    {
        private Mock<ICategoryRepository> _categoryRepository;

        [OneTimeSetUp]
        public void Init()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
        }

        [Test]
        public void For_Index_Action_ViewResult_Result()
        {
            // Arrange
            CategoriesController categoriesController = new CategoriesController(_categoryRepository.Object);

            // Act
            var result = categoriesController.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Test]
        public void For_Image_Action_FileContent_Result()
        {
            // Arrange
            var bitmap = SixLabors.ImageSharp.Image.Load("./w3c_home.bmp");

            var category = new Category();
            category.CategoryId = 1;
            category.Picture = ImageToByteArray(bitmap);

            var categories = new List<Category>
            {
                category
            };

            _categoryRepository.Setup(x => x.GetCategories())
                .Returns(categories);

            CategoriesController categoriesController = new CategoriesController(_categoryRepository.Object);

            // Act
            var result = categoriesController.Image(1);

            // Assert
            Assert.IsType<FileContentResult>(result);
        }

        [Test]
        public void For_ChangeImage_Action_With_Input_Null_BadRequest_Result()
        {
            // Arrange
            CategoriesController categoriesController = new CategoriesController(_categoryRepository.Object);

            // Act
            var result = categoriesController.ChangeImage(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Test]
        public void For_NewImage_Action_With_Index_Null_BadRequest_Result()
        {
            // Arrange
            var bitmap = SixLabors.ImageSharp.Image.Load("./w3c_home.bmp");

            var category = new Category();
            category.CategoryId = 1;
            category.Picture = ImageToByteArray(bitmap);

            var categories = new List<Category>
            {
                category
            };

            _categoryRepository.Setup(x => x.GetCategories())
                .Returns(categories);

            CategoriesController categoriesController = new CategoriesController(_categoryRepository.Object);

            ImageFileUpload imageFileUpload = new ImageFileUpload();

            // Act
            var result = categoriesController.NewImage(1, imageFileUpload);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        private byte[] ImageToByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, new BmpEncoder());
            return ms.ToArray();
        }
    }
}
