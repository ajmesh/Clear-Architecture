// See https://aka.ms/new-console-template for more information
using ProductApi.Application.DTOs;
using System.Net.Http.Json;


// Define the base URL of the API
string apiUrl = "https://localhost:7262/";

// Create an instance of HttpClient
using HttpClient client = new HttpClient
{
   BaseAddress = new Uri(apiUrl)
};

// Make an HTTP GET request
try
{
   var response1 = await client.GetFromJsonAsync<ProductDto>("/api/Product");
   var response = await client.GetAsync("/api/Product");

   // Check if the request was successful
   response.EnsureSuccessStatusCode();

   // Read the response content as a string
   var content = await response.Content.ReadAsStringAsync();

   // Optionally, deserialize the JSON response into a C# object
   var data = await response.Content.ReadFromJsonAsync<ProductDto>();

   // Output the response to the console
   Console.WriteLine(content);

   // Or output the deserialized object
   Console.WriteLine(data);
}
catch (HttpRequestException e)
{
   Console.WriteLine($"Request error: {e.Message}");
}