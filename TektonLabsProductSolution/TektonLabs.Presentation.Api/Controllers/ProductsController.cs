using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Core.Domain.Common;
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
        private readonly IAppCache _cache;

        public ProductsController(IProductService service, IMapper mapper, IAppCache cache)
        {
            _service = service;
            _mapper = mapper;
            _cache = cache;
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
            if (response == null)
            {
                return NotFound();
            }

            Func<List<StatusData>> status = () => GetStatus();
            List<StatusData> statusData = _cache.GetOrAdd("statuscache", status, DateTimeOffset.Now.AddMinutes(5));

            response.StatusName = statusData.Find(s => s.Id == product.Status).Name;

            return Ok(response);
        }

        private List<StatusData> GetStatus()
        {
            return _service.GetStatusData();
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

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Product product)
        {
            bool result = await _service.UpdateProductAsync(product);
            if (!result)
            {
                return BadRequest(_service.GetValidationErrors());
            }

            return NoContent();
        }
    }
}
