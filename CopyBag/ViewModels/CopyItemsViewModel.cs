using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CopyBag.Models.CopyBin;
using CopyBag.Pages;
using CopyBag.Services;
using System.Collections.ObjectModel;

namespace CopyBag.ViewModels
{
    [QueryProperty(nameof(FolderId), "FolderId")]
    public partial class CopyItemsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ICopyItemRepository _copyItemRepository;
        private readonly IPopupService _popupService;
        private List<CopyItem> _copyItems;
        private bool IsSortAtoZ = true;
        private bool IsBusy;
        private int editId = 0;

        [ObservableProperty] string titleItemCopy = string.Empty;
        [ObservableProperty] string popupTitleOne = string.Empty;
        [ObservableProperty] string popupCopyTextOne = string.Empty;
        [ObservableProperty] string popupTitleTwo = string.Empty;
        [ObservableProperty] string popupCopyTextTwo = string.Empty;
        [ObservableProperty] string searchText = string.Empty;

        [ObservableProperty] string pageTitle;

        [ObservableProperty] int folderId;

        [ObservableProperty] ObservableCollection<CopyItem> copyItems;

        public CopyItemsViewModel(ICopyItemRepository copyItemRepository, IPopupService popupService)
        {
            _copyItemRepository = copyItemRepository;
            _popupService = popupService;
        }

        [RelayCommand]
        public async Task OpenPopupCreate()
        {
            ReSetVariables();
            await OpenPopup(IsTwoInOne: false);
        }

        [RelayCommand]
        public async Task OpenPopup(bool IsTwoInOne = false)
        {
            Page page = Application.Current.MainPage;
            await page.ShowPopupAsync(new CopyItemPopup(IsTwoInOne));
        }

        [RelayCommand]
        public async Task ExcutePopupTask()
        {
            if (editId == 0)
            {
                await AddCopyItem();
            }
            else
            {
                await EditCopyItem(editId);
            }

        }

        private async Task AddCopyItem()
        {
            if (IsNotNull())
            {
                CopyItem copyItem = new CopyItem(TitleItemCopy, PopupTitleOne, PopupCopyTextOne, PopupTitleTwo, PopupCopyTextTwo, FolderId);

                bool result = await IsNotDuplicate(isTwoInOne: copyItem.IsTwin);
                if (result)
                {
                    ReSetVariables();
                    var operationResult = await _copyItemRepository.Add(copyItem);
                    if (operationResult)
                    {
                        Task.Run(() => LoadCopyItems());
                        DisplayPopupService.PopupStatus("آیتم اضافه شد", DisplayPopupService.PopupTarget.Insert);
                    }
                    else
                    {
                        DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
                    }
                }
            }

        }

        private void ReSetVariables()
        {
            TitleItemCopy = string.Empty;
            PopupTitleOne = string.Empty;
            PopupCopyTextOne = string.Empty;
            PopupTitleTwo = string.Empty;
            PopupCopyTextTwo = string.Empty;
            editId = 0;
        }

        //private bool IsFolderExists()
        //{
        //    if (string.IsNullOrWhiteSpace(PopupTitleTwo) || string.IsNullOrWhiteSpace(PopupCopyTextTwo)) return false;
        //    return true;
        //}

        private async Task<bool> IsNotDuplicate(int editId = 0, bool isTwoInOne = false)
        {
            if (editId != 0)
            {
                if (await _copyItemRepository.Exists(x => x.FolderId == FolderId && x.TitleItemCopy == TitleItemCopy && x.IsTwin == isTwoInOne && x.Id != editId))
                {
                    await Application.Current.MainPage.DisplayAlert("تغییر ناموفق", $"آیتمی با عنوان تکراری وجود دارد", "متوجه شدم!", FlowDirection.RightToLeft);
                    return false;
                }
            }
            else
            {
                if (await _copyItemRepository.Exists(x => x.FolderId == FolderId && x.TitleItemCopy == TitleItemCopy && x.IsTwin == isTwoInOne))
                {
                    await Application.Current.MainPage.DisplayAlert("تغییر ناموفق", $"آیتمی با عنوان تکراری وجود دارد", "متوجه شدم!", FlowDirection.RightToLeft);
                    return false;
                }
            }
            return true;
        }

