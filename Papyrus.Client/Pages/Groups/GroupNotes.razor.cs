using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Papyrus.Shared.DTOs.Groups.Rights;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupNotes : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    private GroupNoteRightsDTO Rights { get; set; } = new();
    private List<NoteLightDTO> Notes { get; set; } = new();
    private NotePublishType PublishType { get; set; } = NotePublishType.All;
    private bool ArchivedStatus { get; set; } = false;
    private bool PageEnabled { get; set; } = false;

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

        Notes = await NoteService.GetByGroup(GroupId, PublishType, ArchivedStatus);

        await InvokeAsync(StateHasChanged);
    }

    private async Task Create()
    {
        var result = await NoteService.CreateEmpty(GroupId);

        if (ObjectHelper.IsNotNull(result))
        {
            await Refresh();
            await JSRuntime.InvokeAsync<object>("open", $"/notes/editor/{result.Id}", "_blank");
        }
    }

    private async Task HandlePublishTypeChange(NotePublishType publishType)
    {
        PublishType = publishType;
        await Refresh();
    }

    private async Task HandleArchivedStatusChange(bool status)
    {
        ArchivedStatus = status;
        await Refresh();
    }
}
