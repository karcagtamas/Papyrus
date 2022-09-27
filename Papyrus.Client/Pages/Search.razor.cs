using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Pages;

public partial class Search : ComponentBase
{
    private SearchQueryModel QueryModel { get; set; } = new();
    private DateRange? Range { get; set; }
    private bool OnlyPublicAvailable { get; set; } = false;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        OnlyPublicAvailable = Auth.IsLoggedIn();
    }

    private void DoSearch()
    {
        if (ObjectHelper.IsNull(Range))
        {
            QueryModel.StartDate = null;
            QueryModel.EndDate = null;
        }
        else
        {
            QueryModel.StartDate = Range.Start;
            QueryModel.EndDate = Range.End;
        }

        // Send search request
        var list = NoteService.Search(QueryModel);
    }
}
