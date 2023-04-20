namespace WebDotNetMentoringProgram.Models
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> GetSuppliers();
        IEnumerable<string> GetSupplierNames();
        Supplier GetSupplierById(int id);
        Supplier GetSupplierByName(string name);
    }
}
