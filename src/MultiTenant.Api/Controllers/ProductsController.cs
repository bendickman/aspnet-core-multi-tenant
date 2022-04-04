using HashidsNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Api.Controllers.Requests;
using MultiTenant.Core.Entities;
using MultiTenant.Core.Interfaces;

namespace MultiTenant.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/products/")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Tenant")]
    
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IHashids _hashids;

        public ProductsController(
            IProductService service,
            IHashids hashids)
        {
            _service = service;
            _hashids = hashids;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(500)]
        [Route("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var ids = _hashids
                .Decode(id);

            if (!ids?.Any() ?? true)
            {
                return NotFound();
            }

            var product = await _service
                .GetByIdAsync(ids.First());

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<Product>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> List()
        {
            var products = await _service
                .GetAllAsync();

            if (products is null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAsync(CreateProductRequest request)
        {
            var product = await _service
                .CreateAsync(request.Name, request.Description, request.Rate);

            if (product is null)
            {
                return BadRequest();
            }

            var url = Url.Link(
                "GetProduct", 
                new { id = product.Id });

            return Created(
               url,
                product);
        }
    }
}
