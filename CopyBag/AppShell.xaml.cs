using CopyBag.Pages;

namespace CopyBag
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LockScreen), typeof(LockScreen));
            Routing.RegisterRoute(nameof(ContactGroupPage), typeof(ContactGroupPage));
            Routing.RegisterRoute(nameof(ContactsByGroupPage), typeof(ContactsByGroupPage));
            Routing.RegisterRoute(nameof(ContactCopyFolderPage), typeof(ContactCopyFolderPage));
            Routing.RegisterRoute(nameof(CopyItemsFolderPage), typeof(CopyItemsFolderPage));
            Routing.RegisterRoute(nameof(CopyBagSettingPage), typeof(CopyBagSettingPage));
        }
    }
}
