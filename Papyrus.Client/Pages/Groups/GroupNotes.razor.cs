using Microsoft.AspNetCore.Components;
using Papyrus.Shared.DTOs.Groups.Rights;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupNotes : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    private GroupNoteRightsDTO Rights { get; set; } = new();
    private bool PageEnabled { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!await SetPageStatus())
        {
            return;
        }

        await Refresh();
    }

    private async Task<bool> SetPageStatus()
    {
        var rights = await GroupService.GetPageRights(GroupId);

        if (!rights.NotePageEnabled)
        {
            GroupService.NavigateToBase(GroupId, !rights.DataPageEnabled);
            return false;
        }

        PageEnabled = true;
        return true;
    }

    private async Task Refresh()
    {
        Rights = await GroupService.GetNoteRights(GroupId);

        await InvokeAsync(StateHasChanged);
    }
}
