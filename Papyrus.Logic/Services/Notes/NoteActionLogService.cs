using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Logic.Services.Notes;

public class NoteActionLogService : INoteActionLogService
{
    private readonly PapyrusContext context;
    private readonly IActionLogService actionLogService;
    private readonly ILanguageService languageService;
    private readonly ITranslationService translationService;
    private readonly string NoteSegment = "Note";

    public NoteActionLogService(PapyrusContext context, IActionLogService actionLogService, ILanguageService languageService, ITranslationService translationService)
    {
        this.context = context;
        this.actionLogService = actionLogService;
        this.languageService = languageService;
        this.translationService = translationService;
    }

    public void AddActionLog(string note, string performer, NoteActionLogType type)
    {
        var languages = languageService.GetAll();

        var texts = new Dictionary<string, string>();

        var typeKey = NoteActionLogTypeConverter.GetLogKey(type);

        foreach (var lang in languages)
        {
            texts.Add(lang.ShortName, translationService.GetValue(typeKey, NoteSegment, lang.ShortName));
        }

        actionLogService.AddActionLog(note, NoteSegment, typeKey, performer, texts);
    }

    public IQueryable<ActionLog> GetQuery(string noteId)
    {
        var lang = languageService.GetUserLangOrDefault();

        return context.Set<ActionLog>()
            .AsQueryable()
            .Where(x => x.Key == noteId && x.Segment == NoteSegment && x.Language == lang);
    }
}
