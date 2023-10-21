using AutoMapper;
using TektonLabs.Core.Domain.Entities;
using TektonLabs.Presentation.Api.ApiModels.Request;
using TektonLabs.Presentation.Api.ApiModels.Response;

namespace TektonLabs.Presentation.Api.Helpers.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<ProductRequest, Product>().ReverseMap();
            CreateMap<ProductResponse, Product>().ReverseMap();
        }
    }
}
