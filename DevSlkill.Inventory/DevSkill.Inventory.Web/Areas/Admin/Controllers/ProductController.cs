using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        public ProductController(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
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
    }
}
