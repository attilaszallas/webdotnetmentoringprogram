using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Controllers;
using WebDotNetMentoringProgram.ViewModels;
using WebDotNetMentoringProgramTest.Mocks;

namespace WebDotNetMentoringProgramTest
{
    public class CategoryTests
    {
        [Fact]
        public void Index_ViewResult_Test()
        {
            // Arrange
            var categoryRepositoryMock = new CategoryRepositoryMock();

            CategoriesController categoriesController = new CategoriesController(categoryRepositoryMock);

            // Act
            var result = categoriesController.Index().Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Image_CategoryRepositoryNull_Failure_Test()
        {
            // Arrange
            var categoryRepositoryMock = new CategoryRepositoryMock();

            CategoriesController categoriesController = new CategoriesController(null);

            // Act
            var result = categoriesController.Image(1).Result;

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public void ChangeImage_CategoryRepositoryNull_Failure_Test()
        {
            // Arrange
            var categoryRepositoryMock = new CategoryRepositoryMock();

            CategoriesController categoriesController = new CategoriesController(null);

            // Act
            var result = categoriesController.ChangeImage(1).Result;

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public void NewImage_CategoryRepositoryNull_Failure_Test()
        {
            // Arrange
            var categoryRepositoryMock = new CategoryRepositoryMock();

            CategoriesController categoriesController = new CategoriesController(null);

            ImageFileUpload imageFileUpload = new ImageFileUpload();

            // Act
            var result = categoriesController.NewImage(1, imageFileUpload).Result;

            // Assert
            Assert.IsType<ObjectResult>(result);
        }
    }
}