        private bool IsNotNull()
        {
            if (string.IsNullOrWhiteSpace(TitleItemCopy) || string.IsNullOrWhiteSpace(PopupCopyTextOne)) return false;

            return true;
        }

        private async Task EditCopyItem(int id)
        {
            var copyItem = await _copyItemRepository.Get(id);
            if (IsNotNull())
            {
                copyItem.Edit(TitleItemCopy, PopupTitleOne, PopupCopyTextOne, PopupTitleTwo, PopupCopyTextTwo);
                var result = await IsNotDuplicate(copyItem.Id, copyItem.IsTwin);
                if (result)
                {
                    ReSetVariables();
                    var operationResult = await _copyItemRepository.Edit(copyItem);
                    if (operationResult)
                    {
                        Task.Run(() => LoadCopyItems());
                        DisplayPopupService.PopupStatus("آیتم تغییر کرد", DisplayPopupService.PopupTarget.Update);
                    }
                    else
                    {
                        DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
                    }
                }
            }
        }

        [RelayCommand]
        public async Task OpenEditPopup(int id)
        {
            ReSetVariables();
            if (id == 0) return;

            CopyItem copyItem = await _copyItemRepository.Get(id);

            if (copyItem is null) return;

            TitleItemCopy = copyItem.TitleItemCopy;
            PopupTitleOne = copyItem.TitleOne;
            PopupCopyTextOne = copyItem.CopyTextOne;
            PopupTitleTwo = copyItem.TitleTwo;
            PopupCopyTextTwo = copyItem.CopyTextTwo;
            editId = id;
            await OpenPopup(copyItem.IsTwin);
        }

        [RelayCommand]
        public async Task DeleteCopyItem(int id)
        {
            if (id == 0) return;

            var copyItem = await _copyItemRepository.Get(id);

            if (copyItem is null) return;

            bool result = await Application.Current.MainPage.DisplayAlert($"حذف آیتم {copyItem.TitleOne}", "آیتم حذف شود؟", "تایید", "لغو", FlowDirection.RightToLeft);

            if (result)
            {
                var operationResult = await _copyItemRepository.Remove(copyItem);
                if (operationResult)
                {
                    Task.Run(() => LoadCopyItems());
                    DisplayPopupService.PopupStatus("آیتم حذف شد", DisplayPopupService.PopupTarget.Delete);
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
            if (_copyItems is null) return;
            if (IsBusy) return;
            IsBusy = true;
            await SortBasedOnState(_copyItems.Where(x => x.TitleOne.Contains(SearchText)));
            IsBusy = false;
        }

        private async Task SortBasedOnState(IEnumerable<CopyItem> sortcollection)
        {
            if (IsSortAtoZ)
            {
                CopyItems = new ObservableCollection<CopyItem>(sortcollection.OrderBy(x => x.TitleOne).ToList());
            }
            else
            {
                CopyItems = new ObservableCollection<CopyItem>(sortcollection.OrderByDescending(x => x.CreationDate).ToList());
            }
        }

        [RelayCommand]
        public async Task SortAtoZ()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsSortAtoZ = true;
            await SortBasedOnState(CopyItems);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task SortTime()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsSortAtoZ = false;
            await SortBasedOnState(CopyItems);
            IsBusy = false;
        }
        [RelayCommand]
        public async Task ClosePop()
        {
            ReSetVariables();
        }

        [RelayCommand]
        public async Task CopyItemToClipboard(string textToCopy)
        {
            //if (id == 0) return;
            //string textToCopy = await _copyItemRepository.GetCopyTextByAsync(id);

            if (string.IsNullOrWhiteSpace(textToCopy)) return;

            await Clipboard.SetTextAsync(textToCopy);
            DisplayPopupService.PopupStatus("کپی شد", DisplayPopupService.PopupTarget.Copy);
        }

        public async void LoadCopyItems()
        {
            var itemsAwait = await _copyItemRepository.GetCopyItemsByFolderIdAsync(FolderId);
            await SortBasedOnState(itemsAwait);

            if (string.IsNullOrWhiteSpace(PageTitle)) await LoadPageTitle();
        }

        private async Task LoadPageTitle()
        {
            PageTitle = $"پوشه - {await _copyItemRepository.GetFolderNameByAsync(FolderId)}";
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            FolderId = int.Parse(query["FolderId"].ToString());
            LoadCopyItems();
        }
    }
}
