using KarcagS.Blazor.Common.Store;
using Microsoft.AspNetCore.Components;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupTags : ComponentBase, IDisposable
{
    [Inject]
    private IStoreService Store { get; set; } = default!;

    [Parameter]
    public int GroupId { get; set; }

    protected override Task OnInitializedAsync()
    {
        Store.Add("GroupId", GroupId);
        return base.OnInitializedAsync();
    }

    public void Dispose()
    {
        // Store.Remove("GroupId");
    }
}
