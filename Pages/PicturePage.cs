using Plugin.Media;
using Xamarin.Forms;

namespace FaceOff
{
	public class PicturePage : ContentPage
	{

		public PicturePage()
		{
			var viewModel = new PictureViewModel();
			BindingContext = viewModel;

			this.SetBinding(ContentPage.TitleProperty, "PageTitle");
			BackgroundColor = Color.FromHex("#91E2F4");


			#region Create Photo Labels
			var photo1Label = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};
			photo1Label.SetBinding(Label.TextProperty, "Photo1LabelText");

			var photo2Label = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};
			photo2Label.SetBinding(Label.TextProperty, "Photo2LabelText");
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

			#region Create Photo 1 Stack
			var takePhoto1Button = new FaceOffButton
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

			#region Create Photo 2 Button
			var takePhoto2Button = new FaceOffButton
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
			var photoImage1 = new Image();
			photoImage1.SetBinding(Image.SourceProperty, "Photo1ImageSource");
			photoImage1.SetBinding(Image.IsVisibleProperty, "IsPhotoImage1Enabled");

			var photoImage2 = new Image();
			photoImage2.SetBinding(Image.SourceProperty, "Photo2ImageSource");
			photoImage1.SetBinding(Image.IsVisibleProperty, "IsPhotoImage2Enabled");
			#endregion

			#region Create Photo 1 Stack
			var photo1Stack = new StackLayout
			{
				Children = {
					photoImage1,
					photo1Label,
					photo1ActivityIndicator
				},
			};
			#endregion

			#region Create Photo 2 Stack
			var photo2Stack = new StackLayout
			{
				Children = {
					photoImage2,
					photo2Label,
					photo2ActivityIndicator
				}
			};
			#endregion

			#region Create Reset Button Stack
			var resetButton = new FaceOffButton
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

			/*<StackLayout Padding="24,24,24,14" Spacing="14"	IsEnabled="{Binding IsBusy, Converter={x:Static local:InverseBoolConverter.Instance}}">

					<local:SportButton Text="CHALLENGE" StyleId="challengeButton" HorizontalOptions="FillAndExpand"
						BackgroundColor="{Binding League.Theme.Dark}" Clicked="HandleChallengeClicked" IsVisible="{Binding CanChallenge}" />

					<local:SportButton Text="LEADERBOARD" StyleId="leaderboardButton" HorizontalOptions="FillAndExpand"
						Clicked="HandleRankingsClicked" />

					<local:SportButton Text="JOIN LEAGUE" StyleId="joinButton" HorizontalOptions="FillAndExpand" Clicked="HandleJoinClicked"
						BackgroundColor="{Binding League.Theme.Dark}" IsVisible="{Binding IsMember, Converter={x:Static local:InverseBoolConverter.Instance}}" />
				</StackLayout>*/

			#region Create Relative Laout
			var buttonImageRelativeLayout = new RelativeLayout();
			buttonImageRelativeLayout.Children.Add(photo1Stack,
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
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 7 / 8;
				})
			);

			buttonImageRelativeLayout.Children.Add(photo2Stack,
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
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 7 / 8;
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
					return parent.Height * 7 / 8;
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

			#region Set Page Content and Padding
			Content = new ScrollView{
				Content = buttonImageRelativeLayout
			};
			#endregion
		}

	}
}

