using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CopyBag.Infrastructure.Repositories;

namespace CopyBag.Pages;


public partial class CopyBagSettingPage : ContentPage
{
    private string appPassword;
    string askPassword;
    DatabaseActions _databaseActions;
    public CopyBagSettingPage(DatabaseActions databaseActions)
    {
        InitializeComponent();
        _databaseActions = databaseActions;
        this.Loaded += CopyBagSettingPage_Loaded;
    }

    private async void CopyBagSettingPage_Loaded(object? sender, EventArgs e)
    {
        this.Loaded -= CopyBagSettingPage_Loaded;
        askPassword = await SecureStorage.Default.GetAsync("AskPassword") ?? "false";
        appPassword = await SecureStorage.Default.GetAsync("ApplicationPassword") ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(askPassword))
        {
            if (askPassword == "true")
            {
                AskPasswordSwitch.IsToggled = true;

                if (!string.IsNullOrWhiteSpace(appPassword))
                    ChangePasswordButton.IsEnabled = true;
            }
            else
            {
                AskPasswordSwitch.IsToggled = false;
                ChangePasswordButton.IsEnabled = false;
            }
        }
    }

    private async void AskPasswordToggle_Toggled(object sender, ToggledEventArgs e)
    {
        if (AskPasswordSwitch.IsToggled)
        {
            ChangePasswordButton.IsEnabled = true;
            await SecureStorage.Default.SetAsync("AskPassword", "true");
        }
        else
        {
            await SecureStorage.Default.SetAsync("AskPassword", "false");
            ChangePasswordButton.IsEnabled = false;
        }
    }

    private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("تغییر گذرواژه", "گذرواژه جدید را وارد کنید", "تایید", "لغو");
        if (!string.IsNullOrWhiteSpace(result))
        {
            await SecureStorage.Default.SetAsync("ApplicationPassword", result);
            var toast = Toast.Make("گذرواژه تغییر یافت", ToastDuration.Short, 14);

            await toast.Show();
        }
    }
    private async void DeleteAllDataButton_Clicked(object sender, EventArgs e)
    {
        bool result = await Application.Current.MainPage.DisplayAlert("حذف آیتم", "آیتم حذف شود؟", "تایید", "لغو", FlowDirection.RightToLeft);
        if (result)
        {
            await _databaseActions.ResetDatabase();
            var toast = Toast.Make("تمامی اطلاعات حذف گردید", ToastDuration.Short, 14);

            await toast.Show();
        }
    }
}