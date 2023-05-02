using Microsoft.AspNetCore.Mvc;
using Moq;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Controllers;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.ViewModels;
using WebDotNetMentoringProgramTest.Mocks;

namespace WebDotNetMentoringProgramTest
{
    public class ProductTests
    {
        [Fact]
        public void Index_RequestOneProduct_ViewResult_Test()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();

            ProductsController productsController = new ProductsController(productRepositoryMock.Object, categoryRepositoryMock.Object, supplierRepositoryMock.Object);

            // Act
            var result = productsController.Index(1).Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Details_IndexOutOfRangeInput_NotFoundResult_Test()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();

            ProductsController productsController = new ProductsController(productRepositoryMock.Object, categoryRepositoryMock.Object, supplierRepositoryMock.Object);

            // Act
            var result = productsController.Details(0).Result;

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_IndexInRangeInput_ViewResult_Test()
        {
            // Arrange
            var productRepositoryMock = new ProductRepositoryMock();
            var categoryRepositoryMock = new CategoryRepositoryMock();
            var supplierRepositoryMock = new SupplierRepositoryMock();

            ProductsController productsController = new ProductsController(productRepositoryMock, categoryRepositoryMock, supplierRepositoryMock);

            // Act
            var result = productsController.Details(1).Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_NewProduct_ValidModel_Test()
        {
            // Arrange
            var productRepositoryMock = new ProductRepositoryMock();
            var categoryRepositoryMock = new CategoryRepositoryMock();
            var supplierRepositoryMock = new SupplierRepositoryMock();

            ProductsController productsController = new ProductsController(productRepositoryMock, categoryRepositoryMock, supplierRepositoryMock);

            // Act
            var result = productsController.Create(new Product(1, "TestProduct", 1, 2, "", decimal.One, 1, 0, 0, false)).Result;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Edit_ProductId1_ViewModel_Test()
        {
            // Arrange
            var productRepositoryMock = new ProductRepositoryMock();
            var categoryRepositoryMock = new CategoryRepositoryMock();
            var supplierRepositoryMock = new SupplierRepositoryMock();

            ProductsController productsController = new ProductsController(productRepositoryMock, categoryRepositoryMock, supplierRepositoryMock);

            // Act
            var result = productsController.Edit(1).Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_Product_ViewModel_Test()
        {
            // Arrange
            var productRepositoryMock = new ProductRepositoryMock();
            var categoryRepositoryMock = new CategoryRepositoryMock();
            var supplierRepositoryMock = new SupplierRepositoryMock();

            ProductsController productsController = new ProductsController(productRepositoryMock, categoryRepositoryMock, supplierRepositoryMock);

            ProductTableViewModel productTableViewModel = new ProductTableViewModel();

            productTableViewModel.ProductID = 1;

            // Act
            var result = productsController.Edit(1, productTableViewModel).Result;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
