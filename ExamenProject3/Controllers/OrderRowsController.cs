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
    [Authorize]
    public class OrderRowsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OrderRowsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(ICollection<OrderRowRequest> orderrows)
        {
            List<OrderRowEntity> orderRowEntities = new List<OrderRowEntity>();
            foreach (OrderRowRequest order in orderrows)
            {
                orderRowEntities.Add(new OrderRowEntity
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    ArticaleNumber = order.ArticaleNumber,
                    ProductName = order.ProductName,
                    Quantity = order.Quantity,
                    ProductPrice = order.ProductPrice

                });
            }
            _dataContext.OrderRows.AddRange(orderRowEntities);   
            await _dataContext.SaveChangesAsync();  
            return Ok("We have recived your order");
        }
        
        [HttpGet("Order/{id}")]
        public async Task<IActionResult> GetByOrderId(int id)
        {
            var requestrows = new List<OrderRowRequest>();
            var orderrows = await _dataContext.OrderRows.Where(r => r.OrderId == id).ToListAsync();
            if (orderrows.Count == 0)
                return BadRequest("This order is empty");
            foreach(var orderrow in orderrows)
            {
                requestrows.Add(new OrderRowRequest
                {
                    CustomerId=orderrow.CustomerId,
                    OrderId=orderrow.OrderId,
                    ArticaleNumber=orderrow.ArticaleNumber,
                    ProductName=orderrow.ProductName,
                    ProductPrice=orderrow.ProductPrice,
                    Quantity = orderrow.Quantity,   
                }
                );
            }
            return Ok(requestrows);
        }

        [HttpGet("Customer/{id}")]
        public async Task<IActionResult> GetByCustomerId(int id)
        {
            var allUserOrders = await _dataContext.OrderRows.Where(or => or.CustomerId == id).ToListAsync();
            var orderRows = new List<OrderRowModel>();
            var order = new OrderEntity();
            if (allUserOrders.Count == 0)
                return BadRequest("You have no orders yet");
            foreach (var orderrow in allUserOrders)
            {
                order = await _dataContext.Orders.FindAsync(orderrow.OrderId);
                orderRows.Add(new OrderRowModel
                {
                    OrderRowId = orderrow.Id,
                    OrderId = orderrow.OrderId,
                    ProductName = orderrow.ProductName,
                    ProductPrice = orderrow.ProductPrice,
                    ArticaleNumber = orderrow.ArticaleNumber,
                    Quantity = orderrow.Quantity,
                    OrderDate = order!.OrderDate

                });
            }
            return Ok(orderRows);
           
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRows()
        {
            var orderRows = new List<OrderRowModel>();
            return Ok(await _dataContext.OrderRows.ToListAsync());
        }


        [HttpDelete("deleterow/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderrow = await _dataContext.OrderRows.FindAsync(id);

            if (orderrow == null)
                return NotFound();

           
            var order = await _dataContext.Orders.FindAsync(orderrow.OrderId);
            if (order == null)
                return NotFound();
            if(order.DueDate < DateTime.Now)
            {
                return BadRequest("Order cannot be deleted after 24 hours");
            }
                _dataContext.OrderRows.Remove(orderrow);
            await _dataContext.SaveChangesAsync();

            var orderrows = await _dataContext.OrderRows.Where(or => or.OrderId == orderrow.OrderId).ToListAsync();
            if (orderrows.Count == 0)
            {
                _dataContext.Orders.Remove(order);
                await _dataContext.SaveChangesAsync();
            }
            return Ok("Product deleted successfully from the this order");
            
        }
    }
}
