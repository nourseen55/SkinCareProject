using Microsoft.AspNetCore.Mvc;
using MyShopProject.Models;
using MyShopProject.Repositories;
using MyShopProject.Utilities;
using MyShopProject.ViewModels;
using Stripe.BillingPortal;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using SessionService = Stripe.Checkout.SessionService;
using Session = Stripe.Checkout.Session;

namespace MyShopProject.Areas.customer.Controllers
{
    public class cartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCardVM ShoppingCardVM { get; set; }
        public cartController(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork;
        }
	
		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			var userid = claim.Value;

			var shoppingCarts = _unitOfWork.ShoppingCart.GetAll(x => x.UserId == userid, Includeword: "Product");
			var totalPrice = shoppingCarts.Sum(item => item.Count * item.Product.Price);

			ShoppingCardVM = new ShoppingCardVM()
			{
				ShoppingCarts = shoppingCarts,
				totalcarts = totalPrice
			};

			return View(ShoppingCardVM);
		}
        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCardVM = new ShoppingCardVM()
            {
                ShoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == claim.Value, Includeword: "Product"),
                OrderHeader = new()
            };

            ShoppingCardVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstorDefault(x => x.Id == claim.Value);

            ShoppingCardVM.OrderHeader.Name = ShoppingCardVM.OrderHeader.ApplicationUser.Name;
            ShoppingCardVM.OrderHeader.Address = ShoppingCardVM.OrderHeader.ApplicationUser.Address;
            ShoppingCardVM.OrderHeader.City = ShoppingCardVM.OrderHeader.ApplicationUser.City;
            ShoppingCardVM.OrderHeader.Email = ShoppingCardVM.OrderHeader.ApplicationUser.Email;
            ShoppingCardVM.OrderHeader.PhoneNumber = ShoppingCardVM.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var item in ShoppingCardVM.ShoppingCarts)
            {
                ShoppingCardVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            return View(ShoppingCardVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult POSTSummary(ShoppingCardVM ShoppingCardVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCardVM.ShoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == claim.Value, Includeword: "Product");

            ShoppingCardVM.OrderHeader.OrderStatus = SD.Pending;
            ShoppingCardVM.OrderHeader.PaymentStatus = SD.Pending;
            ShoppingCardVM.OrderHeader.OrderTime = DateTime.Now;
            ShoppingCardVM.OrderHeader.ApplicationUserId = claim.Value;

            foreach (var item in ShoppingCardVM.ShoppingCarts)
            {
                ShoppingCardVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            _unitOfWork.OrderHeader.Add(ShoppingCardVM.OrderHeader);
            _unitOfWork.Complete();

            foreach (var item in ShoppingCardVM.ShoppingCarts)
            {
                OrderDetails orderDetail = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = ShoppingCardVM.OrderHeader.Id, // Use the generated OrderHeader Id
                    Price = item.Product.Price,
                    Count = item.Count
                };

                _unitOfWork.OrderDetails.Add(orderDetail);
            }

            // Save the OrderDetails
            _unitOfWork.Complete();

            var domain = "https://localhost:44318/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"cart/orderconfirmation?id={ShoppingCardVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCardVM.ShoppingCarts)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCardVM.OrderHeader.SessionId = session.Id;

            // Update the OrderHeader with SessionId
            _unitOfWork.Complete();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult orderconfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstorDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.Updateorderstatus(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;

				_unitOfWork.Complete();
            }
            List<ShoppingCart> shoppingcarts = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingcarts);
            _unitOfWork.Complete();
            return View(id);
        }

        public IActionResult Plus(int cardid) {
            var shoppingcart=_unitOfWork.ShoppingCart.GetFirstorDefault(x=>x.Id == cardid);
            _unitOfWork.ShoppingCart.IncreaseCount(shoppingcart, 1);
            _unitOfWork.Complete();
        
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int cardid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstorDefault(x => x.Id == cardid);
            if (shoppingcart.Count<=1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingcart);
                var count=_unitOfWork.ShoppingCart.GetAll(x=>x.UserId==shoppingcart.UserId).ToList().Count()-1;  
                HttpContext.Session.SetInt32(SD.SessionKey, count);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);

            }
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }
        public IActionResult remove(int cardid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstorDefault(x => x.Id == cardid);
            _unitOfWork.ShoppingCart.Remove(shoppingcart);
            _unitOfWork.Complete();
            var count = _unitOfWork.ShoppingCart.GetAll(x => x.UserId == shoppingcart.UserId).ToList().Count() - 1;
            HttpContext.Session.SetInt32(SD.SessionKey, count);
            return RedirectToAction("Index");

        }


    }
}
