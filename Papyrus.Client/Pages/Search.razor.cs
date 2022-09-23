using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Pages;

public partial class Search : ComponentBase
{
    private SearchQueryModel QueryModel { get; set; } = new();
    private DateRange? Range { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
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
    }
}
