using CommunityToolkit.Maui.Views;
using CopyBag.Models.CopyBin;

namespace CopyBag.Pages;

public partial class CopyItemPopup : Popup
{
    private const uint Popin = 650;
    private bool IsEdit;
    public CopyItemPopup(bool IsTwoInOne=false)
    {
        InitializeComponent();
        if (IsTwoInOne)
        {
            ToggleDouble();
            CopyItemPanel.HeightRequest = 430d;
            SecondTitleEntry.IsVisible = true;
            SecondTitleEntry.Opacity = 1.0d;
            SecondCopyEntry.IsVisible = true;
            SecondCopyEntry.Opacity = 1.0d;
        }
    }

    private void ToggleDouble()
    {
        SecondTitleEntry.IsVisible = true;
        SecondCopyEntry.IsVisible = true;
        SecondTitleEntry.Text = string.Empty;
        SecondCopyEntry.Text = string.Empty;
        ToggleBtn.Text = "حالت پیشفرض";
    }

    private void ToggleSingle()
    {
        SecondCopyEntry.IsVisible = false;
        SecondTitleEntry.IsVisible = false;
        ToggleBtn.Text = "حالت 2 در 1";
    }

    private void ToggleInputs_Click(object sender, EventArgs e)
    {
        if (SecondTitleEntry.IsVisible == false)
        {
            // Hide Login inputs and show Register inputs

            // Increase the height of the popup to fit the Register input

            CopyItemPanel.Animate("Size",
                callback: v => CopyItemPanel.HeightRequest = (double)v,
                start: 330d,
                end: 430d,
                rate: 32,
                length: 350,
                easing: Easing.SinIn,
                finished: (v, k) => ToggleDouble());

            SecondTitleEntry.Animate("SecondTitle",
                callback: v => SecondTitleEntry.Opacity = (double)v,
                start: 0.0d,
                end: 1.0d,
                rate: 32,
                length: Popin,
                easing: Easing.CubicOut);

            SecondCopyEntry.Animate("SecondCopy",
                callback: v => SecondCopyEntry.Opacity = (double)v,
                start: 0.0d,
                end: 1.0d,
                rate: 32,
                length: Popin,
                easing: Easing.CubicIn);
        }
        else
        {
            // Hide Register inputs and show Login inputs

            //CopyItemPanel.ScaleYTo(1d, 500, Easing.CubicInOut);
            //ToggleDouble();

            SecondTitleEntry.Animate("SecondTitle",
               callback: v => SecondTitleEntry.Opacity = (double)v,
               start: 1.0d,
               end: 0.0d,
               rate: 32,
               length: 250,
               easing: Easing.SinIn);

            SecondCopyEntry.Animate("SecondCopy",
                callback: v => SecondCopyEntry.Opacity = (double)v,
                start: 1.0d,
                end: 0.0d,
                rate: 32,
                length: 250,
                easing: Easing.SinIn);

            CopyItemPanel.Animate("Size",
                callback: v => CopyItemPanel.HeightRequest = (double)v,
                start: 430d,
                end: 330d,
                rate: 32,
                length: 350,
                easing: Easing.CubicIn,
                finished: (v, k) => ToggleSingle());
        }
    }

    private void CloseBtn_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}
