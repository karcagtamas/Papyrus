using KarcagS.Blazor.Common.Components.FileUploader;
using KarcagS.Blazor.Common.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Papyrus.Client.Services.Auth.Interfaces;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Client.Shared.Dialogs;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Client.Pages;

public partial class MyUser : ComponentBase
{
    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private IFileUploaderService FileUploaderService { get; set; } = default!;

    [Inject]
    private ICommonService CommonService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private ITokenService TokenService { get; set; } = default!;

    private UserDTO? User { get; set; }
    private string? Image { get; set; }

    protected override async void OnInitialized()
    {
        await GetUser();
        base.OnInitialized();
    }

    private async Task GetUser()
    {
        User = await UserService.Current();
        ObjectHelper.WhenNotNull(User, u =>
        {
            ObjectHelper.WhenNotNull(u.ImageId, async img =>
            {
                var token = await TokenService.GetAccessToken();

                ObjectHelper.WhenNotNull(token, async t =>
                {
                    Image = await JSRuntime.InvokeAsync<string>("displayProtectedFile", CommonService.GetFileUrl(img), t);
                    await InvokeAsync(StateHasChanged);
                });
            });
        });
        await InvokeAsync(StateHasChanged);
    }

    private async Task Edit()
    {
        var parameters = new DialogParameters { { "UserId", User?.Id ?? "" } };

        await HelperService.OpenEditorDialog<UserEditDialog>("Edit User", async (res) => await GetUser(), parameters);
    }

    private async void ChangeImage()
    {
        if (await FileUploaderService.Open(new FileUploaderDialogInput
        {
            ImageUpload = async data =>
            {
                var imageFile = data.Files.First();
                return await UserService.UpdateImage(new ImageModel
                {
                    Name = imageFile.Name,
                    Content = imageFile.Content,
                    Size = imageFile.Size,
                    Extension = imageFile.Extension
                });
            },
            EnabledExtensions = FileUploaderDialogInput.ImageExtensions,
        },
        "Change Profile Image"))
        {
            await GetUser();
        }
    }

    private async Task ChangePassword() => await HelperService.OpenEditorDialog<ChangePasswordDialog>("Change Password", async (res) => await GetUser(), new DialogParameters { });
}