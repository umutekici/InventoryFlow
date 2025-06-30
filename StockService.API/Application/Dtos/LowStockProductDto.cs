namespace StockService.API.Application.Dtos
{
    public class LowStockProductDto
    {
        public int Code { get; set; }
        public string? Name { get; set; }
        public int Stock { get; set; }
        public int CriticalStock { get; set; }
    }
}
