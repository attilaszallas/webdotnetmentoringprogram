using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Controllers;
using WebDotNetMentoringProgram.Models;
using Assert = Xunit.Assert;

namespace WebDotNetMentoringProgramTest
{
    [TestFixture]
    public class ProductTests
    {
        private Mock<IProductRepository> _productRepository;
        private Mock<ICategoryRepository> _categoryRepository;
        private Mock<ISupplierRepository> _supplierRepository;
        private Mock<IProductTableViewModel> _productTableViewModel;

        [OneTimeSetUp]
        public void Init()
        {
            _productRepository = new Mock<IProductRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _supplierRepository = new Mock<ISupplierRepository>();
            _productTableViewModel = new Mock<IProductTableViewModel>();
        }

        [Test]
        public void For_Index_Action_Request_A_Product_ViewResult_Result()
        {            
            // Arrange
            ProductsController productsController = new ProductsController(_productRepository.Object, _categoryRepository.Object, _supplierRepository.Object);

            // Act
            var result = productsController.Index(1).Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Test]
        public void For_Details_Action_With_Null_Index_Input_BadRequestResult_Result()
        {
            // Arrange
            var product = new Product(1, "productName", 1, 1, "quantity", 1.0m, 1, 1, 1, false);

            _productRepository.Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns(product);

            _supplierRepository.Setup(x => x.GetSupplierById(It.IsAny<int>()))
                .Returns(new Supplier());

            _categoryRepository.Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .Returns(new Category());

            _productTableViewModel.Setup(x => x.ProductID).Returns(1);
            _productTableViewModel.Setup(x => x.CompanyName).Returns("companyName");

            ProductsController productsController = new ProductsController(_productRepository.Object, _categoryRepository.Object, _supplierRepository.Object);

            // Act
            var result = productsController.Details(null).Result;

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Test]
        public void For_Details_Action_With_Index_1_ViewResult_Result()
        {
            // Arrange
            var product = new Product(1, "productName", 1, 1, "quantity", 1.0m, 1, 1, 1, false);

            _productRepository.Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns(product);

            _supplierRepository.Setup(x => x.GetSupplierById(It.IsAny<int>()))
                .Returns(new Supplier());

            _categoryRepository.Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .Returns(new Category());

            _productTableViewModel.Setup(x => x.ProductID).Returns(1);
            _productTableViewModel.Setup(x => x.CompanyName).Returns("companyName");

            ProductsController productsController = new ProductsController(_productRepository.Object, _categoryRepository.Object, _supplierRepository.Object);

            // Act
            var result = productsController.Details(1).Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Test]
        public void For_Create_Action_New_Product_RedirectToAction_Result()
        {
            // Arrange
            ProductsController productsController = new ProductsController(_productRepository.Object, _categoryRepository.Object, _supplierRepository.Object);

            // Act
            var result = productsController.Create(new Product(1, "TestProduct", 1, 2, "", decimal.One, 1, 0, 0, false)).Result;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Test]
        public void For_Edit_Action_With_Product_Id_1_ViewModel_Result()
        {
            // Arrange
            var product = new Product(1, "productName", 1, 1, "quantity", 1.0m, 1, 1, 1, false);

            _productRepository.Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns(product);

            _supplierRepository.Setup(x => x.GetSupplierById(It.IsAny<int>()))
                .Returns(new Supplier());

            _categoryRepository.Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .Returns(new Category());

            ProductsController productsController = new ProductsController(_productRepository.Object, _categoryRepository.Object, _supplierRepository.Object);

            // Act
            var result = productsController.Edit(1).Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Test]
        public void For_Edit_Action_With_Product_RedirectToAction_Result()
        {
            // Arrange
            var product = new Product(1, "productName", 1, 1, "quantity", 1.0m, 1, 1, 1, false);
            var supplier = new Supplier();
            var category = new Category();

            _productRepository.Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns(product);

            _supplierRepository.Setup(x => x.GetSupplierByName(It.IsAny<string>()))
                .Returns(supplier);

            _categoryRepository.Setup(x => x.GetCategoryByName(It.IsAny<string>()))
                .Returns(category);

            _productTableViewModel.Setup(x => x.ProductID).Returns(1);
            _productTableViewModel.Setup(x => x.CompanyName).Returns("companyName");

            ProductsController productsController = new ProductsController(_productRepository.Object, _categoryRepository.Object, _supplierRepository.Object);

            // Act
            var result = productsController.Edit(1, _productTableViewModel.Object).Result;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
