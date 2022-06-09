using KarcagS.Blazor.Common.Components.ImageUploader;
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
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private IImageUploaderService ImageUploaderService { get; set; } = default!;

    private UserDTO? User { get; set; }
    private string? Image { get; set; }

    protected override async void OnInitialized()
    {
        GetUser();
        await base.OnInitializedAsync();
    }

    private async void GetUser()
    {
        User = await UserService.Current();
        if (User is not null && User.ImageData is not null && User.ImageData.Length != 0)
        {
            string b64 = Convert.ToBase64String(User.ImageData);
            Image = $"data:image/gif;base64,{b64}";
        }
        StateHasChanged();
    }

    private async void Edit()
    {
        var parameters = new DialogParameters { { "UserId", User?.Id ?? "" } };
        var dialog = DialogService.Show<UserEditDialog>("Edit User", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            GetUser();
        }
    }

    private async void ChangeImage()
    {
        if (await ImageUploaderService.Open(new ImageUploaderDialogInput
        {
            ImageUpload = async data => await UserService.UpdateImage(data)
        },
        "Change Profile Image"))
        {
            GetUser();
        }
    }

    private async void ChangePassword()
    {
        var parameters = new DialogParameters { };
        var dialog = DialogService.Show<ChangePasswordDialog>("Change Password", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            GetUser();
        }
    }
}