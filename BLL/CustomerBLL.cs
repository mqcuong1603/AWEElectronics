using System;
using System.Collections.Generic;
using AWEElectronics.DAL;
using AWEElectronics.DTO;
using BCrypt.Net;

namespace AWEElectronics.BLL
{
    public class CustomerBLL
    {
        private readonly CustomerDAL _customerDAL;

        public CustomerBLL()
        {
            _customerDAL = new CustomerDAL();
        }

        public LoginResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Email and password are required."
                };
            }

            email = email.Trim();

            Customer customer = _customerDAL.GetByEmail(email);

            if (customer == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid email or password. Please check your credentials and try again."
                };
            }

            if (!customer.IsActive)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Account is inactive. Please contact support."
                };
            }

            // Verify password using BCrypt
            if (string.IsNullOrEmpty(customer.PasswordHash) || !BCrypt.Net.BCrypt.Verify(password, customer.PasswordHash))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid email or password. Please check your credentials and try again."
                };
            }

            return new LoginResult
            {
                Success = true,
                Message = "Login successful.",
                Customer = customer
            };
        }

        public List<Customer> GetAll()
        {
            return _customerDAL.GetAll();
        }

        public Customer GetById(int customerId)
        {
            return _customerDAL.GetById(customerId);
        }

        public Customer GetByEmail(string email)
        {
            return _customerDAL.GetByEmail(email);
        }

        public (bool Success, string Message, int CustomerId) CreateCustomer(Customer customer, string password)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(customer.Email))
                return (false, "Email is required.", 0);

            if (!IsValidEmail(customer.Email))
                return (false, "Invalid email format.", 0);

            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password is required.", 0);

            if (password.Length < 6)
                return (false, "Password must be at least 6 characters.", 0);

            if (string.IsNullOrWhiteSpace(customer.FullName))
                return (false, "Full name is required.", 0);

            if (string.IsNullOrWhiteSpace(customer.Phone))
                return (false, "Phone number is required.", 0);

            // Check if email already exists
            Customer existingCustomer = _customerDAL.GetByEmail(customer.Email);
            if (existingCustomer != null)
                return (false, "Email already exists.", 0);

            // Hash password using BCrypt
            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            customer.IsActive = true;

            try
            {
                int customerId = _customerDAL.Insert(customer);
                return (true, "Customer registered successfully.", customerId);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating customer: {ex.Message}", 0);
            }
        }

        public (bool Success, string Message) UpdateCustomer(Customer customer)
        {
            if (customer.CustomerID <= 0)
                return (false, "Invalid customer ID.");

            if (string.IsNullOrWhiteSpace(customer.FullName))
                return (false, "Full name is required.");

            if (string.IsNullOrWhiteSpace(customer.Email))
                return (false, "Email is required.");

            if (!IsValidEmail(customer.Email))
                return (false, "Invalid email format.");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                return (false, "Phone number is required.");

            try
            {
                bool result = _customerDAL.Update(customer);
                return result ? (true, "Customer updated successfully.") : (false, "Failed to update customer.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating customer: {ex.Message}");
            }
        }

        public (bool Success, string Message) ChangePassword(int customerId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
                return (false, "Current and new passwords are required.");

            if (newPassword.Length < 6)
                return (false, "New password must be at least 6 characters.");

            Customer customer = _customerDAL.GetById(customerId);
            if (customer == null)
                return (false, "Customer not found.");

            // Verify current password using BCrypt
            if (string.IsNullOrEmpty(customer.PasswordHash) || !BCrypt.Net.BCrypt.Verify(currentPassword, customer.PasswordHash))
                return (false, "Current password is incorrect.");

            try
            {
                string newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                bool result = _customerDAL.UpdatePassword(customerId, newHash);
                return result ? (true, "Password changed successfully.") : (false, "Failed to change password.");
            }
            catch (Exception ex)
            {
                return (false, $"Error changing password: {ex.Message}");
            }
        }

        public (bool Success, string Message) DeleteCustomer(int customerId)
        {
            if (customerId <= 0)
                return (false, "Invalid customer ID.");

            try
            {
                bool result = _customerDAL.Delete(customerId);
                return result ? (true, "Customer deleted successfully.") : (false, "Failed to delete customer.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting customer: {ex.Message}");
            }
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
