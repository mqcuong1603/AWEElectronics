using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AWEElectronics.DAL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL
{
    public class CategoryBLL
    {
        private readonly CategoryDAL _categoryDAL;

        public CategoryBLL()
        {
            _categoryDAL = new CategoryDAL();
        }

        public List<Category> GetAll()
        {
            return _categoryDAL.GetAll();
        }

        public List<Category> GetParentCategories()
        {
            return _categoryDAL.GetParentCategories();
        }

        public List<Category> GetSubCategories(int parentId)
        {
            return _categoryDAL.GetSubCategories(parentId);
        }

        public Category GetById(int categoryId)
        {
            return _categoryDAL.GetById(categoryId);
        }

        public (bool Success, string Message, int CategoryId) CreateCategory(Category category)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(category.Name))
                return (false, "Category name is required.", 0);

            if (category.Name.Length < 2)
                return (false, "Category name must be at least 2 characters.", 0);

            // Generate slug if not provided
            if (string.IsNullOrWhiteSpace(category.Slug))
            {
                category.Slug = GenerateSlug(category.Name);
            }

            try
            {
                int categoryId = _categoryDAL.Insert(category);
                return (true, "Category created successfully.", categoryId);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE"))
                    return (false, "Category name or slug already exists.", 0);
                return (false, $"Error creating category: {ex.Message}", 0);
            }
        }

        public (bool Success, string Message) UpdateCategory(Category category)
        {
            if (category.CategoryID <= 0)
                return (false, "Invalid category ID.");

            if (string.IsNullOrWhiteSpace(category.Name))
                return (false, "Category name is required.");

            // Cannot set parent to itself
            if (category.ParentCategoryID == category.CategoryID)
                return (false, "Category cannot be its own parent.");

            try
            {
                bool result = _categoryDAL.Update(category);
                return result ? (true, "Category updated successfully.") : (false, "Failed to update category.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating category: {ex.Message}");
            }
        }

        public (bool Success, string Message) DeleteCategory(int categoryId)
        {
            if (categoryId <= 0)
                return (false, "Invalid category ID.");

            // Check if category has subcategories
            var subCategories = _categoryDAL.GetSubCategories(categoryId);
            if (subCategories.Count > 0)
                return (false, "Cannot delete category with subcategories.");

            try
            {
                bool result = _categoryDAL.Delete(categoryId);
                return result ? (true, "Category deleted successfully.") : (false, "Failed to delete category.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                    return (false, "Cannot delete category with associated products.");
                return (false, $"Error deleting category: {ex.Message}");
            }
        }

        private string GenerateSlug(string name)
        {
            string slug = name.ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"-+", "-");
            return slug.Trim('-');
        }
    }
}
