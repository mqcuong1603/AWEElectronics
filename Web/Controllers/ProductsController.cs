using System;
using System.Linq;
using System.Web.Mvc;
using AWEElectronics.BLL;
using Web.Models;

namespace Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;

        public ProductsController()
        {
            _productBLL = new ProductBLL();
            _categoryBLL = new CategoryBLL();
        }

        // GET: Products
        public ActionResult Index(int? categoryId, string search, string sortBy)
        {
            var viewModel = new ProductListViewModel
            {
                Categories = _categoryBLL.GetAll(),
                SelectedCategoryId = categoryId,
                SearchKeyword = search,
                SortBy = sortBy
            };

            // Get products based on filters
            if (categoryId.HasValue)
            {
                viewModel.Products = _productBLL.GetByCategory(categoryId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(search))
            {
                viewModel.Products = _productBLL.Search(search);
            }
            else
            {
                viewModel.Products = _productBLL.GetPublished();
            }

            // Apply sorting
            switch (sortBy)
            {
                case "price_asc":
                    viewModel.Products = viewModel.Products.OrderBy(p => p.Price).ToList();
                    break;
                case "price_desc":
                    viewModel.Products = viewModel.Products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "name":
                    viewModel.Products = viewModel.Products.OrderBy(p => p.Name).ToList();
                    break;
                default:
                    // Keep default order
                    break;
            }

            return View(viewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            var product = _productBLL.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Category/5
        public ActionResult Category(int id)
        {
            var category = _categoryBLL.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryName = category.Name;
            return RedirectToAction("Index", new { categoryId = id });
        }
    }
}
