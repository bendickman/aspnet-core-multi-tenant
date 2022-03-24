using MultiTenant.Api.Controllers.Requests;
using MultiTenant.Core.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MultiTenant.IntegrationTests.ControllerTests
{
    public class ProductControllerTests : ApiIntegrationTest
    {
        private IDictionary<string, string> _headers = new Dictionary<string, string>
        {
            { "tenant", "TestTenant" },
        };

        private string _requestUrl = $"{BaseUri}v1/product";

        [Test]
        public async Task Post_SuccessfullySavesNewProduct()
        {
            var productRequest = new CreateProductRequest
            {
                Name = "Test Product 1",
                Description = "Test Product 1 Description",
                Rate = 1,
            };

            var response = await RequestSender
                .Post(_requestUrl, productRequest, headers: _headers);

            var createdProduct = await ApiResponse.GetResponse<Product>(response);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(productRequest.Name, createdProduct.Name);
            Assert.AreEqual(productRequest.Description, createdProduct.Description);
            Assert.AreEqual(productRequest.Rate, createdProduct.Rate);
        }
    }
}
