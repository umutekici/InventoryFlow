using StockService.API.Domain.Models.Entities;

namespace StockService.API.Application.Interfaces
{
    public interface IProductRepository
    {
        void Add(Product product);
        List<Product> GetAll();
        List<Product> GetLowStockProducts();
    }
}
