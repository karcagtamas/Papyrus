using KarcagS.Blazor.Common.Components.Confirm;
using KarcagS.Blazor.Common.Components.Tree;
using KarcagS.Blazor.Common.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Client.Shared.Dialogs.Notes;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Shared.Components.Notes;

public partial class TagTree : ComponentBase
{
    [Parameter, EditorRequired]
    public Func<Task<List<TagTreeItemDTO>>> Fetcher { get; set; } = () => Task.FromResult(new List<TagTreeItemDTO>());

    [Parameter]
    public bool CreateEnabled { get; set; } = true;

    [Parameter]
    public bool EditEnabled { get; set; } = true;

    [Parameter]
    public bool RemoveEnabled { get; set; } = true;

    [Parameter]
    public int? GroupId { get; set; }

    [Parameter]
    public EventCallback OnRefresh { get; set; }

    [Inject]
    private ITagService TagService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    [Inject]
    private IConfirmService ConfirmService { get; set; } = default!;

    private HashSet<TreeItem<TagTreeItemDTO>> TreeItems { get; set; } = new();
    private bool Loading { get; set; } = true;

    protected override async void OnInitialized()
    {
        await Refresh();
        base.OnInitialized();
    }

    public async Task Create()
    {
        await Create(null);
    }

    private async Task Refresh()
    {
        Loading = true;
        await InvokeAsync(StateHasChanged);
        TreeItems = Wrap(await Fetcher());
        Loading = false;
        await InvokeAsync(StateHasChanged);
        await OnRefresh.InvokeAsync();
    }

    private async Task Create(int? parentId)
    {
        if (!CreateEnabled)
        {
            return;
        }

        await OpenDialog(null, parentId);
    }

    private async Task Edit(int id)
    {
        if (!EditEnabled)
        {
            return;
        }

        await OpenDialog(id, null);
    }

    private async Task Remove(int id)
    {
        if (!RemoveEnabled)
        {
            return;
        }

        await ConfirmService.Open(new ConfirmDialogInput { Name = "Tag", ActionFunction = async () => await TagService.Delete(id) }, "Confirm Delete", async () => await Refresh());
    }

    private async Task OpenDialog(int? tagId, int? parentId)
    {
        var parameters = new DialogParameters { { "TagId", tagId }, { "GroupId", GroupId }, { "ParentId", parentId } };
        await HelperService.OpenDialog<TagEditDialog>(tagId is null ? "Create Tag" : "Edit Tag", async () => await Refresh(), parameters);
    }

    private static HashSet<TreeItem<TagTreeItemDTO>> Wrap(List<TagTreeItemDTO> src)
    {
        return src.Select(x => new TreeItem<TagTreeItemDTO>(x, (t) => t.Children)).ToHashSet();
    }
}
