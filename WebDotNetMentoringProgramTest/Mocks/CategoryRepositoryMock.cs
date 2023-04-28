using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgramTest.Mocks
{
    internal class CategoryRepositoryMock : ICategoryRepository
    {
        public IEnumerable<Category> GetCategories()
        {
            return Enumerable.Empty<Category>();
        }

        public Category GetCategoryById(int? id)
        {
            var categoryMock = new Category();

            categoryMock.CategoryId = 0;

            return categoryMock;
        }

        public Category GetCategoryByName(string name)
        {
            var categoryMock = new Category();

            categoryMock.CategoryId = 1;
            categoryMock.CategoryName = name;

            return categoryMock;
        }

        public IEnumerable<string> GetCategoryNames() 
        {
            return Enumerable.Empty<string>();
        }

        public void UpdateCategoryById(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
