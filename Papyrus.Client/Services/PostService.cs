using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Interfaces;

namespace Papyrus.Client.Services;

public class PostService : HttpCall<int>, IPostService
{
    public PostService(IHttpService http, IStringLocalizer<PostService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Post", "Post", localizer)
    {
    }
}
