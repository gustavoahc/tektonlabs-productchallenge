using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Core.Domain.Common;
using TektonLabs.Core.Domain.Entities;
using TektonLabs.Presentation.Api.ApiModels.Request;
using TektonLabs.Presentation.Api.ApiModels.Response;
using TektonLabs.Presentation.Api.ApiModels.SettingParameters;
using TektonLabs.Presentation.Api.Helpers.ExternalApi;
using TektonLabs.Presentation.Api.Helpers.Logging;

namespace TektonLabs.Presentation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;
        private readonly Parameter _parameter;
        private readonly ILogging _logging;
        private readonly Stopwatch _timer = new Stopwatch();

        public ProductsController(IProductService service
            , IMapper mapper
            , IAppCache cache
            , IOptions<Parameter> parameter
            , ILogging logging)
        {
            _service = service;
            _mapper = mapper;
            _cache = cache;
            _parameter = parameter.Value;
            _logging = logging;
        }

        /// <summary>
        /// Find product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the requested product</returns>
        /// <response code="200">Returns the requested product</response>
        /// <response code="404">If the specified id cannot be found</response>
        /// <response code="500">If an error occurs invoking the discounts api or database</response>
        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            _timer.Start();

            Product product = await _service.GetProductAsync(id);
            ProductResponse response = _mapper.Map<ProductResponse>(product);
            if (response == null)
            {
                return NotFound();
            }

            string discountsUrl = String.Format("{0}?productId={1}", _parameter.DiscountsUrl, id.ToString());
            HttpResponseMessage httpResponse = await ExternalAccess.GetJsonDataFromUrlAsync(discountsUrl);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return StatusCode(500);
            }

            List<DiscountResponse> discounts = await ExternalAccess.ConvertHttpResponseToObject<DiscountResponse>(httpResponse);
            if (discounts.Count == 0)
            {
                return NotFound();
            }

            int discount = discounts.Find(d => d.ProductId == id).Discount;
            response.Discount = discount;
            response.FinalPrice = response.Price * (100 - discount) / 100;

            Func<List<StatusData>> status = () => GetStatus();
            List<StatusData> statusData = _cache.GetOrAdd("statuscache", status, DateTimeOffset.Now.AddMinutes(5));

            response.StatusName = statusData.Find(s => s.Id == product.Status).Name;

            _logging.LogMessage(String.Format("Elapsed time: {0} milliseconds in endpoint Get({1})"
                                                , _timer.ElapsedMilliseconds.ToString()
                                                , id.ToString())
                );
            _timer.Stop();

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
        /// <returns>Returns the created product</returns>
        /// <response code="201">Returns the created product</response>
        /// <response code="400">If product's properties are wrong</response>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProductRequest product)
        {
            _timer.Start();

            Product productToInsert = _mapper.Map<Product>(product);
            Product result = await _service.InsertProductAsync(productToInsert);

            if (result == null)
            {
                return BadRequest(_service.GetValidationErrors());
            }

            _logging.LogMessage(String.Format("Elapsed time: {0} milliseconds in endpoint Post(product)"
                                                , _timer.ElapsedMilliseconds.ToString())
                );
            _timer.Stop();

            return new CreatedAtRouteResult("GetProduct", new { id = 1 }, result);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Result of update action</returns>
        /// <response code="204">If product is updated successfully</response>
        /// <response code="400">If product's properties are wrong</response>
        /// <response code="404">If the specified product id cannot be found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] Product product)
        {
            _timer.Start();

            Product productResponse = await _service.GetProductAsync(product.ProductId);
            if (productResponse == null)
            {
                return NotFound();
            }

            bool result = await _service.UpdateProductAsync(product);
            if (!result)
            {
                return BadRequest(_service.GetValidationErrors());
            }

            _logging.LogMessage(String.Format("Elapsed time: {0} milliseconds in endpoint Put(product)"
                                                , _timer.ElapsedMilliseconds.ToString())
                );
            _timer.Stop();

            return NoContent();
        }
    }
}
