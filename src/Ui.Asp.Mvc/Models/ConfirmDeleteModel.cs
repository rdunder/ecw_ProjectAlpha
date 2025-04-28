namespace Ui.Asp.Mvc.Models;

public class ConfirmDeleteModel
{
    public Guid Id { get; set; }
    public string Controller { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string Message { get; set; } = null!;
}