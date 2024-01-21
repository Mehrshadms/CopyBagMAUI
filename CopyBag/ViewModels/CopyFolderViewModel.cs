using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CopyBag.Models.CopyBin;
using CopyBag.Pages;
using CopyBag.Services;
using System.Collections.ObjectModel;

namespace CopyBag.ViewModels
{
    [QueryProperty(nameof(ContactId), "ContactId")]
    public partial class CopyFolderViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ICopyFolderRepository _copyFolderRepository;
        private List<CopyFolder> _copyFolders;
        private bool IsSortAtoZ = true;
        private bool IsBusy;

        [ObservableProperty] string searchText = string.Empty;

        [ObservableProperty] string pageTitle;

        [ObservableProperty] int contactId;

        [ObservableProperty] ObservableCollection<CopyFolder> copyFolders = new ObservableCollection<CopyFolder>();

        [RelayCommand]
        public async Task AddFolder()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("نام پوشه", "نام پوشه را بنویسید", "تایید", "لغو");

            if (string.IsNullOrWhiteSpace(result)) return;

            if (await _copyFolderRepository.Exists(x => x.ContactId == ContactId && x.FolderName == result))
            {
                await Application.Current.MainPage.DisplayAlert("ثبت ناموفق", $"پوشه با نام '{result}' وجود دارد", "متوجه شدم!", FlowDirection.RightToLeft);
                return;
            }

            CopyFolder copyFolder = new CopyFolder(result, ContactId);
            var operationResult = await _copyFolderRepository.Add(copyFolder);
            if (operationResult)
            {
                Task.Run(() => LoadFolders());
                DisplayPopupService.PopupStatus("پوشه اضافه شد", DisplayPopupService.PopupTarget.Insert);
            }
            else
            {
                DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
            }
        }

        [RelayCommand]
        public async Task EditFolder(int id)
        {
            if (id == 0) return;

            CopyFolder copyFolder = await _copyFolderRepository.Get(id);

            if (copyFolder is null) return;

            string result = await Application.Current.MainPage.DisplayPromptAsync($"تغییر نام پوشه {copyFolder.FolderName}", "نام جدید پوشه را بنویسید", "تایید", "لغو", initialValue: copyFolder.FolderName);

            if (string.IsNullOrWhiteSpace(result)) return;

            if (await _copyFolderRepository.Exists(x => x.ContactId == ContactId && x.FolderName == result))
            {
                await Application.Current.MainPage.DisplayAlert("تغییر ناموفق", $"پوشه با نام '{result}' وجود دارد", "متوجه شدم!", FlowDirection.RightToLeft);
                return;
            }

            copyFolder.Edit(result, ContactId);
            var operationResult = await _copyFolderRepository.Edit(copyFolder);
            if (operationResult)
            {
                Task.Run(() => LoadFolders());
                DisplayPopupService.PopupStatus("پوشه تغییر کرد", DisplayPopupService.PopupTarget.Update);
            }
            else
            {
                DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
            }
        }

        [RelayCommand]
        public async Task DeleteFolder(int id)
        {
            if (id == 0) return;

            CopyFolder copyFolder = await _copyFolderRepository.Get(id);

            if (copyFolder is null) return;

            bool result = await Application.Current.MainPage.DisplayAlert($"حذف پوشه {copyFolder.FolderName}", "پوشه حذف شود؟", "تایید", "لغو", FlowDirection.RightToLeft);

            if (result)
            {
                var operationResult = await _copyFolderRepository.Remove(copyFolder);
                if (operationResult)
                {
                    Task.Run(() => LoadFolders());
                    DisplayPopupService.PopupStatus("پوشه حذف شد", DisplayPopupService.PopupTarget.Delete);
                }
                else
                {
                    DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
                }
            }
        }

        [RelayCommand]
        public async Task Search()
        {
            if (_copyFolders is null) return;
            if (IsBusy) return;
            IsBusy = true;
            await SortBasedOnState(_copyFolders.Where(x => x.FolderName.Contains(SearchText)));
            IsBusy = false;
        }

        private async Task SortBasedOnState(IEnumerable<CopyFolder> sortcollection)
        {
            if (IsSortAtoZ)
            {
                CopyFolders = new ObservableCollection<CopyFolder>(sortcollection.OrderBy(x => x.FolderName).ToList());
            }
            else
            {
                CopyFolders = new ObservableCollection<CopyFolder>(sortcollection.OrderByDescending(x => x.CreationDate).ToList());
            }
        }

        [RelayCommand]
        public async Task SortAtoZ()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsSortAtoZ = true;
            await SortBasedOnState(CopyFolders);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task SortTime()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsSortAtoZ = false;
            await SortBasedOnState(CopyFolders);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task GoToCopyFolderPageBy(int id)
        {
            await Shell.Current.GoToAsync($"{nameof(CopyItemsFolderPage)}?FolderId={id}");
        }

        public async void LoadFolders()
        {
            _copyFolders = await _copyFolderRepository.GetFoldersByContactIdAsync(ContactId);
            await SortBasedOnState(_copyFolders);

            if (string.IsNullOrWhiteSpace(PageTitle)) await LoadPageTitle();
        }

        private async Task LoadPageTitle()
        {
            PageTitle = $"مخاطب - {await _copyFolderRepository.GetContactNameByAsync(ContactId)}";
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ContactId = int.Parse(query["ContactId"].ToString());
            LoadFolders();
        }

        public CopyFolderViewModel(ICopyFolderRepository copyFolderRepository)
        {
            _copyFolderRepository = copyFolderRepository;
        }
    }
}
