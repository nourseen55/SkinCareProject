using Microsoft.AspNetCore.Mvc;
using MyShopProject.Models;
using MyShopProject.Repositories;

namespace MyShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCat()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCat(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Categories.Add(category);
                _unitOfWork.CategoryRepository.Add(category);
                //_context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            //var categoryIndb = _context.Categories.Find(id);

            var categoryIndb = _unitOfWork.CategoryRepository.GetFirstorDefault(x => x.Id == id);

            return View(categoryIndb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Categories.Update(category);

                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.Complete();
                //_context.SaveChanges();
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var categoryIndb = _unitOfWork.CategoryRepository.GetFirstorDefault(x => x.Id == id);

            return View(categoryIndb);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int? id)
        {
            var categoryIndb = _unitOfWork.CategoryRepository.GetFirstorDefault(x => x.Id == id);
            if (categoryIndb == null)
            {
                NotFound();
            }
            _unitOfWork.CategoryRepository.Remove(categoryIndb);
            //_context.Categories.Remove(categoryIndb);
            //_context.SaveChanges();
            _unitOfWork.Complete();
            TempData["Delete"] = "Item has Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
