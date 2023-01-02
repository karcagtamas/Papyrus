using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Shared.Dialogs.Common;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared;
using Papyrus.Shared.DTOs.Groups.Rights;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupMembers : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    private Dictionary<string, object> TableParams { get; set; } = new();
    private StyleConfiguration Style { get; set; } = StyleConfiguration.Build();
    private Table<int>? Table { get; set; }
    private GroupMemberRightsDTO Rights { get; set; } = new();
    private bool PageEnabled { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!await SetPageStatus())
        {
            return;
        }

        TableParams = new Dictionary<string, object>
        {
            { "groupId", GroupId }
        };
        Style.AddColorGetter(value => value.Tags.Contains(Tags.CurrentUserTag) ? Color.Error : Color.Default);

        await Refresh(false);
    }

    private async Task<bool> SetPageStatus()
    {
        var rights = await GroupService.GetPageRights(GroupId);

        if (!rights.MemberPageEnabled)
        {
            GroupService.NavigateToBase(GroupId, !rights.DataPageEnabled);
            return false;
        }

        PageEnabled = true;
        return true;
    }

    private async Task Refresh(bool tableRefresh = true)
    {
        Rights = await GroupService.GetMemberRights(GroupId);

        if (tableRefresh)
        {
            Table?.Refresh();
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task Add()
    {
        if (!Rights.CanAdd)
        {
            return;
        }

        var userKeys = await GroupMemberService.GetMemberKeys(GroupId, Table?.GetData().Select(x => x.ItemKey).ToList() ?? new());

        var parameters = new DialogParameters { { "Ignored", userKeys } };

        var dialog = DialogService.Show<UserSearchDialog>(L["SearchUserTitle"], parameters, new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (!result.Canceled && result.Data is string userId)
        {
            if (await GroupMemberService.Create(new GroupMemberCreateModel { UserId = userId, GroupId = GroupId }))
            {
                await Refresh();
            }
        }
    }

    private async Task RowClickHandler(ResultRowItem<int> item)
    {
        if (!Rights.CanEdit)
        {
            return;
        }

        var parameters = new DialogParameters { { "GroupId", GroupId }, { "MemberId", item.ItemKey } };

        await HelperService.OpenEditorDialog<GroupMemberEditDialog>("Edit Group Member", async (res) => await Refresh(), parameters);
    }
}
