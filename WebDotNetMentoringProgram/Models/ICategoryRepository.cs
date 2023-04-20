namespace WebDotNetMentoringProgram.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<string> GetCategoryNames();
        Category GetCategoryById(int? id);
        Category GetCategoryByName(string name);
    }
}
