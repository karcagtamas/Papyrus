using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Services.Interfaces;

public interface ICommonService
{
    List<ThemeDTO> GetTranslatedThemeList(string? lang = null);
    void SetUserTheme(int key);
    int GetUserTheme();
}
