namespace ReadersRealm.Common;

public static class ValidationMessages
{
    public static class Category
    {
        public const string CategoryNameRequiredMessage = "The category name is required!";
        public const string CategoryNameLengthMessage = "The category name has to be between {1} and {2} characters!";

        public const string CategoryDisplayOrderRangeMessage = "The display order should be between {1} and {2}!";

        public const string MatchingNameAndDisplayOrderMessage =
            "The Category Name and Display Order cannot be the same!";
    }

}