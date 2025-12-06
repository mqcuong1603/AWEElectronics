using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Web.Filters;

namespace Web.Controllers
{
    [AuthorizeSession]
    public class OrdersController : Controller
    {
        private readonly OrderBLL _orderBLL;

        public OrdersController()
        {
            _orderBLL = new OrderBLL();
        }

        // GET: /Orders
        public ActionResult Index(string status, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                List<Order> orders;

                if (!string.IsNullOrWhiteSpace(status) && status != "All")
                {
                    orders = _orderBLL.GetByStatus(status);
                    ViewBag.SelectedStatus = status;
                }
                else if (startDate.HasValue && endDate.HasValue)
                {
                    orders = _orderBLL.GetByDateRange(startDate.Value, endDate.Value);
                    ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                    ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
                }
                else
                {
                    orders = _orderBLL.GetAll();
                }

                // Sort by most recent first
                orders = orders.OrderByDescending(o => o.OrderDate).ToList();

                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading orders.";
                System.Diagnostics.Debug.WriteLine($"Orders Index error: {ex.Message}");
                return View(new List<Order>());
            }
        }

        // GET: /Orders/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Order order = _orderBLL.GetById(id);

                if (order == null)
                {
                    ViewBag.ErrorMessage = "Order not found.";
                    return View("Error");
                }

                // Get order details
                order.OrderDetails = _orderBLL.GetOrderDetails(id);

                // Get payment info
                var payment = _orderBLL.GetPaymentByOrderId(id);
                ViewBag.Payment = payment;

                return View(order);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading order details.";
                System.Diagnostics.Debug.WriteLine($"Order Details error: {ex.Message}");
                return View("Error");
            }
        }

        // POST: /Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int orderId, string newStatus)
        {
            try
            {
                int userId = (Session["UserId"] as int?) ?? 0;
                var result = _orderBLL.UpdateOrderStatus(orderId, newStatus, userId);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating order status.";
                System.Diagnostics.Debug.WriteLine($"Update Status error: {ex.Message}");
            }

            return RedirectToAction("Details", new { id = orderId });
        }

        // POST: /Orders/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int orderId)
        {
            try
            {
                int userId = (Session["UserId"] as int?) ?? 0;
                var result = _orderBLL.CancelOrder(orderId, userId);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error cancelling order.";
                System.Diagnostics.Debug.WriteLine($"Cancel Order error: {ex.Message}");
            }

            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
