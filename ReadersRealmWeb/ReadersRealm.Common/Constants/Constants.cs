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

        //Model Errors
        public const string Name = "Name";
    }

    public static class Book
    {
        //Action Results
        public const string BookHasBeenSuccessfullyCreated = "The book has been created successfully!";
        public const string BookHasBeenSuccessfullyEdited = "The book has been edited successfully!";
        public const string BookHasBeenSuccessfullyDeleted = "The book has been deleted successfully!";

        //Model Errors
        public const string AuthorId = "AuthorId";
        public const string CategoryId = "CategoryId";

        //Path To Save Images
        public const string PathToSaveImage = @"\images\book\";
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

        public const string ShoppingCartItemHasBeenDeletedSuccessfully = "The items has been deleted successfully!";
    }

    public static class Order
    {
        //Action Result
        public const string OrderHasSuccessfullyBeenUpdated = "The order has been updated successfully!";
    }

    public static class OrderHeader
    {
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusApproved = "Approved";
        public const string OrderStatusInProcess = "Processing";
        public const string OrderStatusShipped = "Shipped";
        public const string OrderStatusCancelled = "Cancelled";
        public const string OrderStatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "Approved for delayed payment";
        public const string PaymentStatusRejected = "Rejected";
    }

    public static class Roles
    {
        public const string CustomerRole = "Customer";
        public const string AdminRole = "Admin";
        public const string CompanyRole = "Company";
        public const string EmployeeRole = "Employee";
    }

    public static class Areas
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
    }

    public static class StripeSettings
    {
        public const string PublishableKeyAsString = "Stripe:PublishableKey";
        public const string SecretKeyAsString = "Stripe:SecretKey";

        public const string FullSuccessUrl = "{0}/Customer/ShoppingCart/OrderConfirmation?orderHeaderId={1}";
        public const string FullCancelUrl = "{0}/Customer/ShoppingCart/Index";
    }
}