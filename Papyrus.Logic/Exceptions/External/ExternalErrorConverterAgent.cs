using KarcagS.Common.Tools.HttpInterceptor.Agents;
using KarcagS.Shared.Http;

namespace Papyrus.Logic.Exceptions.External;

public class ExternalErrorConverterAgent : IErrorConverterAgent
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
        else if (exception is GroupNotFoundException gnfe)
        {
            return new(gnfe)
            {
                Message = new ResourceMessage { Text = "Group Not Found", ResourceKey = "External.Messages.GroupNotFound" },
                SubMessages = Array.Empty<ResourceMessage>(),
            };
        }

        return null;
    }
}
