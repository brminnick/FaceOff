using System;

using Xamarin.Forms;

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
			this.SetBinding(ContentPage.TitleProperty, "PageTitle");
			BackgroundColor = Color.FromHex("#91E2F4");

			NavigationPage.SetHasBackButton(this, false);

			#region Create Score Button 1 Stack
			_photo1ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton1AutomationId);
			_photo1ScoreButton.SetBinding(Button.TextProperty, "ScoreButton1Text");
			_photo1ScoreButton.SetBinding(IsEnabledProperty, "IsScore1ButtonEnabled");
			_photo1ScoreButton.SetBinding(IsVisibleProperty, "IsScore1ButtonVisable");
			_photo1ScoreButton.SetBinding(Button.CommandProperty, "Photo1ScoreButtonPressed");

			var photo1ScoreButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					_photo1ScoreButton
				}
			};
			#endregion

			#region Create Score Button 2 Stack
			_photo2ScoreButton = new BounceButton(AutomationIdConstants.ScoreButton2AutomationId);
			_photo2ScoreButton.SetBinding(Button.TextProperty, "ScoreButton2Text");
			_photo2ScoreButton.SetBinding(IsEnabledProperty, "IsScore2ButtonEnabled");
			_photo2ScoreButton.SetBinding(IsVisibleProperty, "IsScore2ButtonVisable");
			_photo2ScoreButton.SetBinding(Button.CommandProperty, "Photo2ScoreButtonPressed");

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
				AutomationId = AutomationIdConstants.Photo1ActivityIndicatorAutomationId
			};
			photo1ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsCalculatingPhoto1Score");
			photo1ActivityIndicator.SetBinding(ActivityIndicator.IsEnabledProperty, "IsCalculatingPhoto1Score");
			photo1ActivityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsCalculatingPhoto1Score");

			var photo2ActivityIndicator = new ActivityIndicator
			{
				AutomationId = AutomationIdConstants.Photo2ActivityIndicatorAutomationId
			};
			photo2ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsCalculatingPhoto2Score");
			photo2ActivityIndicator.SetBinding(ActivityIndicator.IsEnabledProperty, "IsCalculatingPhoto2Score");
			photo2ActivityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsCalculatingPhoto2Score");
			#endregion

			#region Create Photo 1 Button Stack
			var takePhoto1Button = new BounceButton(AutomationIdConstants.TakePhoto1ButtonAutomationId)
			{
				Text = "Take Photo"
			};
			takePhoto1Button.SetBinding(Button.CommandProperty, "TakePhoto1ButtonPressed");
			takePhoto1Button.SetBinding(IsEnabledProperty, "IsTakeLeftPhotoButtonEnabled");

			var player1NameLabel = new Label
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
			takePhoto1ButtonStack.SetBinding(StackLayout.IsVisibleProperty, "IsTakeLeftPhotoButtonStackVisible");
			#endregion

			#region Create Photo 2 Button Stack
			var takePhoto2Button = new BounceButton(AutomationIdConstants.TakePhoto2ButtonAutomationId)
			{
				Text = "Take Photo"
			};
			takePhoto2Button.SetBinding(Button.CommandProperty, "TakePhoto2ButtonPressed");
			takePhoto2Button.SetBinding(IsEnabledProperty, "IsTakeRightPhotoButtonEnabled");

			var player2NameLabel = new Label
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
			takePhoto2ButtonStack.SetBinding(IsVisibleProperty, "IsTakeRightPhotoButtonStackVisible");
			#endregion

			#region Create Photo Image Containers
			_photoImage1 = new FrameImage(AutomationIdConstants.PhotoImage1AutomationId);
			_photoImage1.ContentImage.SetBinding(Image.SourceProperty, "Photo1ImageSource");
			_photoImage1.SetBinding(IsVisibleProperty, "IsPhotoImage1Enabled");

			_photoImage2 = new FrameImage(AutomationIdConstants.PhotoImage2AutomationId);
			_photoImage2.ContentImage.SetBinding(Image.SourceProperty, "Photo2ImageSource");
			_photoImage2.SetBinding(IsVisibleProperty, "IsPhotoImage2Enabled");
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
			var resetButton = new BounceButton(AutomationIdConstants.ResetButtonAutomationId)
			{
				Text = "Reset"
			};
			resetButton.SetBinding(Button.CommandProperty, "ResetButtonPressed");
			resetButton.SetBinding(Button.IsEnabledProperty, "IsResetButtonEnabled");
			resetButton.SetBinding(IsVisibleProperty, "IsResetButtonEnabled");

			var resetButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					resetButton
				}
			};
			#endregion

			#region Create Hidden Label
			//This label will not appear on the screen, 
			//but it will allow the UITest to determine the current Emotion
			var hiddenEmotionLabel = new Label
			{
				IsVisible = false,
				IsEnabled = false,
				AutomationId = AutomationIdConstants.EmotionLabelAutomationId
			};
			hiddenEmotionLabel.SetBinding(Label.TextProperty, "PageTitle");
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

			#region Initialize View Model, Set Page Content, and Binding Context
			_viewModel = new PictureViewModel();
			BindingContext = _viewModel;

			Content = new ScrollView
			{
				Content = buttonImageRelativeLayout
			};
			#endregion
		}
		#endregion

		#region Methods
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

		async void HandleDisplayEmotionBeforeCameraAlert(object sender, AlertMessageEventArgs e)
		{
			var alertMessage = e.Message;
			bool userResponseToAlert = false;

			userResponseToAlert = await DisplayAlert(alertMessage.Title, alertMessage.Message, "OK", "Cancel");

			_viewModel.UserResponseToAlert = userResponseToAlert;
			_viewModel.UserHasAcknowledgedPopUp = true;
		}

		void HandleDisplayAllEmotionResultsAlert(object sender, TextEventArgs e)
		{
			var allEmotionResults = e.Text;
			DisplayAlert("Results", allEmotionResults, "OK");
		}

		void HandleDisplayNoCameraAvailableAlert(object sender, EventArgs e)
		{
			DisplayAlert("Error", "No Camera Available", "OK");
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

		async void HandleDisplayMultipleFacesDetectedAlert(object sender, EventArgs e)
		{
			await DisplayAlert("Error: Multiple Faces Detected", "Ensure only one face is captured in the photo", "Ok");
		}

		public void SetPhotoImage1(string photoImageSource)
		{
			_viewModel.SetPhotoImage1(photoImageSource);
		}

		public void SetPhotoImage2(string photoImageSource)
		{
			_viewModel.SetPhotoImage2(photoImageSource);
		}
		#endregion
	}
}

