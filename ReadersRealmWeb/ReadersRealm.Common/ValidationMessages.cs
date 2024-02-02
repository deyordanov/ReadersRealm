﻿namespace ReadersRealm.Common;

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

}