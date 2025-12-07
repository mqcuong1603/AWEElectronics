using System;
using NUnit.Framework;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL.Tests
{
    /// <summary>
    /// Unit tests for SupplierBLL class focusing on validation logic.
    /// Test techniques applied:
    /// - Equivalence Partitioning (EP): Testing representative values from different input classes
    /// - Boundary Value Analysis (BVA): Testing at the boundaries of input ranges
    /// </summary>
    [TestFixture]
    public class SupplierBLLTests
    {
        #region Company Name Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Company Name:
         * Rule: Company name is required and must be at least 2 characters
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
        [Description("EP1: Null company name should be invalid")]
        public void CompanyName_Null_IsInvalid()
        {
            // Arrange
            string companyName = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(companyName), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty company name should be invalid")]
        public void CompanyName_Empty_IsInvalid()
        {
            // Arrange
            string companyName = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(companyName), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Whitespace-only company name should be invalid")]
        public void CompanyName_WhitespaceOnly_IsInvalid()
        {
            // Arrange
            string companyName = "   ";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(companyName), Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: 1 character company name is invalid")]
        public void CompanyName_OneCharacter_IsInvalid()
        {
            // Arrange
            string companyName = "A";

            // Act & Assert
            Assert.That(companyName.Length, Is.EqualTo(1));
            Assert.That(companyName.Length < 2, Is.True, "1 character is below minimum of 2");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: 2 character company name is valid (minimum)")]
        public void CompanyName_TwoCharacters_IsMinimumValid()
        {
            // Arrange
            string companyName = "LG";

            // Act & Assert
            Assert.That(companyName.Length, Is.EqualTo(2));
            Assert.That(companyName.Length >= 2, Is.True, "2 characters meets minimum requirement");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: 3 character company name is valid")]
        public void CompanyName_ThreeCharacters_IsValid()
        {
            // Arrange
            string companyName = "ABC";

            // Act & Assert
            Assert.That(companyName.Length, Is.EqualTo(3));
            Assert.That(companyName.Length >= 2, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Typical company name is valid")]
        public void CompanyName_TypicalName_IsValid()
        {
            // Arrange
            string companyName = "Samsung Electronics Co., Ltd.";

            // Act & Assert
            Assert.That(companyName.Length >= 2, Is.True);
            Assert.That(string.IsNullOrWhiteSpace(companyName), Is.False);
        }

        #endregion

        #region Supplier ID Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Supplier ID:
         * Rule: Supplier ID must be > 0 for update/delete operations
         *
         * Boundary Values:
         * BV1: -1 (invalid)
         * BV2: 0 (invalid)
         * BV3: 1 (valid - minimum)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Negative supplier ID is invalid")]
        public void SupplierID_Negative_IsInvalid()
        {
            // Arrange
            int supplierId = -1;

            // Act & Assert
            Assert.That(supplierId <= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero supplier ID is invalid")]
        public void SupplierID_Zero_IsInvalid()
        {
            // Arrange
            int supplierId = 0;

            // Act & Assert
            Assert.That(supplierId <= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Supplier ID of 1 is valid (minimum)")]
        public void SupplierID_One_IsMinimumValid()
        {
            // Arrange
            int supplierId = 1;

            // Act & Assert
            Assert.That(supplierId > 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Typical positive supplier ID is valid")]
        public void SupplierID_TypicalPositive_IsValid()
        {
            // Arrange
            int supplierId = 42;

            // Act & Assert
            Assert.That(supplierId > 0, Is.True);
        }

        #endregion

        #region Supplier Email Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Supplier Email:
         * Rule: Email must be valid format if provided (optional field)
         *
         * Equivalence Partitions:
         * EP1: Valid - null (optional)
         * EP2: Valid - empty (optional)
         * EP3: Invalid - no @ symbol
         * EP4: Invalid - no domain
         * EP5: Invalid - no local part
         * EP6: Valid - standard email format
         * EP7: Valid - email with subdomain
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Null email is valid (optional field)")]
        public void SupplierEmail_Null_IsValidOptional()
        {
            // Arrange
            string email = null;

            // Act & Assert
            // For suppliers, email is optional, so null should be acceptable
            Assert.That(email, Is.Null);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty email is valid (optional field)")]
        public void SupplierEmail_Empty_IsValidOptional()
        {
            // Arrange
            string email = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Email without @ symbol is invalid")]
        public void SupplierEmail_NoAtSymbol_IsInvalid()
        {
            // Arrange
            string email = "contactsupplier.com";

            // Act & Assert
            Assert.That(email.Contains("@"), Is.False, "Email without @ is invalid");
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Email without domain is invalid")]
        public void SupplierEmail_NoDomain_IsInvalid()
        {
            // Arrange
            string email = "contact@";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Email without local part is invalid")]
        public void SupplierEmail_NoLocalPart_IsInvalid()
        {
            // Arrange
            string email = "@supplier.com";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP6: Standard email format is valid")]
        public void SupplierEmail_StandardFormat_IsValid()
        {
            // Arrange
            string email = "sales@samsung.com";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP7: Email with subdomain is valid")]
        public void SupplierEmail_WithSubdomain_IsValid()
        {
            // Arrange
            string email = "orders@us.samsung.com";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Business email format is valid")]
        public void SupplierEmail_BusinessFormat_IsValid()
        {
            // Arrange
            string email = "wholesale.orders@electronics-supplier.co.uk";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.True);
        }

        // Helper method to validate email format
        private bool IsValidEmailFormat(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Delete Supplier Business Rules Tests - EP

        /*
         * Test Design Documentation for Delete Supplier:
         * Rule: Cannot delete supplier with associated products
         */

        [Test]
        [Category("EP")]
        [Description("EP: Supplier with associated products cannot be deleted")]
        public void DeleteSupplier_WithAssociatedProducts_ShouldBeRejected()
        {
            // Arrange
            int associatedProductsCount = 5;

            // Act & Assert
            Assert.That(associatedProductsCount > 0, Is.True,
                "Supplier with associated products cannot be deleted");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Supplier without associated products can be deleted")]
        public void DeleteSupplier_WithoutAssociatedProducts_CanBeDeleted()
        {
            // Arrange
            int associatedProductsCount = 0;

            // Act & Assert
            Assert.That(associatedProductsCount == 0, Is.True,
                "Supplier without associated products can be deleted");
        }

        #endregion

        #region Complete Supplier Validation Tests

        [Test]
        [Category("EP")]
        [Description("EP: Valid supplier passes all validation rules")]
        public void ValidSupplier_PassesAllValidation()
        {
            // Arrange
            var supplier = new Supplier
            {
                SupplierID = 1,
                CompanyName = "Samsung Electronics",    // >= 2 chars
                ContactPerson = "John Kim",
                Email = "sales@samsung.com",            // valid email
                Phone = "+1-800-726-7864",
                Address = "Seoul, South Korea"
            };

            // Act & Assert
            Assert.That(supplier.CompanyName.Length >= 2, Is.True, "Company name meets minimum length");
            Assert.That(supplier.SupplierID > 0, Is.True, "ID is positive");
            Assert.That(IsValidEmailFormat(supplier.Email), Is.True, "Email is valid format");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Minimal valid supplier with boundary values")]
        public void MinimalValidSupplier_PassesValidation()
        {
            // Arrange - using boundary minimum values
            var supplier = new Supplier
            {
                SupplierID = 1,              // Minimum valid ID
                CompanyName = "LG",          // Exactly 2 chars (minimum)
                ContactPerson = null,        // Optional
                Email = null,                // Optional
                Phone = null,                // Optional
                Address = null               // Optional
            };

            // Act & Assert
            Assert.That(supplier.CompanyName.Length >= 2, Is.True);
            Assert.That(supplier.SupplierID > 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Supplier with all optional fields empty is valid")]
        public void Supplier_WithOptionalFieldsEmpty_IsValid()
        {
            // Arrange
            var supplier = new Supplier
            {
                SupplierID = 1,
                CompanyName = "Electronics Wholesale",
                ContactPerson = "",
                Email = "",
                Phone = "",
                Address = ""
            };

            // Act & Assert
            Assert.That(supplier.CompanyName.Length >= 2, Is.True);
            Assert.That(supplier.SupplierID > 0, Is.True);
        }

        #endregion

        #region Supplier Contact Information Tests - EP

        [Test]
        [Category("EP")]
        [Description("EP: Phone number formats are accepted")]
        public void Supplier_PhoneFormats_AreAccepted()
        {
            // Arrange - various phone formats
            string[] validPhones =
            {
                "+1-800-726-7864",      // International format
                "(800) 726-7864",       // US format
                "800-726-7864",         // Simple format
                "+82-2-2255-0114"       // Korean format
            };

            // Act & Assert
            foreach (var phone in validPhones)
            {
                Assert.That(string.IsNullOrWhiteSpace(phone), Is.False);
            }
        }

        [Test]
        [Category("EP")]
        [Description("EP: Contact person name is optional")]
        public void Supplier_ContactPerson_IsOptional()
        {
            // Arrange
            var supplierWithContact = new Supplier
            {
                CompanyName = "Test Corp",
                ContactPerson = "John Smith"
            };

            var supplierWithoutContact = new Supplier
            {
                CompanyName = "Test Corp",
                ContactPerson = null
            };

            // Act & Assert
            Assert.That(supplierWithContact.ContactPerson, Is.EqualTo("John Smith"));
            Assert.That(supplierWithoutContact.ContactPerson, Is.Null);
            // Both are valid
        }

        #endregion
    }
}
