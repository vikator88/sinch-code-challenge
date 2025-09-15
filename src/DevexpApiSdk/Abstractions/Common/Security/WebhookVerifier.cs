using System.Security.Cryptography;
using System.Text;

namespace DevexpApiSdk.Common.Security
{
    public static class WebhookVerifier
    {
        /// <summary>
        /// Verifies the HMAC-SHA256 signature of a webhook request.
        /// </summary>
        /// <param name="messageBody">The raw body of the request as a string.</param>
        /// <param name="authorizationHeader">The value of the Authorization header (format: "Signature {hex}").</param>
        /// <param name="secret">The shared webhook secret used to sign the request.</param>
        /// <returns><c>true</c> if the signature is valid; otherwise, <c>false</c>.</returns>
        public static bool VerifySignature(
            string messageBody,
            string authorizationHeader,
            string secret
        )
        {
            if (
                string.IsNullOrWhiteSpace(authorizationHeader)
                || !authorizationHeader.StartsWith("Signature ")
            )
                return false;

            var providedSignature = authorizationHeader.Substring("Signature ".Length);

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(messageBody));
            var expectedSignature = BitConverter
                .ToString(hashBytes)
                .Replace("-", "")
                .ToLowerInvariant();

            return string.Equals(
                providedSignature,
                expectedSignature,
                StringComparison.OrdinalIgnoreCase
            );
        }
    }
}
