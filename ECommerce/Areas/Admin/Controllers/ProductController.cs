using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModel;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uniteOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork uniteOfWork, IWebHostEnvironment hostEnvironment)
        {
            _uniteOfWork = uniteOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            
            return View();
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            if (obj.ISBN.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    _uniteOfWork.Product.Add(obj);
                    _uniteOfWork.Save();
                    TempData["Success"] = "Product created Sucessfully";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                ModelState.AddModelError("ISBN", "BOOK WITH THIS ISBN ALREADY EXIST");
            }
            return View(obj);

        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _uniteOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                   Text = i.Name,
                   Value = i.Id.ToString()
                }),
                CoverTypeList = _uniteOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null || id == 0)
            {
                ////Create Product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                productVM.Product = _uniteOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
                //update product
            }

            
        }
        //Put
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "Display order can not be exactly match with Name");
            //}
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string FileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Images\Products");
                    var extension = Path.GetExtension(file.FileName);

                    if(obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var filestreams = new FileStream(Path.Combine(uploads, FileName + extension),FileMode.Create))
                    {
                        file.CopyTo(filestreams);
                    }
                    obj.Product.ImageUrl = @"\Images\Products\"+FileName + extension;
                }
                if (obj.Product.Id == 0)
                {
                    _uniteOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _uniteOfWork.Product.Update(obj.Product);
                }
                _uniteOfWork.Save();
                TempData["Success"] = "Product Created Sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    // var dbobj = _uniteOfWork.categories.Find(id);
        //    var dbobj = _uniteOfWork.Product.GetFirstOrDefault(c => c.Id == id);
        //    if (dbobj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(dbobj);
        //}
        //Remove

       

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productlist = _uniteOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new {data=productlist});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _uniteOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return Json(new {success=false,message ="Error While Deleting"});
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _uniteOfWork.Product.Remove(obj);
            _uniteOfWork.Save();
            return Json(new { success = true, message = "Product deleted Succesfully" });
        }
        #endregion
    }


}
