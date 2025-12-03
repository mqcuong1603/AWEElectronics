using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class CategoryDAL
    {
        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();
            string query = @"SELECT c.*, p.Name as ParentCategoryName
                            FROM Categories c
                            LEFT JOIN Categories p ON c.ParentCategoryID = p.CategoryID
                            ORDER BY c.Name";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                categories.Add(MapCategory(row));
            }
            return categories;
        }

        public List<Category> GetParentCategories()
        {
            List<Category> categories = new List<Category>();
            string query = "SELECT * FROM Categories WHERE ParentCategoryID IS NULL ORDER BY Name";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                categories.Add(MapCategory(row));
            }
            return categories;
        }

        public List<Category> GetSubCategories(int parentId)
        {
            List<Category> categories = new List<Category>();
            string query = "SELECT * FROM Categories WHERE ParentCategoryID = @ParentID ORDER BY Name";
            SqlParameter[] parameters = { new SqlParameter("@ParentID", parentId) };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                categories.Add(MapCategory(row));
            }
            return categories;
        }

        public Category GetById(int categoryId)
        {
            string query = @"SELECT c.*, p.Name as ParentCategoryName
                            FROM Categories c
                            LEFT JOIN Categories p ON c.ParentCategoryID = p.CategoryID
                            WHERE c.CategoryID = @CategoryID";
            SqlParameter[] parameters = { new SqlParameter("@CategoryID", categoryId) };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapCategory(dt.Rows[0]);

            return null;
        }

        public int Insert(Category category)
        {
            string query = @"INSERT INTO Categories (Name, ParentCategoryID, Slug)
                            VALUES (@Name, @ParentCategoryID, @Slug);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@Name", category.Name),
                new SqlParameter("@ParentCategoryID", (object)category.ParentCategoryID ?? DBNull.Value),
                new SqlParameter("@Slug", category.Slug)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool Update(Category category)
        {
            string query = @"UPDATE Categories SET
                            Name = @Name,
                            ParentCategoryID = @ParentCategoryID,
                            Slug = @Slug
                            WHERE CategoryID = @CategoryID";

            SqlParameter[] parameters = {
                new SqlParameter("@CategoryID", category.CategoryID),
                new SqlParameter("@Name", category.Name),
                new SqlParameter("@ParentCategoryID", (object)category.ParentCategoryID ?? DBNull.Value),
                new SqlParameter("@Slug", category.Slug)
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int categoryId)
        {
            string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
            SqlParameter[] parameters = { new SqlParameter("@CategoryID", categoryId) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private Category MapCategory(DataRow row)
        {
            return new Category
            {
                CategoryID = Convert.ToInt32(row["CategoryID"]),
                Name = row["Name"].ToString(),
                ParentCategoryID = row["ParentCategoryID"] != DBNull.Value ? Convert.ToInt32(row["ParentCategoryID"]) : (int?)null,
                Slug = row["Slug"].ToString(),
                ParentCategoryName = row.Table.Columns.Contains("ParentCategoryName") ? row["ParentCategoryName"]?.ToString() : null
            };
        }
    }
}
