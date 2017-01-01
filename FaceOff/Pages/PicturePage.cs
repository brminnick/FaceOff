using System;

using Xamarin.Forms;

using FaceOff.Shared;

namespace FaceOff
{
	public class PicturePage : ContentPage
	{
		#region Field Constants
		const int _frameImagePadding = 10;
		#endregion

		#region Fields
		FrameImage _photoImage1, _photoImage2;
		BounceButton _photo1ScoreButton, _photo2ScoreButton;
		PictureViewModel _viewModel;
		#endregion

		#region Constructors
		public PicturePage(string player1NameText, string player2NameText)
		{
			_viewModel = new PictureViewModel(player1NameText, player2NameText);
			BindingContext = _viewModel;

			this.SetBinding<PictureViewModel>(TitleProperty, vm => vm.PageTitle);
			BackgroundColor = Color.FromHex("#91E2F4");

			#region Create Score Button 1 Stack
			_photo1ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton1);
			_photo1ScoreButton.SetBinding<PictureViewModel>(IsEnabledProperty, vm => vm.IsScore1ButtonEnabled);
			_photo1ScoreButton.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsScore1ButtonVisable);
			_photo1ScoreButton.SetBinding<PictureViewModel>(Button.TextProperty, vm => vm.ScoreButton1Text);
			_photo1ScoreButton.SetBinding<PictureViewModel>(Button.CommandProperty, vm => vm.Photo1ScoreButtonPressed);

