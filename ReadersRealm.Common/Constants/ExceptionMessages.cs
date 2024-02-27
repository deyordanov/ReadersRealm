namespace ReadersRealm.Common.Constants;

public static class ExceptionMessages
{
    public static class ApplicationUserExceptionMessages
    {
        public const string UserCreationFailedExceptionMessage = "User creation failed: {0}.";
        public const string ApplicationUserNotFoundExceptionMessage = "The user with id: {0} was not found! Method: {1}";
    }

    public static class ServiceExceptionMessages
    {
        public const string ServiceTypeNotFoundExceptionMessage = "The provided service type - {0} is invalid.";

        public const string ServiceInterfaceNotFoundExceptionMessage =
            "No interface was found for the provided service: {0}.";
    }

    public static class ImageExceptionMessages
    {
        public const string InvalidImageIdExceptionMessage = "The provided image id: {0} is invalid. Method: {1}";
    }
}