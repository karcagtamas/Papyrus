using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Enums.Groups;

namespace Papyrus.Logic.Services.Groups;

public class GroupRoleService : MapperRepository<GroupRole, int, string>, IGroupRoleService
{
    private readonly IGroupActionLogService groupActionLogService;
    private readonly ILanguageService languageService;
    private readonly ITranslationService translationService;
    private readonly string TranslationSegment = "GroupRole";

    public GroupRoleService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupActionLogService groupActionLogService, ILanguageService languageService, ITranslationService translationService) : base(context, loggerService, utilsService, mapper, "Group Role")
    {
        this.groupActionLogService = groupActionLogService;
        this.languageService = languageService;
        this.translationService = translationService;
    }

    public List<RoleCreationResultItem> CreateDefaultRoles(int groupId)
    {
        return new() {
            new RoleCreationResultItem
            {
                Id = CreateAdminRole(groupId),
                IsAdministrator = true
            },
            new RoleCreationResultItem
            {
                Id = CreateModeratorRole(groupId),
                IsAdministrator = false
            },
            new RoleCreationResultItem
            {
                Id = CreateDefaultRole(groupId),
                IsAdministrator = false
            },
        };
    }

    public List<GroupRoleDTO> GetByGroup(int groupId, string? textFilter = null)
    {
        return GetMappedList<GroupRoleDTO>(x => x.GroupId == groupId && (textFilter == null || x.Name.Contains(textFilter)))
            .OrderByDescending(x => x.ReadOnly)
            .ThenBy(x => x.Id)
            .ToList();
    }

    public List<GroupRoleLightDTO> GetLightByGroup(int groupId)
    {
        return GetMappedList<GroupRoleLightDTO>(x => x.GroupId == groupId)
            .OrderBy(x => x.Name)
            .ToList();
    }

    private int CreateAdminRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Administrator",
            ReadOnly = true,
            IsDefault = false,
            GroupEdit = true,
            ReadNoteList = true,
            ReadNote = true,
            DeleteNote = true,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = true,
            ReadRoleList = true,
            EditRoleList = true,
            ReadGroupActionLog = true,
            ReadNoteActionLog = true,
            ReadTagList = true,
            EditTagList = true
        };

        return Create(role);
    }

    private int CreateModeratorRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Moderator",
            ReadOnly = true,
            IsDefault = false,
            GroupEdit = true,
            ReadNoteList = true,
            ReadNote = true,
            DeleteNote = true,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = false,
            ReadRoleList = true,
            EditRoleList = false,
            ReadGroupActionLog = true,
            ReadNoteActionLog = true,
            ReadTagList = true,
            EditTagList = true
        };

        return Create(role);
    }

    private int CreateDefaultRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Default",
            ReadOnly = true,
            IsDefault = true,
            GroupEdit = false,
            ReadNoteList = true,
            ReadNote = true,
            DeleteNote = false,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = false,
            ReadRoleList = false,
            EditRoleList = false,
            ReadGroupActionLog = false,
            ReadNoteActionLog = false,
            ReadTagList = true,
            EditTagList = false
        };

        return Create(role);
    }

    public override int Create(GroupRole entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        int id = base.Create(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.RoleCreate, doPersist);

        return id;
    }

    public override void Update(GroupRole entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Update(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.RoleEdit, doPersist);
    }

    public override void Delete(GroupRole entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Delete(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.RoleRemove, doPersist);
    }

    public bool Exists(int groupId, string name) => GetList(x => x.GroupId == groupId && x.Name == name).Any();

    public List<GroupRoleDTO> GetTranslatedByGroup(int groupId, string? textFilter = null, string? lang = null)
    {
        var roles = GetByGroup(groupId, textFilter);

        string current = languageService.GetLangOrUserLang(lang);

        var translations = translationService.GetValues(TranslationSegment, current);

        return roles.Select(role =>
        {
            var t = translations.Where(x => x.Key == role.Name).FirstOrDefault()?.Value ?? role.Name;
            role.Name = t;

            return role;
        }).ToList();
    }

    public List<GroupRoleLightDTO> GetLightTranslatedByGroup(int groupId, string? lang = null) => GetTranslatedByGroup(groupId, null, lang).Select(x => new GroupRoleLightDTO { Id = x.Id, Name = x.Name }).ToList();

    public GroupRoleLightDTO GetLightTranslated(int id, string? lang = null)
    {
        var role = GetMapped<GroupRoleLightDTO>(id);

        string current = languageService.GetLangOrUserLang(lang);

        var translation = translationService.GetValue(role.Name, TranslationSegment, current);

        role.Name = translation;

        return role;
    }

    public class RoleCreationResultItem
    {
        public int Id { get; set; }
        public bool IsAdministrator { get; set; }
    }
}
