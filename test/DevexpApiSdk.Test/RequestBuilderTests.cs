using System.Net.Http;
using System.Text.Json;
using DevexpApiSdk.Http;
using NUnit.Framework;

namespace MyApiSdk.Tests.Http
{
    [TestFixture]
    public class RequestBuilderTests
    {
        private JsonSerializerOptions _jsonOptions;

        [SetUp]
        public void Setup()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        [Test]
        public void Build_Should_CreateRequestWithoutBody_WhenBodyIsNull()
        {
            // Arrange
            var method = HttpMethod.Get;
            var path = "/contacts";

            // Act
            var request = RequestBuilder.Build(method, path, null, _jsonOptions);

            // Assert
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.RequestUri!.ToString(), Is.EqualTo(path));
            Assert.That(request.Content, Is.Null);
        }

        [Test]
        public void Build_Should_CreateRequestWithJsonBody_WhenBodyIsProvided()
        {
            // Arrange
            var method = HttpMethod.Post;
            var path = "/contacts";
            var body = new { Name = "test", Phone = "+34600111222" };

            // Act
            var request = RequestBuilder.Build(method, path, body, _jsonOptions);

            // Assert
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.RequestUri!.ToString(), Is.EqualTo(path));

            var content = request.Content!.ReadAsStringAsync().Result;
            Assert.That(content, Does.Contain("test").IgnoreCase); // name serializado
            Assert.That(content, Does.Contain("600111222")); // phone serializado

            Assert.That(
                request.Content!.Headers.ContentType!.MediaType,
                Is.EqualTo("application/json")
            );
        }

        [Test]
        public void Build_Should_SerializeUsingProvidedOptions()
        {
            // Arrange
            var method = HttpMethod.Post;
            var path = "/contacts";
            var body = new { Name = "test", Phone = "+34600111222" };

            // Forzamos PascalCase
            var options = new JsonSerializerOptions { PropertyNamingPolicy = null };

            // Act
            var request = RequestBuilder.Build(method, path, body, options);

            // Assert
            var content = request.Content!.ReadAsStringAsync().Result;
            Assert.That(content, Does.Contain("Name")); // No camelCase
            Assert.That(content, Does.Contain("Phone")); // No camelCase
        }
    }
}
