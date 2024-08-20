
using ProductApi.Application.DTOs;

namespace ProductApi.Application.Interfaces;

public interface IProductService
{
   Task<ProductDto> GetByIdAsync(int id);
   Task<IEnumerable<ProductDto>> GetAllAsync();
   Task AddAsync(ProductDto productDto);
   Task UpdateAsync(ProductDto productDto);
   Task DeleteAsync(int id);
}