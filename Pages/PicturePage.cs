using System;

using Xamarin.Forms;

namespace FaceOff
{
	public class PicturePage : ContentPage
	{
		Image PhotoImage1, PhotoImage2;

		public PicturePage()
		{
			var _androidVerticalPadding = 80;

			var viewModel = new PictureViewModel();
			BindingContext = viewModel;

			this.SetBinding(ContentPage.TitleProperty, "PageTitle");
			BackgroundColor = Color.FromHex("#91E2F4");


			#region Create Score Button 1 Stack
			var photo1ScoreButton = new BounceButton
			{
				HorizontalOptions = LayoutOptions.Center,
				Style = StylesConstants.ButtonStyle
			};
			photo1ScoreButton.SetBinding(Button.TextProperty, "ScoreButton1Text");
			photo1ScoreButton.SetBinding(IsEnabledProperty, "IsScore1ButtonEnabled");
			photo1ScoreButton.SetBinding(IsVisibleProperty, "IsScore1ButtonVisable");

			var photo1ScoreButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					photo1ScoreButton
				}
			};
			#endregion

			#region Create Score Button 2 Stack
			var photo2ScoreButton = new BounceButton
			{
				HorizontalOptions = LayoutOptions.Center,
				Style = StylesConstants.ButtonStyle
			};
			photo2ScoreButton.SetBinding(Button.TextProperty, "ScoreButton2Text");
			photo2ScoreButton.SetBinding(IsEnabledProperty, "IsScore2ButtonEnabled");
			photo2ScoreButton.SetBinding(IsVisibleProperty, "IsScore2ButtonVisable");

			var photo2ScoreButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					photo2ScoreButton
				}
			};
			#endregion

			#region Create Photo Activity Indicators
			var photo1ActivityIndicator = new ActivityIndicator();
			photo1ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsCalculatingPhoto1Score");
			photo1ActivityIndicator.SetBinding(ActivityIndicator.IsEnabledProperty, "IsCalculatingPhoto1Score");
			photo1ActivityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsCalculatingPhoto1Score");

			var photo2ActivityIndicator = new ActivityIndicator();
			photo2ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsCalculatingPhoto2Score");
			photo2ActivityIndicator.SetBinding(ActivityIndicator.IsEnabledProperty, "IsCalculatingPhoto2Score");
			photo2ActivityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsCalculatingPhoto2Score");
			#endregion

			#region Create Photo 1 Button Stack
			var takePhoto1Button = new BounceButton
			{
				Text = "Take Photo",
				Style = StylesConstants.ButtonStyle
			};
			takePhoto1Button.SetBinding(Button.CommandProperty, "TakePhoto1ButtonPressed");
			takePhoto1Button.SetBinding(Button.IsEnabledProperty, "IsTakeLeftPhotoButtonEnabled");
			takePhoto1Button.SetBinding(Button.IsVisibleProperty, "IsTakeLeftPhotoButtonEnabled");

			var takePhoto1ButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					takePhoto1Button
				}
			};
			#endregion

			#region Create Photo 2 Button Stack
			var takePhoto2Button = new BounceButton
			{
				Text = "Take Photo",
				Style = StylesConstants.ButtonStyle
			};
			takePhoto2Button.SetBinding(Button.CommandProperty, "TakePhoto2ButtonPressed");
			takePhoto2Button.SetBinding(Button.IsEnabledProperty, "IsTakeRightPhotoButtonEnabled");
			takePhoto2Button.SetBinding(Button.IsVisibleProperty, "IsTakeRightPhotoButtonEnabled");

			var takePhoto2ButtonStack = new StackLayout
			{
				Padding = new Thickness(24, 24, 24, 24),
				Children = {
					takePhoto2Button
				}
			};
			#endregion

			#region Create Photo Image Containers
			PhotoImage1 = new Image();
			PhotoImage1.SetBinding(Image.SourceProperty, "Photo1ImageSource");
			PhotoImage1.SetBinding(Image.IsVisibleProperty, "IsPhotoImage1Enabled");

			PhotoImage2 = new Image();
			PhotoImage2.SetBinding(Image.SourceProperty, "Photo2ImageSource");
			PhotoImage2.SetBinding(Image.IsVisibleProperty, "IsPhotoImage2Enabled");
			#endregion

			#region Create Photo 1 Stack
			var photo1Stack = new StackLayout
			{
				Style = StylesConstants.StackLayoutStyle,
				Children = {
					PhotoImage1,
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
					PhotoImage2,
					photo2ScoreButtonStack,
					photo2ActivityIndicator
				}
			};
			#endregion

			#region Create Reset Button Stack
			var resetButton = new BounceButton
			{
				Text = "Reset",
				Style = StylesConstants.ButtonStyle,
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

			#region Create Relative Laout
			var buttonImageRelativeLayout = new RelativeLayout();
			buttonImageRelativeLayout.Children.Add(photo1Stack,
				Constraint.RelativeToParent(parent =>
				{
					return parent.X;
				}),
				Constraint.RelativeToParent(parent =>
				{
					if (Device.OS == TargetPlatform.Android)
						return parent.Y + _androidVerticalPadding;
					else
						return parent.Y;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					if (Device.OS == TargetPlatform.Android)
						return (parent.Height * 6 / 8) - _androidVerticalPadding;
					else
						return parent.Height * 6 / 8;
				})
			);

			buttonImageRelativeLayout.Children.Add(photo2Stack,
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					if (Device.OS == TargetPlatform.Android)
						return parent.Y + _androidVerticalPadding;
					else
						return parent.Y;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					if (Device.OS == TargetPlatform.Android)
						return (parent.Height * 6 / 8) - _androidVerticalPadding;
					else
						return parent.Height * 6 / 8;
				})
			);

			buttonImageRelativeLayout.Children.Add(takePhoto1ButtonStack,
				Constraint.RelativeToParent(parent =>
				{
					return parent.X;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Y;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				})
			);

			buttonImageRelativeLayout.Children.Add(takePhoto2ButtonStack,
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Y;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				})
			);

			buttonImageRelativeLayout.Children.Add(resetButtonStack,
				Constraint.RelativeToParent(parent =>
				{
					return parent.X;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 6 / 8;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 1 / 8;
				})
			);
			#endregion

			#region Set Page Content, Padding, and Events
			viewModel.RotateImage += HandleRotateImage;

			Content = new ScrollView{
				Content = buttonImageRelativeLayout
			};
			#endregion
		}

		void HandleRotateImage(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				var scalingFactor = 1.5;

				var parameters = (RotatableImageParameters)sender;

				if (parameters.ImageNumberToRotate == 1)
				{
					PhotoImage1.RotateTo(parameters.DegreesOfClockwiseRotation);
					PhotoImage1.ScaleTo(scalingFactor);
				}
				else if(parameters.ImageNumberToRotate == 2)
				{
					PhotoImage2.RotateTo(parameters.DegreesOfClockwiseRotation);
					PhotoImage2.ScaleTo(scalingFactor);
				}
			});
		}

	}
}

