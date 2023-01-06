using KarcagS.Common.Tools.Services;
using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table.Enums;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared;

namespace Papyrus.Logic.Services;

public class UserTableService : TableService<User, string>, IUserTableService
{
    private readonly IUserService userService;
    private readonly IUtilsService<string> utilsService;
    private readonly IRoleService roleService;

    public UserTableService(IUserService userService, IUtilsService<string> utilsService, IRoleService roleService)
    {
        this.userService = userService;
        this.utilsService = utilsService;
        this.roleService = roleService;
        Initialize();
    }

    public override Configuration<User, string> BuildConfiguration()
    {
        var userId = utilsService.GetRequiredCurrentUserId();
        var translatedRoles = roleService.GetAllTranslated();
        return Configuration<User, string>
            .Build("user-table")
            .SetTitle("Users", "Table.Title")
            .AddColumn(Column<User, string>.Build("user-name")
                .SetTitle("User Name", "TableColumn.UserName")
                .AddValueGetter(obj => obj.UserName ?? "N/A"))
            .AddColumn(Column<User, string>.Build("email")
                .SetTitle("E-mail", "TableColumn.Email")
                .AddValueGetter(obj => obj.Email ?? "N/A")
                .SetWidth(280))
            .AddColumn(Column<User, string>.Build("full-name")
                .SetTitle("Full Name", "TableColumn.FullName")
                .AddValueGetter(obj => WriteHelper.WriteNullableField(obj.FullName)))
            .AddColumn(Column<User, string>.Build("last-login")
                .SetTitle("Last Login", "TableColumn.LastLogin")
                .AddValueGetter(obj => obj.LastLogin)
                .SetFormatter(ColumnFormatter.Date)
                .SetWidth(190))
            .AddColumn(Column<User, string>.Build("registration")
                .SetTitle("Registration", "TableColumn.Registration")
                .AddValueGetter(obj => obj.Registration)
                .SetFormatter(ColumnFormatter.Date)
                .SetWidth(190))
            .AddColumn(Column<User, string>.Build("disabled")
                .SetTitle("Disabled", "TableColumn.Disabled")
                .AddValueGetter(obj => obj.Disabled)
                .SetFormatter(ColumnFormatter.Logic, "TrueValue", "FalseValue")
                .SetWidth(80))
            .AddColumn(Column<User, string>.Build("role")
                .SetTitle("Role", "TableColumn.Role")
                .AddValueGetter(obj =>
                {
                    var userRole = obj.Roles.FirstOrDefault();

                    if (ObjectHelper.IsNotNull(userRole))
                    {
                        return translatedRoles.FirstOrDefault(x => x.Id == userRole.RoleId)?.Name ?? "N/A";
                    }

                    return "N/A";
                })
                .SetWidth(180))
            .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true))
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddTagProvider((obj, col) =>
            {
                if (col.Key == "disabled")
                {
                    return obj.Disabled ? Tags.TrueValue : Tags.FalseValue;
                }

                return "";
            })
            .AddTagProvider((obj, col) => userId == obj.Id ? Tags.CurrentUserTag : "")
            .AddTagProvider((obj, col) =>
            {
                if (col.Key == "role")
                {
                    var mappedRoles = obj.Roles.Select(x => translatedRoles.First(r => r.Id == x.RoleId)).ToList();
                    if (mappedRoles.Any(x => x.IsAdmin))
                    {
                        return Tags.AdminRole;
                    }
                    else if (mappedRoles.Any(x => x.IsModerator))
                    {
                        return Tags.ModeratorRole;
                    }
                }

                return "";
            })
            .DisableClickOn(obj => obj.Id == userId);
    }

    public override DataSource<User, string> BuildDataSource()
    {
        return ListTableDataSource<User, string>.Build((query) =>
        {
            if (query.ExtraParams.TryGetValue("role", out object? value))
            {
                return userService.GetListAsQuery(x => x.Roles.Any(r => r.RoleId == (value.ToString() ?? string.Empty))).Include(x => x.Roles);
            }

            return userService.GetAllAsQuery().Include(x => x.Roles);
        })
            .OrderBy(x => x.UserName)
            .ThenBy(x => x.Id)
            .ApplyOrdering()
            .SetEFFilteredEntries("UserName", "Email", "FullName");
    }

    public override Table<User, string> BuildTable()
    {
        return ListTableBuilder<User, string>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
    }
}
