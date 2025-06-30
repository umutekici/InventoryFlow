using Microsoft.AspNetCore.Mvc;
using StockService.API.Application.Dtos;
using StockService.API.Application.Interfaces;
using StockService.API.Application.Services;
using StockService.API.Helpers;

namespace StockService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly StockManager _stockManager;
        private readonly RomanNumeralConverter _romanNumeralConverter;

        public ProductController(IProductService productService, StockManager stockManager, RomanNumeralConverter romanNumeralConverter)
        {
            _productService = productService;
            _stockManager = stockManager;
            _romanNumeralConverter = romanNumeralConverter;
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="request">The product creation request containing necessary product details.</param>
        /// <returns>
        /// Returns a success message and the created product if the request is valid;
        /// otherwise, returns validation errors.
        /// </returns>

        [HttpPost]
        public IActionResult AddProduct([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productService.AddProduct(request);

            return Ok(new
            {
                message = "Product has been successfully added.",
                product = request
            });
        }

        /// <summary>
        /// Retrieves a list of products that are currently low in stock.
        /// </summary>
        /// <returns>A list of low-stock product DTOs.</returns>

        [HttpGet("low-stock")]
        public ActionResult<List<LowStockProductDto>> GetLowStockProducts()
        {
            var lowStockProducts = _productService.GetLowStockProducts();
            return lowStockProducts;
        }

        /// <summary>
        /// Checks stock levels and places order requests for products that are below the critical stock threshold.
        /// </summary>
        /// <returns>
        /// A message indicating the order request was queued, along with the list of affected products.
        /// </returns>
        /// 
        [HttpPost("order/check-and-place")]
        public async Task<IActionResult> CheckAndPlaceOrdersAsync()
        {
            var lowStockProducts = _productService.GetLowStockProducts();

            await _stockManager.CheckAndRequestOrderAsync(lowStockProducts);

            return Ok(new
            {
                Message = "Order requests for low-stock products have been successfully queued for processing.",
                RequestedProducts = lowStockProducts.Select(p => new
                {
                    p.Name,
                    p.Code,
                    p.Stock,
                    p.CriticalStock
                })
            });
        }

        /// <summary>
        /// Converts a numeric stock quantity into Roman numeral representation.
        /// </summary>
        /// <param name="number">The stock number to be converted to a Roman numeral.</param>
        /// <returns>An object containing the original number, its Roman numeral, and a display string.</returns>
        
        [HttpGet("stock-roman")]
        public IActionResult GetStockAsRoman(int number)
        {
            string roman = _romanNumeralConverter.ToRoman(number);

            return Ok(new
            {
                Stock = number,
                Roman = roman,
                Display = $"Stok: {roman} adet"
            });
        }
    }
}
