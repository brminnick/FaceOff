using System;
using System.Threading.Tasks;
using FaceOff.Shared;
using Xamarin.CommunityToolkit.Markup;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.CommunityToolkit.Markup.GridRowsColumns;

namespace FaceOff
{
	class FaceOffPage : BaseContentPage<FaceOffViewModel>
	{
		readonly FrameImage _photoImage1, _photoImage2;
		readonly ScoreButton _scoreButton1, _scoreButton2;

		public FaceOffPage()
		{
			this.SetBinding(TitleProperty, nameof(FaceOffViewModel.PageTitle));

			subscribeEventHandlers();

			Content = new Grid
			{
				Padding = new Thickness(24),

				RowSpacing = 12,
				ColumnSpacing = 24,

				RowDefinitions = Rows.Define(
					(Row.PlayerName, Stars(1)),
					(Row.TakePhoto, Stars(2)),
					(Row.PhotoImage, Stars(2)),
					(Row.PhotoImagePadding, Stars(2)),
					(Row.Results, Stars(2)),
					(Row.ActivityIndicator, Stars(2)),
					(Row.Reset, Stars(12))),

				ColumnDefinitions = Columns.Define(
					(Column.Player1, Stars(1)),
					(Column.Player2, Stars(1))),

				Children =
				{
					new PlayerNameLabel(PreferencesService.Player1Name, AutomationIdConstants.Player1NameLabel).Row(Row.PlayerName).Column(Column.Player1),

					new PlayerNameLabel(PreferencesService.Player2Name, AutomationIdConstants.Player2NameLabel).Row(Row.PlayerName).Column(Column.Player2),

					new TakePhotoButton(AutomationIdConstants.TakePhoto1Button).Row(Row.TakePhoto).Column(Column.Player1)
						.Bind(Button.CommandProperty, nameof(FaceOffViewModel.TakePhotoButton1Tapped)),

					new TakePhotoButton(AutomationIdConstants.TakePhoto2Button).Row(Row.TakePhoto).Column(Column.Player2)
						.Bind(Button.CommandProperty, nameof(FaceOffViewModel.TakePhotoButton2Tapped)),

					new FrameImage(AutomationIdConstants.PhotoImage1, nameof(FaceOffViewModel.FrameImageSource1)).Assign(out _photoImage1).Row(Row.PlayerName).RowSpan(4).Column(Column.Player1),

					new FrameImage(AutomationIdConstants.PhotoImage2, nameof(FaceOffViewModel.FrameImageSource2)).Assign(out _photoImage2).Row(Row.PlayerName).RowSpan(4).Column(Column.Player2),

					new ScoreButton(AutomationIdConstants.ScoreButton1).Assign(out _scoreButton1).Row(Row.Results).Column(Column.Player1)
						.Bind(Button.TextProperty, nameof(FaceOffViewModel.ScoreButton1Text))
						.Bind(Button.CommandProperty, nameof(FaceOffViewModel.ScoreButton1Command)),

					new ScoreButton(AutomationIdConstants.ScoreButton2).Assign(out _scoreButton2).Row(Row.Results).Column(Column.Player2)
						.Bind(Button.TextProperty, nameof(FaceOffViewModel.ScoreButton2Text))
						.Bind(Button.CommandProperty, nameof(FaceOffViewModel.ScoreButton2Command)),

					new DarkBlueActivityIndicator(AutomationIdConstants.Player1ActivityIndicator).Row(Row.ActivityIndicator).Column(Column.Player1)
						.Bind(IsVisibleProperty, nameof(FaceOffViewModel.IsCalculatingPlayer1Score))
						.Bind(ActivityIndicator.IsRunningProperty, nameof(FaceOffViewModel.IsCalculatingPlayer1Score)),

					new DarkBlueActivityIndicator(AutomationIdConstants.Player2ActivityIndicator).Row(Row.ActivityIndicator).Column(Column.Player2)
						.Bind(IsVisibleProperty, nameof(FaceOffViewModel.IsCalculatingPhoto2Score))
						.Bind(ActivityIndicator.IsRunningProperty, nameof(FaceOffViewModel.IsCalculatingPhoto2Score)),

					new ResetButton().Row(Row.Reset).ColumnSpan(All<Column>())
						.Bind(Button.CommandProperty, nameof(FaceOffViewModel.ResetButtonCommand))
						.Invoke(resetButton => resetButton.Clicked += HandleResetButtonClicked)
				}
			};

			void subscribeEventHandlers()
			{
				ViewModel.GameInitialized += HandleGameInitialized;
				ViewModel.EmotionResultsGathered += HandleEmotionResultsGathered;
				ViewModel.GenerateEmotionResultsStarted += HandleGenerateEmotionResultsStarted;
				MediaService.NoCameraDetected += HandleNoCameraDetected;
				MediaService.PermissionsDenied += HandlePermissionsDenied;
				EmotionService.MultipleFacesDetectedAlertTriggered += HandleMultipleFacesDetectedAlertTriggered;
			}
		}

