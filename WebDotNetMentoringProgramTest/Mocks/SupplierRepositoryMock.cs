using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgramTest.Mocks
{
    internal class SupplierRepositoryMock : ISupplierRepository
    {
        public Supplier GetSupplierById(int? id)
        {
            var supplierMock = new Supplier();

            supplierMock.SupplierID = 0;

            return supplierMock;
        }

        public Supplier GetSupplierByName(string name)
        {
            var supplierMock = new Supplier();

            supplierMock.SupplierID = 1;
            supplierMock.CompanyName = name;

            return supplierMock;
        }

        public IEnumerable<string> GetSupplierNames()
        {
            return Enumerable.Empty<string>();
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            return Enumerable.Empty<Supplier>();
        }
    }
}