			var photo1ScoreButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					_photo1ScoreButton
				}
			};
			#endregion

			#region Create Score Button 2 Stack
			_photo2ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton2);
			_photo2ScoreButton.SetBinding<PictureViewModel>(IsEnabledProperty, vm => vm.IsScore2ButtonEnabled);
			_photo2ScoreButton.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsScore2ButtonVisable);
			_photo2ScoreButton.SetBinding<PictureViewModel>(Button.TextProperty, vm => vm.ScoreButton2Text);
			_photo2ScoreButton.SetBinding<PictureViewModel>(Button.CommandProperty, vm => vm.Photo2ScoreButtonPressed);

			var photo2ScoreButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					_photo2ScoreButton
				}
			};
			#endregion

			#region Create Photo Activity Indicators
			var photo1ActivityIndicator = new ActivityIndicator
			{
				AutomationId = AutomationIdConstants.Photo1ActivityIndicator
			};
			photo1ActivityIndicator.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsCalculatingPhoto1Score);
			photo1ActivityIndicator.SetBinding<PictureViewModel>(ActivityIndicator.IsRunningProperty, vm => vm.IsCalculatingPhoto1Score);

			var photo2ActivityIndicator = new ActivityIndicator
			{
				AutomationId = AutomationIdConstants.Photo2ActivityIndicator
			};
			photo2ActivityIndicator.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsCalculatingPhoto2Score);
			photo2ActivityIndicator.SetBinding<PictureViewModel>(ActivityIndicator.IsRunningProperty, vm => vm.IsCalculatingPhoto2Score);
			#endregion

			#region Create Photo 1 Button Stack
			var takePhoto1Button = new BounceButton(AutomationIdConstants.TakePhoto1Button)
			{
				Text = "Take Photo"
			};
			takePhoto1Button.SetBinding<PictureViewModel>(IsEnabledProperty, vm => vm.IsTakeLeftPhotoButtonEnabled);
			takePhoto1Button.SetBinding<PictureViewModel>(Button.CommandProperty, vm => vm.TakePhoto1ButtonPressed);

			var player1NameLabel = new DarkBlueLabel
			{
				Text = player1NameText,
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
			takePhoto1ButtonStack.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsTakeLeftPhotoButtonStackVisible);
			#endregion

			#region Create Photo 2 Button Stack
			var takePhoto2Button = new BounceButton(AutomationIdConstants.TakePhoto2Button)
			{
				Text = "Take Photo"
			};
			takePhoto2Button.SetBinding<PictureViewModel>(IsEnabledProperty, vm => vm.IsTakeRightPhotoButtonEnabled);
			takePhoto2Button.SetBinding<PictureViewModel>(Button.CommandProperty, vm => vm.TakePhoto2ButtonPressed);

			var player2NameLabel = new DarkBlueLabel
			{
				Text = player2NameText,
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
			takePhoto2ButtonStack.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsTakeRightPhotoButtonStackVisible);
			#endregion

			#region Create Photo Image Containers
			_photoImage1 = new FrameImage(AutomationIdConstants.PhotoImage1);
			_photoImage1.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsPhotoImage1Enabled);
			_photoImage1.ContentImage.SetBinding<PictureViewModel>(Image.SourceProperty, vm => vm.Photo1ImageSource);

			_photoImage2 = new FrameImage(AutomationIdConstants.PhotoImage2);
			_photoImage2.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsPhotoImage2Enabled);
			_photoImage2.ContentImage.SetBinding<PictureViewModel>(Image.SourceProperty, vm => vm.Photo2ImageSource);
			#endregion

			#region Create Photo 1 Stack
			var photo1Stack = new StackLayout
			{
				Style = StylesConstants.StackLayoutStyle,
				Children = {
					_photoImage1,
					photo1ScoreButtonStack,
					photo1ActivityIndicator
				},
			};
			#endregion

			#region Create Photo 2 Stack
			var photo2Stack = new StackLayout
			{
				Style = StylesConstants.StackLayoutStyle,
				Children = {
					_photoImage2,
					photo2ScoreButtonStack,
					photo2ActivityIndicator
				}
			};
			#endregion

			#region Create Reset Button Stack
			var resetButton = new BounceButton(AutomationIdConstants.ResetButton)
			{
				Text = "Reset"
			};
			resetButton.SetBinding<PictureViewModel>(IsEnabledProperty, vm => vm.IsResetButtonEnabled);
			resetButton.SetBinding<PictureViewModel>(IsVisibleProperty, vm => vm.IsResetButtonEnabled);
			resetButton.SetBinding<PictureViewModel>(Button.CommandProperty, vm => vm.ResetButtonPressed);

			var resetButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					resetButton
				}
			};
			#endregion

			#region Create Relative Laout
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
			#endregion

			#region Set Page Content


			Content = new ScrollView
			{
				Content = buttonImageRelativeLayout
			};
			#endregion
		}
		#endregion

		#region Methods
		public void SetPhotoImage1(string photoImageSource)
		{
			_viewModel.SetPhotoImage1(photoImageSource);
		}

		public void SetPhotoImage2(string photoImageSource)
		{
			_viewModel.SetPhotoImage2(photoImageSource);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			#region Subscribe Event Handlers
			_viewModel.RevealPhotoImage1WithAnimation += HandleRevealPhoto1WithAnimation;
			_viewModel.RevealPhotoImage2WithAnimation += HandleRevealPhoto2WithAnimation;
			_viewModel.DisplayNoCameraAvailableAlert += HandleDisplayNoCameraAvailableAlert;
			_viewModel.DisplayAllEmotionResultsAlert += HandleDisplayAllEmotionResultsAlert;
			_viewModel.DisplayEmotionBeforeCameraAlert += HandleDisplayEmotionBeforeCameraAlert;
			_viewModel.RevealScoreButton1WithAnimation += HandleRevealScoreButton1WithAnimation;
			_viewModel.RevealScoreButton2WithAnimation += HandleRevealScoreButton2WithAnimation;
			_viewModel.DisplayMultipleFacesDetectedAlert += HandleDisplayMultipleFacesDetectedAlert;
			#endregion
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			#region Unsubscribe Event Handlers
			_viewModel.RevealPhotoImage1WithAnimation -= HandleRevealPhoto1WithAnimation;
			_viewModel.RevealPhotoImage2WithAnimation -= HandleRevealPhoto2WithAnimation;
			_viewModel.DisplayNoCameraAvailableAlert -= HandleDisplayNoCameraAvailableAlert;
			_viewModel.DisplayAllEmotionResultsAlert -= HandleDisplayAllEmotionResultsAlert;
			_viewModel.DisplayEmotionBeforeCameraAlert -= HandleDisplayEmotionBeforeCameraAlert;
			_viewModel.RevealScoreButton1WithAnimation -= HandleRevealScoreButton1WithAnimation;
			_viewModel.RevealScoreButton2WithAnimation -= HandleRevealScoreButton2WithAnimation;
			_viewModel.DisplayMultipleFacesDetectedAlert -= HandleDisplayMultipleFacesDetectedAlert;
			#endregion
		}

		void HandleDisplayEmotionBeforeCameraAlert(object sender, AlertMessageEventArgs e)
		{
			var alertMessage = e.Message;
			bool userResponseToAlert = false;

			Device.BeginInvokeOnMainThread(async () =>
			{
				userResponseToAlert = await DisplayAlert(alertMessage.Title, alertMessage.Message, "OK", "Cancel");

				_viewModel.UserResponseToAlert = userResponseToAlert;
				_viewModel.HasUserAcknowledgedPopUp = true;
			});
		}

		void HandleDisplayAllEmotionResultsAlert(object sender, TextEventArgs e)
		{
			var allEmotionResults = e.Text;
			Device.BeginInvokeOnMainThread(() => DisplayAlert("Results", allEmotionResults, "OK"));
		}

		void HandleDisplayNoCameraAvailableAlert(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "No Camera Available", "OK"));
		}

		void HandleRevealScoreButton1WithAnimation(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				_photo1ScoreButton.Scale = 0;
				_viewModel.IsScore1ButtonVisable = true;

				await _photo1ScoreButton?.ScaleTo(AnimationConstants.ScoreButtonMaxSize, AnimationConstants.ScoreButonAninmationTime);
				await _photo1ScoreButton?.ScaleTo(AnimationConstants.ScoreButtonNormalSize, AnimationConstants.ScoreButonAninmationTime);

				_viewModel.IsScore1ButtonEnabled = true;
				_viewModel.IsTakeRightPhotoButtonEnabled = !_viewModel.IsPhotoImage2Enabled;
			});
		}

		void HandleRevealScoreButton2WithAnimation(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				_photo2ScoreButton.Scale = 0;
				_viewModel.IsScore2ButtonVisable = true;

				await _photo2ScoreButton?.ScaleTo(AnimationConstants.ScoreButtonMaxSize, AnimationConstants.ScoreButonAninmationTime);
				await _photo2ScoreButton?.ScaleTo(AnimationConstants.ScoreButtonNormalSize, AnimationConstants.ScoreButonAninmationTime);

				_viewModel.IsScore2ButtonEnabled = true;
				_viewModel.IsTakeLeftPhotoButtonEnabled = !_viewModel.IsPhotoImage1Enabled;
			});
		}

		void HandleRevealPhoto1WithAnimation(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				_photoImage1.Scale = 0;
				_viewModel.IsPhotoImage1Enabled = true;

				await _photoImage1?.ScaleTo(AnimationConstants.PhotoImageMaxSize, AnimationConstants.PhotoImageAninmationTime);
				await _photoImage1?.ScaleTo(AnimationConstants.PhotoImageNormalSize, AnimationConstants.PhotoImageAninmationTime);
			});
		}

		void HandleRevealPhoto2WithAnimation(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				_photoImage2.Scale = 0;
				_viewModel.IsPhotoImage2Enabled = true;

				await _photoImage2?.ScaleTo(AnimationConstants.PhotoImageMaxSize, AnimationConstants.PhotoImageAninmationTime);
				await _photoImage2?.ScaleTo(AnimationConstants.PhotoImageNormalSize, AnimationConstants.PhotoImageAninmationTime);
			});
		}

		void HandleDisplayMultipleFacesDetectedAlert(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error: Multiple Faces Detected", "Ensure only one face is captured in the photo", "Ok"));
		}
		#endregion
	}
}

