using CommunityToolkit.Maui.Alerts;

namespace CopyBag.Pages;

public partial class LockScreen : ContentPage
{

    public LockScreen()
    {
        InitializeComponent();

    }

    private async void btnSubmit_Clicked(object sender, EventArgs e)
    {
        LoadingIndicator.IsRunning = true;
        var passordString = await SecureStorage.Default.GetAsync("ApplicationPassword");
        if (!string.IsNullOrWhiteSpace(PassworyEntry.Text))
        {
            if (passordString == PassworyEntry.Text)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                var toast = Toast.Make("گذرواژه نادرست است", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
            }
        }
        LoadingIndicator.IsRunning = false;
    }
}