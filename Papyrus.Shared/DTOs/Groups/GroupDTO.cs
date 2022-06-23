namespace Papyrus.Shared.DTOs.Groups;

public class GroupDTO : GroupListDTO
{
    public int Members { get; set; }
    public int Roles { get; set; }
    public int Notes { get; set; }
    public int Tags { get; set; }
    public DateTime? LastAction { get; set; }
    public bool IsClosed { get; set; }
}
