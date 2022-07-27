using KarcagS.Blazor.Common.Components.FileUploader;
using KarcagS.Blazor.Common.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Client.Shared.Dialogs;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Pages;

public partial class MyUser : ComponentBase
{
    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private IFileUploaderService FileUploaderService { get; set; } = default!;

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
        if (User is not null && User.ImageData is not null && User.ImageData.Length != 0)
        {
            string b64 = Convert.ToBase64String(User.ImageData);
            Image = $"data:image/gif;base64,{b64}";
        }
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
            ImageUpload = async data => await UserService.UpdateImage(data.Files.First().Content),
            EnabledExtensions = FileUploaderDialogInput.ImageExtensions,
        },
        "Change Profile Image"))
        {
            await GetUser();
        }
    }

    private async Task ChangePassword() => await HelperService.OpenEditorDialog<ChangePasswordDialog>("Change Password", async (res) => await GetUser(), new DialogParameters { });
}