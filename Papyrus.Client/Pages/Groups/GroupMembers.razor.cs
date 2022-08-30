using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Common;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupMembers : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IGroupMemberTableService GroupMemberTableService { get; set; } = default!;

    private Dictionary<string, object> ExtraParams { get; set; } = new();

    private StyleConfiguration Style { get; set; } = StyleConfiguration.Build();

    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    [Inject]
    private IGroupMemberService GroupMemberService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    private Table<int>? Table { get; set; }
    private GroupMemberRightsDTO Rights { get; set; } = new();

    protected override async void OnInitialized()
    {
        ExtraParams = new Dictionary<string, object>
        {
            { "groupId", GroupId }
        };
        Style.AddColorGetter(value => value.Tags.Contains(Tags.CurrentUserTag) ? Color.Error : Color.Default);

        await Refresh(false);

        base.OnInitialized();
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

        var userKeys = await GroupMemberService.GetMemberKeys(Table?.GetData().Select(x => x.ItemKey).ToList() ?? new());

        var parameters = new DialogParameters { { "Ignored", userKeys } };

        var dialog = DialogService.Show<UserSearchDialog>(L["SearchUserTitle"], parameters, new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (!result.Cancelled && result.Data is string userId)
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
