using System.ComponentModel.DataAnnotations;

namespace StockService.API.Application.Dtos
{
    public class CreateProductRequest
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Category { get; set; }

        [Range(0, int.MaxValue)]
        public int CriticalStock { get; set; }

        [Range(0, int.MaxValue)]
        public int InitialStock { get; set; }
    }
}
