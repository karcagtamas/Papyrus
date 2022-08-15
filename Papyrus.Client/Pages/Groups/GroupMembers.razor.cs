using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Blazor.Common.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Auth.Interfaces;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Common;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupMembers : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    [Inject]
    private IGroupMemberService GroupMemberService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ITokenService TokenService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    private ListTable<GroupMemberDTO, int>? ListTable { get; set; }
    private TableDataSource<GroupMemberDTO, int> DataSource { get; set; } = default!;
    private TableConfiguration<GroupMemberDTO, int> Config { get; set; } = default!;

    private string? User { get; set; }
    private GroupMemberRightsDTO Rights { get; set; } = new();

    protected override async void OnInitialized()
    {
        await Refresh(false);
        DataSource = new TableDataSource<GroupMemberDTO, int>((filter) => GroupMemberService.GetByGroup(GroupId));
        Config = TableConfiguration<GroupMemberDTO, int>.Build()
            .AddTitle("Group Members")
            .AddColumn(
                new()
                {
                    Key = "user",
                    Title = "User",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => obj.User.UserName,
                    ColorGetter = (obj, i) => obj.User.Id == User ? Color.Error : Color.Default,
                }
            )
            .AddColumn(
                new()
                {
                    Key = "role",
                    Title = "Role",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => obj.Role.Name
                }
            )
            .AddColumn(
                new()
                {
                    Key = "added-by",
                    Title = "Added By",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => WriteHelper.WriteNullableField(obj.AddedBy?.UserName)
                }
            )
            .AddColumn(
                new()
                {
                    Key = "join",
                    Title = "Join",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => DateHelper.DateToString(obj.Join)
                }
            )
            .DisableClickOn((obj) => !Rights.CanEdit || (User is not null && obj.User.Id == User))
            .AddFilter(TableFilterConfiguration.Build().IsTextFilterEnabled(true));
        await InvokeAsync(StateHasChanged);
        base.OnInitialized();
    }

    private async Task Refresh(bool tableRefresh = true)
    {
        User = (await TokenService.GetUser()).UserId;
        Rights = await GroupService.GetMemberRights(GroupId);

        if (tableRefresh)
        {
            ListTable?.Refresh();
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task Add()
    {
        if (!Rights.CanAdd)
        {
            return;
        }

        var parameters = new DialogParameters { { "Ignored", DataSource.RawData.Select(x => x.User.Id).ToList() } };

        var dialog = DialogService.Show<UserSearchDialog>("Search User", parameters, new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (!result.Cancelled && result.Data is string userId)
        {
            if (await GroupMemberService.Create(new GroupMemberCreateModel { UserId = userId, GroupId = GroupId }))
            {
                await Refresh();
            }
        }
    }

    private async Task RowClickHandler(RowItem<GroupMemberDTO, int> item)
    {
        var parameters = new DialogParameters { { "GroupId", GroupId }, { "Member", item.Data } };

        await HelperService.OpenEditorDialog<GroupMemberEditDialog>("Edit Group Member", async (res) => await Refresh(), parameters);
    }
}
