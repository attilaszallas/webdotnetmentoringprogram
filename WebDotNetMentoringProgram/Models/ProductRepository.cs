using WebDotNetMentoringProgram.Data;

namespace WebDotNetMentoringProgram.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebDotNetMentoringProgramContext _webDotNetMentoringProgramContext;

        public ProductRepository(WebDotNetMentoringProgramContext webDotNetMentoringProgramContext)
        {
            _webDotNetMentoringProgramContext = webDotNetMentoringProgramContext;
        }

        public IEnumerable<Product> GetProducts()
        {
            return (from product in _webDotNetMentoringProgramContext.Products
                    select product).ToList();
        }

        public IEnumerable<string> GetProductNames()
        {
            return (from product in _webDotNetMentoringProgramContext.Products
                    select product.ProductName).ToList();
        }

        public Product GetProductById(int? id)
        {
            return (from product in _webDotNetMentoringProgramContext.Products
                    where product.ProductID == id
                    select product).FirstOrDefault();
        }

        public Product GetProductByName(string name)
        {
            return (from product in _webDotNetMentoringProgramContext.Products
                    where product.ProductName == name
                    select product).FirstOrDefault();
        }

        public int GetProductCount()
        {
            return (from product in _webDotNetMentoringProgramContext.Products
                    select product).ToList().Count;
        }

        public IEnumerable<Product> GetProductsWithLimit(int limit)
        {
            return (from product in _webDotNetMentoringProgramContext.Products
                    select product).Take(limit).ToList();
        }

        public void Add(Product product)
        {
            _webDotNetMentoringProgramContext.Add(product);
            _webDotNetMentoringProgramContext.SaveChangesAsync();
        }

        public void Update(Product product)
        {
            _webDotNetMentoringProgramContext.Update(product);
            _webDotNetMentoringProgramContext.SaveChangesAsync();
        }

        public void Remove(Product product)
        {
            _webDotNetMentoringProgramContext.Remove(product);
            _webDotNetMentoringProgramContext.SaveChangesAsync();
        }
    }
}
