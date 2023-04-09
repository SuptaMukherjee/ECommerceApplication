using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _uniteOfWork;
        

        public CompanyController(IUnitOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
          
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
        public IActionResult Create(Company obj)
        {
            if (obj.Name.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    _uniteOfWork.Company.Add(obj);
                    _uniteOfWork.Save();
                    TempData["Success"] = "Company created Sucessfully";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                ModelState.AddModelError("Name", "COMPANY WITH THIS NAME ALREADY EXIST");
            }
            return View(obj);

        }
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            
            if (id == null || id == 0)
            {
                ////Create Company
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(company);
            }
            else
            {
                company = _uniteOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
                //update Company
            }

            
        }
        //Put
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "Display order can not be exactly match with Name");
            //}
            if (ModelState.IsValid)
            {
                
                if (obj.Id == 0)
                {
                    _uniteOfWork.Company.Add(obj);
                    TempData["Success"] = "Company Created Sucessfully";
                }
                else
                {
                    _uniteOfWork.Company.Update(obj);
                    TempData["Success"] = "Company Updated Sucessfully";
                }
                _uniteOfWork.Save();
                
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
        //    var dbobj = _uniteOfWork.Company.GetFirstOrDefault(c => c.Id == id);
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
            var Companylist = _uniteOfWork.Company.GetAll();
            return Json(new {data=Companylist});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _uniteOfWork.Company.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return Json(new {success=false,message ="Error While Deleting"});
            }
           
            _uniteOfWork.Company.Remove(obj);
            _uniteOfWork.Save();
            return Json(new { success = true, message = "Company deleted Succesfully" });
        }
        #endregion
    }


}
