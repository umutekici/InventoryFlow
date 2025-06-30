namespace StockService.API.Domain.Models.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Code { get; set; }

        public string? Name { get; set; }
        public string? Category { get; set; }

        public int CriticalStock { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsLowStock() => Stock < CriticalStock;

        public Product(string? name, string? category, int initialStock, int criticalStock, int? code = 0)
        {
            Name = name;
            Category = category;
            Stock = initialStock;
            CriticalStock = criticalStock;
            CreatedAt = DateTime.UtcNow;
            Id = Guid.NewGuid();
            Code = code ?? 0;
        }
    }
}
