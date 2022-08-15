using KarcagS.Blazor.Common.Components.Table;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups.ActionsLogs;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupActionLogs : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IGroupActionLogService GroupActionLogService { get; set; } = default!;

    private TableDataSource<GroupActionLogDTO, long> DataSource { get; set; } = default!;
    private TableConfiguration<GroupActionLogDTO, long> Config { get; set; } = default!;

    protected override async void OnInitialized()
    {
        DataSource = new TableDataSource<GroupActionLogDTO, long>((filter) => GroupActionLogService.GetByGroup(GroupId));
        Config = TableConfiguration<GroupActionLogDTO, long>.Build()
            .AddTitle("Action Logs")
            .AddColumn(
                new()
                {
                    Key = "text",
                    Title = "Text",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => obj.Text
                }
            )
            .AddColumn(
                new()
                {
                    Key = "performer",
                    Title = "Performer",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => obj.Performer ?? "N/A"
                }
            )
            .AddColumn(
                new()
                {
                    Key = "creation",
                    Title = "Creation",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => obj.Creation
                }
            );
        await InvokeAsync(StateHasChanged);
        base.OnInitialized();
    }
}
