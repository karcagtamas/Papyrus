namespace Papyrus.Logic.Utils;

public interface IExternalUrlBuilder
{
    string Build(string? path);
}

public class BasicUrlBuilder : IExternalUrlBuilder
{
    public BasicUrlBuilder()
    {

    }

    public string Build(string? path) => ExternalUrlHelper.ConstructApiUrl(path ?? "");
}

public class GroupUrlBuilder : IExternalUrlBuilder
{
    private readonly int groupId;

    public GroupUrlBuilder(int groupId)
    {
        this.groupId = groupId;
    }

    public string Build(string? path) => ExternalUrlHelper.ConstructGroupUrl(groupId, path);
}
