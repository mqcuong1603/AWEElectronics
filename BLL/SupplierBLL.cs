using System;
using System.Collections.Generic;
using AWEElectronics.DAL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL
{
    public class SupplierBLL
    {
        private readonly SupplierDAL _supplierDAL;

        public SupplierBLL()
        {
            _supplierDAL = new SupplierDAL();
        }

        public List<Supplier> GetAll()
        {
            return _supplierDAL.GetAll();
        }

        public Supplier GetById(int supplierId)
        {
            return _supplierDAL.GetById(supplierId);
        }

        public (bool Success, string Message, int SupplierId) CreateSupplier(Supplier supplier)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(supplier.CompanyName))
                return (false, "Company name is required.", 0);

            if (supplier.CompanyName.Length < 2)
                return (false, "Company name must be at least 2 characters.", 0);

            if (!string.IsNullOrWhiteSpace(supplier.Email) && !IsValidEmail(supplier.Email))
                return (false, "Invalid email format.", 0);

            try
            {
                int supplierId = _supplierDAL.Insert(supplier);
                return (true, "Supplier created successfully.", supplierId);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating supplier: {ex.Message}", 0);
            }
        }

        public (bool Success, string Message) UpdateSupplier(Supplier supplier)
        {
            if (supplier.SupplierID <= 0)
                return (false, "Invalid supplier ID.");

            if (string.IsNullOrWhiteSpace(supplier.CompanyName))
                return (false, "Company name is required.");

            if (!string.IsNullOrWhiteSpace(supplier.Email) && !IsValidEmail(supplier.Email))
                return (false, "Invalid email format.");

            try
            {
                bool result = _supplierDAL.Update(supplier);
                return result ? (true, "Supplier updated successfully.") : (false, "Failed to update supplier.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating supplier: {ex.Message}");
            }
        }

        public (bool Success, string Message) DeleteSupplier(int supplierId)
        {
            if (supplierId <= 0)
                return (false, "Invalid supplier ID.");

            try
            {
                bool result = _supplierDAL.Delete(supplierId);
                return result ? (true, "Supplier deleted successfully.") : (false, "Failed to delete supplier.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                    return (false, "Cannot delete supplier with associated products.");
                return (false, $"Error deleting supplier: {ex.Message}");
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
