using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
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
            ViewData["HideNavbar"] = true;
            ViewData["IsSidebarCollapsed"] = true;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Insert()
        {
            ViewData["HideNavbar"] = true;
            ViewData["IsSidebarCollapsed"] = true;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Insert(ProductInsertModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Quantity = model.Quantity,
                    MinLevel = model.MinLevel,
                    Price = model.Price,
                    Tags = model.Tags,
                    Notes = model.Notes,
                    CreatedDate = DateTime.UtcNow
                };

                if (model.Image != null && model.Image.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploadedImages", model.Image.FileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    product.Image = $"/uploadedImages/{model.Image.FileName}";
                }

                _productManagementService.InsertProduct(product);
                TempData["NewItemId"] = product.Id;
                return RedirectToAction("Items");
            }
            return View(model);
        }

        public IActionResult Items()
        {
            ViewData["HideNavbar"] = true;
            ViewData["IsSidebarCollapsed"] = true;

            var products = _productManagementService.GetAllProducts(); 
            return View(products);
        }
        //public IActionResult AddItem()
        //{
        //    ViewData["HideNavbar"] = true;
        //    ViewData["IsSidebarCollapsed"] = true;
        //    return View();
        //}
    }
}
