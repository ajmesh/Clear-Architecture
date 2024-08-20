using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Mapping;
using ProductApi.Application.Services;
using ProductApi.Core.Entities;
using ProductApi.Core.Interfaces;
using ProductApi.Infrastructure;
using ProductApi.Infrastructure.Repositories;
using ProductApi.WebAPI.Controllers;




using ProductApi.WebAPI;

namespace ProductApi.Tests;






public class ProductControllerTests //: IClassFixture<CustomWebApplicationFactory>
{
   private readonly HttpClient _client;

   public ProductControllerTests(CustomWebApplicationFactory factory)
   {
      _client = factory.CreateClient();
   }

   [Fact]
   public async Task Get_ReturnsProduct()
   {
      // Arrange
      var requestUri = "/api/product/1";

      // Act
      var response = await _client.GetAsync(requestUri);
      response.EnsureSuccessStatusCode();

      var responseString = await response.Content.ReadAsStringAsync();
      var productDto = JsonConvert.DeserializeObject<ProductDto>(responseString);

      // Assert
      Assert.NotNull(productDto);
      Assert.Equal("Test Product", productDto.Name);
      Assert.Equal(10.00m, productDto.Price);
   }

   [Fact]
   public async Task Post_CreatesProduct()
   {
      // Arrange
      var newProduct = new ProductDto { Name = "New Product", Price = 20.00m };
      var content = new StringContent(JsonConvert.SerializeObject(newProduct), System.Text.Encoding.UTF8, "application/json");
      var requestUri = "/api/product";

      // Act
      var response = await _client.PostAsync(requestUri, content);
      response.EnsureSuccessStatusCode();

      // Assert
      var getResponse = await _client.GetAsync("/api/product/2");
      getResponse.EnsureSuccessStatusCode();

      var responseString = await getResponse.Content.ReadAsStringAsync();
      var productDto = JsonConvert.DeserializeObject<ProductDto>(responseString);

      Assert.NotNull(productDto);
      Assert.Equal("New Product", productDto.Name);
      Assert.Equal(20.00m, productDto.Price);
   }

   // Add more tests for PUT, DELETE, and other scenarios as needed.
}


public class CustomWebApplicationFactory //: WebApplicationFactory<Startup>
{
   protected override void ConfigureWebHost(IWebHostBuilder builder)
   {
      builder.ConfigureServices(services =>
      {
         // Remove the app's DbContext registration.
         var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
         if (descriptor != null)
         {
            services.Remove(descriptor);
         }

         // Add an in-memory database for testing.
         services.AddDbContext<AppDbContext>(options =>
         {
            options.UseInMemoryDatabase("TestDatabase");
         });

         // Build the service provider.
         var serviceProvider = services.BuildServiceProvider();

         // Create a scope to get a reference to the database context.
         using (var scope = serviceProvider.CreateScope())
         {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Ensure the database is created.
            db.Database.EnsureCreated();

            // Seed the database with test data if needed.
            if (!db.Products.Any())
            {
               db.Products.Add(new Product { Name = "Test Product", Price = 10.00m });
               db.SaveChanges();
            }
         }
      });
   }
}