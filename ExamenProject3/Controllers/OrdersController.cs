using ExamenProject3.Data;
using ExamenProject3.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamenProject3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OrdersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(OrderRequest request)
        {

            var order = new OrderEntity
            {
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                Address = request.Address,
                OrderDate = request.OrderDate,
                DueDate = request.DueDate,
                OrderStatus = request.OrderStatus
            };
            _dataContext.Orders.Add(order); 
            await _dataContext.SaveChangesAsync();
            return  Ok(order.Id);

            
        }
    }
}
