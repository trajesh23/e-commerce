using E_Commerce.DataAccess.Context;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DataAccess.Respositories
{
    public class OrderProductRepository : IOrderProductRepository
    {
        private readonly EcommerceContext _context;

        public OrderProductRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(OrderProduct entity)
        {
            // Add to the database
            await _context.OrderProducts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            // Find the order to be deleted
            var orderProduct = await _context.OrderProducts.FindAsync(id);

            // Remove order
            _context.OrderProducts.Remove(orderProduct!);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderProduct>> GetAllAsync()
        {
            return await _context.OrderProducts.ToListAsync(); // Get all orders in a list
        }

        public async Task<OrderProduct> GetByIdAsync(int id)
        {
            // Find requested order
            var orderProduct = await _context.OrderProducts.FirstOrDefaultAsync(o => o.OrderId == id);

            return orderProduct!;
        }

        //public async Task<OrderProduct> GetByProductIdAsync(int id)
        //{
        //    // Find requested order
        //    var productOrder = await _context.OrderProducts.FirstOrDefaultAsync(o => o.ProductId == id);

        //    return productOrder!;
        //}

        public async Task UpdateAsync(OrderProduct entity)
        {
            // Update order
            _context.OrderProducts.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

