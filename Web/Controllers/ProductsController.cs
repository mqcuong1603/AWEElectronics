using System;
using System.Linq;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductBLL _productBLL;

        public ProductsController()
        {
            _productBLL = new ProductBLL();
        }

        // GET: Products
        public ActionResult Index(string search)
        {
            // Check authentication
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var products = string.IsNullOrWhiteSpace(search) 
                ? _productBLL.GetAll() 
                : _productBLL.Search(search);

            ViewBag.SearchQuery = search;
            ViewBag.TotalProducts = products.Count;

            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            // Check authentication
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _productBLL.GetById(id);

            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Index");
            }

            return View(product);
        }
    }
}
