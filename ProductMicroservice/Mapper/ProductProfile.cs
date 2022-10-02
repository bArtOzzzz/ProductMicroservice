using AutoMapper;
using ProductMicroservice.Models.Request;
using ProductMicroservice.Models.Response;
using Repositories.Entities;
using Services.Dto;

namespace ProductMicroservice.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, ProductResponse>();
            CreateMap<ProductResponse, ProductDto>();

            CreateMap<ProductModel, ProductDto>();
            CreateMap<ProductDto, ProductModel>();

            CreateMap<ProductEntity, ProductDto>();
            CreateMap<ProductDto, ProductEntity>();
        }
    }
}
