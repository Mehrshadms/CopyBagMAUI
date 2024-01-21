using CopyBag.Pages;

namespace CopyBag.Services
{
    public static class DisplayPopupService
    {
        public static async Task PopupStatus(string statusText, PopupTarget popupTarget)
        {
            object color;
            object glyph;
            switch (popupTarget)
            {
                case PopupTarget.Error:
                    Application.Current.Resources.TryGetValue("DangerYellow", out color);
                    Application.Current.Resources.TryGetValue("ErrorIcon", out glyph);
                    break;
                case PopupTarget.Insert:
                    Application.Current.Resources.TryGetValue("SuccessGreen", out color);
                    Application.Current.Resources.TryGetValue("SuccessIcon", out glyph);
                    break;
                case PopupTarget.Update:
                    Application.Current.Resources.TryGetValue("Primary", out color);
                    Application.Current.Resources.TryGetValue("EditIcon", out glyph);
                    break;
                case PopupTarget.Delete:
                    Application.Current.Resources.TryGetValue("RedButton", out color);
                    Application.Current.Resources.TryGetValue("DeleteIcon", out glyph);
                    break;
                case PopupTarget.Copy:
                    Application.Current.Resources.TryGetValue("SecondaryDarkText", out color);
                    Application.Current.Resources.TryGetValue("CopyIcon", out glyph);
                    break;
                default:
                    Application.Current.Resources.TryGetValue("White", out color);
                    Application.Current.Resources.TryGetValue("QuestionIcon", out glyph);
                    break;
            }
            var pop = new PopupPage(statusText, (string)glyph, (Color)color);
            Application.Current.MainPage.Navigation.PushModalAsync(pop);
            await Task.Delay(1000);
            await pop.PoppingOut();
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        public enum PopupTarget
        {
            Error,
            Insert,
            Update,
            Delete,
            Copy
        }
    }
}
