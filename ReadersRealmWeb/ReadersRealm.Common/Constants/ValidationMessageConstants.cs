namespace ReadersRealm.Common.Constants;

public static class ValidationMessageConstants
{
    public static class Category
    {
        public const string CategoryNameRequiredMessage = "{0} is required!";
        public const string CategoryNameLengthMessage = "{0} has to be between {1} and {2} characters!";

        public const string CategoryDisplayOrderRangeMessage = "{0} should be between {1} and {2}!";

        public const string MatchingNameAndDisplayOrderMessage =
            "{0} and {1} cannot be the same!";
    }

    public static class Book
    {
        public const string BookTitleRequiredMessage = "{0} is required!";
        public const string BookTitleLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string BookDescriptionLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string BookIsbnRequiredMessage = "{0} is required!";
        public const string BookIsbnLengthMessage = "{0} must be exactly {1} characters!";

        public const string BookPriceRequiredMessage = "{0} is required!";
        public const string BookPriceRangeMessage = "{0} must be between {1}$ and {2}$ !";

        public const string BookPagesRangeMessage = "{0} must be between {2} and {1}!";

        public const string BookUsedRequiredMessage = "{0} is required!";
    }

    public static class Company
    {
        public const string CompanyNameRequiredMessage = "{0} is required!";
        public const string CompanyNameLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string CompanyUicRequiredMessage = "{0} is required!";
        public const string CompanyUicLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string CompanyEmailRequiredMessage = "{0} is required!";
        public const string CompanyEmailLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string CompanyStreetAddressLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string CompanyCityLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string CompanyStateLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string CompanyPostalCodeLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string CompanyPhoneNumberLengthMessage = "{0} must be between {2} and {1} characters!";
    }

    public static class RegisterModel
    {
        public const string RegisterModelFirstNameLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string RegisterModelLastNameLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string RegisterModelStreetAddressLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string RegisterModelCityLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string RegisterModelStateLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string RegisterModelPostalCodeLengthMessage = "{0} must be between {2} and {1} characters!";

        public const string RegisterModelPhoneNumberLengthMessage = "{0} must be between {2} and {1} characters!";
    }
}