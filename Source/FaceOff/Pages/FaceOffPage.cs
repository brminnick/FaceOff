using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using FaceOff.Shared;

namespace FaceOff
{
    public class FaceOffPage : BaseContentPage<FaceOffViewModel>
    {
        #region Constant Fields
        const int _frameImagePadding = 10;

        readonly FrameImage _photoImage1, _photoImage2;
        readonly BounceButton _photo1ScoreButton, _photo2ScoreButton;
        #endregion

        #region Constructors
        public FaceOffPage()
        {
            this.SetBinding(TitleProperty, nameof(ViewModel.PageTitle));

            _photo1ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton1)
            {
                Padding = new Thickness(24, 12),
                IsVisible = false,
                Scale = 0,
            };
            _photo1ScoreButton.SetBinding(IsEnabledProperty, nameof(ViewModel.IsScore1ButtonEnabled));
            _photo1ScoreButton.SetBinding(Button.TextProperty, nameof(ViewModel.ScoreButton1Text));
            _photo1ScoreButton.SetBinding(Button.CommandProperty, nameof(ViewModel.Photo1ScoreButtonPressed));

            _photo2ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton2) { Padding = new Thickness(24, 12), IsVisible = false };
            _photo2ScoreButton.SetBinding(IsEnabledProperty, nameof(ViewModel.IsScore2ButtonEnabled));
            _photo2ScoreButton.SetBinding(Button.TextProperty, nameof(ViewModel.ScoreButton2Text));
            _photo2ScoreButton.SetBinding(Button.CommandProperty, nameof(ViewModel.Photo2ScoreButtonPressed));

