using ExamenProject3.Data;
using ExamenProject3.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return Ok(order.Id);


        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var ordermodels = new List<OrderModel>();
            var orders = await _dataContext.Orders.ToListAsync();
            foreach (var order in orders)
            {
                ordermodels.Add(new OrderModel()
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = order.CustomerName,
                    Address = order.Address,
                    OrderDate = order.OrderDate,
                    DueDate = order.DueDate,
                    OrderStatus = order.OrderStatus
                });
            }
            return Ok(ordermodels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _dataContext.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);   
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id ,OrderModel model)
        {
            var order = await _dataContext.Orders.FindAsync(id);

            if(order == null)
                return NotFound("Not found");
            order.Id = id;
            order.CustomerId = model.CustomerId;
            order.CustomerName = model.CustomerName;
            order.Address = model.Address;
            order.OrderStatus = model.OrderStatus;

            await _dataContext.SaveChangesAsync();
            var updatedOrder = new OrderModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                Address = order.Address,
                OrderStatus = order.OrderStatus
            };
            return Ok(updatedOrder);   
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _dataContext.Orders.FindAsync(id);
            if (order == null)
                return NotFound("Not found");
            var orderRows = await _dataContext.OrderRows.Where(or => or.OrderId == id).ToListAsync();


            _dataContext.OrderRows.RemoveRange(orderRows);  
            _dataContext.Orders.Remove(order);
            await _dataContext.SaveChangesAsync();  
            return Ok("Order deleted successfully");
        }
    }
}
