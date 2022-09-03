using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;

namespace Papyrus.Logic.Services;

public class TranslationService : ITranslationService
{
    private readonly PapyrusContext context;

    public TranslationService(PapyrusContext context)
    {
        this.context = context;
    }

    public string GetValue(string key, string segment, string lang)
    {
        var t = context.Set<Translation>().Where(x => x.Key == key && x.Segment == segment && x.Language == lang).FirstOrDefault();

        return ObjectHelper.OrElse(t?.Value, key);
    }

    public List<Translation> GetValues(string segment, string lang) => context.Set<Translation>().Where(x => x.Segment == segment && x.Language == lang).ToList();
}
