using Microsoft.JSInterop;
using MudBlazor.Utilities;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services.Interfaces;

public interface ICommonService
{
    string GetFileUrl(string id);
    Task<List<ThemeDTO>> GetThemeList(string? lang = null);
    Task<int> GetUserTheme();
    Task SetUserTheme(int key);
    Task SetLocalTheme(int key, bool post = true);
    Task OpenNote(string id, IJSRuntime jsRuntime);
    Task<MudColor?> OpenColorPickerDialog(MudColor? selected = null);
}
