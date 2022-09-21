using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services.Interfaces;

public interface ICommonService
{
    string GetFileUrl(string id);
    Task<List<ThemeDTO>> GetThemeList(string? lang = null);
    Task<int> GetUserTheme();
    Task SetUserTheme(int key);
}
