using System.Net;
using System.Net.Http;
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
            var ex = ApiExceptionFactory.Create(response, "bad request body");

            Assert.That(ex, Is.TypeOf<ApiValidationException>());
            Assert.That(ex.Message, Does.Contain("Validation error"));
        }

        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        public void Create_ShouldReturnAuthException_When401Or403(HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            var ex = ApiExceptionFactory.Create(response, "auth failed");

            Assert.That(ex, Is.TypeOf<ApiAuthException>());
            Assert.That(ex.Message, Does.Contain("Authentication/authorization failed"));
        }

        [Test]
        public void Create_ShouldReturnNotFoundException_When404()
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            var ex = ApiExceptionFactory.Create(response, "not found");

            Assert.That(ex, Is.TypeOf<ApiNotFoundException>());
            Assert.That(ex.Message, Does.Contain("Resource not found"));
        }

        [Test]
        public void Create_ShouldReturnServerException_When500()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var ex = ApiExceptionFactory.Create(response, "server error");

            Assert.That(ex, Is.TypeOf<ApiServerException>());
            Assert.That(
                ((ApiServerException)ex).StatusCode,
                Is.EqualTo(HttpStatusCode.InternalServerError)
            );
        }

        [Test]
        public void Create_ShouldReturnGenericApiException_WhenUnexpectedCode()
        {
            var response = new HttpResponseMessage((HttpStatusCode)418); // I'm a teapot
            var ex = ApiExceptionFactory.Create(response, "weird error");

            Assert.That(ex, Is.TypeOf<ApiException>());
            Assert.That(((ApiException)ex).StatusCode, Is.EqualTo((HttpStatusCode)418));
        }
    }
}
