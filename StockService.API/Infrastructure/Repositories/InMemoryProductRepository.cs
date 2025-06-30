using StockService.API.Application.Interfaces;
using StockService.API.Domain.Models.Entities;

namespace StockService.API.Infrastructure.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public InMemoryProductRepository()
        {
            _products = new List<Product>
            {
                new Product("Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops","men's clothing", 10, 5, 1),
                new Product("Mens Casual Premium Slim Fit T-Shirts","men's clothing", 12, 10, 2),
                new Product("Mens Cotton Jacket","men's clothing", 3, 7, 3),
                new Product("DANVOUY Womens T Shirt Casual Cotton Shor", "women's clothing", 1, 2, 20),
                new Product("Solid Gold Petite Micropave","jewelery", 5, 3, 6),
            };
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public List<Product> GetAll()
        {
            return _products.ToList();
        }

        public List<Product> GetLowStockProducts()
        {
            return _products.Where(p => p.IsLowStock()).ToList();
        }
    }
}
