using Microsoft.AspNetCore.Mvc.Rendering;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.ViewModels
{
    public class EditTableViewModel
    {
        public ProductTableViewModel? Product { get; set; }
        public List<SelectListItem>? SupplierList { get; set; }
        public List<SelectListItem>? CategoryList { get; set; }
    }
}
