namespace ReadersRealm.Common.Constants;

public static class Constants
{
    public static class Shared
    {
        /// <summary>
        /// Key for temp data. Used when an operation is successful - displayed with toastr.
        /// </summary>
        public const string Success = "Success";
        /// <summary>
        /// Key for temp data. Used when there is an error - displayed with toastr.
        /// </summary>
        public const string Error = "Error";
    }

    public static class Category
    {
        //Display Names
        public const string CategoryName = "Category Name";
        public const string CategoryDisplayOrder = "Display Order";

        //Action Results
        public const string CategoryHasBeenSuccessfullyCreated = "The category has been created successfully!";
        public const string CategoryHasBeenSuccessfullyEdited = "The category has been edited successfully!";
        public const string CategoryHasBeenSuccessfullyDeleted = "The category has been deleted successfully!";
    }

    public static class Book
    {
        //Action Results
        public const string BookHasBeenSuccessfullyCreated = "The book has been created successfully!";
        public const string BookHasBeenSuccessfullyEdited = "The book has been edited successfully!";
        public const string BookHasBeenSuccessfullyDeleted = "The book has been deleted successfully!";
    }

    public static class Company
    {
        //Action Results
        public const string CompanyHasBeenSuccessfullyCreated = "The company has been created successfully!";
        public const string CompanyHasBeenSuccessfullyEdited = "The company has been edited successfully!";
        public const string CompanyHasBeenSuccessfullyDeleted = "The company has been deleted successfully!";
    }

    public static class ShoppingCart
    {
        //Action Results
        public const string ShoppingCartItemsHaveBeenAddedSuccessfully =
            "The items have been added successfully to your shopping cart!";
    }

    public static class Roles
    {
        public const string CustomerRole = "Customer";
        public const string AdminRole = "Admin";
        public const string CompanyRole = "Company";
        public const string EmployeeRole = "Employee";
    }
}