using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.Rights;

namespace Papyrus.Client.Services.Groups;

public class GroupService : HttpCall<int>, IGroupService
{
    private readonly IStringLocalizer<GroupService> localizer;
    private readonly NavigationManager navigation;

    public GroupService(IHttpService http, IStringLocalizer<GroupService> localizer, NavigationManager navigation) : base(http, $"{ApplicationSettings.BaseApiUrl}/Group", "Group", localizer)
    {
        this.localizer = localizer;
        this.navigation = navigation;
    }

    public Task<bool> Close(int id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Close"))
            .AddToaster(localizer["Toaster.Close"]);

        return Http.PutWithoutBody(settings).Execute();
    }

    public Task<bool> Open(int id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Open"))
            .AddToaster(localizer["Toaster.Open"]);

        return Http.PutWithoutBody(settings).Execute();
    }

    public Task<bool> Remove(int id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Remove"))
            .AddToaster(localizer["Toaster.Remove"]);

        return Http.PutWithoutBody(settings).Execute();
    }

    public Task<List<GroupListDTO>> GetUserList(bool hideClosed = false)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("hideClosed", hideClosed);

        var settings = new HttpSettings(Http.BuildUrl(Url, "User")).AddQueryParams(queryParams);

        return Http.Get<List<GroupListDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<GroupRightsDTO> GetRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return Http.Get<GroupRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<GroupRoleRightsDTO> GetRoleRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights")
            .Add("Role");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return Http.Get<GroupRoleRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<GroupTagRightsDTO> GetTagRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights")
            .Add("Tag");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return Http.Get<GroupTagRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<GroupMemberRightsDTO> GetMemberRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights")
            .Add("Member");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return Http.Get<GroupMemberRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<GroupPageRightsDTO> GetPageRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights")
            .Add("Pages");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return Http.Get<GroupPageRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public void NavigateToBase(int id, bool redirectToList) => navigation.NavigateTo(redirectToList ? "my-groups" : $"groups/{id}");

    public Task<GroupNoteRightsDTO> GetNoteRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
             .Add(id)
             .Add("Rights")
             .Add("Note");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return Http.Get<GroupNoteRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }
}
