using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared;
using Papyrus.Shared.DTOs.Groups.Rights;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupRoles : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    private Dictionary<string, object> ExtraParams { get; set; } = new();
    private StyleConfiguration Style { get; set; } = StyleConfiguration.Build();
    private Table<int>? Table { get; set; }
    private GroupRoleRightsDTO Rights { get; set; } = new();
    private bool PageEnabled { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!await SetPageStatus())
        {
            return;
        }

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
    }

    private async Task<bool> SetPageStatus()
    {
        var rights = await GroupService.GetPageRights(GroupId);

        if (!rights.RolePageEnabled)
        {
            GroupService.NavigateToBase(GroupId);
            return false;
        }

        PageEnabled = true;
        return true;
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
