using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Controllers;
using WebDotNetMentoringProgramTest.Mocks;

namespace WebDotNetMentoringProgramTest
{
    public class SupplierTests
    {
        [Fact]
        public void Index_ViewResult_Test()
        {
            // Arrange
            var supplierRepositoryMock = new SupplierRepositoryMock();

            SuppliersController suppliersController = new SuppliersController(supplierRepositoryMock);

            // Act
            var result = suppliersController.Index().Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
