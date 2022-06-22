using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Common;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupMembers : ComponentBase
{
    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Parameter]
    public int GroupId { get; set; }

    private List<GroupMemberDTO> Members { get; set; } = new();
    private bool Loading { get; set; } = true;

    protected override async void OnInitialized()
    {
        await GetMembers();
        base.OnInitialized();
    }

    private async Task GetMembers()
    {
        Loading = true;
        await InvokeAsync(StateHasChanged);
        Members = await GroupService.GetMembers(GroupId);
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
                if (await GroupService.AddMember(GroupId, userId))
                {
                    await GetMembers();
                }
            }
        }
    }
}
