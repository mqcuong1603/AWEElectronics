using System;
using System.Collections.Generic;
using AWEElectronics.DAL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL
{
    public class InventoryBLL
    {
        private readonly InventoryTransactionDAL _inventoryDAL;
        private readonly ProductDAL _productDAL;

        public InventoryBLL()
        {
            _inventoryDAL = new InventoryTransactionDAL();
            _productDAL = new ProductDAL();
        }

        public List<InventoryTransaction> GetAllTransactions()
        {
            return _inventoryDAL.GetAll();
        }

        public List<InventoryTransaction> GetTransactionsByProduct(int productId)
        {
            return _inventoryDAL.GetByProductId(productId);
        }

        public List<InventoryTransaction> GetGoodsReceivedNotes()
        {
            return _inventoryDAL.GetByType("IN");
        }

        public List<InventoryTransaction> GetGoodsDeliveryNotes()
        {
            return _inventoryDAL.GetByType("OUT");
        }

        public List<InventoryTransaction> GetAdjustments()
        {
            return _inventoryDAL.GetByType("ADJUST");
        }

        public (bool Success, string Message, string ReferenceNumber) CreateGoodsReceivedNote(
            int productId, int quantity, int performedBy, string notes = null)
        {
            if (productId <= 0)
                return (false, "Invalid product ID.", null);

            if (quantity <= 0)
                return (false, "Quantity must be greater than zero.", null);

            var product = _productDAL.GetById(productId);
            if (product == null)
                return (false, "Product not found.", null);

            try
            {
                string refNumber = _inventoryDAL.GenerateReferenceNumber("IN");

                // Create transaction record
                var transaction = new InventoryTransaction
                {
                    ProductID = productId,
                    Type = "IN",
                    Quantity = quantity,
                    ReferenceNumber = refNumber,
                    PerformedBy = performedBy,
                    Notes = notes ?? $"Goods received: {quantity} units of {product.Name}"
                };
                _inventoryDAL.Insert(transaction);

                // Update product stock
                _productDAL.UpdateStock(productId, quantity);

                return (true, $"Goods received note created. New stock: {product.StockLevel + quantity}", refNumber);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating goods received note: {ex.Message}", null);
            }
        }

        public (bool Success, string Message, string ReferenceNumber) CreateGoodsDeliveryNote(
            int productId, int quantity, int performedBy, string notes = null)
        {
            if (productId <= 0)
                return (false, "Invalid product ID.", null);

            if (quantity <= 0)
                return (false, "Quantity must be greater than zero.", null);

            var product = _productDAL.GetById(productId);
            if (product == null)
                return (false, "Product not found.", null);

            if (product.StockLevel < quantity)
                return (false, $"Insufficient stock. Available: {product.StockLevel}", null);

            try
            {
                string refNumber = _inventoryDAL.GenerateReferenceNumber("OUT");

                // Create transaction record
                var transaction = new InventoryTransaction
                {
                    ProductID = productId,
                    Type = "OUT",
                    Quantity = quantity,
                    ReferenceNumber = refNumber,
                    PerformedBy = performedBy,
                    Notes = notes ?? $"Goods delivered: {quantity} units of {product.Name}"
                };
                _inventoryDAL.Insert(transaction);

                // Update product stock
                _productDAL.UpdateStock(productId, -quantity);

                return (true, $"Goods delivery note created. New stock: {product.StockLevel - quantity}", refNumber);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating goods delivery note: {ex.Message}", null);
            }
        }

        public (bool Success, string Message) AdjustInventory(
            int productId, int newQuantity, int performedBy, string reason)
        {
            if (productId <= 0)
                return (false, "Invalid product ID.");

            if (newQuantity < 0)
                return (false, "Quantity cannot be negative.");

            if (string.IsNullOrWhiteSpace(reason))
                return (false, "Reason for adjustment is required.");

            var product = _productDAL.GetById(productId);
            if (product == null)
                return (false, "Product not found.");

            int difference = newQuantity - product.StockLevel;
            if (difference == 0)
                return (false, "No change in quantity.");

            try
            {
                string refNumber = _inventoryDAL.GenerateReferenceNumber("ADJUST");

                // Create adjustment transaction
                var transaction = new InventoryTransaction
                {
                    ProductID = productId,
                    Type = "ADJUST",
                    Quantity = Math.Abs(difference),
                    ReferenceNumber = refNumber,
                    PerformedBy = performedBy,
                    Notes = $"Adjustment: {product.StockLevel} → {newQuantity}. Reason: {reason}"
                };
                _inventoryDAL.Insert(transaction);

                // Update product stock
                _productDAL.UpdateStock(productId, difference);

                return (true, $"Inventory adjusted. New stock: {newQuantity}");
            }
            catch (Exception ex)
            {
                return (false, $"Error adjusting inventory: {ex.Message}");
            }
        }
    }
}
