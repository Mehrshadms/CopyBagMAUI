using CopyBag.ViewModels;

namespace CopyBag.Pages;

public partial class ContactGroupPage : ContentPage
{
    private ContactGroupViewModel _viewModel;
    public ContactGroupPage(ContactGroupViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = vm;
        VisualStateManager.GoToState(SortAtoZBtn, "Selected");
        VisualStateManager.GoToState(SortTimeBtn, "UnSelected");

        //Application.Current.MainPage = new NavigationPage(this);
        //Shell.Current.Navigation.RemovePage(this);
        //var stack = Shell.Current.Navigation.NavigationStack;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadGroups();
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_viewModel is null) _viewModel = (ContactGroupViewModel)BindingContext;

        _viewModel.Search();
    }

    private void SortButton_Click(object sender, EventArgs e)
    {
        // Reset the visual state of all buttons to "Normal"
        VisualStateManager.GoToState(SortAtoZBtn, "UnSelected");
        VisualStateManager.GoToState(SortTimeBtn, "UnSelected");

        // Get the clicked button
        Button clickedButton = (Button)sender;

        // Change the visual state of the clicked button to "Selected"
        VisualStateManager.GoToState(clickedButton, "Selected");
    }
}