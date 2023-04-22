using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System.Security.Claims;

namespace ECommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM  ShoppingCartVM { get; set; }
        public  int OrderTotal { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentiy = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentiy.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM() 
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u=> u.ApplicationUserId == claim.Value,includeProperties:"Product")
            };
            foreach(var cart in shoppingCartVM.ListCart)
            {
                cart.Price = GetPriceOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                shoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }
            return View(shoppingCartVM);
        }

        private double GetPriceOnQuantity(double quantity,double price,double price50 ,double price100)
        {
            if(quantity <= 50)
            {
                return price;
            }
            else 
            {
                if (quantity <= 100)
                {
                    return price50;
                }
                return price100;
            }
          
        }
    }
}
