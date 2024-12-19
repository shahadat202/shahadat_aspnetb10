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
using Newtonsoft.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ProductController : Controller
    {
        const string AgeRestriction = "AgeRestriction";

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
        public async Task<IActionResult> Index()
        {
            var products =  await _productManagementService.GetAllProductsAsync();
            var logs = await _activityLogRepository.GetRecentLogAsync();

            SetProductStatistics(products);
            ViewBag.LatestActivities = logs;

            return View(products);
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public async Task<IActionResult> Items() 
        {
            var products = await _productManagementService.GetAllProductsAsync();
            SetProductStatistics(products);
            return View(products);
        }

        [Authorize(Policy = "AgeRestriction")]
        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "AgeRestriction")]
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
                // Image upload logic
                product.Image = await HandleImageUpload(model.Image);
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
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productManagementService.GetProductAsync(id);

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

        [Authorize(Policy = "AgeRestriction")]
        public async Task<IActionResult> Update(Guid id)
        {
            Product product = await _productManagementService.GetProductAsync(id);
            
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

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "AgeRestriction")]
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
                product.Image = await HandleImageUpload(model.Image);
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var product = await _productManagementService.GetProductAsync(id);
                if (product == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product not found",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Index");
                }

                var username = User.Identity.Name;
                _productManagementService.DeleteProduct(id, username);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Item deleted successfully!",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error deleting item: " + ex.Message,
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Index");
            }
        }

        //[HttpPost, Authorize(Roles = "Member,Admin,Support")]
        //public async Task<JsonResult> GetProductJsonDataSP([FromBody] ProductListModel model)
        //{
        //    var searchDto = model.SearchItem;

        //    var result = await _productManagementService.GetProductsSP(
        //        model.PageIndex,
        //        model.PageSize,
        //        searchDto,
        //        model.Order
        //    );

        //    var productJsonData = new
        //    {
        //        recordsTotal = result.total,
        //        recordsFiltered = result.totalDisplay,
        //        data = (from record in result.data
        //                select new string[]
        //                {
        //                    HttpUtility.HtmlEncode(record.Title),
        //                    record.Quantity.ToString(),
        //                    record.Price.ToString("F2"),
        //                    record.MinLevel.ToString(),
        //                    record.Tags,
        //                    record.CreatedDate.ToString("yyyy/MM/dd"),
        //                    record.Id.ToString()
        //                }).ToArray()
        //    };

        //    return Json(productJsonData);
        //}
        //public async Task<IActionResult> Search()
        //{
        //    var products = await _productManagementService.GetAllProductsAsync();

        //    var itemCount = products.Count();
        //    var totalQuantity = products.Sum(p => p.Quantity);
        //    var totalValue = products.Sum(p => p.TotalValue);

        //    var viewModel = new ProductSearchViewModel
        //    {
        //        Products = products,
        //        ProductListModel = new ProductListModel()
        //    };

        //    ViewBag.ItemCount = itemCount;
        //    ViewBag.TotalQuantity = totalQuantity;
        //    ViewBag.TotalValue = totalValue;

        //    return View(viewModel);
        //}

        public async Task<IActionResult> Search(string title, int? quantity, int? minLevel,
        string tag, decimal? priceFrom, decimal? priceTo, DateTime? dateFrom, DateTime? dateTo)
        {
            var products = await _productManagementService.GetAllProductsAsync();

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

            var itemCount = products.Count();
            var totalQuantity = products.Sum(p => p.Quantity);
            var totalValue = products.Sum(p => p.TotalValue);

            ViewBag.FilterTitle = title;
            ViewBag.FilterQuantity = quantity;
            ViewBag.FilterMinLevel = minLevel;
            ViewBag.FilterTag = tag;
            ViewBag.FilterPriceFrom = priceFrom;
            ViewBag.FilterPriceTo = priceTo;
            ViewBag.FilterDateFrom = dateFrom;
            ViewBag.FilterDateTo = dateTo;

            ViewBag.ItemCount = itemCount;
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.TotalValue = totalValue;

            return View(products.ToList());
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public async Task<IActionResult> Tags()
        {
            var products = await _productManagementService.GetAllProductsAsync();
            SetProductStatistics(products);
            return View(products);
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Reports() 
        {
            return View();
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public async Task<IActionResult> ActivityHistory()
        {
            try
            {
                var logs = await _activityLogRepository.GetRecentLogAsync();
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
        public async Task<IActionResult> InventorySummary(string searchTerm)
        {
            var products = await _productManagementService.GetAllProductsAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            SetProductStatistics(products);

            return PartialView("_InventorySummary", products);
        }

        // Countable value
        private void SetProductStatistics(IEnumerable<Product> products)
        {
            ViewBag.ItemCount = products.Count();
            ViewBag.TotalQuantity = products.Sum(p => p.Quantity);
            ViewBag.TotalValue = products.Sum(p => p.TotalValue);
        }

        // Image upload logic
        private async Task<string> HandleImageUpload(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var guid = Guid.NewGuid();
                var fileExtension = Path.GetExtension(image.FileName);
                var uniqueFileName = $"{guid}{fileExtension}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploadedImages", uniqueFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return $"/uploadedImages/{uniqueFileName}";
            }
            return null;
        }

        // AWS Image upload logic
        //private async Task<string> HandleImageUpload(IFormFile image)
        //{
        //    if (image != null && image.Length > 0)
        //    {
        //        var guid = Guid.NewGuid();
        //        var fileExtension = Path.GetExtension(image.FileName);
        //        var uniqueFileName = $"{guid}{fileExtension}";

        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploadedImages", uniqueFileName);
        //        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await image.CopyToAsync(stream);
        //        }

        //        // Send message to SQS queue
        //        var sqsClient = new AmazonSQSClient(Amazon.RegionEndpoint.USEast1);
        //        var queueUrl = "https://sqs.us-east-1.amazonaws.com/847888492411/shahadat_queue";

        //        var message = new
        //        {
        //            FilePath = filePath,
        //            FileName = uniqueFileName
        //        };

        //        await sqsClient.SendMessageAsync(new SendMessageRequest
        //        {
        //            QueueUrl = queueUrl,
        //            MessageBody = JsonConvert.SerializeObject(message)
        //        });

        //        return $"/uploadedImages/{uniqueFileName}";
        //    }

        //    return null;
        //}

    }
}
