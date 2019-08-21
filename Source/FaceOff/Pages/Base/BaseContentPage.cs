using Xamarin.Forms;

namespace FaceOff
{
    public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel, new()
    {
        protected BaseContentPage()
        {
            BindingContext = ViewModel;
            BackgroundColor = Color.FromHex("#91E2F4");
        }

        protected TViewModel ViewModel { get; } = new TViewModel();
    }
}
