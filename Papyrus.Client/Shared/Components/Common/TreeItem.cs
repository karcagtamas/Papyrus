namespace Papyrus.Client.Shared.Components.Common;

public class TreeItem<T>
{
    public T Data { get; set; } = default!;
    public bool IsExpanded { get; set; } = true;
    public bool HasChild { get => Items.Count > 0; }
    public HashSet<TreeItem<T>> Items { get; set; } = new();

    public TreeItem(T data, Func<T, List<T>> itemExtractor)
    {
        Data = data;
        Items = itemExtractor(data).Select(x => new TreeItem<T>(x, itemExtractor)).ToHashSet();
    }
}
