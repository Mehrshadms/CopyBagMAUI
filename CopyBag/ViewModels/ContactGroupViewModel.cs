using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CopyBag.Models.Contacts;
using CopyBag.Pages;
using CopyBag.Services;
using System.Collections.ObjectModel;

namespace CopyBag.ViewModels
{
    public partial class ContactGroupViewModel : ObservableObject
    {
        private readonly IContactGroupRepository _contactGroupRepository;
        private List<ContactGroup> _contactGroups;
        private bool IsSortAtoZ = true;
        [ObservableProperty] bool isBusy;

        [ObservableProperty] string searchText = string.Empty;


        [ObservableProperty] public ObservableCollection<ContactGroup> groups = [];

        [RelayCommand]
        public async Task RefreshData()
        {
            if(IsBusy) return;

            IsBusy = true;
            await LoadGroups();
            IsBusy = false;
        }

        [RelayCommand]
        public async Task AddContactGroup()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("نام گروه مخاطبان", "نام گروه را بنویسید", "تایید", "لغو");

            if (string.IsNullOrWhiteSpace(result)) return;

            if (await _contactGroupRepository.Exists(x => x.GroupName == result))
            {
                await Application.Current.MainPage.DisplayAlert("ثبت ناموفق", $"گروهی با نام '{result}' وجود دارد", "متوجه شدم");
                return;
            }

            ContactGroup contactGroup = new ContactGroup(result);
            var operationResult = await _contactGroupRepository.Add(contactGroup);
            if (operationResult)
            {
                Task.Run(() => LoadGroups());
                await DisplayPopupService.PopupStatus("گروه اضافه شد", DisplayPopupService.PopupTarget.Insert);
            }
            else
            {
                await DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
            }
        }

        [RelayCommand]
        public async Task EditContactGroup(int id)
        {
            if (id == 0) return;

            ContactGroup contactGroup = await _contactGroupRepository.Get(id);

            if (contactGroup is null) return;

            string result = await Application.Current.MainPage.DisplayPromptAsync($"تغییر نام گروه {contactGroup.GroupName}", "نام جدید گروه را بنویسید", "تایید", "لغو", initialValue: contactGroup.GroupName);

            if (string.IsNullOrWhiteSpace(result)) return;

            if (await _contactGroupRepository.Exists(x => x.GroupName == result))
            {
                await Application.Current.MainPage.DisplayAlert("تغییر ناموفق", $"گروهی با نام '{result}' وجود دارد", "متوجه شدم!", FlowDirection.RightToLeft);
                return;
            }

            contactGroup.Edit(result);
            var operationResult = await _contactGroupRepository.Edit(contactGroup);
            if (operationResult)
            {
                Task.Run(() => LoadGroups());
                await DisplayPopupService.PopupStatus("گروه تغییر کرد", DisplayPopupService.PopupTarget.Update);
            }
            else
            {
                await DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
            }
        }

        [RelayCommand]
        public async Task DeleteContactGroup(int id)
        {
            if (id == 0) return;

            ContactGroup contactGroup = await _contactGroupRepository.Get(id);

            bool result = await Application.Current.MainPage.DisplayAlert($"حذف گروه {contactGroup.GroupName}", "گروه مخاطب حذف شود؟", "تایید", "لغو", FlowDirection.RightToLeft);
            if (result)
            {
                var operationResult = await _contactGroupRepository.Remove(contactGroup);
                if (operationResult)
                {
                    Task.Run(() => LoadGroups());
                    await DisplayPopupService.PopupStatus("گروه حدف شد", DisplayPopupService.PopupTarget.Delete);
                }
                else
                {
                    await DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
                }
            }
        }

        [RelayCommand]
        public async Task Search()
        {
            if (_contactGroups is null) return;
            if (IsBusy) return;
            IsBusy = true;
            await SortBasedOnState(_contactGroups.Where(x => x.GroupName.Contains(SearchText)));
            IsBusy = false;
        }

        private async Task SortBasedOnState(IEnumerable<ContactGroup> sortcollection)
        {
            if (IsSortAtoZ)
            {
                Groups = new ObservableCollection<ContactGroup>(sortcollection.OrderBy(x => x.GroupName).ToList());
            }
            else
            {
                Groups = new ObservableCollection<ContactGroup>(sortcollection.OrderByDescending(x => x.CreationDate).ToList());
            }
        }

        [RelayCommand]
        public async Task SortAtoZ()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsSortAtoZ = true;
            await SortBasedOnState(Groups);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task SortTime()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsSortAtoZ = false;
            await SortBasedOnState(Groups);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task GoToContactPageBy(int id)
        {
            await Shell.Current.GoToAsync($"{nameof(ContactsByGroupPage)}?GroupId={id}", true);
        }

        public ContactGroupViewModel(IContactGroupRepository contactGroupRepository)
        {
            _contactGroupRepository = contactGroupRepository;
            Task.Run(LoadGroups);
        }

        public async Task LoadGroups()
        {
            _contactGroups = await _contactGroupRepository.GetAll();
            await SortBasedOnState(_contactGroups);
        }
    }
}
