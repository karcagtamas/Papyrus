using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using System;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupNotes : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private INoteService NoteService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private List<NoteLightDTO> Notes { get; set; } = new();

    protected override async void OnInitialized()
    {
        await Refresh();
        base.OnInitialized();
    }

    private async Task Refresh()
    {
        Notes = await NoteService.GetByGroup(GroupId);

        await InvokeAsync(StateHasChanged);
    }

    private async Task Create()
    {
        var result = await NoteService.CreateEmpty(GroupId);

        if (ObjectHelper.IsNotNull(result))
        {
            await JSRuntime.InvokeAsync<object>("open", $"/notes/editor/{result.Id}", "_blank");
        }
    }
}
