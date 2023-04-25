namespace WebDotNetMentoringProgram.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<string> GetCategoryNames();
        Category GetCategoryById(int? id);
        void UpdateCategoryById(Category category);
        Category GetCategoryByName(string name);
    }
}
