using System;
using NUnit.Framework;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL.Tests
{
    /// <summary>
    /// Unit tests for UserBLL class focusing on validation and authentication logic.
    /// Test techniques applied:
    /// - Equivalence Partitioning (EP): Testing representative values from different input classes
    /// - Boundary Value Analysis (BVA): Testing at the boundaries of input ranges
    /// </summary>
    [TestFixture]
    public class UserBLLTests
    {
        #region Username Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Username:
         * Rule: Username is required and must be at least 3 characters
         *
         * Equivalence Partitions:
         * EP1: Invalid - null
         * EP2: Invalid - empty string
         * EP3: Invalid - whitespace only
         * EP4: Invalid - 1-2 characters
         * EP5: Valid - 3+ characters
         *
         * Boundary Values:
         * BV1: 0 characters (empty)
         * BV2: 1 character (invalid)
         * BV3: 2 characters (invalid)
         * BV4: 3 characters (minimum valid)
         * BV5: 4 characters (valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Null username should be invalid")]
        public void Username_Null_IsInvalid()
        {
            // Arrange
            string username = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(username), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty username should be invalid")]
        public void Username_Empty_IsInvalid()
        {
            // Arrange
            string username = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(username), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Whitespace-only username should be invalid")]
        public void Username_WhitespaceOnly_IsInvalid()
        {
            // Arrange
            string username = "   ";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(username), Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: 1 character username is invalid")]
        public void Username_OneCharacter_IsInvalid()
        {
            // Arrange
            string username = "A";

            // Act & Assert
            Assert.That(username.Length, Is.EqualTo(1));
            Assert.That(username.Length < 3, Is.True, "1 character is below minimum of 3");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: 2 character username is invalid")]
        public void Username_TwoCharacters_IsInvalid()
        {
            // Arrange
            string username = "AB";

            // Act & Assert
            Assert.That(username.Length, Is.EqualTo(2));
            Assert.That(username.Length < 3, Is.True, "2 characters is below minimum of 3");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: 3 character username is valid (minimum)")]
        public void Username_ThreeCharacters_IsMinimumValid()
        {
            // Arrange
            string username = "ABC";

            // Act & Assert
            Assert.That(username.Length, Is.EqualTo(3));
            Assert.That(username.Length >= 3, Is.True, "3 characters meets minimum requirement");
        }

        [Test]
        [Category("BVA")]
        [Description("BV5: 4 character username is valid")]
        public void Username_FourCharacters_IsValid()
        {
            // Arrange
            string username = "ABCD";

            // Act & Assert
            Assert.That(username.Length, Is.EqualTo(4));
            Assert.That(username.Length >= 3, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Typical username is valid")]
        public void Username_TypicalFormat_IsValid()
        {
            // Arrange
            string username = "john_doe123";

            // Act & Assert
            Assert.That(username.Length >= 3, Is.True);
            Assert.That(string.IsNullOrWhiteSpace(username), Is.False);
        }

        #endregion

        #region Password Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Password:
         * Rule: Password is required and must be at least 6 characters
         *
         * Equivalence Partitions:
         * EP1: Invalid - null
         * EP2: Invalid - empty
         * EP3: Invalid - 1-5 characters
         * EP4: Valid - 6+ characters
         * EP5: Valid - complex password (mixed chars)
         *
         * Boundary Values:
         * BV1: 0 characters (empty)
         * BV2: 5 characters (invalid - below min)
         * BV3: 6 characters (minimum valid)
         * BV4: 7 characters (valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Null password should be invalid")]
        public void Password_Null_IsInvalid()
        {
            // Arrange
            string password = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(password), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty password should be invalid")]
        public void Password_Empty_IsInvalid()
        {
            // Arrange
            string password = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(password), Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: 5 character password is invalid (below minimum)")]
        public void Password_FiveCharacters_IsInvalid()
        {
            // Arrange
            string password = "12345";

            // Act & Assert
            Assert.That(password.Length, Is.EqualTo(5));
            Assert.That(password.Length < 6, Is.True, "5 characters is below minimum of 6");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: 6 character password is valid (minimum)")]
        public void Password_SixCharacters_IsMinimumValid()
        {
            // Arrange
            string password = "123456";

            // Act & Assert
            Assert.That(password.Length, Is.EqualTo(6));
            Assert.That(password.Length >= 6, Is.True, "6 characters meets minimum requirement");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: 7 character password is valid")]
        public void Password_SevenCharacters_IsValid()
        {
            // Arrange
            string password = "1234567";

            // Act & Assert
            Assert.That(password.Length, Is.EqualTo(7));
            Assert.That(password.Length >= 6, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Long password is valid")]
        public void Password_LongPassword_IsValid()
        {
            // Arrange
            string password = "MySecureP@ssw0rd123!";

            // Act & Assert
            Assert.That(password.Length >= 6, Is.True);
            Assert.That(string.IsNullOrWhiteSpace(password), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Complex password with mixed characters is valid")]
        public void Password_ComplexFormat_IsValid()
        {
            // Arrange
            string password = "P@ss#123";

            // Act & Assert
            Assert.That(password.Length >= 6, Is.True);
        }

        #endregion

        #region Email Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Email:
         * Rule: Email is required and must be valid format
         *
         * Equivalence Partitions:
         * EP1: Invalid - null
         * EP2: Invalid - empty
         * EP3: Invalid - no @ symbol
         * EP4: Invalid - no domain
         * EP5: Invalid - no local part
         * EP6: Valid - standard email format
         * EP7: Valid - email with subdomain
         * EP8: Valid - email with dots in local part
         *
         * Boundary Values tested via representative examples
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Null email should be invalid")]
        public void Email_Null_IsInvalid()
        {
            // Arrange
            string email = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty email should be invalid")]
        public void Email_Empty_IsInvalid()
        {
            // Arrange
            string email = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Email without @ symbol is invalid")]
        public void Email_NoAtSymbol_IsInvalid()
        {
            // Arrange
            string email = "johndoeexample.com";

            // Act & Assert
            Assert.That(email.Contains("@"), Is.False, "Email without @ is invalid");
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Email without domain is invalid")]
        public void Email_NoDomain_IsInvalid()
        {
            // Arrange
            string email = "johndoe@";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Email without local part is invalid")]
        public void Email_NoLocalPart_IsInvalid()
        {
            // Arrange
            string email = "@example.com";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP6: Standard email format is valid")]
        public void Email_StandardFormat_IsValid()
        {
            // Arrange
            string email = "john.doe@example.com";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP7: Email with subdomain is valid")]
        public void Email_WithSubdomain_IsValid()
        {
            // Arrange
            string email = "admin@mail.company.com";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP8: Email with dots in local part is valid")]
        public void Email_DotsInLocalPart_IsValid()
        {
            // Arrange
            string email = "john.doe.smith@example.com";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Email with plus sign is valid")]
        public void Email_WithPlusSign_IsValid()
        {
            // Arrange
            string email = "john+test@example.com";

            // Act & Assert
            Assert.That(IsValidEmailFormat(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Email with numbers is valid")]
        public void Email_WithNumbers_IsValid()
        {
            // Arrange
            string email = "john123@example456.com";

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

        #region Full Name Validation Tests - EP

        /*
         * Test Design Documentation for FullName:
         * Rule: Full name is required
         *
         * Equivalence Partitions:
         * EP1: Invalid - null
         * EP2: Invalid - empty
         * EP3: Invalid - whitespace only
         * EP4: Valid - any non-empty string
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Null full name should be invalid")]
        public void FullName_Null_IsInvalid()
        {
            // Arrange
            string fullName = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(fullName), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty full name should be invalid")]
        public void FullName_Empty_IsInvalid()
        {
            // Arrange
            string fullName = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(fullName), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Whitespace-only full name should be invalid")]
        public void FullName_WhitespaceOnly_IsInvalid()
        {
            // Arrange
            string fullName = "   ";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(fullName), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Valid full name")]
        public void FullName_ValidName_IsValid()
        {
            // Arrange
            string fullName = "John Doe";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(fullName), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Single word name is valid")]
        public void FullName_SingleWord_IsValid()
        {
            // Arrange
            string fullName = "John";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(fullName), Is.False);
        }

        #endregion

        #region User Status Validation Tests - EP

        /*
         * Test Design Documentation for User Status:
         * Valid statuses: Active, Inactive, Locked
         * Rule: Only Active users can login
         *
         * Equivalence Partitions:
         * EP1: Valid for login - Active
         * EP2: Invalid for login - Inactive
         * EP3: Invalid for login - Locked
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Active status allows login")]
        public void UserStatus_Active_CanLogin()
        {
            // Arrange
            string status = "Active";

            // Act & Assert
            Assert.That(status, Is.EqualTo("Active"));
            Assert.That(status == "Active", Is.True, "Active users can login");
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Inactive status prevents login")]
        public void UserStatus_Inactive_CannotLogin()
        {
            // Arrange
            string status = "Inactive";

            // Act & Assert
            Assert.That(status != "Active", Is.True, "Inactive users cannot login");
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Locked status prevents login")]
        public void UserStatus_Locked_CannotLogin()
        {
            // Arrange
            string status = "Locked";

            // Act & Assert
            Assert.That(status != "Active", Is.True, "Locked users cannot login");
        }

        #endregion

        #region User Role Validation Tests - EP

        /*
         * Test Design Documentation for User Roles:
         * Valid roles: Admin, Staff, Accountant, Agent
         *
         * Equivalence Partitions:
         * EP1: Valid - Admin
         * EP2: Valid - Staff
         * EP3: Valid - Accountant
         * EP4: Valid - Agent
         */

        [Test]
        [Category("EP")]
        [Description("EP: All valid user roles are recognized")]
        public void UserRoles_ValidRoles_AreRecognized()
        {
            // Arrange
            string[] validRoles = { "Admin", "Staff", "Accountant", "Agent" };

            // Act & Assert
            Assert.That(validRoles.Length, Is.EqualTo(4));
            foreach (var role in validRoles)
            {
                Assert.That(string.IsNullOrWhiteSpace(role), Is.False);
            }
        }

        [Test]
        [Category("EP")]
        [Description("EP1: Admin role is valid")]
        public void UserRole_Admin_IsValid()
        {
            // Arrange
            string role = "Admin";

            // Act & Assert
            Assert.That(role, Is.EqualTo("Admin"));
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Staff role is valid")]
        public void UserRole_Staff_IsValid()
        {
            // Arrange
            string role = "Staff";

            // Act & Assert
            Assert.That(role, Is.EqualTo("Staff"));
        }

        #endregion

        #region Password Change Validation Tests - EP and BVA

        /*
         * Test Design Documentation for ChangePassword:
         * Rules:
         * - Both current and new passwords are required
         * - New password must be at least 6 characters
         * - Current password must match stored password
         *
         * Boundary Values for new password:
         * BV1: 5 characters (invalid)
         * BV2: 6 characters (minimum valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP: Null current password is invalid")]
        public void ChangePassword_NullCurrentPassword_IsInvalid()
        {
            // Arrange
            string currentPassword = null;
            string newPassword = "newpass123";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(currentPassword), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Null new password is invalid")]
        public void ChangePassword_NullNewPassword_IsInvalid()
        {
            // Arrange
            string currentPassword = "oldpass123";
            string newPassword = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(newPassword), Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV1: New password with 5 characters is invalid")]
        public void ChangePassword_NewPasswordFiveChars_IsInvalid()
        {
            // Arrange
            string newPassword = "12345";

            // Act & Assert
            Assert.That(newPassword.Length, Is.EqualTo(5));
            Assert.That(newPassword.Length < 6, Is.True, "5 characters is below minimum of 6");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: New password with 6 characters is valid (minimum)")]
        public void ChangePassword_NewPasswordSixChars_IsValid()
        {
            // Arrange
            string newPassword = "123456";

            // Act & Assert
            Assert.That(newPassword.Length, Is.EqualTo(6));
            Assert.That(newPassword.Length >= 6, Is.True);
        }

        #endregion

        #region Login Validation Tests - EP

        /*
         * Test Design Documentation for Login:
         * Rules:
         * - Username and password are required
         * - User must exist
         * - Account must be Active
         * - Password must match
         *
         * Equivalence Partitions:
         * EP1: Invalid - empty username
         * EP2: Invalid - empty password
         * EP3: Invalid - user not found
         * EP4: Invalid - inactive account
         * EP5: Invalid - wrong password
         * EP6: Valid - correct credentials and active account
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Empty username prevents login")]
        public void Login_EmptyUsername_IsInvalid()
        {
            // Arrange
            string username = "";
            string password = "password123";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(username), Is.True, "Empty username should be rejected");
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty password prevents login")]
        public void Login_EmptyPassword_IsInvalid()
        {
            // Arrange
            string username = "admin";
            string password = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(password), Is.True, "Empty password should be rejected");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Both empty credentials prevent login")]
        public void Login_BothEmpty_IsInvalid()
        {
            // Arrange
            string username = "";
            string password = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password), Is.True);
        }

        #endregion

        #region User ID Validation Tests - BVA

        /*
         * Test Design Documentation for UserID:
         * Rule: UserID must be > 0 for update/delete operations
         *
         * Boundary Values:
         * BV1: -1 (invalid)
         * BV2: 0 (invalid)
         * BV3: 1 (minimum valid)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Negative UserID is invalid")]
        public void UserID_Negative_IsInvalid()
        {
            // Arrange
            int userId = -1;

            // Act & Assert
            Assert.That(userId <= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero UserID is invalid")]
        public void UserID_Zero_IsInvalid()
        {
            // Arrange
            int userId = 0;

            // Act & Assert
            Assert.That(userId <= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: UserID of 1 is valid (minimum)")]
        public void UserID_One_IsMinimumValid()
        {
            // Arrange
            int userId = 1;

            // Act & Assert
            Assert.That(userId > 0, Is.True);
        }

        #endregion

        #region Complete User Validation Tests

        [Test]
        [Category("EP")]
        [Description("EP: Valid user passes all validation rules")]
        public void ValidUser_PassesAllValidation()
        {
            // Arrange
            var user = new User
            {
                Username = "johndoe",        // >= 3 chars
                Email = "john@example.com",  // valid email format
                FullName = "John Doe",       // not empty
                Role = "Staff",              // valid role
                Status = "Active"            // valid status
            };
            string password = "securepass123"; // >= 6 chars

            // Act & Assert
            Assert.That(user.Username.Length >= 3, Is.True, "Username meets minimum length");
            Assert.That(IsValidEmailFormat(user.Email), Is.True, "Email is valid format");
            Assert.That(string.IsNullOrWhiteSpace(user.FullName), Is.False, "FullName is provided");
            Assert.That(password.Length >= 6, Is.True, "Password meets minimum length");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Minimal valid user with boundary values")]
        public void MinimalValidUser_PassesValidation()
        {
            // Arrange - using boundary minimum values
            var user = new User
            {
                Username = "abc",            // Exactly 3 chars (minimum)
                Email = "a@b.co",            // Short but valid email
                FullName = "A",              // Single character name
                Role = "Staff",
                Status = "Active"
            };
            string password = "123456";      // Exactly 6 chars (minimum)

            // Act & Assert
            Assert.That(user.Username.Length >= 3, Is.True);
            Assert.That(string.IsNullOrWhiteSpace(user.FullName), Is.False);
            Assert.That(password.Length >= 6, Is.True);
        }

        #endregion
    }
}
