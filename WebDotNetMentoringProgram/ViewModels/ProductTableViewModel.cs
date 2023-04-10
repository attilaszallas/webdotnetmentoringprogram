using System.ComponentModel.DataAnnotations;

namespace WebDotNetMentoringProgram.ViewModels
{
    public class ProductTableViewModel
	{
        [Required]
        [Range(0, 100)]
        public int ProductID { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "ProductName length should be 4 - 100 character long.")]
        public string? ProductName { get; set; }
        [Required]
        public string? CompanyName { get; set; }
        [Required]
        public string? CategoryName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "ProductName length should be 4 - 100 character long.")]
        public string? QuantityPerUnit { get; set; }
        [Required]
        [Range(1, 10000)]
        public decimal UnitPrice { get; set; }
        [Required]
        [Range(0, 10000, ErrorMessage = "UnitsInStock should be 0 - 10000.")]
        public short UnitsInStock { get; set; }
        [Required]
        public short UnitsOnOrder { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "ReorderLevel should be 0 - 100.")]
        public short ReorderLevel { get; set; }
        [Required]
        public bool Discontinued { get; set; }
    }
}
