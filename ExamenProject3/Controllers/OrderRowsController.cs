using ExamenProject3.Data;
using ExamenProject3.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamenProject3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderRowsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OrderRowsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ICollection<OrderRowRequest> orderrows)
        {
            List<OrderRowEntity> orderRowEntities = new List<OrderRowEntity>();
            foreach (OrderRowRequest order in orderrows)
            {
                orderRowEntities.Add(new OrderRowEntity
                {
                    OrderId = order.OrderId,
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
    }
}
