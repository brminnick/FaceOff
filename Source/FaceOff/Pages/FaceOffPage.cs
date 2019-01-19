using System;

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

            _photo1ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton1) { Padding = new Thickness(24, 12) };
            _photo1ScoreButton.SetBinding(IsEnabledProperty, nameof(ViewModel.IsScore1ButtonEnabled));
            _photo1ScoreButton.SetBinding(IsVisibleProperty, nameof(ViewModel.IsScore1ButtonVisable));
            _photo1ScoreButton.SetBinding(Button.TextProperty, nameof(ViewModel.ScoreButton1Text));
            _photo1ScoreButton.SetBinding(Button.CommandProperty, nameof(ViewModel.Photo1ScoreButtonPressed));

            _photo2ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton2) { Padding = new Thickness(24, 12) };
            _photo2ScoreButton.SetBinding(IsEnabledProperty, nameof(ViewModel.IsScore2ButtonEnabled));
            _photo2ScoreButton.SetBinding(IsVisibleProperty, nameof(ViewModel.IsScore2ButtonVisable));
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
                Text = PreferencesService.Player1Name,
                HorizontalOptions = LayoutOptions.Center
            };

            var takePhoto1ButtonStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(24, 24, 24, 24),
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
                Text = PreferencesService.Player2Name,
                HorizontalOptions = LayoutOptions.Center,
            };

            var takePhoto2ButtonStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(24, 24, 24, 24),
                Children = {
                    player2NameLabel,
                    takePhoto2Button
                }
            };
            takePhoto2ButtonStack.SetBinding(IsVisibleProperty, nameof(ViewModel.IsTakeRightPhotoButtonStackVisible));

            _photoImage1 = new FrameImage(AutomationIdConstants.PhotoImage1);
            _photoImage1.SetBinding(IsVisibleProperty, nameof(ViewModel.IsPhotoImage1Enabled));
            _photoImage1.ContentImage.SetBinding(Image.SourceProperty, nameof(ViewModel.Photo1ImageSource));

            _photoImage2 = new FrameImage(AutomationIdConstants.PhotoImage2);
            _photoImage2.SetBinding(IsVisibleProperty, nameof(ViewModel.IsPhotoImage2Enabled));
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

            Content = buttonImageRelativeLayout;
        }
        #endregion

        #region Methods
        void SubscribeEventHandlers()
        {
            ViewModel.PhotoImage1RevealTriggered += HandlePhotoImage1RevealTriggered;
            ViewModel.PhotoImage2RevealTriggered += HandlePhotoImage2RevealTriggered;
            ViewModel.AllEmotionResultsAlertTriggered += HandleAllEmotionResultsAlertTriggered;
            ViewModel.PopUpAlertAboutEmotionTriggered += HandlePopUpAlertAboutEmotionTriggered;
            ViewModel.ScoreButton1RevealTriggered += HandleScoreButton1RevealTriggered;
            ViewModel.ScoreButton2RevealTriggered += HandleScoreButton2RevealTriggered;
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

        void HandleScoreButton1RevealTriggered(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                _photo1ScoreButton.Scale = 0;
                ViewModel.IsScore1ButtonVisable = true;

                await _photo1ScoreButton?.ScaleTo(AnimationConstants.ScoreButtonMaxSize, AnimationConstants.ScoreButonAninmationTime);
                await _photo1ScoreButton?.ScaleTo(AnimationConstants.ScoreButtonNormalSize, AnimationConstants.ScoreButonAninmationTime);

                ViewModel.IsScore1ButtonEnabled = true;
                ViewModel.IsTakeRightPhotoButtonEnabled = !ViewModel.IsPhotoImage2Enabled;
            });
        }

        void HandleScoreButton2RevealTriggered(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                _photo2ScoreButton.Scale = 0;
                ViewModel.IsScore2ButtonVisable = true;

                await _photo2ScoreButton?.ScaleTo(AnimationConstants.ScoreButtonMaxSize, AnimationConstants.ScoreButonAninmationTime);
                await _photo2ScoreButton?.ScaleTo(AnimationConstants.ScoreButtonNormalSize, AnimationConstants.ScoreButonAninmationTime);

                ViewModel.IsScore2ButtonEnabled = true;
                ViewModel.IsTakeLeftPhotoButtonEnabled = !ViewModel.IsPhotoImage1Enabled;
            });
        }

        void HandlePhotoImage1RevealTriggered(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                _photoImage1.Scale = 0;
                ViewModel.IsPhotoImage1Enabled = true;

                await _photoImage1?.ScaleTo(AnimationConstants.PhotoImageMaxSize, AnimationConstants.PhotoImageAninmationTime);
                await _photoImage1?.ScaleTo(AnimationConstants.PhotoImageNormalSize, AnimationConstants.PhotoImageAninmationTime);
            });
        }

        void HandlePhotoImage2RevealTriggered(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                _photoImage2.Scale = 0;
                ViewModel.IsPhotoImage2Enabled = true;

                await _photoImage2?.ScaleTo(AnimationConstants.PhotoImageMaxSize, AnimationConstants.PhotoImageAninmationTime);
                await _photoImage2?.ScaleTo(AnimationConstants.PhotoImageNormalSize, AnimationConstants.PhotoImageAninmationTime);
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

            public Image ContentImage { get; set; }
        }
        #endregion
    }
}

