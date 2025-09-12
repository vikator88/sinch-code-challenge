namespace DevexpApiSdk.Common.Exceptions
{
    public class InvalidPhoneNumberException : Exception
    {
        public string Phone { get; }

        public InvalidPhoneNumberException(string phone)
            : base($"The phone number '{phone}' is not a valid E.164 format.")
        {
            Phone = phone;
        }
    }
}
