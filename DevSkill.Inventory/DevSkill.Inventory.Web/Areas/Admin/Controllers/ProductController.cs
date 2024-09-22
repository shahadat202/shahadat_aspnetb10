using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        public ProductController(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public IActionResult Insert(ProductInsertModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product { Id = Guid.NewGuid(), Title = model.Title };

                _productManagementService.InsertProduct(product);
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Items()
        {
            ViewData["HideNavbar"] = true;
            ViewData["IsSidebarCollapsed"] = true;
            return View();
        }

    }
}
