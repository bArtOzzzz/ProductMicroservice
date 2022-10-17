using ProductMicroservice.Models.Response;
using ProductMicroservice.Models.Request;
using Repositories.Entities;
using Services.Dto;
using AutoMapper;

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
