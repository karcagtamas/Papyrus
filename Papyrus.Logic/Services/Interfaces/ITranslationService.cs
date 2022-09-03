using Papyrus.DataAccess.Entities;

namespace Papyrus.Logic.Services.Interfaces;

public interface ITranslationService
{
    string GetValue(string key, string segment, string lang);
    List<Translation> GetValues(string segment, string lang);
}
