using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Data;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly WebDotNetMentoringProgramContext _webDotNetMentoringProgramContext;

        public SupplierRepository(WebDotNetMentoringProgramContext webDotNetMentoringProgramContext)
        {
            _webDotNetMentoringProgramContext = webDotNetMentoringProgramContext;
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            return (from supplier in _webDotNetMentoringProgramContext.Suppliers
                    select supplier).ToList();
        }

        public IEnumerable<string> GetSupplierNames()
        {
            return (from supplier in _webDotNetMentoringProgramContext.Suppliers
                    select supplier.CompanyName).ToList();
        }

        public Supplier GetSupplierById(int? id)
        {
            return (from supplier in _webDotNetMentoringProgramContext.Suppliers
                    where supplier.SupplierID == id
                    select supplier).FirstOrDefault();
        }

        public Supplier GetSupplierByName(string name)
        {
            return (from supplier in _webDotNetMentoringProgramContext.Suppliers
                    where supplier.CompanyName == name
                    select supplier).FirstOrDefault();
        }
    }
}
