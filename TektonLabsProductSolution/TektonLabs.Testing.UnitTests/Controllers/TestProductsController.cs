using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Core.Domain.Entities;
using TektonLabs.Presentation.Api.ApiModels.Request;
using TektonLabs.Presentation.Api.ApiModels.Response;
using TektonLabs.Presentation.Api.Controllers;
using TektonLabs.Presentation.Api.Helpers.Mapping;

namespace TektonLabs.Testing.UnitTests.Controllers
{
    public class TestProductsController
    {
        private Mock<IProductService> _service;
        private readonly IMapper _mapper;

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
        }

        [Test]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            Product product = new Product { ProductId = 1, Name = "Product", Price = 1, Status = 1, Stock = 1 };
            _service.Setup(s => s.GetProductAsync(1))
                .ReturnsAsync(product);
            var controller = new ProductsController(_service.Object, _mapper);

            var result = (OkObjectResult)await controller.Get(1);

            result.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task Get_OnSuccess_ReturnsProduct()
        {
            Product product = new Product { ProductId = 1, Name = "Product", Price = 1, Status = 1, Stock = 1 };
            _service.Setup(s => s.GetProductAsync(1))
                .ReturnsAsync(product);
            var controller = new ProductsController(_service.Object, _mapper);

            var result = (OkObjectResult)await controller.Get(1);

            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<ProductResponse>();
        }

        [Test]
        public async Task Get_OnNoProductFound_ReturnsStatusCode404()
        {
            var controller = new ProductsController(_service.Object, _mapper);

            var result = (NotFoundResult)await controller.Get(1);

            result.StatusCode.Should().Be(404);
        }

        [Test]
        public async Task Get_OnSuccessCreating_ReturnsStatusCode201()
        {
            Product product = new Product { ProductId = 1, Name = "Product", Price = 1, Status = 1, Stock = 1 };
            _service.Setup(s => s.InsertProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

            var controller = new ProductsController(_service.Object, _mapper);
            ProductRequest productRequest = _mapper.Map<ProductRequest>(product);

            var result = (CreatedAtRouteResult)await controller.Post(productRequest);

            result.StatusCode.Should().Be(201);
        }

        [Test]
        public async Task Get_OnValidationError_ReturnsStatusCode400()
        {
            _service.Setup(s => s.GetValidationErrors())
                .Returns(new List<FluentValidation.Results.ValidationFailure>());

            var controller = new ProductsController(_service.Object, _mapper);
            ProductRequest productRequest = new ProductRequest();

            var result = (BadRequestObjectResult)await controller.Post(productRequest);

            result.StatusCode.Should().Be(400);
        }
    }
}
