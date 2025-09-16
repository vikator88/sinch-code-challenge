using System;

namespace DevexpApiSdk.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when no API key is provided.
    /// </summary>
    public class ApiKeyMissingException : Exception
    {
        public ApiKeyMissingException()
            : base(
                "No API key was provided. Please configure DevexpApiOptions with a valid ApiKey."
            ) { }
    }
}
