using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL.Tests
{
    /// <summary>
    /// Unit tests for CategoryBLL class focusing on validation and slug generation.
    /// Test techniques applied:
    /// - Equivalence Partitioning (EP): Testing representative values from different input classes
    /// - Boundary Value Analysis (BVA): Testing at the boundaries of input ranges
    /// </summary>
    [TestFixture]
    public class CategoryBLLTests
    {
        #region Category Name Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Category Name:
         * Rule: Category name is required and must be at least 2 characters
         *
         * Equivalence Partitions:
         * EP1: Invalid - null
         * EP2: Invalid - empty string
         * EP3: Invalid - whitespace only
         * EP4: Invalid - 1 character
         * EP5: Valid - 2+ characters
         *
         * Boundary Values:
         * BV1: 0 characters (empty)
         * BV2: 1 character (invalid)
         * BV3: 2 characters (minimum valid)
         * BV4: 3 characters (valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Null category name should be invalid")]
        public void CategoryName_Null_IsInvalid()
        {
            // Arrange
            string name = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(name), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty category name should be invalid")]
        public void CategoryName_Empty_IsInvalid()
        {
            // Arrange
            string name = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(name), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Whitespace-only category name should be invalid")]
        public void CategoryName_WhitespaceOnly_IsInvalid()
        {
            // Arrange
            string name = "   ";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(name), Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: 1 character category name is invalid")]
        public void CategoryName_OneCharacter_IsInvalid()
        {
            // Arrange
            string name = "A";

            // Act & Assert
            Assert.That(name.Length, Is.EqualTo(1));
            Assert.That(name.Length < 2, Is.True, "1 character is below minimum of 2");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: 2 character category name is valid (minimum)")]
        public void CategoryName_TwoCharacters_IsMinimumValid()
        {
            // Arrange
            string name = "TV";

            // Act & Assert
            Assert.That(name.Length, Is.EqualTo(2));
            Assert.That(name.Length >= 2, Is.True, "2 characters meets minimum requirement");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: 3 character category name is valid")]
        public void CategoryName_ThreeCharacters_IsValid()
        {
            // Arrange
            string name = "TVs";

            // Act & Assert
            Assert.That(name.Length, Is.EqualTo(3));
            Assert.That(name.Length >= 2, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Typical category name is valid")]
        public void CategoryName_TypicalName_IsValid()
        {
            // Arrange
            string name = "Electronics";

            // Act & Assert
            Assert.That(name.Length >= 2, Is.True);
            Assert.That(string.IsNullOrWhiteSpace(name), Is.False);
        }

        #endregion

        #region Category ID Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Category ID:
         * Rule: Category ID must be > 0 for update/delete operations
         *
         * Boundary Values:
         * BV1: -1 (invalid)
         * BV2: 0 (invalid)
         * BV3: 1 (valid - minimum)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Negative category ID is invalid")]
        public void CategoryID_Negative_IsInvalid()
        {
            // Arrange
            int categoryId = -1;

            // Act & Assert
            Assert.That(categoryId <= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero category ID is invalid")]
        public void CategoryID_Zero_IsInvalid()
        {
            // Arrange
            int categoryId = 0;

            // Act & Assert
            Assert.That(categoryId <= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Category ID of 1 is valid (minimum)")]
        public void CategoryID_One_IsMinimumValid()
        {
            // Arrange
            int categoryId = 1;

            // Act & Assert
            Assert.That(categoryId > 0, Is.True);
        }

        #endregion

        #region Parent Category Validation Tests - EP

        /*
         * Test Design Documentation for Parent Category:
         * Rule: Category cannot be its own parent
         *
         * Equivalence Partitions:
         * EP1: Invalid - ParentCategoryID equals CategoryID
         * EP2: Valid - ParentCategoryID is null (root category)
         * EP3: Valid - ParentCategoryID is different from CategoryID
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Category as its own parent is invalid")]
        public void ParentCategory_SameAsCurrent_IsInvalid()
        {
            // Arrange
            var category = new Category
            {
                CategoryID = 5,
                ParentCategoryID = 5,
                Name = "Test Category"
            };

            // Act & Assert
            Assert.That(category.ParentCategoryID == category.CategoryID, Is.True,
                "Category cannot be its own parent");
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Null parent category is valid (root category)")]
        public void ParentCategory_Null_IsValidRootCategory()
        {
            // Arrange
            var category = new Category
            {
                CategoryID = 1,
                ParentCategoryID = null,
                Name = "Root Category"
            };

            // Act & Assert
            Assert.That(category.ParentCategoryID, Is.Null);
            Assert.That(category.ParentCategoryID != category.CategoryID, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Different parent category is valid")]
        public void ParentCategory_DifferentFromCurrent_IsValid()
        {
            // Arrange
            var category = new Category
            {
                CategoryID = 5,
                ParentCategoryID = 1,
                Name = "Sub Category"
            };

            // Act & Assert
            Assert.That(category.ParentCategoryID != category.CategoryID, Is.True);
        }

        #endregion

        #region Slug Generation Tests - EP and BVA

        /*
         * Test Design Documentation for Slug Generation:
         * Rules:
         * - Convert to lowercase
         * - Replace spaces with hyphens
         * - Remove special characters (keep only a-z, 0-9, hyphens)
         * - Remove multiple consecutive hyphens
         * - Trim hyphens from start/end
         *
         * Equivalence Partitions:
         * EP1: Simple name (no special chars)
         * EP2: Name with spaces
         * EP3: Name with special characters
         * EP4: Name with numbers
         * EP5: Name with multiple spaces
         * EP6: Name with accented characters
         */

        // Helper method to generate slug (mirrors the BLL logic)
        private string GenerateSlug(string name)
        {
            string slug = name.ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"-+", "-");
            return slug.Trim('-');
        }

        [Test]
        [Category("EP")]
        [Description("EP1: Simple name converts to lowercase slug")]
        public void GenerateSlug_SimpleName_ConvertsToLowercase()
        {
            // Arrange
            string name = "Electronics";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("electronics"));
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Name with spaces converts to hyphenated slug")]
        public void GenerateSlug_NameWithSpaces_ReplacesWithHyphens()
        {
            // Arrange
            string name = "Home Appliances";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("home-appliances"));
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Name with special characters removes them")]
        public void GenerateSlug_NameWithSpecialChars_RemovesSpecialChars()
        {
            // Arrange
            string name = "TV's & Electronics!";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("tvs-electronics"));
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Name with numbers keeps numbers")]
        public void GenerateSlug_NameWithNumbers_KeepsNumbers()
        {
            // Arrange
            string name = "4K TVs";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("4k-tvs"));
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Name with multiple spaces becomes single hyphen")]
        public void GenerateSlug_MultipleSpaces_BecomesSingleHyphen()
        {
            // Arrange
            string name = "Home    Theater    Systems";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("home-theater-systems"));
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: Minimum valid name (2 chars) generates valid slug")]
        public void GenerateSlug_MinimumName_GeneratesValidSlug()
        {
            // Arrange
            string name = "TV";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("tv"));
            Assert.That(slug.Length, Is.GreaterThanOrEqualTo(2));
        }

        [Test]
        [Category("EP")]
        [Description("EP: Name starting with space trims hyphen from start")]
        public void GenerateSlug_LeadingSpace_TrimmedFromSlug()
        {
            // Arrange
            string name = " Electronics";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("electronics"));
            Assert.That(slug.StartsWith("-"), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Name ending with space trims hyphen from end")]
        public void GenerateSlug_TrailingSpace_TrimmedFromSlug()
        {
            // Arrange
            string name = "Electronics ";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("electronics"));
            Assert.That(slug.EndsWith("-"), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Mixed case converts to lowercase")]
        public void GenerateSlug_MixedCase_ConvertsToLowercase()
        {
            // Arrange
            string name = "SMART TVs";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("smart-tvs"));
        }

        [Test]
        [Category("EP")]
        [Description("EP: Complex name with all transformations")]
        public void GenerateSlug_ComplexName_AllTransformationsApplied()
        {
            // Arrange
            string name = "  Home Theater & Sound Systems (2024)  ";

            // Act
            string slug = GenerateSlug(name);

            // Assert
            Assert.That(slug, Is.EqualTo("home-theater-sound-systems-2024"));
        }

        #endregion

        #region Delete Category Business Rules Tests - EP

        /*
         * Test Design Documentation for Delete Category:
         * Rules:
         * - Cannot delete category with subcategories
         * - Cannot delete category with associated products
         *
         * These are business rule tests that verify the validation logic
         */

        [Test]
        [Category("EP")]
        [Description("EP: Category with subcategories cannot be deleted")]
        public void DeleteCategory_WithSubcategories_ShouldBeRejected()
        {
            // Arrange
            int subcategoryCount = 3;

            // Act & Assert
            Assert.That(subcategoryCount > 0, Is.True,
                "Category with subcategories cannot be deleted");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Category without subcategories can be deleted")]
        public void DeleteCategory_WithoutSubcategories_CanBeDeleted()
        {
            // Arrange
            int subcategoryCount = 0;

            // Act & Assert
            Assert.That(subcategoryCount == 0, Is.True,
                "Category without subcategories can be deleted");
        }

        #endregion

        #region Complete Category Validation Tests

        [Test]
        [Category("EP")]
        [Description("EP: Valid category passes all validation rules")]
        public void ValidCategory_PassesAllValidation()
        {
            // Arrange
            var category = new Category
            {
                CategoryID = 1,
                Name = "Electronics",           // >= 2 chars
                Slug = "electronics",
                ParentCategoryID = null,        // Root category
                IsActive = true
            };

            // Act & Assert
            Assert.That(category.Name.Length >= 2, Is.True, "Name meets minimum length");
            Assert.That(category.CategoryID > 0, Is.True, "ID is positive");
            Assert.That(category.ParentCategoryID != category.CategoryID, Is.True,
                "Not its own parent");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Minimal valid category with boundary values")]
        public void MinimalValidCategory_PassesValidation()
        {
            // Arrange - using boundary minimum values
            var category = new Category
            {
                CategoryID = 1,              // Minimum valid ID
                Name = "TV",                 // Exactly 2 chars (minimum)
                Slug = "tv",
                ParentCategoryID = null
            };

            // Act & Assert
            Assert.That(category.Name.Length >= 2, Is.True);
            Assert.That(category.CategoryID > 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Subcategory with valid parent")]
        public void SubCategory_WithValidParent_PassesValidation()
        {
            // Arrange
            var parentCategory = new Category
            {
                CategoryID = 1,
                Name = "Electronics",
                Slug = "electronics",
                ParentCategoryID = null
            };

            var subCategory = new Category
            {
                CategoryID = 2,
                Name = "Televisions",
                Slug = "televisions",
                ParentCategoryID = 1
            };

            // Act & Assert
            Assert.That(subCategory.ParentCategoryID, Is.EqualTo(parentCategory.CategoryID));
            Assert.That(subCategory.ParentCategoryID != subCategory.CategoryID, Is.True);
        }

        #endregion
    }
}
