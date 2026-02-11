namespace GymManagement.Models;

/// <summary>
/// ViewModel for the error page.
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
