using Blazored.LocalStorage;
using KarcagS.Blazor.Common.Store;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services;

public class TokenService : ITokenService
{
    private readonly ILocalStorageService localStorageService;
    private readonly IStoreService storeService;

    private const string UserKey = "user";
    private const string AccessTokenKey = "access-token";
    private const string RefreshTokenKey = "refresh-token";
    private const string ClientIdKey = "client-id";

    public TokenService(ILocalStorageService localStorageService, IStoreService storeService)
    {
        this.localStorageService = localStorageService;
        this.storeService = storeService;
    }

    public async Task<TokenDTO> GetUser()
    {
        return await localStorageService.GetItemAsync<TokenDTO>(UserKey);
    }

    public async Task RefreshStore()
    {
        var accessToken = await GetAccessToken();
        var refreshToken = await GetRefreshToken();
        var clientId = await GetClientId();

        var user = await GetUser();

        if (user is null)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                await localStorageService.RemoveItemAsync(AccessTokenKey);
            }

            return;
        }

        if (user.ClientId != clientId || user.AccessToken != accessToken || user.RefreshToken != refreshToken)
        {
            var tokenObj = new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ClientId = clientId
            };
            storeService.Add(UserKey, tokenObj);
            await localStorageService.SetItemAsync(UserKey, tokenObj);
        }
    }

    public async Task SetUser(TokenDTO dto)
    {
        await localStorageService.SetItemAsync(UserKey, dto);
        await localStorageService.SetItemAsync(AccessTokenKey, dto.AccessToken);
        await localStorageService.SetItemAsync(RefreshTokenKey, dto.RefreshToken);
        await localStorageService.SetItemAsync(ClientIdKey, dto.ClientId);
        storeService.Add(UserKey, dto);
    }

    public async Task ClearUser()
    {
        await localStorageService.RemoveItemAsync(UserKey);
        await localStorageService.RemoveItemAsync(AccessTokenKey);
        await localStorageService.RemoveItemAsync(RefreshTokenKey);
        await localStorageService.RemoveItemAsync(ClientIdKey);
        storeService.Remove(UserKey);
    }

    public bool UserInStore()
    {
        return storeService.IsExists(UserKey);
    }

    public async Task<string> GetClientId()
    {
        return await localStorageService.GetItemAsync<string>(ClientIdKey);
    }

    private async Task<string> GetAccessToken()
    {
        return await localStorageService.GetItemAsync<string>(AccessTokenKey);
    }

    private async Task<string> GetRefreshToken()
    {
        return await localStorageService.GetItemAsync<string>(RefreshTokenKey);
    }
}
