using Xamarin.Forms;

namespace FaceOff
{
    public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel, new()
    {
        #region Constructors
        protected BaseContentPage()
        {
            BindingContext = ViewModel;
            BackgroundColor = Color.FromHex("#91E2F4");
        }
        #endregion

        #region Properties
        protected TViewModel ViewModel { get; } = new TViewModel();
        #endregion
    }
}
