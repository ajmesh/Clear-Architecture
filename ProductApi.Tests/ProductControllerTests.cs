using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.WebAPI.Controllers;

namespace ProductApi.Tests;

public class ProductControllerTests
{
   private readonly Mock<IProductService> _mockProductService;
   private readonly ProductController _controller;

   public ProductControllerTests()
   {
      _mockProductService = new Mock<IProductService>();
      _controller = new ProductController(_mockProductService.Object);
   }

   [Fact]
   public async Task Get_ReturnsOk_WithProductDto()
   {
      // Arrange
      var productId = 1;
      var productDto = new ProductDto { Id = productId, Name = "Test Product" };
      _mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync(productDto);

      // Act
      var result = await _controller.Get(productId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<ProductDto>(okResult.Value);
      Assert.Equal(productId, returnValue.Id);
   }

   [Fact]
   public async Task Get_ReturnsNotFound_WhenProductDoesNotExist()
   {
      // Arrange
      var productId = 1;
      _mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync((ProductDto)null);

      // Act
      var result = await _controller.Get(productId);

      // Assert
      Assert.IsType<NotFoundResult>(result.Result);
   }

   [Fact]
   public async Task GetAll_ReturnsOk_WithListOfProducts()
   {
      // Arrange
      var products = new List<ProductDto>
         {
             new ProductDto { Id = 1, Name = "Product 1" },
             new ProductDto { Id = 2, Name = "Product 2" }
         };
      _mockProductService.Setup(service => service.GetAllAsync()).ReturnsAsync(products);

      // Act
      var result = await _controller.GetAll();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
      Assert.Equal(2, returnValue.Count);
   }

   [Fact]
   public async Task Post_ReturnsCreatedAtAction_WithProductDto()
   {
      // Arrange
      var productDto = new ProductDto { Id = 1, Name = "New Product" };
      _mockProductService.Setup(service => service.AddAsync(productDto)).Returns(Task.CompletedTask);

      // Act
      var result = await _controller.Post(productDto);

      // Assert
      var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
      var returnValue = Assert.IsType<ProductDto>(createdAtActionResult.Value);
      Assert.Equal(productDto.Id, returnValue.Id);
   }

   [Fact]
   public async Task Put_ReturnsNoContent()
   {
      // Arrange
      var productDto = new ProductDto { Id = 1, Name = "Updated Product" };
      _mockProductService.Setup(service => service.UpdateAsync(productDto)).Returns(Task.CompletedTask);

      // Act
      var result = await _controller.Put(productDto);

      // Assert
      Assert.IsType<NoContentResult>(result);
   }

   [Fact]
   public async Task Delete_ReturnsNoContent()
   {
      // Arrange
      var productId = 1;
      _mockProductService.Setup(service => service.DeleteAsync(productId)).Returns(Task.CompletedTask);

      // Act
      var result = await _controller.Delete(productId);

      // Assert
      Assert.IsType<NoContentResult>(result);
   }
}
