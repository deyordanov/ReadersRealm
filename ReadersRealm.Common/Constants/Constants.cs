namespace ReadersRealm.Common.Constants;

using System.Text;

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
            "The item has been added successfully to your shopping cart!";

        public const string ShoppingCartItemHasBeenDeletedSuccessfully = "The item has been deleted successfully!";
    }

    public static class Order
    {
        //Action Results
        public const string OrderHasSuccessfullyBeenUpdated = "The order has been updated successfully!";
    }

    public static class OrderHeader
    {
        //Action Results
        public const string OrderStatusHasSuccessfullyBeenUpdated = "The order status has successfully been updated!";
        public const string OrderTrackingNumberAndCarrierAreNotSet =
            "Both the order tracking number and carrier must be filled out before shipping!";

        //Order Status
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusApproved = "Approved";
        public const string OrderStatusInProcess = "Processing";
        public const string OrderStatusShipped = "Shipped";
        public const string OrderStatusCancelled = "Cancelled";

        //Payment Status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "Approved for delayed payment";
        public const string PaymentStatusRejected = "Rejected";
        public const string PaymentStatusRefunded = "Refunded";
        public const string PaymentStatusCancelled = "Cancelled";
    }

    // Used when creating an admin
    public static class User
    {
        //Action Results
        public const string UserHasSuccessfullyBeenCreated = "The user has been created successfully!";

        //User Creation Details
        public const string AdminUserEmail = "admin@gmail.com";
        public const string AdminUserFirstName = "Admin";
        public const string AdminUserLastName = "Adminovich";
        public const string AdminUserState = "Adminzona";
        public const string AdminUserCity = "Admincity";
        public const string AdminUserStreetAddress = "Adminova 1";
        public const string AdminUserPostalCode = "2343";
        public const string AdminUserPhoneNumber = "0893453255";
        public const string AdminUserPassword = "Admin123!";
        public const bool AdminUserEmailConfirmed = true;
    }

    public static class ApplicationUser
    {
        //Display Names
        public const string FirstNameDisplay = "First Name";
        public const string LastNameDisplay = "Last Name";
        public const string UserRolesDisplay = "User's Roles";
        public const string CompanyIdDisplay = "Company";
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
    public static class Error
    {
        //Paths
        public const string NotFound404Path = "/Customer/Error/404";

        //Action Names
        public const string ErrorPageNotFoundAction = "NotFound404";
    }

    public static class Image
    {
        public const string DefaultImage = "DefaultImage";

        public const string ContentType = "image/jpeg";
    }

    public static class StripeSettings
    {
        public const string PublishableKeyAsString = "Stripe:PublishableKey";
        public const string SecretKeyAsString = "Stripe:SecretKey";

        public const string FullSuccessUrlShoppingCart = "{0}/Customer/ShoppingCart/OrderConfirmation?orderHeaderId={1}";
        public const string FullCancelUrlShoppingCart = "{0}/Customer/ShoppingCart/Index";

        public const string FullSuccessUrlOrder = "{0}/Admin/Order/OrderConfirmation?orderHeaderId={1}";
        public const string FullCancelUrlOrder = "{0}/Admin/Order/Index";
    }

    public static class SendGridSettings
    {
        public const string ApiKey = "SendGrid:SecretKey";

        //Email Configuration
        public const string FromEmail = "deyordanov@students.softuni.bg";
        public const string FromUserName = "Readers Realm";

        //Email Confirmation
        public const string EmailConfirmationSubject = "Welcome to Readers Realm! Please Confirm Your Email Address.";
        public const string EmailConfirmationHeaderMessage = "Dear {0},";
        public const string EmailConfirmationBodyMessage =
            $"We're thrilled to have you on board! Before you dive into the exciting world of our book collection, there's just one small step we need to complete. To activate your account and start your journey with us, please confirm your email address by <a href='{{0}}'>clicking here</a>.This ensures that we can communicate important updates and offers to you without a hitch.";
        public const string EmailConfirmationFooterMessage = $"Welcome aboard, and happy shopping! Warmest regards, The Readers Realm Team.";

        //Order
        public const string EmailOrderSubject = "Your order has been received.";
        public const string EmailOrderHeaderMessage = "Dear {0},";
        public const string EmailOrderBodyMessage = "A new order has been created, with id: {0}.";
        public const string EmailOrderFooterMessage = "Welcome aboard, and happy shopping! Warmest regards, The Readers Realm Team.";
    }

    public static class Configuration
    {
        //Configuration Section Keys
        public const string MongoDbSettingsKey = "MongoDb";
    }

    public static class SessionKeys
    {
        public const string ShoppingCartSessionKey = "ShoppingCartSession";
    }
}