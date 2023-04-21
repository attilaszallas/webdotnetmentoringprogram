using Microsoft.AspNetCore.Mvc;
using Moq;
using WebDotNetMentoringProgram.Controllers;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgramTest.Mocks;

namespace WebDotNetMentoringProgramTest
{
    public class ProductTests
    {
        [Fact]
        public void CreateNewProductTest()
        {
            // Arrange
            var mockCategoryRepo = new Mock<ICategoryRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockSupplierRepo = new Mock<ISupplierRepository>();

            ProductRepositoryMock productRepositoryMock = new ProductRepositoryMock();

            var controller = new ProductsController(productRepositoryMock, mockCategoryRepo.Object, mockSupplierRepo.Object);

            // Act
            Product productToAdd = new Product(223, "TestProduct", 1, 1, "1 box", 100, 1, 0, 0, false);

            var result = controller.Create(productToAdd);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
        }

        [Fact]
        public void EditExistingProductTest()
        {
            ProductRepositoryMock productRepositoryMock = new ProductRepositoryMock();

            // Edit Product id == 1
            Product productToModify = productRepositoryMock.GetProductById(1);


            Product productToAdd = new Product(223, "TestProduct", 1, 1, "1 box", 100, 1, 0, 0, false);

            productRepositoryMock.Add(productToAdd);

            Assert.True(productRepositoryMock.GetProductById(223).ProductName == "TestProduct");
        }
    }
}
