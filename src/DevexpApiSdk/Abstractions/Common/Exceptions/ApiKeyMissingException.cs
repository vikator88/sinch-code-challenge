using System;

namespace DevexpApiSdk.Common.Exceptions
{
    public class ApiKeyMissingException : Exception
    {
        public ApiKeyMissingException()
            : base(
                "No API key was provided. Please configure DevexpApiOptions with a valid ApiKey."
            ) { }
    }
}
