using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Editor.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services.Editor;

public class EditorService : IEditorService
{
    private readonly IHttpService httpService;
    private readonly string url = $"{ApplicationSettings.BaseApiUrl}/Editor";

    public EditorService(IHttpService httpService)
    {
        this.httpService = httpService;
    }

    public async Task<List<UserLightDTO>> GetMembers(string id)
    {
        var settings = new HttpSettings(httpService.BuildUrl(url, id, "Members"));

        return await httpService.Get<List<UserLightDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}
