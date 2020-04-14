using System;
using System.Threading.Tasks;
using FaceOff.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FaceOff
{
    class FaceOffPage : BaseContentPage<FaceOffViewModel>
    {
        const int _frameImagePadding = 10;

        readonly FrameImage _photoImage1, _photoImage2;
        readonly BounceButton _photo1ScoreButton, _photo2ScoreButton;

        public FaceOffPage()
        {
            this.SetBinding(TitleProperty, nameof(FaceOffViewModel.PageTitle));

            _photo1ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton1)
            {
                Scale = 0,
                IsVisible = false,
                Padding = new Thickness(24, 12),
            };
            _photo1ScoreButton.SetBinding(IsEnabledProperty, nameof(FaceOffViewModel.IsScore1ButtonEnabled));
            _photo1ScoreButton.SetBinding(Button.TextProperty, nameof(FaceOffViewModel.ScoreButton1Text));
            _photo1ScoreButton.SetBinding(Button.CommandProperty, nameof(FaceOffViewModel.Photo1ScoreButtonPressed));

            _photo2ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton2)
            {
                IsVisible = false,
                Padding = new Thickness(24, 12)
            };
            _photo2ScoreButton.SetBinding(IsEnabledProperty, nameof(FaceOffViewModel.IsScore2ButtonEnabled));
            _photo2ScoreButton.SetBinding(Button.TextProperty, nameof(FaceOffViewModel.ScoreButton2Text));
            _photo2ScoreButton.SetBinding(Button.CommandProperty, nameof(FaceOffViewModel.Photo2ScoreButtonPressed));

            var photo1ActivityIndicator = new ActivityIndicator
            {
                AutomationId = AutomationIdConstants.Photo1ActivityIndicator,
                Color = ColorConstants.ActivityIndicatorColor
            };
            photo1ActivityIndicator.SetBinding(IsVisibleProperty, nameof(FaceOffViewModel.IsCalculatingPhoto1Score));
            photo1ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(FaceOffViewModel.IsCalculatingPhoto1Score));

            var photo2ActivityIndicator = new ActivityIndicator
            {
                AutomationId = AutomationIdConstants.Photo2ActivityIndicator,
                Color = ColorConstants.ActivityIndicatorColor
            };
            photo2ActivityIndicator.SetBinding(IsVisibleProperty, nameof(FaceOffViewModel.IsCalculatingPhoto2Score));
            photo2ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(FaceOffViewModel.IsCalculatingPhoto2Score));

            var takePhoto1Button = new BounceButton(AutomationIdConstants.TakePhoto1Button) { Text = "Take Photo" };
            takePhoto1Button.SetBinding(IsEnabledProperty, nameof(FaceOffViewModel.IsTakeLeftPhotoButtonEnabled));
            takePhoto1Button.SetBinding(Button.CommandProperty, nameof(FaceOffViewModel.TakePhoto1ButtonPressed));

            var player1NameLabel = new DarkBlueLabel(PreferencesService.Player1Name)
            {
                HorizontalOptions = LayoutOptions.Center,
                AutomationId = AutomationIdConstants.Player1NameLabel,
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
            takePhoto1ButtonStack.SetBinding(IsVisibleProperty, nameof(FaceOffViewModel.IsTakeLeftPhotoButtonStackVisible));

            var takePhoto2Button = new BounceButton(AutomationIdConstants.TakePhoto2Button) { Text = "Take Photo" };
            takePhoto2Button.SetBinding(IsEnabledProperty, nameof(FaceOffViewModel.IsTakeRightPhotoButtonEnabled));
            takePhoto2Button.SetBinding(Button.CommandProperty, nameof(FaceOffViewModel.TakePhoto2ButtonPressed));

            var player2NameLabel = new DarkBlueLabel(PreferencesService.Player2Name)
            {
                HorizontalOptions = LayoutOptions.Center,
                AutomationId = AutomationIdConstants.Player2NameLabel
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
            takePhoto2ButtonStack.SetBinding(IsVisibleProperty, nameof(FaceOffViewModel.IsTakeRightPhotoButtonStackVisible));

            _photoImage1 = new FrameImage(AutomationIdConstants.PhotoImage1)
            {
                IsVisible = false,
                Scale = 0,
            };
            _photoImage1.ContentImage.SetBinding(Image.SourceProperty, nameof(FaceOffViewModel.Photo1ImageSource));

            _photoImage2 = new FrameImage(AutomationIdConstants.PhotoImage2)
            {
                IsVisible = false,
                Scale = 0,
            };
            _photoImage2.ContentImage.SetBinding(Image.SourceProperty, nameof(FaceOffViewModel.Photo2ImageSource));

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
            resetButton.SetBinding(IsEnabledProperty, nameof(FaceOffViewModel.IsResetButtonEnabled));
            resetButton.SetBinding(IsVisibleProperty, nameof(FaceOffViewModel.IsResetButtonEnabled));
            resetButton.SetBinding(Button.CommandProperty, nameof(FaceOffViewModel.ResetButtonPressed));

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

        async void HandleAllEmotionResultsAlertTriggered(object sender, string message) =>
            await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Results", message, "OK"));

        async void HandleNoCameraDetected(object sender, EventArgs e) =>
            await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error", "No Camera Available", "OK"));

        async void HandleMultipleFacesDetectedAlertTriggered(object sender, EventArgs e) =>
            await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error: Multiple Faces Detected", "Ensure only one face is captured in the photo", "Ok"));

        void HandlePermissionsDenied(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var isAlertAccepted = await DisplayAlert("Open Settings?", "Storage and Camera Permission Need To Be Enabled", "Ok", "Cancel");
                if (isAlertAccepted)
                    AppInfo.ShowSettingsUI();
            });
        }

        void HandlePopUpAlertAboutEmotionTriggered(object sender, AlertMessageEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
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
            return MainThread.InvokeOnMainThreadAsync(async () =>
            {
                view.Scale = 0;
                view.IsVisible = true;

                await view.ScaleTo(maxImageSize, animationTime);
                await view.ScaleTo(normalImageSize, animationTime);
            });
        }

        Task HideView(View view) => MainThread.InvokeOnMainThreadAsync(() =>
        {
            view.Scale = 0;
            view.IsVisible = false;
        });

        class FrameImage : Frame
        {
            public FrameImage(in string automationId)
            {
                AutomationId = automationId;
                BackgroundColor = Color.White;
                HasShadow = false;
                Content = ContentImage;
            }

            public Image ContentImage { get; } = new Image();
        }
    }
}

