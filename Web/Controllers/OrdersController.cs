using System;
using System.Linq;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderBLL _orderBLL;

        public OrdersController()
        {
            _orderBLL = new OrderBLL();
        }

        // GET: Orders
        public ActionResult Index(string status)
        {
            // Check authentication
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = string.IsNullOrWhiteSpace(status)
                ? _orderBLL.GetAll()
                : _orderBLL.GetByStatus(status);

            ViewBag.StatusFilter = status;
            ViewBag.TotalOrders = orders.Count;

            // Pass available statuses for filter dropdown
            ViewBag.Statuses = new SelectList(new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });

            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            // Check authentication
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = _orderBLL.GetById(id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index");
            }

            // Get payment information
            var payment = _orderBLL.GetPaymentByOrderId(id);
            ViewBag.Payment = payment;

            return View(order);
        }

        // POST: Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int orderId, string newStatus)
        {
            // Check authentication
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get current user ID
            int? staffId = Session["UserID"] as int?;

            var result = _orderBLL.UpdateOrderStatus(orderId, newStatus, staffId);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
