﻿using AutoMapper;
using FluentAssertions;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Core.Domain.Common;
using TektonLabs.Core.Domain.Entities;
using TektonLabs.Presentation.Api.ApiModels.Request;
using TektonLabs.Presentation.Api.ApiModels.Response;
using TektonLabs.Presentation.Api.Controllers;
using TektonLabs.Presentation.Api.Helpers.Mapping;
using TektonLabs.Testing.UnitTests.TestData;

namespace TektonLabs.Testing.UnitTests.Controllers
{
    public class TestProductsController
    {
        private Mock<IProductService> _service;
        private readonly IMapper _mapper;
        private Mock<IAppCache> _cache;

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
        }

        [Test]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            Product product = new Product { ProductId = 1, Name = "Product", Price = 1, Status = 1, Stock = 1 };
            _service.Setup(s => s.GetProductAsync(1))
                .ReturnsAsync(product);

            _cache.Setup(c => c.GetOrAdd(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, List<StatusData>>>(), It.IsAny<MemoryCacheEntryOptions>()))
                .Returns(ProductTestData.GetStatusData());

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object);

            var result = (OkObjectResult)await controller.Get(1);

            result.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task Get_OnSuccess_ReturnsProduct()
        {
            Product product = new Product { ProductId = 1, Name = "Product", Price = 1, Status = 1, Stock = 1 };
            _service.Setup(s => s.GetProductAsync(1))
                .ReturnsAsync(product);

            _cache.Setup(c => c.GetOrAdd(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, List<StatusData>>>(), It.IsAny<MemoryCacheEntryOptions>()))
                .Returns(ProductTestData.GetStatusData());

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object);

            var result = (OkObjectResult)await controller.Get(1);

            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<ProductResponse>();
        }

        [Test]
        public async Task Get_OnNoProductFound_ReturnsStatusCode404()
        {
            var controller = new ProductsController(_service.Object, _mapper, _cache.Object);

            var result = (NotFoundResult)await controller.Get(1);

            result.StatusCode.Should().Be(404);
        }

        [Test]
        public async Task Get_OnSuccessCreating_ReturnsStatusCode201()
        {
            Product product = new Product { ProductId = 1, Name = "Product", Price = 1, Status = 1, Stock = 1 };
            _service.Setup(s => s.InsertProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object);
            ProductRequest productRequest = _mapper.Map<ProductRequest>(product);

            var result = (CreatedAtRouteResult)await controller.Post(productRequest);

            result.StatusCode.Should().Be(201);
        }

        [Test]
        public async Task Get_OnValidationError_ReturnsStatusCode400()
        {
            _service.Setup(s => s.GetValidationErrors())
                .Returns(new List<FluentValidation.Results.ValidationFailure>());

            var controller = new ProductsController(_service.Object, _mapper, _cache.Object);
            ProductRequest productRequest = new ProductRequest();

            var result = (BadRequestObjectResult)await controller.Post(productRequest);

            result.StatusCode.Should().Be(400);
        }
    }
}
