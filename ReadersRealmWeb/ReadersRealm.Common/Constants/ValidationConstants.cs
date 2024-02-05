namespace ReadersRealm.Common.Constants;

public static class ValidationConstants
{
    public static class Category
    {
        public const int CategoryNameMaxLength = 50;
        public const int CategoryNameMinLength = 2;

        public const int CategoryDisplayOrderMaxRange = 100;
        public const int CategoryDisplayOrderMinRange = 1;
    }

    public static class Book
    {
        public const int BookTitleMaxLength = 100;
        public const int BookTitleMinLength = 3;

        public const int BookDescriptionMaxLength = 5000;
        public const int BookDescriptionMinLength = 80;

        public const int BookIsbnMaxLength = 13;
        public const int BookIsbnMinLength = 13;

        public const double BookPriceMaxRange = 1000.0;
        public const double BookPriceMinRange = 0.99;

        public const int BookPagesMaxRange = 2000;
        public const int BookPagesMinRange = 15;
    }

    public static class Author
    {
        public const int AuthorFirstNameMaxLength = 40;
        public const int AuthorFirstNameMinLength = 1;

        public const int AuthorMiddleNameMaxLength = 40;
        public const int AuthorMiddleNameMinLength = 1;

        public const int AuthorLastNameMaxLength = 40;
        public const int AuthorLastNameMinLength = 1;

        public const int AuthorAgeMaxRange = 120;
        public const int AuthorAgeMinRange = 18;

        public const int AuthorEmailMaxLength = 254;
        public const int AuthorEmailMinLength = 3;

        public const int AuthorPhoneNumberMaxLength = 20;
        public const int AuthorPhoneNumberMinLength = 7;
    }
}