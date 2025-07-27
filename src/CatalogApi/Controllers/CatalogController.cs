using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Services;
using WebApi.Models.Entities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _CatalogService;
        public CatalogController(ICatalogService CatalogService)
        {
            _CatalogService = CatalogService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCatalogs([FromBody] CatalogItem Catalog)
        {
            try
            {
                return Ok(await _CatalogService.AddCatalog(Catalog));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update Catalog details
        /// </summary>
        /// <param name="id">Catalog Id</param>
        /// <param name="Catalog">Catalog Object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCatalogs(Guid id, [FromBody] CatalogItem Catalog)
        {
            return Ok(await _CatalogService.UpdateCatalog(id, Catalog));
        }

        /// <summary>
        /// Update Catalog details partial method
        /// </summary>
        /// <param name="id">Catalog Id</param>
        /// <param name="Catalog">Catalog Object</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartialCatalogs(Guid id, [FromBody] CatalogItem Catalog)
        {
            return Ok(await _CatalogService.UpdateCatalog(id, Catalog));
        }

        /// <summary>
        /// Fetch Catalog details by Catalog Id
        /// </summary>
        /// <param name="id">Catalog Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> FetchCatalogs(Guid id)
        {
            return Ok(await _CatalogService.GetCatalogById(id));
        }

        /// <summary>
        /// Get all Catalogs Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        public async Task<IActionResult> FetchCatalogsByCategory()
        {
            return Ok(await _CatalogService.GetCatalogCategories());
        }

        /// <summary>
        /// Remove Catalog by Catalog Id
        /// </summary>
        /// <param name="id">Catalog Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCatalog(Guid id)
        {
            await _CatalogService.DeleteCatalog(id);
            return NoContent();
        }
    }
}
