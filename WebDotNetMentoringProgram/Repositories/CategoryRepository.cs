﻿using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Data;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly WebDotNetMentoringProgramContext _webDotNetMentoringProgramContext;

        public CategoryRepository(WebDotNetMentoringProgramContext webDotNetMentoringProgramContext)
        {
            _webDotNetMentoringProgramContext = webDotNetMentoringProgramContext;
        }

        public IEnumerable<Category> GetCategories()
        {
            return (from category in _webDotNetMentoringProgramContext.Categories
                    select category).ToList();
        }

        public IEnumerable<string> GetCategoryNames()
        {
            return (from category in _webDotNetMentoringProgramContext.Categories
                    select category.CategoryName).ToList();
        }

        public Category GetCategoryById(int? id)
        {
            return (from category in _webDotNetMentoringProgramContext.Categories
                    where category.CategoryId == id
                    select category).FirstOrDefault();
        }

        public void UpdateCategoryById(Category category)
        {
            _webDotNetMentoringProgramContext.Categories.Update(category);
            _webDotNetMentoringProgramContext.SaveChangesAsync();
        }

        public Category GetCategoryByName(string name)
        {
            return (from category in _webDotNetMentoringProgramContext.Categories
                    where category.CategoryName == name
                    select category).FirstOrDefault();
        }
    }
}
