namespace ReadersRealm.Common.Constants;

public static class ExceptionMessages
{
    public static class ApplicationUserExceptionMessages
    {
        public const string UserCreationFailedExceptionMessage = "User creation failed: {0}.";
    }

    public static class ServiceExceptionMessages
    {
        public const string ServiceTypeNotFoundExceptionMessage = "The provided service type - {0} is invalid.";

        public const string ServiceInterfaceNotFoundExceptionMessage = "No interface was found for the provided service: {0}.";
    }
}