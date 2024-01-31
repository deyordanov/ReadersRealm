namespace ReadersRealm.Web.ViewModels;

/// <summary>
/// ViewModel representing error information in the Readers Realm application.
/// This model is typically used to display error details to the user or for logging purposes.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the unique request identifier associated with the error.
    /// </summary>
    /// <value>
    /// A string representing the unique identifier of the request during which the error occurred.
    /// This can be null if the request ID is not available or not applicable.
    /// </value>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the request ID should be shown.
    /// </summary>
    /// <value>
    /// True if the request ID is not null or empty, indicating that there is a request ID associated with the error; otherwise, false.
    /// </value>
    /// <remarks>
    /// This property can be used in views to conditionally display the request ID.
    /// It simplifies checking for the presence of a request ID in the view.
    /// </remarks>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}