using CommerceAPI.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceAPI.Models;

namespace CommerceAPITests.EndpointTests
{
    public class ProductCrudTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _factory;

        public ProductCrudTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private CommerceApiContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CommerceApiContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new CommerceApiContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        private string ObjectToJson(object obj)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });

            return json;
        }


        [Fact]
        public async void GetProducts_ReturnsAllProductsforSpecficMerchant()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var merchant1 = new Merchant { Name = "Biker Jim's", Category = "Restaurant" };
            context.Merchants.Add(merchant1);
            Product product1 = new Product { Name = "FOOD", Description = "desription", Category = "bike food", MerchantId = merchant1.Id };
            Product product2 = new Product { Name = "burger", Description="desription", Category = "people food", MerchantId = merchant1.Id };
            List<Product> products = new() { product1, product2 };
            context.Products.AddRange(products);

            context.SaveChanges();

            var response = await client.GetAsync($"/api/merchants/{merchant1.Id}/products");
            var content = await response.Content.ReadAsStringAsync();

            string expected = ObjectToJson(products);

            response.EnsureSuccessStatusCode();
            Assert.Equal(expected, content);

        }

        [Fact]
        public async void GetProducts_ReturnsSingleProduct()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var merchant1 = new Merchant { Name = "Biker Jim's", Category = "Restaurant" };
            context.Merchants.Add(merchant1);
            Product product1 = new Product { Name = "FOOD", Description = "desription", Category = "bike food", MerchantId = merchant1.Id };
            Product product2 = new Product { Name = "burger", Description = "desription", Category = "people food", MerchantId = merchant1.Id };
            List<Product> products = new() { product1, product2 };
            context.Products.AddRange(products);

            context.SaveChanges();

            var response = await client.GetAsync($"/api/merchants/{merchant1.Id}/products/{product1.Id}");
            var content = await response.Content.ReadAsStringAsync();

            string expected = ObjectToJson(product1);

            response.EnsureSuccessStatusCode();
            Assert.Equal(expected, content);
            Assert.DoesNotContain(ObjectToJson(product2), content);

        }

        [Fact]
        public async void CreateProduct_CreatesProduct()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var merchant1 = new Merchant { Name = "Biker Jim's", Category = "Restaurant" };
            context.Merchants.Add(merchant1);
            context.SaveChanges();

            var jsonString = "{ \"Name\": \"Coffee Maker\", \"Description\": \"Brews up to 12 cups then breaks\", \"Category\": \"Home Appliances\", \"Price\": 1100, \"StockQuantity\": 20, \"ReleaseDate\": \"2023-03-01T00:00:00.000Z\", \"MerchantId\": 1}";
            var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/merchants/{merchant1.Id}/products", requestContent);

            response.EnsureSuccessStatusCode();

            var newProduct = context.Products.First();
            Assert.Equal("Coffee Maker", newProduct.Name);
        }

    }
}
