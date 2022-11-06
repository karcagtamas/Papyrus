using KarcagS.Common.Tools.HttpInterceptor.Agents;
using KarcagS.Shared.Http;

namespace Papyrus.Logic.Exceptions.Profile;

public class ApplicationErrorConverterAgent : IErrorConverterAgent
{
    public HttpErrorResult? TryToConvert(Exception exception)
    {
        if (exception is ApplicationNotFoundException anfe)
        {
            return new(anfe)
            {
                Message = new ResourceMessage { Text = "Application Not Found", ResourceKey = "External.Messages.AppNotFound" },
                SubMessages = Array.Empty<ResourceMessage>(),
            };
        }

        return null;
    }
}
