using Microsoft.AspNetCore.Components;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Client.Shared.Components.Notes;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupNoteTags : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    [Inject]
    private ITagService TagService { get; set; } = default!;

    private GroupTagRightsDTO Rights { get; set; } = new();
    private TagTree? TagTree { get; set; }

    protected override async void OnInitialized()
    {
        await Refresh();
        base.OnInitialized();
    }

    private async Task Refresh()
    {
        Rights = await GroupService.GetTagRights(GroupId);
        await InvokeAsync(StateHasChanged);
    }

    private Task<List<TagTreeItemDTO>> Fetcher() => TagService.GetTree(GroupId);

    private async Task Create()
    {
        if (!Rights.CanCreate)
        {
            return;
        }

        if (ObjectHelper.IsNotNull(TagTree))
        {
            await TagTree.Create();
        }
    }
}
