using KarcagS.Shared.Helpers;

namespace Papyrus.Logic.Utils;

public static class ExternalUrlHelper
{
    public static string ConstructGroupUrl(int id, string? path = null) => ConstructApiUrl(String.Join("/", new List<string?> { "Groups", id.ToString(), path }.Where(x => ObjectHelper.IsNotNull(x)).ToList()));

    public static string ConstructApiUrl(string path) => $"/api/External/{path}";
}
