using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Core.Domain.Entities;
using TektonLabs.Presentation.Api.ApiModels.Request;
using TektonLabs.Presentation.Api.ApiModels.Response;

namespace TektonLabs.Presentation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductsController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Find product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> Get(int id)
        {
            Product product = await _service.GetProductAsync(id);
            ProductResponse response = _mapper.Map<ProductResponse>(product);
            if (response != null)
            {
                return Ok(response);
            }

            return NotFound();
        }

        /// <summary>
        /// Add a new product to database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductRequest product)
        {
            Product productToInsert = _mapper.Map<Product>(product);
            Product result = await _service.InsertProductAsync(productToInsert);

            if (result == null)
            {
                return BadRequest(_service.GetValidationErrors());
            }

            return new CreatedAtRouteResult("GetProduct", new { id = 1 }, result);
        }
    }
}
