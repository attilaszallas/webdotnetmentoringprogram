using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Abstractions
{
    // all interfaces and abstract class should be placed in folder abstractions 
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<string> GetCategoryNames();
        Category GetCategoryById(int? id);
        void UpdateCategoryById(Category category);
        Category GetCategoryByName(string name);
    }
}
