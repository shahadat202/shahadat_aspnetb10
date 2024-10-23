using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DevSkill.Inventory.Infrastructure;
using AutoMapper;
using MailKit.Search;
using DevSkill.Inventory.Domain;
using System.Web;
using DevSkill.Inventory.Domain.RepositoryContracts;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        private readonly ILogger<ProductController> _logger;
        private readonly IActivityLogRepository _activityLogRepository;
        //private readonly IMapper _mapper;
        
        public ProductController(ILogger<ProductController> logger,
            IProductManagementService productManagementService,
            IActivityLogRepository activityLogRepository)
        {
            _logger = logger;
            _productManagementService = productManagementService;
            _activityLogRepository = activityLogRepository;
            //_mapper = mapper;
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Index()
        {
            var products = _productManagementService.GetAllProducts();
            var logs = _activityLogRepository.GetRecentLogs();

            var itemCount = products.Count();
            var totalQuantity = products.Sum(p => p.Quantity);
            var totalValue = products.Sum(p => p.TotalValue);

            ViewBag.ItemCount = itemCount;
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.TotalValue = totalValue;
            ViewBag.LatestActivities = logs;

            return View(products);
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Items() 
        {
            var products = _productManagementService.GetAllProducts();

            var itemCount = products.Count();
            var totalQuantity = products.Sum(p => p.Quantity);
            var totalValue = products.Sum(p => p.TotalValue);

            ViewBag.ItemCount = itemCount;
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.TotalValue = totalValue;

            return View(products);
        }

        [Authorize(Policy = "CreatePermission")]
        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePermission")]
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
                    TotalValue = model.Quantity * model.Price,
                    Tags = model.Tags,
                    Notes = model.Notes,
                    CreatedDate = DateTime.UtcNow
                };
                if (model.Image != null && model.Image.Length > 0)
                {
                    var guid = Guid.NewGuid();

                    var fileExtension = Path.GetExtension(model.Image.FileName);
                    var uniqueFileName = $"{guid}{fileExtension}";

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploadedImages", uniqueFileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    product.Image = $"/uploadedImages/{uniqueFileName}";
                }
                try
                {
                    var username = User.Identity.Name;
                    _productManagementService.InsertProduct(product, username);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product inserted successfully",
                        Type = ResponseTypes.Success
                    });
                    TempData["NewItemId"] = product.Id;
                    return RedirectToAction("Items");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product insertion failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Product insertion failed");
                    return RedirectToAction("Items");
                }
            }
            //model.SetCategoryValues(_categoryManagementService.GetCategories());

            return View(model);
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Details(Guid id)
        {
            var product = _productManagementService.GetProduct(id);

            if (product == null)
                return NotFound();
            
            var model = new ProductDetailsModel
            {
                Id = product.Id,
                Title = product.Title,
                Quantity = product.Quantity,
                Price = product.Price,
                TotalValue = product.TotalValue,
                MinLevel = product.MinLevel,
                Tags = product.Tags,
                Notes = product.Notes,
                Image = product.Image,
                CreatedDate = product.CreatedDate,
            };

            return View(model);
        }

        [Authorize(Policy = "CustomAccess")]
        public IActionResult Update(Guid id)
        {
            Product product = _productManagementService.GetProduct(id);
            
            var model = new ProductUpdateModel
            {
                Id = product.Id,
                Title = product.Title,
                Quantity = product.Quantity,
                Price = product.Price,
                TotalValue = product.TotalValue,
                MinLevel = product.MinLevel,
                Tags = product.Tags,
                Notes = product.Notes,
                CreatedDate = product.CreatedDate,
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CustomAccess")]
        public async Task<IActionResult> Update(ProductUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Id = model.Id,
                    Title = model.Title,
                    Quantity = model.Quantity,
                    MinLevel = model.MinLevel,
                    Price = model.Price,
                    TotalValue = model.Quantity * model.Price,
                    Tags = model.Tags,
                    Notes = model.Notes,
                };
                // Image upload logic
                if (model.Image != null && model.Image.Length > 0)
                {
                    var guid = Guid.NewGuid();

                    var fileExtension = Path.GetExtension(model.Image.FileName);
                    var uniqueFileName = $"{guid}{fileExtension}";

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploadedImages", uniqueFileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    product.Image = $"/uploadedImages/{uniqueFileName}";
                }
                try
                {
                    var username = User.Identity.Name;
                    _productManagementService.UpdateProduct(product, username);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product updated successfully",
                        Type = ResponseTypes.Success
                    });
                    TempData["NewItemId"] = product.Id;
                    return RedirectToAction("Items");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product update failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Product update failed");
                }
                return RedirectToAction("Items");
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var product = _productManagementService.GetProduct(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }
                var username = User.Identity.Name;
                _productManagementService.DeleteProduct(id, username);

                return Json(new { success = true, message = "Item deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting item: " + ex.Message });
            }
        }

        //public IActionResult Search(string title, int? quantity, int? minLevel,
        //string tag, decimal? priceFrom, decimal? priceTo, DateTime? dateFrom, DateTime? dateTo)
        //{
        //    var products = _productManagementService.GetAllProducts();

        //    // Apply filters
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        products = products.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        //    }
        //    if (quantity.HasValue)
        //    {
        //        products = products.Where(p => p.Quantity == quantity.Value);
        //    }
        //    if (minLevel.HasValue)
        //    {
        //        products = products.Where(p => p.MinLevel >= minLevel.Value);
        //    }
        //    if (!string.IsNullOrEmpty(tag))
        //    {
        //        products = products.Where(p => p.Tags.Contains(tag, StringComparison.OrdinalIgnoreCase));
        //    }
        //    if (priceFrom.HasValue)
        //    {
        //        products = products.Where(p => p.Price >= priceFrom.Value);
        //    }
        //    if (priceTo.HasValue)
        //    {
        //        products = products.Where(p => p.Price <= priceTo.Value);
        //    }
        //    if (dateFrom.HasValue)
        //    {
        //        products = products.Where(p => p.CreatedDate >= dateFrom.Value);
        //    }
        //    if (dateTo.HasValue)
        //    {
        //        products = products.Where(p => p.CreatedDate <= dateTo.Value);
        //    }

        //    var itemCount = products.Count();
        //    var totalQuantity = products.Sum(p => p.Quantity);
        //    var totalValue = products.Sum(p => p.TotalValue);

        //    ViewBag.FilterTitle = title;
        //    ViewBag.FilterQuantity = quantity;
        //    ViewBag.FilterMinLevel = minLevel;
        //    ViewBag.FilterTag = tag;
        //    ViewBag.FilterPriceFrom = priceFrom;
        //    ViewBag.FilterPriceTo = priceTo;
        //    ViewBag.FilterDateFrom = dateFrom;
        //    ViewBag.FilterDateTo = dateTo;

        //    ViewBag.ItemCount = itemCount;
        //    ViewBag.TotalQuantity = totalQuantity;
        //    ViewBag.TotalValue = totalValue;

        //    return View(products.ToList()); 
        //}

        [HttpPost]
        public IActionResult ApplyFilters(string title, int? quantity, int? minLevel, 
            string tag, decimal? priceFrom, decimal? priceTo, DateTime? dateFrom, DateTime? dateTo)
        {
            return RedirectToAction("Search", new
            {
                title = title,
                quantity = quantity,
                minLevel = minLevel,
                tag = tag,
                priceFrom = priceFrom,
                priceTo = priceTo,
                dateFrom = dateFrom,
                dateTo = dateTo
            });
        }
        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Search(string title, int? quantity, int? minLevel, 
            string tag, decimal? priceFrom, decimal? priceTo, DateTime? dateFrom, DateTime? dateTo)
        {
            var products = _productManagementService.GetAllProducts();

            // Apply filters
            if (!string.IsNullOrEmpty(title))
            {
                products = products.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }
            if (quantity.HasValue)
            {
                products = products.Where(p => p.Quantity == quantity.Value);
            }
            if (minLevel.HasValue)
            {
                products = products.Where(p => p.MinLevel >= minLevel.Value);
            }
            if (!string.IsNullOrEmpty(tag))
            {
                products = products.Where(p => p.Tags.Contains(tag, StringComparison.OrdinalIgnoreCase));
            }
            if (priceFrom.HasValue)
            {
                products = products.Where(p => p.Price >= priceFrom.Value);
            }
            if (priceTo.HasValue)
            {
                products = products.Where(p => p.Price <= priceTo.Value);
            }
            if (dateFrom.HasValue)
            {
                products = products.Where(p => p.CreatedDate >= dateFrom.Value);
            }
            if (dateTo.HasValue)
            {
                products = products.Where(p => p.CreatedDate <= dateTo.Value);
            }

            ViewBag.FilterTitle = title;
            ViewBag.FilterQuantity = quantity;
            ViewBag.FilterMinLevel = minLevel;
            ViewBag.FilterTag = tag;
            ViewBag.FilterPriceFrom = priceFrom;
            ViewBag.FilterPriceTo = priceTo;
            ViewBag.FilterDateFrom = dateFrom;
            ViewBag.FilterDateTo = dateTo;

            ViewBag.ItemCount = products.Count();
            ViewBag.TotalQuantity = products.Sum(p => p.Quantity);
            ViewBag.TotalValue = products.Sum(p => p.TotalValue);

            return View(products.ToList());
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Tags()
        {
            var products = _productManagementService.GetAllProducts();

            var itemCount = products.Count();
            var totalQuantity = products.Sum(p => p.Quantity);
            var totalValue = products.Sum(p => p.TotalValue);

            ViewBag.ItemCount = itemCount;
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.TotalValue = totalValue;

            return View(products);
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Reports() 
        {
            return View();
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult ActivityHistory()
        {
            try
            {
                var logs = _activityLogRepository.GetRecentLogs();
                if (logs == null || !logs.Any())
                {
                    _logger.LogWarning("No activity logs found.");
                }

                return PartialView("_ActivityHistory", logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load activity history.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult InventorySummary(string searchTerm)
        {
            var products = _productManagementService.GetAllProducts();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var itemCount = products.Count();
            var totalQuantity = products.Sum(p => p.Quantity);
            var totalValue = products.Sum(p => p.TotalValue);

            ViewBag.ItemCount = itemCount;
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.TotalValue = totalValue;

            return PartialView("_InventorySummary", products);
        }
    }
}
