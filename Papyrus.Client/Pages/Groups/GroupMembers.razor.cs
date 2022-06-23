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
    [Inject]
    private IGroupMemberService GroupMemberService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ITokenService TokenService { get; set; } = default!;

    [Parameter]
    public int GroupId { get; set; }

    private string? User { get; set; }
    private List<GroupMemberDTO> Members { get; set; } = new();
    private bool Loading { get; set; } = true;

    protected override async void OnInitialized()
    {
        await GetUser();
        await GetMembers();
        base.OnInitialized();
    }

    private async Task GetUser()
    {
        User = (await TokenService.GetUser()).UserId;
    }

    private async Task GetMembers()
    {
        Loading = true;
        await InvokeAsync(StateHasChanged);
        Members = await GroupMemberService.GetByGroup(GroupId);
        Loading = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task Add()
    {
        var parameters = new DialogParameters { { "Ignored", Members.Select(x => x.User.Id).ToList() } };

        var dialog = DialogService.Show<UserSearchDialog>("Search User", parameters, new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            if (result.Data is string userId)
            {
                if (await GroupMemberService.Create(new GroupMemberCreateModel { UserId = userId, GroupId = GroupId }))
                {
                    await GetMembers();
                }
            }
        }
    }

    private async Task Edit(TableRowClickEventArgs<GroupMemberDTO> e)
    {
        if (User is not null && e.Item.User.Id == User)
        {
            return;
        }

        var parameters = new DialogParameters { { "GroupId", GroupId }, { "Member", e.Item } };

        var dialog = DialogService.Show<GroupMemberEditDialog>("Edit Group Member", parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await GetMembers();
        }
    }
}
