using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Core.Entities;
using ProductApi.Core.Interfaces;

namespace ProductApi.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
   public async Task<ProductDto> GetByIdAsync(int id)
   {
      var product = await productRepository.GetByIdAsync(id);
      return mapper.Map<ProductDto>(product);
   }

   public async Task<IEnumerable<ProductDto>> GetAllAsync()
   {
      var products = await productRepository.GetAllAsync();
      return mapper.Map<IEnumerable<ProductDto>>(products);
   }

   public async Task AddAsync(ProductDto productDto)
   {
      var product = mapper.Map<Product>(productDto);
      await productRepository.AddAsync(product);
   }

   public async Task UpdateAsync(ProductDto productDto)
   {
      var product = mapper.Map<Product>(productDto);
      await productRepository.UpdateAsync(product);
   }

   public async Task DeleteAsync(int id)
      => await productRepository.DeleteAsync(id);
}
