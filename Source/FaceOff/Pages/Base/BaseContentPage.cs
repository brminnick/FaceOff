using Xamarin.Forms;

namespace FaceOff
{
    public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel, new()
    {
        #region Fields
        TViewModel _viewModel;
        #endregion

        #region Constructors
        protected BaseContentPage()
        {
            BindingContext = ViewModel;
            BackgroundColor = Color.FromHex("#91E2F4");
        }
        #endregion

        #region Properties
        protected TViewModel ViewModel => _viewModel ?? (_viewModel = new TViewModel());
        #endregion

        #region Methods
        protected abstract void SubscribeEventHandlers();
        protected abstract void UnsubscribeEventHandlers();

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SubscribeEventHandlers();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            UnsubscribeEventHandlers();
        }
        #endregion
    }
}
