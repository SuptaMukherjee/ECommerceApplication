using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _uniteOfWork;

        public CategoryController(IUnitOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _uniteOfWork.Category.GetAll();
            return View(objCategoryList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Display order can not be exactly match with Name");
            }
            if (ModelState.IsValid)
            {
                _uniteOfWork.Category.Add(obj);
                _uniteOfWork.Save();
                TempData["Success"] = "Category created Sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var dbobj = _uniteOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            // var Databaseobj = _uniteOfWork.categories.FirstOrDefault(c => c.Id == id);
            if (dbobj == null)
            {
                return NotFound();
            }
            return View(dbobj);
        }
        //Put
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Display order can not be exactly match with Name");
            }
            if (ModelState.IsValid)
            {
                _uniteOfWork.Category.Update(obj);
                _uniteOfWork.Save();
                TempData["Success"] = "Category Updated Sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var dbobj = _uniteOfWork.categories.Find(id);
            var dbobj = _uniteOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (dbobj == null)
            {
                return NotFound();
            }
            return View(dbobj);
        }
        //Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _uniteOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _uniteOfWork.Category.Remove(obj);
            _uniteOfWork.Save();
            TempData["Success"] = "Category Removed Sucessfully";
            return RedirectToAction("Index");


        }
    }
}
