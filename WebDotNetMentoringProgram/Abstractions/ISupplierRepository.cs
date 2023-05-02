using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Abstractions
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> GetSuppliers();
        IEnumerable<string> GetSupplierNames();
        Supplier GetSupplierById(int? id);
        Supplier GetSupplierByName(string name);
    }
}
