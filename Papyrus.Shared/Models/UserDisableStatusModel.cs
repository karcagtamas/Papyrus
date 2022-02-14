namespace Papyrus.Shared.Models;

public class UserDisableStatusModel
{
    public List<string> Ids { get; set; } = new();
    public bool Status { get; set; }
}