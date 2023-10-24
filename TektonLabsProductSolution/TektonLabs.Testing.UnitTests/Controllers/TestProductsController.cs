using AutoMapper;
using FluentAssertions;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Core.Domain.Common;
using TektonLabs.Core.Domain.Entities;
using TektonLabs.Presentation.Api.ApiModels.Request;
using TektonLabs.Presentation.Api.ApiModels.Response;
using TektonLabs.Presentation.Api.ApiModels.SettingParameters;
using TektonLabs.Presentation.Api.Controllers;
using TektonLabs.Presentation.Api.Helpers.Logging;
using TektonLabs.Presentation.Api.Helpers.Mapping;
using TektonLabs.Testing.UnitTests.TestData;

namespace TektonLabs.Testing.UnitTests.Controllers
{
    public class TestProductsController
    {
        private Mock<IProductService> _service;
        private readonly IMapper _mapper;
        private Mock<IAppCache> _cache;
        private IOptions<Parameter> _parameters;
        private IOptions<Parameter> _parametersError;
        private Mock<ILogging> _logging;

        public TestProductsController()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingConfiguration());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void Setup()
        {
            _service = new Mock<IProductService>();
            _cache = new Mock<IAppCache>();
            _logging = new Mock<ILogging>();

            _parameters = Options.Create(new Parameter()
            {
                DiscountsUrl = "https://653182fb4d4c2e3f333d17ac.mockapi.io/api/v1/discounts"
            });
            _parametersError = Options.Create(new Parameter()
            {
                DiscountsUrl = "https://653182fb4d3f333d17ac.mockapi.io/api/v1/discounts"
            });
        }

        [Test]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            _service.Setup(s => s.GetProductAsync(1))
                .ReturnsAsync(ProductTestData.GetProduct());

            _cache.Setup(c => c.GetOrAdd(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, List<StatusData>>>(), It.IsAny<MemoryCacheEntryOptions>()))
                .Returns(ProductTestData.GetStatusDataList());

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object, _parameters, _logging.Object);

            var result = (OkObjectResult)await controller.Get(1);

            result.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task Get_OnSuccess_ReturnsProduct()
        {
            _service.Setup(s => s.GetProductAsync(1))
                .ReturnsAsync(ProductTestData.GetProduct());

            _cache.Setup(c => c.GetOrAdd(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, List<StatusData>>>(), It.IsAny<MemoryCacheEntryOptions>()))
                .Returns(ProductTestData.GetStatusDataList());

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object, _parameters, _logging.Object);

            var result = (OkObjectResult)await controller.Get(1);

            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<ProductResponse>();
        }

        [Test]
        public async Task Get_OnNoProductFound_ReturnsStatusCode404()
        {
            var controller = new ProductsController(_service.Object, _mapper, _cache.Object, _parameters, _logging.Object);

            var result = (NotFoundResult)await controller.Get(1);

            result.StatusCode.Should().Be(404);
        }

        [Test]
        public async Task Get_OnErrorConsumingDiscountService_ReturnsStatusCode500()
        {
            _service.Setup(s => s.GetProductAsync(1))
                .ReturnsAsync(ProductTestData.GetProduct());

            _cache.Setup(c => c.GetOrAdd(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, List<StatusData>>>(), It.IsAny<MemoryCacheEntryOptions>()))
                .Returns(ProductTestData.GetStatusDataList());

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object, _parametersError, _logging.Object);

            var result = (StatusCodeResult)await controller.Get(1);

            result.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task Post_OnSuccessCreating_ReturnsStatusCode201()
        {
            Product product = ProductTestData.GetProduct();
            _service.Setup(s => s.InsertProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object, _parameters, _logging.Object);
            ProductRequest productRequest = _mapper.Map<ProductRequest>(product);

            var result = (CreatedAtRouteResult)await controller.Post(productRequest);

            result.StatusCode.Should().Be(201);
        }

        [Test]
        public async Task Post_OnValidationError_ReturnsStatusCode400()
        {
            _service.Setup(s => s.GetValidationErrors())
                .Returns(new List<FluentValidation.Results.ValidationFailure>());

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object, _parameters, _logging.Object);
            ProductRequest productRequest = new ProductRequest();

            var result = (BadRequestObjectResult)await controller.Post(productRequest);

            result.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task Put_OnSuccess_ReturnsStatusCode201()
        {
            _service.Setup(s => s.GetProductAsync(1))
                .ReturnsAsync(ProductTestData.GetProduct());
            _service.Setup(s => s.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object, _parameters, _logging.Object);

            var result = (NoContentResult)await controller.Put(ProductTestData.GetUpdatedProduct());

            result.StatusCode.Should().Be(204);
        }

        [Test]
        public async Task Put_OnSuccess_ReturnsStatusCode400()
        {
            _service.Setup(s => s.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(ProductTestData.GetProduct());

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object, _parameters, _logging.Object);

            var result = (BadRequestObjectResult)await controller.Put(ProductTestData.GetInvalidProduct());

            result.StatusCode.Should().Be(400);
        }
    }
}
