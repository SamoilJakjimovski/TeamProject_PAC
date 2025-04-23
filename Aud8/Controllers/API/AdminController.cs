//using domain.DTO;
//using domain.Identity;
//using domain.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using service.implementation;
//using service.Interface;

//namespace Aud8.Controllers.API
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AdminController : ControllerBase
//    {
//        private readonly IOrderService _orderService;
//        private readonly IShoppingCartService _shoppingCartService;
//        private readonly UserManager<PetAdoptionCenterUser> userManager;

//        public AdminController(IOrderService orderService, IShoppingCartService shoppingCartService, UserManager<PetAdoptionCenterUser> userManager)
//        {
//            _orderService = orderService;
//            _shoppingCartService = shoppingCartService;
//            this.userManager = userManager;
//        }

//        [HttpGet("[action]")]
//        public List<Order> GetAllActiveOrders()
//        {
//            return _orderService.GetAllOrders();
//        }

//        [HttpGet("[action]/{id}")]
//        public Order GetAllActiveOrders(Guid id)
//        {
           
//            Order order =  _orderService.GetOrderDetails(id);
//            /*order.ProductInOrders = _shoppingCartService.getProductInOrderList(id);*/
//            return order;
//        }

//        [HttpPost("[action]")]
//        public bool ImportAllUsers(List<UserRegistrationDto> model)
//        {
//            bool status = true;
//            foreach (var item in model)
//            {
//                var userCheck = userManager.FindByEmailAsync(item.Email).Result;
//                if(userCheck == null)
//                {
//                    var user = new PetAdoptionCenterUser
//                    {
//                        UserName = item.Email,
//                        NormalizedUserName = item.Email,
//                        Email = item.Email,
//                        EmailConfirmed = true,
//                        PhoneNumberConfirmed = true,
//                        UserCart = new ShoppingCart()
//                    };
//                    var result = userManager.CreateAsync(user, item.Password).Result;

//                    status = status && result.Succeeded;
//                }
//                else
//                {
//                    continue;
//                }
//            }

//            return status;
//        }
//    }
//}
