using System;
using System.Collections.Generic;
using AWEElectronics.DAL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL
{
    public class ProductBLL
    {
        private readonly ProductDAL _productDAL;
        private readonly InventoryTransactionDAL _inventoryDAL;

        public ProductBLL()
        {
            _productDAL = new ProductDAL();
            _inventoryDAL = new InventoryTransactionDAL();
        }

        public List<Product> GetAll()
        {
            return _productDAL.GetAll();
        }

        public List<Product> GetPublished()
        {
            return _productDAL.GetPublished();
        }

        public Product GetById(int productId)
        {
            return _productDAL.GetById(productId);
        }

        public Product GetBySKU(string sku)
        {
            return _productDAL.GetBySKU(sku);
        }

        public List<Product> GetByCategory(int categoryId)
        {
            return _productDAL.GetByCategory(categoryId);
        }

        public List<Product> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAll();
            return _productDAL.Search(keyword);
        }

        public List<Product> GetLowStock(int threshold = 10)
        {
            return _productDAL.GetLowStock(threshold);
        }

        public (bool Success, string Message, int ProductId) CreateProduct(Product product)
        {
            // Validation
            var validationResult = ValidateProduct(product);
            if (!validationResult.IsValid)
                return (false, validationResult.Message, 0);

            // Check if SKU already exists
            var existingProduct = _productDAL.GetBySKU(product.SKU);
            if (existingProduct != null)
                return (false, "SKU already exists.", 0);

            try
            {
                int productId = _productDAL.Insert(product);
                return (true, "Product created successfully.", productId);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating product: {ex.Message}", 0);
            }
        }

        public (bool Success, string Message) UpdateProduct(Product product)
        {
            if (product.ProductID <= 0)
                return (false, "Invalid product ID.");

            var validationResult = ValidateProduct(product);
            if (!validationResult.IsValid)
                return (false, validationResult.Message);

            // Check if SKU is unique (excluding current product)
            var existingProduct = _productDAL.GetBySKU(product.SKU);
            if (existingProduct != null && existingProduct.ProductID != product.ProductID)
                return (false, "SKU already exists.");

            try
            {
                bool result = _productDAL.Update(product);
                return result ? (true, "Product updated successfully.") : (false, "Failed to update product.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating product: {ex.Message}");
            }
        }

        public (bool Success, string Message) DeleteProduct(int productId)
        {
            if (productId <= 0)
                return (false, "Invalid product ID.");

            try
            {
                bool result = _productDAL.Delete(productId);
                return result ? (true, "Product deleted successfully.") : (false, "Failed to delete product.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                    return (false, "Cannot delete product with associated orders.");
                return (false, $"Error deleting product: {ex.Message}");
            }
        }

        public (bool Success, string Message) AdjustStock(int productId, int quantityChange, int performedBy, string notes = null)
        {
            if (productId <= 0)
                return (false, "Invalid product ID.");

            if (quantityChange == 0)
                return (false, "Quantity change cannot be zero.");

            var product = _productDAL.GetById(productId);
            if (product == null)
                return (false, "Product not found.");

            int newStockLevel = product.StockLevel + quantityChange;
            if (newStockLevel < 0)
                return (false, $"Insufficient stock. Current stock: {product.StockLevel}");

            try
            {
                // Update product stock
                bool stockUpdated = _productDAL.UpdateStock(productId, quantityChange);
                if (!stockUpdated)
                    return (false, "Failed to update stock.");

                // Create inventory transaction
                string type = quantityChange > 0 ? "IN" : "OUT";
                var transaction = new InventoryTransaction
                {
                    ProductID = productId,
                    Type = type,
                    Quantity = Math.Abs(quantityChange),
                    ReferenceNumber = _inventoryDAL.GenerateReferenceNumber(type),
                    PerformedBy = performedBy,
                    Notes = notes ?? $"Stock {type}: {Math.Abs(quantityChange)} units"
                };
                _inventoryDAL.Insert(transaction);

                return (true, $"Stock updated. New stock level: {newStockLevel}");
            }
            catch (Exception ex)
            {
                return (false, $"Error adjusting stock: {ex.Message}");
            }
        }

        public (bool Success, string Message) ReceiveGoods(int productId, int quantity, int performedBy, string referenceNumber = null, string notes = null)
        {
            if (quantity <= 0)
                return (false, "Quantity must be greater than zero.");

            return AdjustStock(productId, quantity, performedBy, notes ?? $"Goods received: {quantity} units. Ref: {referenceNumber}");
        }

        public (bool Success, string Message) DeliverGoods(int productId, int quantity, int performedBy, string referenceNumber = null, string notes = null)
        {
            if (quantity <= 0)
                return (false, "Quantity must be greater than zero.");

            return AdjustStock(productId, -quantity, performedBy, notes ?? $"Goods delivered: {quantity} units. Ref: {referenceNumber}");
        }

        private (bool IsValid, string Message) ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return (false, "Product name is required.");

            if (product.Name.Length < 2)
                return (false, "Product name must be at least 2 characters.");

            if (string.IsNullOrWhiteSpace(product.SKU))
                return (false, "SKU is required.");

            if (product.SKU.Length < 3)
                return (false, "SKU must be at least 3 characters.");

            if (product.CategoryID <= 0)
                return (false, "Category is required.");

            if (product.Price < 0)
                return (false, "Price cannot be negative.");

            if (product.StockLevel < 0)
                return (false, "Stock level cannot be negative.");

            return (true, string.Empty);
        }
    }
}
