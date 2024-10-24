using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Route.APIs.Errors;
using Store.Route.Core;
using Store.Route.Core.DTOs.Orders;
using Store.Route.Core.Entities.Order;
using Store.Route.Core.Services.Contract;
using System.Security.Claims;

namespace Store.Route.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork,IOrderService orderService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var address=  _mapper.Map<Address>(model.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(userEmail, model.BasketId, model.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
          
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }


        [Authorize]
        [HttpGet] 
        public async Task<IActionResult> GetOrderForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
           
            var orders = await _orderService.GetOrdersForSpecificUserAsync(userEmail);
            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(_mapper.Map<IEnumerable<OrderToReturnDto>>(orders));
        }


        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderForSpecificUser(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var order = await _orderService.GetOrderByIdForSpecificUserAsync(userEmail,orderId);
            if (order is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
            if (deliveryMethods is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(deliveryMethods);


        }
    }
}
