using Microsoft.AspNetCore.Mvc;
using TektonLabs.Presentation.Api.ApiModels.Response;

namespace TektonLabs.Presentation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController()
        {

        }

        /// <summary>
        /// Find product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> Get(int id)
        {
            ProductResponse response = new ProductResponse();
            if (id == 0)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
