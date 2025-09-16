using DevexpApiSdk.Http;

namespace MyApiSdk.Tests.Http
{
    [TestFixture]
    public class RequestBuilderTests
    {
        [Test]
        public void Build_Should_CreateRequestWithoutBody_WhenBodyIsNull()
        {
            // Arrange
            var method = HttpMethod.Get;
            var path = "/contacts";

            // Act
            var request = RequestBuilder.Build(method, path, null);

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
            var request = RequestBuilder.Build(method, path, body);

            // Assert
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.RequestUri!.ToString(), Is.EqualTo(path));

            var content = request.Content!.ReadAsStringAsync().Result;
            Assert.That(content, Does.Contain("test").IgnoreCase); // name serialized
            Assert.That(content, Does.Contain("600111222")); // phone serialized

            Assert.That(
                request.Content!.Headers.ContentType!.MediaType,
                Is.EqualTo("application/json")
            );
        }
    }
}
