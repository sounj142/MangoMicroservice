using AutoMapper;
using Mango.ProductAPI.Dtos;
using Mango.ProductAPI.Models;

namespace Mango.ProductAPI.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.CategoryName, s => s.MapFrom(x => x.Category!.Name));
        CreateMap<ProductCreateDto, Product>();
        CreateMap<Category, CategoryDto>();
    }
}