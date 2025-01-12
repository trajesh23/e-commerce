using E_Commerce.DataAccess.Respositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_Commerce.DataAccess.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository'lere erişim
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IOrderProductRepository OrderProducts { get; }

        // Transaction ve Save işlemleri
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
