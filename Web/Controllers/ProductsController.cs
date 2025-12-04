using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Web.Filters;

namespace Web.Controllers
{
    [AuthorizeSession]
    public class ProductsController : Controller
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;

        public ProductsController()
        {
            _productBLL = new ProductBLL();
            _categoryBLL = new CategoryBLL();
        }

        // GET: /Products
        public ActionResult Index(string search, int? categoryId)
        {
            try
            {
                List<Product> products;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    products = _productBLL.Search(search);
                    ViewBag.SearchTerm = search;
                }
                else if (categoryId.HasValue && categoryId.Value > 0)
                {
                    products = _productBLL.GetByCategory(categoryId.Value);
                    ViewBag.CategoryId = categoryId.Value;
                }
                else
                {
                    products = _productBLL.GetAll();
                }

                // Get categories for filter
                var categories = _categoryBLL.GetAll();
                ViewBag.Categories = categories;

                return View(products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading products.";
                System.Diagnostics.Debug.WriteLine($"Products Index error: {ex.Message}");
                return View(new List<Product>());
            }
        }

        // GET: /Products/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Product product = _productBLL.GetById(id);

                if (product == null)
                {
                    ViewBag.ErrorMessage = "Product not found.";
                    return View("Error");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading product details.";
                System.Diagnostics.Debug.WriteLine($"Product Details error: {ex.Message}");
                return View("Error");
            }
        }
    }
}