            var photo1ActivityIndicator = new ActivityIndicator { AutomationId = AutomationIdConstants.Photo1ActivityIndicator };
            photo1ActivityIndicator.SetBinding(IsVisibleProperty, nameof(ViewModel.IsCalculatingPhoto1Score));
            photo1ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(ViewModel.IsCalculatingPhoto1Score));

            var photo2ActivityIndicator = new ActivityIndicator { AutomationId = AutomationIdConstants.Photo2ActivityIndicator };
            photo2ActivityIndicator.SetBinding(IsVisibleProperty, nameof(ViewModel.IsCalculatingPhoto2Score));
            photo2ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(ViewModel.IsCalculatingPhoto2Score));

            var takePhoto1Button = new BounceButton(AutomationIdConstants.TakePhoto1Button) { Text = "Take Photo" };
            takePhoto1Button.SetBinding(IsEnabledProperty, nameof(ViewModel.IsTakeLeftPhotoButtonEnabled));
            takePhoto1Button.SetBinding(Button.CommandProperty, nameof(ViewModel.TakePhoto1ButtonPressed));

            var player1NameLabel = new DarkBlueLabel
            {
                AutomationId = AutomationIdConstants.Player1NameLabel,
                Text = PreferencesService.Player1Name,
                HorizontalOptions = LayoutOptions.Center
            };

            var takePhoto1ButtonStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(24),
                Children = {
                    player1NameLabel,
                    takePhoto1Button
                }
            };
            takePhoto1ButtonStack.SetBinding(IsVisibleProperty, nameof(ViewModel.IsTakeLeftPhotoButtonStackVisible));

            var takePhoto2Button = new BounceButton(AutomationIdConstants.TakePhoto2Button) { Text = "Take Photo" };
            takePhoto2Button.SetBinding(IsEnabledProperty, nameof(ViewModel.IsTakeRightPhotoButtonEnabled));
            takePhoto2Button.SetBinding(Button.CommandProperty, nameof(ViewModel.TakePhoto2ButtonPressed));

            var player2NameLabel = new DarkBlueLabel
            {
                AutomationId = AutomationIdConstants.Player2NameLabel,
                Text = PreferencesService.Player2Name,
                HorizontalOptions = LayoutOptions.Center,
            };

            var takePhoto2ButtonStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(24),
                Children = {
                    player2NameLabel,
                    takePhoto2Button
                }
            };
            takePhoto2ButtonStack.SetBinding(IsVisibleProperty, nameof(ViewModel.IsTakeRightPhotoButtonStackVisible));

            _photoImage1 = new FrameImage(AutomationIdConstants.PhotoImage1)
            {
                IsVisible = false,
                Scale = 0,
            };
            _photoImage1.ContentImage.SetBinding(Image.SourceProperty, nameof(ViewModel.Photo1ImageSource));

            _photoImage2 = new FrameImage(AutomationIdConstants.PhotoImage2)
            {
                IsVisible = false,
                Scale = 0,
            };
            _photoImage2.ContentImage.SetBinding(Image.SourceProperty, nameof(ViewModel.Photo2ImageSource));

            var photo1Stack = new StackLayout
            {
                Spacing = 50,
                Children = {
                    _photoImage1,
                    _photo1ScoreButton,
                    photo1ActivityIndicator
                }
            };

            var photo2Stack = new StackLayout
            {
                Spacing = 50,
                Children = {
                    _photoImage2,
                    _photo2ScoreButton,
                    photo2ActivityIndicator
                }
            };

            var resetButton = new BounceButton(AutomationIdConstants.ResetButton) { Text = "Reset" };
            resetButton.SetBinding(IsEnabledProperty, nameof(ViewModel.IsResetButtonEnabled));
            resetButton.SetBinding(IsVisibleProperty, nameof(ViewModel.IsResetButtonEnabled));
            resetButton.SetBinding(Button.CommandProperty, nameof(ViewModel.ResetButtonPressed));

            var resetButtonStack = new StackLayout
            {
                Padding = new Thickness(24, 24, 24, 24),
                Children = {
                    resetButton
                }
            };

            var buttonImageRelativeLayout = new RelativeLayout();

            buttonImageRelativeLayout.Children.Add(photo1Stack,
                Constraint.RelativeToParent(parent => parent.X + _frameImagePadding),
                Constraint.RelativeToParent(parent => parent.Y + _frameImagePadding),
                Constraint.RelativeToParent(parent => parent.Width / 2 - 2 * _frameImagePadding),
                Constraint.RelativeToParent(parent => parent.Height * 7 / 8)
            );

            buttonImageRelativeLayout.Children.Add(photo2Stack,
                Constraint.RelativeToParent(parent => parent.Width / 2 + _frameImagePadding),
                Constraint.RelativeToParent(parent => parent.Y + _frameImagePadding),
                Constraint.RelativeToParent(parent => parent.Width / 2 - 2 * _frameImagePadding),
                Constraint.RelativeToParent(parent => parent.Height * 7 / 8)
            );

            buttonImageRelativeLayout.Children.Add(takePhoto1ButtonStack,
                Constraint.RelativeToParent(parent => parent.X),
                Constraint.RelativeToParent(parent => parent.Y),
                Constraint.RelativeToParent(parent => parent.Width / 2)
            );

            buttonImageRelativeLayout.Children.Add(takePhoto2ButtonStack,
                Constraint.RelativeToParent(parent => parent.Width / 2),
                Constraint.RelativeToParent(parent => parent.Y),
                Constraint.RelativeToParent(parent => parent.Width / 2)
            );

            buttonImageRelativeLayout.Children.Add(resetButtonStack,
                Constraint.RelativeToParent(parent => parent.X),
                Constraint.RelativeToParent(parent => parent.Height * 7 / 8 - resetButtonStack.Height),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height * 1 / 8)
            );

            SubscribeEventHandlers();

            Content = buttonImageRelativeLayout;
        }
        #endregion

        #region Methods
        void SubscribeEventHandlers()
        {
            ViewModel.PhotoImage1RevealTriggered += HandlePhotoImage1RevealTriggered;
            ViewModel.PhotoImage2RevealTriggered += HandlePhotoImage2RevealTriggered;
            ViewModel.ScoreButton1RevealTriggered += HandleScoreButton1RevealTriggered;
            ViewModel.ScoreButton2RevealTriggered += HandleScoreButton2RevealTriggered;
            ViewModel.PhotoImage1HideTriggered += HandlePhotoImage1HideTriggered;
            ViewModel.PhotoImage2HideTriggered += HandlePhotoImage2HideTriggered;
            ViewModel.ScoreButton1HideTriggered += HandleScoreButton1HideTriggered;
            ViewModel.ScoreButton2HideTriggered += HandleScoreButton2HideTriggered;
            ViewModel.AllEmotionResultsAlertTriggered += HandleAllEmotionResultsAlertTriggered;
            ViewModel.PopUpAlertAboutEmotionTriggered += HandlePopUpAlertAboutEmotionTriggered;
            EmotionService.MultipleFacesDetectedAlertTriggered += HandleMultipleFacesDetectedAlertTriggered;
            MediaService.NoCameraDetected += HandleNoCameraDetected;
            MediaService.PermissionsDenied += HandlePermissionsDenied;
        }

        void HandleAllEmotionResultsAlertTriggered(object sender, string message) =>
            Device.BeginInvokeOnMainThread(() => DisplayAlert("Results", message, "OK"));

        void HandleNoCameraDetected(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "No Camera Available", "OK"));

        void HandleMultipleFacesDetectedAlertTriggered(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error: Multiple Faces Detected", "Ensure only one face is captured in the photo", "Ok"));

        void HandlePermissionsDenied(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var isAlertAccepted = await DisplayAlert("Open Settings?", "Storage and Camera Permission Need To Be Enabled", "Ok", "Cancel");
                if (isAlertAccepted)
                    Xamarin.Essentials.AppInfo.ShowSettingsUI();
            });
        }

        void HandlePopUpAlertAboutEmotionTriggered(object sender, AlertMessageEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var userResponseToAlert = await DisplayAlert(e.Title, e.Message, "OK", "Cancel");
                ViewModel.EmotionPopUpAlertResponseCommand?.Execute(new EmotionPopupResponseModel(userResponseToAlert, e.Player));
            });
        }

        async void HandleScoreButton1RevealTriggered(object sender, EventArgs e) => await RevealView(_photo1ScoreButton);
        async void HandleScoreButton2RevealTriggered(object sender, EventArgs e) => await RevealView(_photo2ScoreButton);
        async void HandlePhotoImage1RevealTriggered(object sender, EventArgs e) => await RevealView(_photoImage1);
        async void HandlePhotoImage2RevealTriggered(object sender, EventArgs e) => await RevealView(_photoImage2);
        async void HandleScoreButton1HideTriggered(object sender, EventArgs e) => await HideView(_photo1ScoreButton);
        async void HandleScoreButton2HideTriggered(object sender, EventArgs e) => await HideView(_photo2ScoreButton);
        async void HandlePhotoImage1HideTriggered(object sender, EventArgs e) => await HideView(_photoImage1);
        async void HandlePhotoImage2HideTriggered(object sender, EventArgs e) => await HideView(_photoImage2);

        Task RevealView(View view,
                        uint animationTime = AnimationConstants.DefaultAnimationTime,
                        double maxImageSize = AnimationConstants.DefaultMaxImageSize,
                        double normalImageSize = AnimationConstants.DefaultNormalSize)
        {
            return Device.InvokeOnMainThreadAsync(async () =>
            {
                view.Scale = 0;
                view.IsVisible = true;

                await view.ScaleTo(maxImageSize, animationTime);
                await view.ScaleTo(normalImageSize, animationTime);
            });
        }

        Task HideView(View view)
        {
            return Device.InvokeOnMainThreadAsync(() =>
            {
                view.Scale = 0;
                view.IsVisible = false;
            });
        }
        #endregion

        #region Classes
        class FrameImage : Frame
        {
            public FrameImage(string automationId)
            {
                AutomationId = automationId;
                HasShadow = false;
                ContentImage = new Image();
                Content = ContentImage;
            }

            public Image ContentImage { get; }
        }
        #endregion
    }
}

