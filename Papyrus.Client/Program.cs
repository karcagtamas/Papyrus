using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using KarcagS.Blazor.Common;
using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Localization;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Blazor.Common.Store;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using Papyrus.Client;
using Papyrus.Client.Services;
using Papyrus.Client.Services.Auth;
using Papyrus.Client.Services.Auth.Interfaces;
using Papyrus.Client.Services.Editor;
using Papyrus.Client.Services.Editor.Interfaces;
using Papyrus.Client.Services.Groups;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Client.Services.Notes;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Auth;
using Papyrus.Shared.Enums;
using Papyrus.Shared.Localization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, PapyrusAuthStateProvider>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpService(config =>
{
    config.IsTokenBearer = true;
    config.UnauthorizedPath = "/logout";
    config.UnauthorizedPathRedirectQueryParamName = "redirectUri";
    config.AccessTokenName = "access-token";
    config.IsTokenRefresher = true;
    config.RefreshTokenName = "refresh-token";
    config.RefreshUri = builder.Configuration.GetSection("RefreshUri").Value;
    config.RefreshTokenPlaceholder = ":refreshToken";
    config.ClientIdName = "client-id";
    config.ClientIdPlaceholder = ":clientId";
});
builder.Services.AddStoreService(async (storeService, localStorage) =>
{
    const string userKey = "user";
    var user = await localStorage.GetItemAsync<TokenDTO>(userKey);

    if (ObjectHelper.IsNotNull(user))
    {
        storeService.Add(userKey, user);
    }

    ObjectHelper.WhenNotNull(user, u => storeService.Add(userKey, u));

    const string themeKey = "uitheme";
    var theme = await localStorage.GetItemAsync<int>(themeKey);

    ObjectHelper.WhenNotNull(theme, t => storeService.Add("theme", t));
});

builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IHelperService, HelperService>();
builder.Services.AddScoped<IToasterService, ToasterService>();
builder.Services.AddScoped<IConfirmService, ConfirmService>();
builder.Services.AddScoped<IFileUploaderService, FileUploaderService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTableService, UserTableService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupRoleService, GroupRoleService>();
builder.Services.AddScoped<IGroupRoleTableService, GroupRoleTableService>();
builder.Services.AddScoped<IGroupMemberService, GroupMemberService>();
builder.Services.AddScoped<IGroupMemberTableService, GroupMemberTableService>();
builder.Services.AddScoped<IGroupActionLogTableService, GroupActionLogTableService>();

builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteActionLogTableService, NoteActionLogTableService>();

builder.Services.AddScoped<IEditorService, EditorService>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});

builder.Services.AddLocalization(opt =>
{
    opt.ResourcesPath = "Resources";
});

builder.Services.AddLibraryLocalization();

ApplicationSettings.BaseUrl = builder.Configuration.GetSection("SecureApi").Value;
ApplicationSettings.BaseApiUrl = $"{ApplicationSettings.BaseUrl}/api";

ApplicationContext.ApplicationName = "Papyrus";

var host = builder.Build();


// Set local language
CultureInfo culture;
var js = host.Services.GetRequiredService<IJSRuntime>();
var langService = host.Services.GetRequiredService<ILanguageService>();

var userLanguage = await langService.GetUserLanguage();

if (userLanguage != null)
{
    culture = new CultureInfo(userLanguage.ShortName);
    await js.InvokeVoidAsync("blazorCulture.set", userLanguage.ShortName);
}
else
{
    var result = await js.InvokeAsync<string>("blazorCulture.get");

    if (result != null)
    {
        culture = new CultureInfo(result);
    }
    else
    {
        culture = new CultureInfo("en-US");
        await js.InvokeVoidAsync("blazorCulture.set", "en-US");
    }
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// Set local theme
var commonService = host.Services.GetRequiredService<ICommonService>();
try
{
    var theme = await commonService.GetUserTheme();
    await commonService.SetLocalTheme(theme, false);
}
catch (Exception)
{
    await commonService.SetLocalTheme((int)Theme.Light);
}

// Set logging
var localizerFactory = host.Services.GetRequiredService<IStringLocalizerFactory>();
var localizer = localizerFactory.Create(typeof(Program));
ErrorMessageLocalizer.GetInstance().AddLocalizer(localizer);
LocalizationExtensions.RegisterLibraryLocalizator(localizer);

await host.RunAsync();
