using Microsoft.AspNetCore.Mvc;
using MyShopProject.Repositories;
using MyShopProject.Utilities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyShopProject.ViewComponents
{
    public class ShoppingcartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingcartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpContext.Session.Clear();

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var user = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (user != null)
            {
                int itemCount;
                if (HttpContext.Session.GetInt32(SD.SessionKey) != null)
                {
                    itemCount = HttpContext.Session.GetInt32(SD.SessionKey) ?? 0;
                }
                else
                {
                    itemCount =  _unitOfWork.ShoppingCart
                        .GetAll(x => x.UserId == user.Value).ToList()
                        .Count();

                    HttpContext.Session.SetInt32(SD.SessionKey, itemCount);
                }

                return View(itemCount);
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
