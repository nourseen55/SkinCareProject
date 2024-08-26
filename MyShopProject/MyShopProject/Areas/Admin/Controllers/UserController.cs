using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShopProject.Data;
using MyShopProject.Repositories;
using MyShopProject.Utilities;
using System.Security.Claims;

namespace MyShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(ApplicationDbContext context,IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public IActionResult dashboard()
        {
            ViewBag.Orders = _unitOfWork.OrderHeader.GetAll().Count();
            ViewBag.ApprovedOrders = _unitOfWork.CategoryRepository.GetAll().Count();
            ViewBag.Users = _unitOfWork.ApplicationUser.GetAll().Count();
            ViewBag.Products = _unitOfWork.Product.GetAll().Count();
            return View();
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;
            return View(_context.ApplicationUsers.Where(x => x.Id != userid));
        }
        public IActionResult LockUnLock(string? Id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == Id);
            if (user != null)
            {
                if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
                {
                    user.LockoutEnd = DateTime.Now.AddDays(1);

                }
                else
                {
                    user.LockoutEnd = DateTime.Now;
                }
            }
            else
            {
                return NotFound();
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
