using Microsoft.AspNetCore.Components;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Client.Shared.Components.Notes;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Pages.Notes;

public partial class NoteTags : ComponentBase
{
    [Inject]
    private ITagService TagService { get; set; } = default!;

    private TagTree? TagTree { get; set; }

    private Task<List<TagTreeItemDTO>> Fetcher() => TagService.GetTree();

    private async Task Create()
    {
        if (ObjectHelper.IsNotNull(TagTree))
        {
            await TagTree.Create();
        }
    }
}
