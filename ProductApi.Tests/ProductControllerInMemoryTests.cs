using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Mapping;
using ProductApi.Application.Services;
using ProductApi.Core.Entities;
using ProductApi.Core.Interfaces;
using ProductApi.Infrastructure;
using ProductApi.Infrastructure.Repositories;
using ProductApi.WebAPI.Controllers;

namespace ProductApi.Tests;

public class ProductControllerInMemoryTests
{
   private readonly ProductController _controller;
   private readonly IProductRepository _productRepository;
   private readonly IProductService _productService;
   private readonly AppDbContext _context;
   private readonly IMapper _mapper;

   public ProductControllerInMemoryTests()
   {
      // Setup in-memory database
      var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseInMemoryDatabase(databaseName: "ProductDatabase")
          .Options;

      _context = new AppDbContext(options);

      // Seed the database with some data
      SeedDatabase();

      // Initialize the ProductService and ProductController
      var config = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
      _mapper = config.CreateMapper();
      _productRepository = new ProductRepository(_context);
      _productService = new ProductService(_productRepository, _mapper);
      _controller = new ProductController(_productService);
   }

   private void SeedDatabase()
   {
      _context.Database.EnsureCreated();
      if (_context.Products.Any()) return;
      _context.Products.AddRange(
         new Product { Name = "Sample Product 1", Price = 9.99m },
         new Product { Name = "Sample Product 2", Price = 11.25m }
      );
      _context.SaveChanges();
   }

   [Fact]
   public async Task Get_ReturnsOk_WithProductDto()
   {
      // Act
      var result = await _controller.Get(1);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<ProductDto>(okResult.Value);
      Assert.Equal(1, returnValue.Id);
   }

   [Fact]
   public async Task Get_ReturnsNotFound_WhenProductDoesNotExist()
   {
      // Act
      var result = await _controller.Get(99);

      // Assert
      Assert.IsType<NotFoundResult>(result.Result);
   }

   [Fact]
   public async Task GetAll_ReturnsOk_WithListOfProducts()
   {
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
      var productDto = new ProductDto { Name = "Sample Product 3", Price = 30.25m };
      // Act
      var result = await _controller.Post(productDto);
      
      var x = _context.Products.ToList();
      // Assert
      var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
      var returnValue = Assert.IsType<ProductDto>(createdAtActionResult.Value);
      Assert.Equal(productDto.Id, returnValue.Id);

      // Verify that the product was actually added to the database
      var productInDb = await _context.Products.FindAsync(3);
      Assert.NotNull(productInDb);
      Assert.Equal("Sample Product 3", productInDb.Name);
   }

   [Fact]
   public async Task Put_ReturnsNoContent()
   {
      // Arrange
      var productDto = new ProductDto { Id= 1, Name = "Updated Product", Price = 40.75m };
      var x = _context.Products.ToList();

      // Act
      var result = await _controller.Put(productDto);

      // Assert
      Assert.IsType<NoContentResult>(result);

      // Verify that the product was actually updated in the database
      var productInDb = await _context.Products.FindAsync(3);
      Assert.Equal("Updated Product", productInDb.Name);
   }

   [Fact]
   public async Task Delete_ReturnsNoContent()
   {
      // Act
      var result = await _controller.Delete(1);

      // Assert
      Assert.IsType<NoContentResult>(result);

      // Verify that the product was actually deleted from the database
      var productInDb = await _context.Products.FindAsync(1);
      Assert.Null(productInDb);
   }
}

//using Newtonsoft.Json;
//using ProductApi.Application.DTOs;

//namespace ProductApi.Tests;

//public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory>
//{
//   private readonly HttpClient _client;

//   public ProductControllerTests(CustomWebApplicationFactory factory)
//   {
//      _client = factory.CreateClient();
//   }

//   [Fact]
//   public async Task Get_ReturnsProduct()
//   {
//      // Arrange
//      var requestUri = "/api/product/1";

//      // Act
//      var response = await _client.GetAsync(requestUri);
//      response.EnsureSuccessStatusCode();

//      var responseString = await response.Content.ReadAsStringAsync();
//      var productDto = JsonConvert.DeserializeObject<ProductDto>(responseString);

//      // Assert
//      Assert.NotNull(productDto);
//      Assert.Equal("Test Product", productDto.Name);
//      Assert.Equal(10.00m, productDto.Price);
//   }

//   [Fact]
//   public async Task Post_CreatesProduct()
//   {
//      // Arrange
//      var newProduct = new ProductDto { Name = "New Product", Price = 20.00m };
//      var content = new StringContent(JsonConvert.SerializeObject(newProduct), System.Text.Encoding.UTF8, "application/json");
//      var requestUri = "/api/product";

//      // Act
//      var response = await _client.PostAsync(requestUri, content);
//      response.EnsureSuccessStatusCode();

//      // Assert
//      var getResponse = await _client.GetAsync("/api/product/2");
//      getResponse.EnsureSuccessStatusCode();

//      var responseString = await getResponse.Content.ReadAsStringAsync();
//      var productDto = JsonConvert.DeserializeObject<ProductDto>(responseString);

//      Assert.NotNull(productDto);
//      Assert.Equal("New Product", productDto.Name);
//      Assert.Equal(20.00m, productDto.Price);
//   }

//   // Add more tests for PUT, DELETE, and other scenarios as needed.
//}
