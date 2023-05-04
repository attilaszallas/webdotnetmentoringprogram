using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Controllers;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.ViewModels;

namespace WebDotNetMentoringProgramTest
{
    [TestFixture]
    public class ProperProductTests
    {
        private Mock<IProductRepository> _productRepository;
        private Mock<ICategoryRepository> _categoryRepository;
        private Mock<ISupplierRepository> _supplierRepository;

        private ProductsController productsController;

        [OneTimeSetUp]
        public void Init()
        {
            _productRepository = new Mock<IProductRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _supplierRepository = new Mock<ISupplierRepository>();
        }

        [Test]
        public void For_Edit_Action_With_Id_Return_View_Result()
        {
            // Arrange
            var product = new Product(1, "productName", 1, 1, "qunatity", 1.0m, 1, 1, 1, false);

            _productRepository.Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns(product);

            _supplierRepository.Setup(x => x.GetSupplierById(It.IsAny<int>()))
                .Returns(new Supplier());

            _categoryRepository.Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .Returns(new Category());

            productsController = new ProductsController(_productRepository.Object, _categoryRepository.Object, _supplierRepository.Object);

            // Act
            var result = productsController.Edit(1);

            // Assert
            result.Should().BeOfType<ViewResult>();

            var resultViewModel = result as ViewResult;
            resultViewModel.Model.Should().NotBeNull();
            resultViewModel.Model.Should().BeOfType<ProductTableViewModel>();
        }
    }
}
