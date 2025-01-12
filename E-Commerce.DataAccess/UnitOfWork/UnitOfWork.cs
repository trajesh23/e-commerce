using E_Commerce.DataAccess.Context;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.DataAccess.Respositories;
using E_Commerce.DataAccess.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly EcommerceContext _context;

    private IUserRepository _userRepository;
    private IProductRepository _productRepository;
    private IOrderRepository _orderRepository;
    private IOrderProductRepository _orderProductRepository;

    public UnitOfWork(EcommerceContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _userRepository ??= new UserRepository(_context);
    public IProductRepository Products => _productRepository ??= new ProductRepository(_context);
    public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);
    public IOrderProductRepository OrderProducts => _orderProductRepository ??= new OrderProductRepository(_context);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
