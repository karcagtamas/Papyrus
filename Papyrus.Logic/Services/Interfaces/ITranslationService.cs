using Papyrus.DataAccess.Entities;

namespace Papyrus.Logic.Services.Interfaces;

public interface ITranslationService
{
    string GetValue(string key, string segment, string lang, bool ignoreCache = false);
    List<Translation> GetValues(string segment, string lang);
}
