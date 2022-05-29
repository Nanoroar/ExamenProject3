using AutoMapper;
using ExamenProject3.Data;
using ExamenProject3.Models.Order;
using ExamenProject3.Models.Product;

namespace ExamenProject3.Services
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(IEnumerable<Product> shoppingcart, User user);
    }
    public class OrderService: IOrderService
    {
        private readonly DataContext _db;
        private readonly IMapper _map;

        public OrderService(DataContext db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        public Task<Order> CreateAsync(IEnumerable<Product> shoppingcart, User user)
        {
            throw new NotImplementedException();
        }
    }
}
