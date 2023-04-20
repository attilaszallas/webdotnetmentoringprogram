using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDotNetMentoringProgram.Models
{
	public class Category
	{
        [Key]
        public int CategoryId { get; set; }
		public string? CategoryName { get; set; }
		public string? Description { get; set; }
		[NotMapped]
		public object? Picture { get; set; }
	}
}
