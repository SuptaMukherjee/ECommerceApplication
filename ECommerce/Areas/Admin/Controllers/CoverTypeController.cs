using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _uniteOfWork;

        public CoverTypeController(IUnitOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _uniteOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _uniteOfWork.CoverType.Add(obj);
                _uniteOfWork.Save();
                TempData["Success"] = "CoverType created Sucessfully";
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
            var dbobj = _uniteOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
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
        public IActionResult Edit(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _uniteOfWork.CoverType.Update(obj);
                _uniteOfWork.Save();
                TempData["Success"] = "CoverType Updated Sucessfully";
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
            var dbobj = _uniteOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
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
            var obj = _uniteOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _uniteOfWork.CoverType.Remove(obj);
            _uniteOfWork.Save();
            TempData["Success"] = "CoverType Removed Sucessfully";
            return RedirectToAction("Index");


        }
    }
}
