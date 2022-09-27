using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Pages;

public partial class Search : ComponentBase
{
    private SearchQueryModel QueryModel { get; set; } = new();
    private DateRange? Range { get; set; }
    private bool OnlyPublicAvailable { get; set; } = false;
    private bool Loading { get; set; } = false;

    private List<SearchResultDTO> ResultList { get; set; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        OnlyPublicAvailable = Auth.IsLoggedIn();

        StateHasChanged();
    }

    private async void DoSearch()
    {
        Loading = true;
        StateHasChanged();

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
        ResultList = await NoteService.Search(QueryModel);
        Loading = false;
        StateHasChanged();
    }
}
