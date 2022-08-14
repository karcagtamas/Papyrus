namespace Papyrus.Shared.Models;

public class ImageModel
{
    public string Name { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public byte[] Content { get; set; } = default!;
    public long Size { get; set; }
}
