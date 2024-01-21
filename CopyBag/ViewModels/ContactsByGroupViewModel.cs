using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CopyBag.Models.Contacts;
using CopyBag.Pages;
using CopyBag.Services;
using System.Collections.ObjectModel;

namespace CopyBag.ViewModels
{
    [QueryProperty(nameof(GroupId), "GroupId")]
    public partial class ContactsByGroupViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IContactRepository _contactRepository;
        private List<Models.Contacts.Contact> _contacts;
        private bool IsSortAtoZ = true;
        private bool IsBusy;

        public ContactsByGroupViewModel(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [ObservableProperty] string searchText = string.Empty;

        [ObservableProperty] int groupId;

        [ObservableProperty] string pageTitle;

        [ObservableProperty] ObservableCollection<Models.Contacts.Contact> contacts = [];

        [RelayCommand]
        public async Task AddContact()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("نام مخاطب", "نام مخاطب را بنویسید", "تایید", "لغو");

            if (string.IsNullOrWhiteSpace(result)) return;

            if (await _contactRepository.Exists(x => x.GroupId == GroupId && x.FullName == result))
            {
                await Application.Current.MainPage.DisplayAlert("ثبت ناموفق", $"مخاطبی با نام '{result}' وجود دارد", "متوجه شدم!", FlowDirection.RightToLeft);
                return;
            }
            Models.Contacts.Contact contact = new Models.Contacts.Contact(result, GroupId);
            var operationResult = await _contactRepository.Add(contact);
            if (operationResult)
            {
                Task.Run(() => LoadContacts());
                DisplayPopupService.PopupStatus("مخاطب اضافه شد", DisplayPopupService.PopupTarget.Insert);
            }
            else
            {
                DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
            }

        }

        [RelayCommand]
        public async Task EditContact(int id)
        {
            if (id == 0) return;

            Models.Contacts.Contact contact = await _contactRepository.Get(id);

            if (contact is null) return;

            string result = await Application.Current.MainPage.DisplayPromptAsync($"تغییر نام مخاطب {contact.FullName}", "نام جدید مخاطب را بنویسید", "تایید", "لغو", initialValue: contact.FullName);

            if (string.IsNullOrWhiteSpace(result)) return;

            if (await _contactRepository.Exists(x => x.GroupId == GroupId && x.FullName == result))
            {
                Application.Current.MainPage.DisplayAlert("تغییر ناموفق", $"مخاطبی با نام '{result}' وجود دارد", "متوجه شدم!", FlowDirection.RightToLeft);
                return;
            }

            contact.Edit(result, GroupId);
            var operationResult = await _contactRepository.Edit(contact);
            if (operationResult)
            {
                Task.Run(() => LoadContacts());
                DisplayPopupService.PopupStatus("مخاطب تغییر کرد", DisplayPopupService.PopupTarget.Update);
            }
            else
            {
                DisplayPopupService.PopupStatus("مشکلی رخ داد", DisplayPopupService.PopupTarget.Error);
            }
        }

        [RelayCommand]
        public async Task DeleteContact(int id)
        {
            if (id == 0) return;

            Models.Contacts.Contact contact = await _contactRepository.Get(id);

            if (contact is null) return;

            bool result = await Application.Current.MainPage.DisplayAlert($"حذف مخاطب {contact.FullName}", "مخاطب حذف شود؟", "تایید", "لغو", FlowDirection.RightToLeft);

            if (result)
            {
                var operationResult = await _contactRepository.Remove(contact);
                if (operationResult)
                {
                    Task.Run(() => LoadContacts());
                    DisplayPopupService.PopupStatus("مخاطب حذف شد", DisplayPopupService.PopupTarget.Delete);
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
            if (_contacts is null) return;
            if (IsBusy) return;
            IsBusy = true;
            await SortBasedOnState(_contacts.Where(x => x.FullName.Contains(SearchText)));
            IsBusy = false;
        }

        private async Task SortBasedOnState(IEnumerable<Models.Contacts.Contact> sortcollection)
        {
            if (IsSortAtoZ)
            {
                Contacts = new ObservableCollection<Models.Contacts.Contact>(sortcollection.OrderBy(x => x.FullName).ToList());
            }
            else
            {
                Contacts = new ObservableCollection<Models.Contacts.Contact>(sortcollection.OrderByDescending(x => x.CreationDate).ToList());
            }
        }

        [RelayCommand]
        public async Task SortAtoZ()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsSortAtoZ = true;
            await SortBasedOnState(Contacts);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task SortTime()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsSortAtoZ = false;
            await SortBasedOnState(Contacts);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task GoToContactFolderPageBy(int id)
        {
            await Shell.Current.GoToAsync($"{nameof(ContactCopyFolderPage)}?ContactId={id}");
        }

        public async Task LoadContacts()
        {
            _contacts = await _contactRepository.GetContactsByGroupIdAsync(GroupId);
            await SortBasedOnState(_contacts);

            if (string.IsNullOrWhiteSpace(PageTitle)) await LoadPageTitle();
        }

        private async Task LoadPageTitle()
        {
            PageTitle = $"مخاطبین - {await _contactRepository.GetGroupNameByAsync(GroupId)}";
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            GroupId = int.Parse(query["GroupId"].ToString());
            LoadContacts();
        }
    }
}
