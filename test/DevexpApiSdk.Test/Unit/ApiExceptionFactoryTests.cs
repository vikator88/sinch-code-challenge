using System.Net;
using DevexpApiSdk.Common.Exceptions;
using DevexpApiSdk.Http;
using NUnit.Framework;

namespace MyApiSdk.Tests.Http
{
    [TestFixture]
    public class ApiExceptionFactoryTests
    {
        [Test]
        public void Create_ShouldReturnValidationException_When400()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var rawBody = @"{ ""message"": ""Invalid input"" }";

            var ex = ApiExceptionFactory.Create(response, rawBody);

            Assert.That(ex, Is.TypeOf<ApiValidationException>());
            Assert.That(ex.Message, Does.Contain("Validation error"));
            Assert.That(((ApiValidationException)ex).ApiMessage, Is.EqualTo("Invalid input"));
        }

        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        public void Create_ShouldReturnAuthException_When401Or403(HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            var rawBody = @"{ ""message"": ""Token expired"" }";

            var ex = ApiExceptionFactory.Create(response, rawBody);

            Assert.That(ex, Is.TypeOf<ApiAuthException>());
            Assert.That(ex.Message, Does.Contain("Authentication/authorization failed"));
            Assert.That(((ApiAuthException)ex).ApiMessage, Is.EqualTo("Token expired"));
        }

        [Test]
        public void Create_ShouldReturnNotFoundException_When404()
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            var rawBody = @"{ ""id"": ""123"", ""message"": ""User not found"" }";

            var ex = ApiExceptionFactory.Create(response, rawBody);

            Assert.That(ex, Is.TypeOf<ApiNotFoundException>());
            var notFoundEx = (ApiNotFoundException)ex;
            Assert.That(notFoundEx.ResourceId, Is.EqualTo("123"));
            Assert.That(notFoundEx.ApiMessage, Is.EqualTo("User not found"));
        }

        [Test]
        public void Create_ShouldReturnServerException_When500()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var rawBody = @"{ ""message"": ""Database down"" }";

            var ex = ApiExceptionFactory.Create(response, rawBody);

            Assert.That(ex, Is.TypeOf<ApiServerException>());
            var serverEx = (ApiServerException)ex;
            Assert.That(serverEx.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(serverEx.ApiMessage, Is.EqualTo("Database down"));
        }

        [Test]
        public void Create_ShouldReturnGenericApiException_WhenUnexpectedCode()
        {
            var response = new HttpResponseMessage((HttpStatusCode)418); // I'm a teapot
            var rawBody = @"{ ""message"": ""Short and stout"" }";

            var ex = ApiExceptionFactory.Create(response, rawBody);

            Assert.That(ex, Is.TypeOf<ApiException>());
            var apiEx = (ApiException)ex;
            Assert.That(apiEx.StatusCode, Is.EqualTo((HttpStatusCode)418));
            Assert.That(apiEx.ApiMessage, Is.EqualTo("Short and stout"));
        }

        [Test]
        public void Create_ShouldHandleJsonWithoutExpectedFields()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var rawBody = @"{ ""foo"": ""bar"" }"; // no tiene "message"

            var ex = ApiExceptionFactory.Create(response, rawBody);

            Assert.That(ex, Is.TypeOf<ApiException>());
            // el factory mete "unknown validation error" cuando no hay message
            Assert.That(ex.ApiMessage, Is.EqualTo("no additional error information"));
        }

        [Test]
        public void Create_ShouldThrowJsonException_WhenBodyIsNotValidJson()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var rawBody = @"<<<NOT_JSON>>>";

            var ex = ApiExceptionFactory.Create(response, rawBody);

            Assert.That(ex, Is.TypeOf<ApiException>());
            // el factory mete "unexpected error" cuando no hay message
            Assert.That(ex.ApiMessage, Is.EqualTo("no additional error information"));
        }
    }
}
