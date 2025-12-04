using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using AWEElectronics.DAL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL
{
    public class UserBLL
    {
        private readonly UserDAL _userDAL;

        public UserBLL()
        {
            _userDAL = new UserDAL();
        }

        public LoginResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Username and password are required."
                };
            }

            // Trim whitespace from username
            username = username.Trim();

            User user = _userDAL.GetByUsername(username);

            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid username or password. Please check your credentials and try again."
                };
            }

            if (user.Status != "Active")
            {
                return new LoginResult
                {
                    Success = false,
                    Message = $"Account is {user.Status}. Please contact administrator."
                };
            }

            // Verify password (using simple hash for demo - use BCrypt in production)
            string hashedPassword = HashPassword(password);

            // For demo purposes, also check against the stored hash directly
            // In production, use proper password verification
            if (user.PasswordHash != hashedPassword && !VerifyPassword(password, user.PasswordHash))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid username or password. Please check your credentials and try again."
                };
            }

            return new LoginResult
            {
                Success = true,
                Message = "Login successful.",
                User = user
            };
        }

        public List<User> GetAll()
        {
            return _userDAL.GetAll();
        }

        public User GetById(int userId)
        {
            return _userDAL.GetById(userId);
        }

        public List<User> GetByRole(string role)
        {
            return _userDAL.GetByRole(role);
        }

        public (bool Success, string Message, int UserId) CreateUser(User user, string password)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(user.Username))
                return (false, "Username is required.", 0);

            if (user.Username.Length < 3)
                return (false, "Username must be at least 3 characters.", 0);

            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password is required.", 0);

            if (password.Length < 6)
                return (false, "Password must be at least 6 characters.", 0);

            if (string.IsNullOrWhiteSpace(user.Email))
                return (false, "Email is required.", 0);

            if (!IsValidEmail(user.Email))
                return (false, "Invalid email format.", 0);

            if (string.IsNullOrWhiteSpace(user.FullName))
                return (false, "Full name is required.", 0);

            // Check if username already exists
            User existingUser = _userDAL.GetByUsername(user.Username);
            if (existingUser != null)
                return (false, "Username already exists.", 0);

            // Hash password
            user.PasswordHash = HashPassword(password);
            user.Status = user.Status ?? "Active";

            try
            {
                int userId = _userDAL.Insert(user);
                return (true, "User created successfully.", userId);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating user: {ex.Message}", 0);
            }
        }

        public (bool Success, string Message) UpdateUser(User user)
        {
            if (user.UserID <= 0)
                return (false, "Invalid user ID.");

            if (string.IsNullOrWhiteSpace(user.FullName))
                return (false, "Full name is required.");

            if (string.IsNullOrWhiteSpace(user.Email))
                return (false, "Email is required.");

            if (!IsValidEmail(user.Email))
                return (false, "Invalid email format.");

            try
            {
                bool result = _userDAL.Update(user);
                return result ? (true, "User updated successfully.") : (false, "Failed to update user.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating user: {ex.Message}");
            }
        }

        public (bool Success, string Message) ChangePassword(int userId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
                return (false, "Current and new passwords are required.");

            if (newPassword.Length < 6)
                return (false, "New password must be at least 6 characters.");

            User user = _userDAL.GetById(userId);
            if (user == null)
                return (false, "User not found.");

            // Verify current password
            if (!VerifyPassword(currentPassword, user.PasswordHash))
                return (false, "Current password is incorrect.");

            try
            {
                string newHash = HashPassword(newPassword);
                bool result = _userDAL.UpdatePassword(userId, newHash);
                return result ? (true, "Password changed successfully.") : (false, "Failed to change password.");
            }
            catch (Exception ex)
            {
                return (false, $"Error changing password: {ex.Message}");
            }
        }

        public (bool Success, string Message) DeleteUser(int userId)
        {
            if (userId <= 0)
                return (false, "Invalid user ID.");

            try
            {
                bool result = _userDAL.Delete(userId);
                return result ? (true, "User deleted successfully.") : (false, "Failed to delete user.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting user: {ex.Message}");
            }
        }

        // Helper methods
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            string hash = HashPassword(password);
            return hash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsValidEmail(string email)
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
    }
}
