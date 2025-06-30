using StockService.API.Application.Dtos;
using StockService.API.Application.Interfaces;
using StockService.API.Domain.Models.Entities;

namespace StockService.API.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(CreateProductRequest request)
        {
            var product = new Product(request.Name, request.Category, request.InitialStock, request.CriticalStock);
            _productRepository.Add(product);
        }

        public List<LowStockProductDto> GetLowStockProducts()
        {
            var lowStockProducts = _productRepository.GetLowStockProducts();

            return lowStockProducts.Select(p => new LowStockProductDto
            {
                Name = p.Name,
                Stock = p.Stock,
                CriticalStock = p.CriticalStock,
                Code = p.Code
            }).ToList();
        }
    }
}