		enum Row { PlayerName, TakePhoto, PhotoImage, PhotoImagePadding, Results, ActivityIndicator, Reset }
		enum Column { Player1, Player2 }

		async void HandleEmotionResultsGathered(object sender, string message) =>
			await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Results", message, "OK"));

		async void HandleNoCameraDetected(object sender, EventArgs e) =>
			await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error", "No Camera Available", "OK"));

		async void HandleMultipleFacesDetectedAlertTriggered(object sender, EventArgs e) =>
			await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error: Multiple Faces Detected", "Ensure only one face is captured in the photo", "Ok"));

		async void HandleResetButtonClicked(object sender, EventArgs e)
		{
			await Task.WhenAll(HideView(_scoreButton1),
								HideView(_scoreButton2),
								HideView(_photoImage1),
								HideView(_photoImage2));
		}

		void HandlePermissionsDenied(object sender, EventArgs e)
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				var isAlertAccepted = await DisplayAlert("Open Settings?", "Storage and Camera Permission Need To Be Enabled", "Ok", "Cancel");
				if (isAlertAccepted)
					AppInfo.ShowSettingsUI();
			});
		}

		void HandleGameInitialized(object sender, GameInitializedEventArgs e)
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				var userResponseToAlert = await DisplayAlert(e.Title, e.Message, "OK", "Cancel");
				ViewModel.EmotionPopUpAlertResponseCommand.Execute(new EmotionPopupResponseModel(userResponseToAlert, e.Player));
			});
		}

		async void HandleGenerateEmotionResultsStarted(object sender, PlayerNumberType playerNumber)
		{
			switch (playerNumber)
			{
				case PlayerNumberType.Player1:
					await RevealView(_photoImage1);
					await RevealView(_scoreButton1);
					break;
				case PlayerNumberType.Player2:
					await RevealView(_photoImage2);
					await RevealView(_scoreButton2);
					break;
				default:
					throw new NotSupportedException();
			}
		}

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

		class ResetButton : BounceButton
		{
			public ResetButton() : base(AutomationIdConstants.ResetButton)
			{
				Text = "Reset";
				Margin = new Thickness(0, 24);
				VerticalOptions = LayoutOptions.End;
				HorizontalOptions = LayoutOptions.FillAndExpand;
			}
		}

		class FrameImage : Frame
		{
			public FrameImage(in string automationId, in string imageSourceBindingPath)
			{
				AutomationId = automationId;
				BackgroundColor = Color.White;

				Scale = 0;
				HasShadow = false;
				IsVisible = false;

				Content = new Image().Bind(Image.SourceProperty, imageSourceBindingPath);
			}
		}

		class TakePhotoButton : BounceButton
		{
			public TakePhotoButton(in string automationId) : base(automationId)
			{
				Text = "Take Photo";
				VerticalOptions = LayoutOptions.Start;
			}
		}

		class ScoreButton : BounceButton
		{
			public ScoreButton(in string automationId) : base(automationId)
			{
				Scale = 0;
				IsVisible = false;
				Padding = new Thickness(24, 0);
				Margin = new Thickness(0, 6);
			}
		}

		class PlayerNameLabel : DarkBlueLabel
		{
			public PlayerNameLabel(in string text, in string automationId) : base(text)
			{
				AutomationId = automationId;

				HorizontalOptions = LayoutOptions.Center;
				HorizontalTextAlignment = TextAlignment.Center;

				VerticalOptions = LayoutOptions.End;
				VerticalTextAlignment = TextAlignment.End;
			}
		}

		class DarkBlueActivityIndicator : ActivityIndicator
		{
			public DarkBlueActivityIndicator(in string automationId)
			{
				AutomationId = automationId;
				Color = ColorConstants.ActivityIndicatorColor;
			}
		}
	}
}