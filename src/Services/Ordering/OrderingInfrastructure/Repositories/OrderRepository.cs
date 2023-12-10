using Microsoft.EntityFrameworkCore;
using OrderingApplication.Contracts.Persistence;
using OrderingDomain.Entities;
using OrderingInfrastructure.Persistence;

namespace OrderingInfrastructure.Repositories;
public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderContext dbContext) : base(dbContext)
    {

    }
    public async Task<IEnumerable<Order>> GetOrdersByUserName(string username)
    {
        return await _context.Orders.Where(o => o.UserName == username).ToListAsync();

    }
}
