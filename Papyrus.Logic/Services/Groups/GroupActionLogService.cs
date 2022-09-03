using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.Enums.Groups;

namespace Papyrus.Logic.Services.Groups;

public class GroupActionLogService : IGroupActionLogService
{
    private readonly PapyrusContext context;
    private readonly IActionLogService actionLogService;
    private readonly ILanguageService languageService;
    private readonly ITranslationService translationService;
    private readonly string GroupSegment = "Group";

    public GroupActionLogService(PapyrusContext context, IActionLogService actionLogService, ILanguageService languageService, ITranslationService translationService) : base()
    {
        this.context = context;
        this.actionLogService = actionLogService;
        this.languageService = languageService;
        this.translationService = translationService;
    }

    public void AddActionLog(int group, string performer, GroupActionLogType type)
    {
        var languages = languageService.GetAll();

        var texts = new Dictionary<string, string>();

        var typeKey = GroupActionLogTypeConverter.GetLogKey(type);

        foreach (var lang in languages)
        {
            texts.Add(lang.ShortName, translationService.GetValue(typeKey, GroupSegment, lang.ShortName));
        }

        actionLogService.AddActionLog(group.ToString(), GroupSegment, typeKey, performer, texts);
    }

    public IQueryable<ActionLog> GetQuery(int groupId)
    {
        var lang = languageService.GetUserLangOrDefault();

        return context.Set<ActionLog>()
            .AsQueryable()
            .Where(x => x.Key == groupId.ToString() && x.Segment == GroupSegment && x.Language == lang);
    }
}
