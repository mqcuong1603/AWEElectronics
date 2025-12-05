using System;
using System.Linq;
using System.Web.Mvc;
using AWEElectronics.BLL;

namespace Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OrderBLL _orderBLL;

        public OrdersController()
        {
            _orderBLL = new OrderBLL();
        }

        // GET: Orders/MyOrders
        public ActionResult MyOrders()
        {
            if (Session["UserID"] == null)
            {
                TempData["ErrorMessage"] = "Please login to view your orders.";
                return RedirectToAction("Login", "Account");
            }

            // For now, return an empty view - you can implement customer orders later
            // This is a placeholder to fix the 404 error
            ViewBag.UserName = Session["FullName"];
            
            return View();
        }

        // GET: Orders/Details/{id}
        public ActionResult Details(int id)
        {
            if (Session["UserID"] == null)
            {
                TempData["ErrorMessage"] = "Please login to view order details.";
                return RedirectToAction("Login", "Account");
            }

            var order = _orderBLL.GetById(id);
            
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("MyOrders");
            }

            return View(order);
        }
    }
}
