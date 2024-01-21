using CommunityToolkit.Maui;
using CopyBag.Infrastructure;
using CopyBag.Infrastructure.Repositories;
using CopyBag.Models.Contacts;
using CopyBag.Models.CopyBin;
using CopyBag.Pages;
using CopyBag.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace CopyBag
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().UseSkiaSharp().UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Pro-Regular-400.otf", "FAPR");

                });

            //Repos
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "CopyPackDb.db");
            builder.Services.AddSingleton<DbContext>(db => ActivatorUtilities.CreateInstance<DbContext>(db, dbPath));
            builder.Services.AddSingleton<IContactGroupRepository, ContactGroupRepository>();
            builder.Services.AddSingleton<IContactRepository, ContactRepository>();
            builder.Services.AddSingleton<ICopyFolderRepository, CopyFolderRepository>();
            builder.Services.AddSingleton<ICopyItemRepository, CopyItemRepository>();
            builder.Services.AddSingleton<DatabaseActions>();

            builder.Services.AddSingleton<CopyBagSettingPage>();

            //Lockscreen
            builder.Services.AddSingleton<LockScreenViewModel>();
            builder.Services.AddSingleton<LockScreen>();

            //Groups
            builder.Services.AddSingleton<ContactGroupViewModel>();
            builder.Services.AddSingleton<ContactGroupPage>();

            //Contacts Within a group
            builder.Services.AddTransient<ContactsByGroupPage>();
            builder.Services.AddTransient<ContactsByGroupViewModel>();

            //Folders Within a Contact
            builder.Services.AddTransient<ContactCopyFolderPage>();
            builder.Services.AddTransient<CopyFolderViewModel>();

            //Copies Within a Folder
            builder.Services.AddTransient<CopyItemsFolderPage>();
            builder.Services.AddTransient<CopyItemsViewModel>();


            return builder.Build();
        }
    }
}
