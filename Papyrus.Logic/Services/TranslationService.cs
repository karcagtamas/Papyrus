using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;

namespace Papyrus.Logic.Services;

public class TranslationService : ITranslationService
{
    private readonly PapyrusContext context;
    private readonly List<Translation> cache = new();

    public TranslationService(PapyrusContext context)
    {
        this.context = context;
    }

    public string GetValue(string key, string segment, string lang, bool ignoreCache = false)
    {
        if (!ignoreCache)
        {
            var c = cache.Where(x => x.Key == key && x.Segment == segment && x.Language == lang).FirstOrDefault();

            if (ObjectHelper.IsNotNull(c))
            {
                return c.Value;
            }
        }

        var t = context.Set<Translation>().Where(x => x.Key == key && x.Segment == segment && x.Language == lang).FirstOrDefault();

        if (ObjectHelper.IsNotNull(t))
        {
            AddToCache(t);
            return t.Value;
        }

        return key;
    }

    public List<Translation> GetValues(string segment, string lang)
    {
        var values = context.Set<Translation>().Where(x => x.Segment == segment && x.Language == lang).ToList();

        values.ForEach(x => AddToCache(x));

        return values;
    }

    private void AddToCache(Translation t)
    {
        var val = cache.FirstOrDefault(c => c.Key == t.Key && c.Segment == t.Segment && c.Language == t.Language);

        if (ObjectHelper.IsNotNull(val))
        {
            cache.Remove(val);
        }

        cache.Add(t);
    }
}
