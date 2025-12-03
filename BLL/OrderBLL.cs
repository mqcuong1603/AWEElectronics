using System;
using System.Collections.Generic;
using AWEElectronics.DAL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL
{
    public class OrderBLL
    {
        private readonly OrderDAL _orderDAL;
        private readonly OrderDetailDAL _orderDetailDAL;
        private readonly ProductDAL _productDAL;
        private readonly PaymentDAL _paymentDAL;

        private const decimal TAX_RATE = 0.10m; // 10% tax

        public OrderBLL()
        {
            _orderDAL = new OrderDAL();
            _orderDetailDAL = new OrderDetailDAL();
            _productDAL = new ProductDAL();
            _paymentDAL = new PaymentDAL();
        }

        public List<Order> GetAll()
        {
            return _orderDAL.GetAll();
        }

        public List<Order> GetByStatus(string status)
        {
            return _orderDAL.GetByStatus(status);
        }

        public List<Order> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _orderDAL.GetByDateRange(startDate, endDate);
        }

        public Order GetById(int orderId)
        {
            Order order = _orderDAL.GetById(orderId);
            if (order != null)
            {
                order.OrderDetails = _orderDetailDAL.GetByOrderId(orderId);
            }
            return order;
        }

        public Order GetByOrderCode(string orderCode)
        {
            Order order = _orderDAL.GetByOrderCode(orderCode);
            if (order != null)
            {
                order.OrderDetails = _orderDetailDAL.GetByOrderId(order.OrderID);
            }
            return order;
        }

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            return _orderDetailDAL.GetByOrderId(orderId);
        }

        public (bool Success, string Message) UpdateOrderStatus(int orderId, string newStatus, int? staffId = null)
        {
            // Valid status transitions
            var validTransitions = new Dictionary<string, List<string>>
            {
                { "Pending", new List<string> { "Processing", "Cancelled" } },
                { "Processing", new List<string> { "Shipped", "Cancelled" } },
                { "Shipped", new List<string> { "Delivered" } },
                { "Delivered", new List<string>() },
                { "Cancelled", new List<string>() }
            };

            Order order = _orderDAL.GetById(orderId);
            if (order == null)
                return (false, "Order not found.");

            if (!validTransitions.ContainsKey(order.Status))
                return (false, "Invalid current order status.");

            if (!validTransitions[order.Status].Contains(newStatus))
                return (false, $"Cannot change status from '{order.Status}' to '{newStatus}'.");

            try
            {
                bool result = _orderDAL.UpdateStatus(orderId, newStatus, staffId);
                return result ? (true, $"Order status updated to '{newStatus}'.") : (false, "Failed to update order status.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating order status: {ex.Message}");
            }
        }

        public (bool Success, string Message) CancelOrder(int orderId, int staffId)
        {
            Order order = _orderDAL.GetById(orderId);
            if (order == null)
                return (false, "Order not found.");

            if (order.Status == "Shipped" || order.Status == "Delivered")
                return (false, "Cannot cancel shipped or delivered orders.");

            if (order.Status == "Cancelled")
                return (false, "Order is already cancelled.");

            try
            {
                // Restore stock for each item
                var details = _orderDetailDAL.GetByOrderId(orderId);
                foreach (var detail in details)
                {
                    _productDAL.UpdateStock(detail.ProductID, detail.Quantity);
                }

                // Update order status
                bool result = _orderDAL.UpdateStatus(orderId, "Cancelled", staffId);

                // Update payment status if exists
                var payment = _paymentDAL.GetByOrderId(orderId);
                if (payment != null && payment.Status == "Pending")
                {
                    _paymentDAL.UpdateStatus(payment.PaymentID, "Failed");
                }

                return result ? (true, "Order cancelled successfully.") : (false, "Failed to cancel order.");
            }
            catch (Exception ex)
            {
                return (false, $"Error cancelling order: {ex.Message}");
            }
        }

        public decimal CalculateSubTotal(List<OrderDetail> items)
        {
            decimal subTotal = 0;
            foreach (var item in items)
            {
                subTotal += item.UnitPrice * item.Quantity;
            }
            return subTotal;
        }

        public decimal CalculateTax(decimal subTotal)
        {
            return Math.Round(subTotal * TAX_RATE, 2);
        }

        public decimal CalculateGrandTotal(decimal subTotal, decimal tax)
        {
            return subTotal + tax;
        }

        public Payment GetPaymentByOrderId(int orderId)
        {
            return _paymentDAL.GetByOrderId(orderId);
        }
    }
}
