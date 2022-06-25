using Microsoft.AspNetCore.Components;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Client.Shared.Components.Common;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupTags : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private ITagService TagService { get; set; } = default!;

    private HashSet<TreeItem<GroupTagTreeItemDTO>> TreeItems { get; set; } = new();
    private bool Loading { get; set; } = true;

    protected override async void OnInitialized()
    {
        await GetTagTree();
        base.OnInitialized();
    }

    private async Task GetTagTree()
    {
        Loading = true;
        await InvokeAsync(StateHasChanged);
        TreeItems = Wrap(await TagService.GetTreeByGroup(GroupId));
        Loading = false;
        await InvokeAsync(StateHasChanged);
    }

    private HashSet<TreeItem<GroupTagTreeItemDTO>> Wrap(List<GroupTagTreeItemDTO> src)
    {
        return src.Select(x => new TreeItem<GroupTagTreeItemDTO>(x, (t) => t.Children)).ToHashSet();
    }
}
