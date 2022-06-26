using KarcagS.Blazor.Common.Components.Confirm;
using KarcagS.Blazor.Common.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Client.Shared.Components.Common;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupTags : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private ITagService TagService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    [Inject]
    private IConfirmService ConfirmService { get; set; } = default!;

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

    private async Task Create(int? parentId)
    {
        await OpenDialog(null, parentId);
    }

    private async Task Edit(int id)
    {
        await OpenDialog(id, null);
    }

    private async Task Remove(int id)
    {
        await ConfirmService.Open(new ConfirmDialogInput { Name = "Tag", ActionFunction = async () => await TagService.Delete(id) }, "Confirm Delete", async () => await GetTagTree());
    }

    private async Task OpenDialog(int? tagId, int? parentId)
    {
        var parameters = new DialogParameters { { "TagId", tagId }, { "GroupId", GroupId }, { "ParentId", parentId } };
        await HelperService.OpenDialog<GroupTagEditDialog>(tagId is null ? "Create Group Tag" : "Edit Group Tag", async () => await GetTagTree(), parameters);
    }
    private static HashSet<TreeItem<GroupTagTreeItemDTO>> Wrap(List<GroupTagTreeItemDTO> src)
    {
        return src.Select(x => new TreeItem<GroupTagTreeItemDTO>(x, (t) => t.Children)).ToHashSet();
    }
}
