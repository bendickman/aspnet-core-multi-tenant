using Microsoft.AspNetCore.Mvc;
using MultiTenant.Api.Controllers.Requests;
using MultiTenant.Core.Entities;
using MultiTenant.Core.Interfaces;

namespace MultiTenant.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/product/")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(500)]
        [Route("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var product = await _service
                .GetByIdAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
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
