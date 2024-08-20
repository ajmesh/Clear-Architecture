using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Core.Entities;

namespace ProductApi.Application.Mapping;

public class MappingProfile : Profile
{
   public MappingProfile()
   {
      CreateMap<Product, ProductDto>().ReverseMap();
   }
}
