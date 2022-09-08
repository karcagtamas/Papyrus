using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Interfaces;

namespace Papyrus.Client.Services;

public class CommonService : ICommonService
{
    public string GetFileUrl(string id) => $"{ApplicationSettings.BaseApiUrl}/File/{id}";
}
