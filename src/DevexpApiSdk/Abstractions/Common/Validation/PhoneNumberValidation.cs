using System.Text.RegularExpressions;

namespace DevexpApiSdk.Common.Validation
{
    internal static class PhoneNumberValidator
    {
        private static readonly Regex E164Regex = new(@"^\+[1-9]\d{1,14}$", RegexOptions.Compiled);

        internal static bool IsValidE164(string phone) =>
            !string.IsNullOrWhiteSpace(phone) && E164Regex.IsMatch(phone);
    }
}
