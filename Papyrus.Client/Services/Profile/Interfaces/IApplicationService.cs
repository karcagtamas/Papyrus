using KarcagS.Blazor.Common.Http;

namespace Papyrus.Client.Services.Profile.Interfaces;

public interface IApplicationService : IHttpCall<string>
{
    Task<bool> RefreshSecret(string id);
}
