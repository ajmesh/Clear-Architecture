using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;

namespace ProductApi.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
   private readonly IProductService _productService;

   public ProductController(IProductService productService)
   {
      _productService = productService;
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<ProductDto>> Get(int id)
   {
      var product = await _productService.GetByIdAsync(id);
      if (product == null)
      {
         return NotFound();
      }
      return Ok(product);
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
   {
      var products = await _productService.GetAllAsync();
      return Ok(products);
   }

   [HttpPost]
   public async Task<IActionResult> Post([FromBody] ProductDto productDto)
   {
      await _productService.AddAsync(productDto);
      return CreatedAtAction(nameof(Get), new { id = productDto.Id }, productDto);
   }

   [HttpPut]
   public async Task<IActionResult> Put([FromBody] ProductDto productDto)
   {
      await _productService.UpdateAsync(productDto);
      return NoContent();
   }

   [HttpDelete("{id}")]
   public async Task<IActionResult> Delete(int id)
   {
      await _productService.DeleteAsync(id);
      return NoContent();
   }
}
