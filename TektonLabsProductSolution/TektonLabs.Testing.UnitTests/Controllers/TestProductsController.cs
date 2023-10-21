using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TektonLabs.Presentation.Api.ApiModels.Response;
using TektonLabs.Presentation.Api.Controllers;

namespace TektonLabs.Testing.UnitTests.Controllers
{
    public class TestProductsController
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            var controller = new ProductsController();

            var result = (OkObjectResult)await controller.Get(1);

            result.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task Get_OnSuccess_ReturnsProduct()
        {
            var controller = new ProductsController();

            var result = (OkObjectResult)await controller.Get(1);

            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<ProductResponse>();
        }

        [Test]
        public async Task Get_OnNoProductFound_ReturnsStatusCode404()
        {
            var controller = new ProductsController();

            var result = (NotFoundResult)await controller.Get(0);

            result.StatusCode.Should().Be(404);
        }
    }
}
