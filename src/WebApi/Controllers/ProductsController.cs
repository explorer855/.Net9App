using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProducts([FromBody] Product product)
        {
            try
            {
                return Ok(await _productService.AddProduct(product));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update Product details
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="product">Product Object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducts(Guid id, [FromBody] Product product)
        {
            return Ok(await _productService.UpdateProduct(id, product));
        }

        /// <summary>
        /// Update Product details partial method
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="product">Product Object</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartialProducts(Guid id, [FromBody] Product product)
        {
            return Ok(await _productService.UpdateProduct(id, product));
        }

        /// <summary>
        /// Fetch Product details by Product Id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> FetchProducts(Guid id)
        {
            return Ok(await _productService.GetProductById(id));
        }

        /// <summary>
        /// Remove Product by Product Id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(Guid id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
