using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgramTest.Mocks
{
    internal class ProductRepositoryMock : IProductRepository
    {
        private List<Product> _repository;

        public ProductRepositoryMock()
        {
            _repository = new List<Product>();

            Add(new Product(0, "Apple", 0, 0, "1 box", 16.0m, 13, 2, 1, false));
            Add(new Product(1, "Grape", 0, 0, "1 box", 16.0m, 13, 2, 1, false));
            Add(new Product(2, "Banana", 0, 0, "1 box", 16.0m, 13, 2, 1, false));
        }

        public void Add(Product product)
        {
            _repository.Add(product);
        }

        public Product GetProductById(int? id)
        {
            foreach (Product product in _repository)
            {
                if (product.ProductID == id)
                    return product;
            }

            throw new InvalidOperationException($"Product ID: {id} could not be found.");
        }

        public Product GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public int GetProductCount()
        {
            return _repository.Count;
        }

        public IEnumerable<string> GetProductNames()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProducts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProductsWithLimit(int limit)
        {
            throw new NotImplementedException();
        }

        public void Remove(Product product)
        {
        }

        public void Update(Product product)
        {
        }
    }
}
