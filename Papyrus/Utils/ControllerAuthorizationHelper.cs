using Microsoft.AspNetCore.Mvc;

namespace Papyrus.Utils;

public static class ControllerAuthorizationHelper
{
    public static Task<ActionResult<T>> Authorize<T>(Func<Task<bool>> authorizer, Func<T> constructor) => Authorize(authorizer, () => Task.FromResult(constructor()));

    public static async Task<ActionResult<T>> Authorize<T>(Func<Task<bool>> authorizer, Func<Task<T>> constructor)
    {
        if (!await authorizer())
        {
            return new EmptyResult();
        }

        return await constructor();
    }

    public static async Task<ActionResult> AuthorizeVoid(Func<Task<bool>> authorizer, Action action)
    {
        if (!await authorizer())
        {
            return new EmptyResult();
        }

        action();

        return new OkResult();
    }
}
