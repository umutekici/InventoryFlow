using StockService.API.Application.Dtos;

namespace StockService.API.Application.Interfaces
{
    public interface IProductService
    {
        void AddProduct(CreateProductRequest request);
        List<LowStockProductDto> GetLowStockProducts();
    }
}
