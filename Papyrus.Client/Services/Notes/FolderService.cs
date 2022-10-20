using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Services.Notes;

public class FolderService : HttpCall<string>, IFolderService
{
    public FolderService(IHttpService http, IStringLocalizer<FolderService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Folder", "Folder", localizer)
    {
    }

    public Task<bool> Exists(string parentFolderId, string name)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("parentFolder", parentFolderId)
            .Add("name", name);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Exists"))
            .AddQueryParams(queryParams);

        return Http.GetBool(settings).ExecuteWithResult();
    }

    public Task<FolderContentDTO> GetContent(string? folder, int? groupId)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("group", groupId)
            .Add("folder", folder);

        var settings = new HttpSettings(Http.BuildUrl(Url))
            .AddQueryParams(queryParams);

        return Http.Get<FolderContentDTO>(settings).ExecuteWithResultOrElse(new FolderContentDTO
        {
            ParentFolder = new(),
            Folders = new(),
            Notes = new()
        });
    }
}
