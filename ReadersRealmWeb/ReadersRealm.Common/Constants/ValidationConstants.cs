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

    public static class ApplicationUser
    {
        public const int ApplicationUserFirstNameMaxLength = 40;
        public const int ApplicationUserFirstNameMinLength = 1;

        public const int ApplicationUserLastNameMaxLength = 40;
        public const int ApplicationUserLastNameMinLength = 1;

        public const int ApplicationUserStreetAddressMaxLength = 255;
        public const int ApplicationUserStreetAddressMinLength = 5;

        public const int ApplicationUserCityMaxLength = 50;
        public const int ApplicationUserCityMinLength = 2;

        public const int ApplicationUserStateMaxLength = 50;
        public const int ApplicationUserStateMinLength = 2;

        public const int ApplicationUserPostalCodeMaxLength = 12;
        public const int ApplicationUserPostalCodeMinLength = 4;
    }

    public static class RegisterModel
    {
        public const int RegisterModelFirstNameMaxLength = 40;
        public const int RegisterModelFirstNameMinLength = 1;

        public const int RegisterModelLastNameMaxLength = 40;
        public const int RegisterModelLastNameMinLength = 1;

        public const int RegisterModelStreetAddressMaxLength = 255;
        public const int RegisterModelStreetAddressMinLength = 5;
                 
        public const int RegisterModelCityMaxLength = 50;
        public const int RegisterModelCityMinLength = 2;
              
        public const int RegisterModelStateMaxLength = 50;
        public const int RegisterModelStateMinLength = 2;
          
        public const int RegisterModelPostalCodeMaxLength = 12;
        public const int RegisterModelPostalCodeMinLength = 4;
         
        public const int RegisterModelPhoneNumberMaxLength = 20;
        public const int RegisterModelPhoneNumberMinLength = 7;
    }

    public static class Company
    {
        public const int CompanyNameMaxLength = 255;
        public const int CompanyNameMinLength = 1;

        public const int CompanyUicMaxLength = 13;
        public const int CompanyUicMinLength = 9;

        public const int CompanyEmailMaxLength = 254;
        public const int CompanyEmailMinLength = 3;

        public const int CompanyStreetAddressMaxLength = 255;
        public const int CompanyStreetAddressMinLength = 5;

        public const int CompanyCityMaxLength = 50;
        public const int CompanyCityMinLength = 2;

        public const int CompanyStateMaxLength = 50;
        public const int CompanyStateMinLength = 2;

        public const int CompanyPostalCodeMaxLength = 12;
        public const int CompanyPostalCodeMinLength = 4;

        public const int CompanyPhoneNumberMaxLength = 20;
        public const int CompanyPhoneNumberMinLength = 7;
    }

    public static class ShoppingCart
    {
        public const int ShoppingCartCountMaxRange = 1000;
        public const int ShoppingCartCountMinRange = 1;
    }
}