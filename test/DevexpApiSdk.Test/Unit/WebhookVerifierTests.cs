using System;
using System.Security.Cryptography;
using System.Text;
using DevexpApiSdk.Common.Security;
using NUnit.Framework;

namespace DevexpApiSdk.Tests.Common.Security
{
    [TestFixture]
    public class WebhookVerifierTests
    {
        private const string Secret = "mySecret";
        private const string Message = "{\"id\":\"123\",\"content\":\"hello world\"}";

        private static string GenerateSignature(string message, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        [Test]
        public void VerifySignature_ValidSignature_ReturnsTrue()
        {
            // Arrange
            var expectedSignature = GenerateSignature(Message, Secret);
            var authHeader = $"Signature {expectedSignature}";

            // Act
            var result = WebhookVerifier.VerifySignature(Message, authHeader, Secret);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void VerifySignature_InvalidSignature_ReturnsFalse()
        {
            // Arrange
            var invalidAuthHeader = "Signature deadbeef";

            // Act
            var result = WebhookVerifier.VerifySignature(Message, invalidAuthHeader, Secret);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void VerifySignature_MissingSignaturePrefix_ReturnsFalse()
        {
            // Arrange
            var expectedSignature = GenerateSignature(Message, Secret);
            var invalidAuthHeader = expectedSignature; // no "Signature " prefix

            // Act
            var result = WebhookVerifier.VerifySignature(Message, invalidAuthHeader, Secret);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void VerifySignature_WrongSecret_ReturnsFalse()
        {
            // Arrange
            var expectedSignature = GenerateSignature(Message, "wrongSecret");
            var authHeader = $"Signature {expectedSignature}";

            // Act
            var result = WebhookVerifier.VerifySignature(Message, authHeader, Secret);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
