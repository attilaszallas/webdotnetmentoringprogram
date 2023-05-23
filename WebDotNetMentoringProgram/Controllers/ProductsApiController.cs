using Microsoft.AspNetCore.Mvc;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgramApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsApiController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            var _products = _productRepository.GetProducts();

            IEnumerable<string> productNames =
                from product in _products
                select product.ProductName;

            return Ok(productNames);
        }

        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            _productRepository.Add(product);

            return Ok();
        }
        
        [HttpPost("UpdateProduct")]
        public IActionResult UpdateProduct(Product product)
        {
            if (product == null) 
            {
                return BadRequest();
            }

            _productRepository.Update(product);

            return Ok();
        }
 
        [HttpPost("DeleteProduct")]
        public IActionResult DeleteProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            _productRepository.Remove(product);

            return Ok();
        }
    }
}
