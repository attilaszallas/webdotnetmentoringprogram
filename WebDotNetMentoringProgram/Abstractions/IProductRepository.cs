using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Abstractions
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<string> GetProductNames();
        Product GetProductById(int? id);
        Product GetProductByName(string name);
        int GetProductCount();
        IEnumerable<Product> GetProductsWithLimit(int limit);
        void Add(Product product);
        void Update(Product product);
        void Remove(Product product);
    }
}
