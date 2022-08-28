using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupRoles : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IGroupRoleTableService GroupRoleTableService { get; set; } = default!;

    private Dictionary<string, object> ExtraParams { get; set; } = new();

    private StyleConfiguration Style { get; set; } = StyleConfiguration.Build();

    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    private Table<int>? Table { get; set; }

    private GroupRoleRightsDTO Rights { get; set; } = new();

    protected override async void OnInitialized()
    {
        ExtraParams = new Dictionary<string, object>
        {
            { "groupId", GroupId }
        };
        Style.AddColorGetter(item =>
        {
            if (item.Tags.Contains(Tags.TrueValue))
            {
                return Color.Success;
            }
            else if (item.Tags.Contains(Tags.FalseValue) || item.Tags.Contains(Tags.ReadOnly))
            {
                return Color.Error;
            }
            else if (item.Tags.Contains(Tags.RoleName))
            {
                return Color.Secondary;
            }

            return Color.Default;
        }).AddTitleColorGetter(col =>
        {
            if (col == "name" || col == "readonly")
            {
                return Color.Primary;
            }

            return Color.Secondary;
        });

        await Refresh(false);

        base.OnInitialized();
    }

    private async Task Refresh(bool tableRefresh = true)
    {
        Rights = await GroupService.GetRoleRights(GroupId);

        if (tableRefresh)
        {
            Table?.Refresh();
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task Create()
    {
        if (!Rights.CanCreate)
        {
            return;
        }

        await OpenDialog(null);
    }

    private async Task OpenDialog(int? groupRoleId)
    {
        var parameters = new DialogParameters { { "GroupRoleId", groupRoleId }, { "GroupId", GroupId } };

        await HelperService.OpenEditorDialog<GroupRoleEditDialog>(groupRoleId is null ? "Create Group Role" : "Edit Group Role", async (res) => await Refresh(), parameters, new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }

    private async Task RowClickHandler(ResultRowItem<int> item) => await OpenDialog(item.ItemKey);
}
