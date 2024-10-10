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

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        private readonly ILogger<ProductController> _logger;
        //private readonly IMapper _mapper;
        
        public ProductController(ILogger<ProductController> logger,
            IProductManagementService productManagementService)
        {
            _logger = logger;
            _productManagementService = productManagementService;
            //_mapper = mapper;
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Index() //dashboard
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

        [Authorize(Roles = "Admin")]
        public IActionResult Insert()
        {
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
                    TotalValue = model.Quantity * model.Price,
                    Tags = model.Tags,
                    Notes = model.Notes,
                    CreatedDate = DateTime.UtcNow
                };

                // Image upload logic
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
                try
                {
                    _productManagementService.InsertProduct(product);

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
                }
            }
            //model.SetCategoryValues(_categoryManagementService.GetCategories());

            return View(model);
        }

        [Authorize(Roles = "Admin")]
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

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
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
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploadedImages", model.Image.FileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    product.Image = $"/uploadedImages/{model.Image.FileName}";
                }
                try
                {
                    _productManagementService.UpdateProduct(product);
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
            //model.SetCategoryValues(_categoryManagementService.GetCategories());
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "No items selected for deletion",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Index");
                }

                var idList = ids.Split(',').Select(id => Guid.Parse(id)) .ToList();

                foreach (var id in idList)
                {
                    _productManagementService.DeleteProduct(id);
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Selected items deleted successfully",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            catch (FormatException)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "One or more IDs are not valid GUIDs",
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to delete selected items",
                    Type = ResponseTypes.Danger
                });
                _logger.LogError(ex, "Error occurred while deleting items");
                return RedirectToAction("Index");
            }
        }


        //[HttpPost, ValidateAntiForgeryToken]
        //public IActionResult Delete(List<Guid> ids)
        //{
        //    try
        //    {
        //        if (ids == null || !ids.Any())
        //        {
        //            TempData.Put("ResponseMessage", new ResponseModel
        //            {
        //                Message = "No items selected for deletion",
        //                Type = ResponseTypes.Danger
        //            });
        //            return RedirectToAction("Index");
        //        }

        //        foreach (var id in ids)
        //        {
        //            _productManagementService.DeleteProduct(id);
        //        }

        //        TempData.Put("ResponseMessage", new ResponseModel
        //        {
        //            Message = "Selected items deleted successfully",
        //            Type = ResponseTypes.Success
        //        });

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData.Put("ResponseMessage", new ResponseModel
        //        {
        //            Message = "Failed to delete selected items",
        //            Type = ResponseTypes.Danger
        //        });
        //        _logger.LogError(ex, "Error occurred while deleting items");
        //        return RedirectToAction("Index");
        //    }
        //}

        public IActionResult Search()
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


    }
}
