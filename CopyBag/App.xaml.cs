using CopyBag.Pages;

namespace CopyBag
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new StartupPage();
        }
    }
}
