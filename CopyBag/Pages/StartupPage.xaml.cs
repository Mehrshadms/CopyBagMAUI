namespace CopyBag.Pages;

public partial class StartupPage : ContentPage
{
    public StartupPage()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        await IsFirstLaunch();
        await NavigateBasedOnAskPassword();
        base.OnAppearing();
    }

    public async Task<bool> IsFirstLaunch()
    {
        string firstLaunch = await SecureStorage.Default.GetAsync("FirstLaunch") ?? "true";
        if (firstLaunch == "true")
        {
            await SecureStorage.Default.SetAsync("FirstLaunch", "false");
            await SecureStorage.Default.SetAsync("ApplicationPassword", "  ");
            await SecureStorage.Default.SetAsync("AskPassword", "false");
            return true;
        }
        return false;
    }

    public async Task NavigateBasedOnAskPassword()
    {
        var askPassword = await SecureStorage.Default.GetAsync("AskPassword") ?? "false";
        var appPassword = await SecureStorage.Default.GetAsync("ApplicationPassword") ?? " ";
        if (askPassword == "true" && !string.IsNullOrWhiteSpace(appPassword))
        {
            Application.Current.MainPage = new LockScreen();
        }
        else
        {
            Application.Current.MainPage = new AppShell();
        }

    }
}