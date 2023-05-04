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
    public class SupplierTests
    {
        private Mock<ISupplierRepository> _supplierRepository;

        [OneTimeSetUp]
        public void Init()
        {
            _supplierRepository = new Mock<ISupplierRepository>();
        }

        [Test]
        public void For_Index_Action_ViewResult_Test()
        {
            // Arrange
            var supplier = new Supplier();

            supplier.SupplierID = 0;

            _supplierRepository.Setup(x => x.GetSupplierById(It.IsAny<int>()))
                .Returns(supplier);

            SuppliersController suppliersController = new SuppliersController(_supplierRepository.Object);

            // Act
            var result = suppliersController.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
