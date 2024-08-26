using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopProject.Data;
using MyShopProject.Implementation;
using MyShopProject.Models;
using MyShopProject.Repositories;
using MyShopProject.Utilities;
using MyShopProject.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace MyShopProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork unitOfWork;
		private readonly ApplicationDbContext _context;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork,ApplicationDbContext context)
		{
			_logger = logger;
			this.unitOfWork = unitOfWork;
			_context = context;
		}
        public IActionResult Details(int ProductId)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = ProductId,
                Product = unitOfWork.Product.GetFirstorDefault(v => v.Id == ProductId, Includeword: "Category"),
                Count = 1
            };
            return View(obj);
        }



        [HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public IActionResult Details(ShoppingCart shoppingCart)
		{
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;
			shoppingCart.UserId = userid;
			ShoppingCart oldcart=unitOfWork.ShoppingCart.GetFirstorDefault(x=>x.UserId == userid&&x.ProductId==shoppingCart.ProductId);
			if (oldcart != null) 
			{
				unitOfWork.ShoppingCart.IncreaseCount(oldcart,shoppingCart.Count);
                unitOfWork.Complete();

            }
            else
			{
                unitOfWork.ShoppingCart.Add(shoppingCart);
                unitOfWork.Complete();
                HttpContext.Session.SetInt32(SD.SessionKey, unitOfWork.ShoppingCart.GetAll(x => x.UserId == userid).ToList().Count());


            }

            return RedirectToAction("Index", "Home");

        }
	
        public IActionResult Index(int?page)
		{
			int pagenum = page ?? 1;
			int pagesize = 8;
			
			var pros = unitOfWork.Product.GetAll().ToPagedList(pagenum,pagesize);
			return View(pros);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
