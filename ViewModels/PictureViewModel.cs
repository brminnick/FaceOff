using System;
using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

using Xamarin;
using Xamarin.Forms;

namespace FaceOff
{
	public class PictureViewModel : BaseViewModel
	{
		#region Constant Fields
		readonly string[] _emotionStrings = { "Anger", "Contempt", "Disgust", "Fear", "Happiness", "Neutral", "Sadness", "Surprise" };
		readonly string[] _emotionStringsForAlertMessage = { "angry", "disrespectful", "disgusted", "scared", "happy", "blank", "sad", "surprised" };

		readonly string[] ErrorMessage = { "No Face Detected", "Error" };
		const string MakeAFaceAlertMessage = "take a selfie looking ";
		const string CalculatingScore = "Analyzing";

		readonly string _player1NameText, _player2NameText;
		#endregion

		#region Fields
		ImageSource _photo1ImageSource, _photo2ImageSource;
		string _scoreButton1Text, _scoreButton2Text;
		bool _isTakeLeftPhotoButtonEnabled = true;
		bool _isTakeLeftPhotoButtonStackVisible = true;
		bool _isTakeRightPhotoButtonEnabled = true;
		bool _isTakeRightPhotoButtonStackVisible = true;
		bool _isResetButtonEnabled;
		string _pageTitle;
		int _emotionNumber;
		bool _isCalculatingPhoto1Score, _isCalculatingPhoto2Score;
		bool _isScore1ButtonEnabled, _isScore2ButtonEnabled, _isScore1ButtonVisable, _isScore2ButtonVisable;
		bool _isPhotoImage1Enabled, _isPhotoImage2Enabled;
		string _photo1Results, _photo2Results;
		ICommand _takePhoto1ButtonPressed, _takePhoto2ButtonPressed;
		ICommand _photo1ScoreButtonPressed, _photo2ScoreButtonPressed;
		ICommand _resetButtonPressed;

		public event EventHandler<AlertMessageEventArgs> DisplayEmotionBeforeCameraAlert;
		public event EventHandler<TextEventArgs> DisplayAllEmotionResultsAlert;
		public event EventHandler DisplayMultipleFacesDetectedAlert;
		public event EventHandler DisplayNoCameraAvailableAlert;
		public event EventHandler RevealScoreButton1WithAnimation;
		public event EventHandler RevealScoreButton2WithAnimation;
		public event EventHandler RevealPhotoImage1WithAnimation;
		public event EventHandler RevealPhotoImage2WithAnimation;
		#endregion

		#region Constructors
		public PictureViewModel(string player1NameText, string player2NameText)
		{
			IsResetButtonEnabled = false;

			_player1NameText = player1NameText;
			_player2NameText = player2NameText;

			SetEmotion();
		}
		#endregion

		#region Properties
		public ICommand TakePhoto1ButtonPressed =>
		_takePhoto1ButtonPressed ??
		(_takePhoto1ButtonPressed = new Command(async () => await ExecuteTakePhoto1ButtonPressed()));

		public ICommand TakePhoto2ButtonPressed =>
		_takePhoto2ButtonPressed ??
		(_takePhoto2ButtonPressed = new Command(async () => await ExecuteTakePhoto2ButtonPressed()));

		public ICommand ResetButtonPressed =>
		_resetButtonPressed ??
		(_resetButtonPressed = new Command(ExecuteResetButtonPressed));

		public ICommand Photo1ScoreButtonPressed =>
		_photo1ScoreButtonPressed ??
		(_photo1ScoreButtonPressed = new Command(ExecutePhoto1ScoreButtonPressed));

		public ICommand Photo2ScoreButtonPressed =>
		 _photo2ScoreButtonPressed ??
		 (_photo2ScoreButtonPressed = new Command(ExecutePhoto2ScoreButtonPressed));

		public ImageSource Photo1ImageSource
		{
			get { return _photo1ImageSource; }
			set { SetProperty(ref _photo1ImageSource, value); }
		}

		public ImageSource Photo2ImageSource
		{
			get { return _photo2ImageSource; }
			set { SetProperty(ref _photo2ImageSource, value); }
		}

		public bool IsPhotoImage1Enabled
		{
			get { return _isPhotoImage1Enabled; }
			set { SetProperty(ref _isPhotoImage1Enabled, value); }
		}

		public bool IsPhotoImage2Enabled
		{
			get { return _isPhotoImage2Enabled; }
			set { SetProperty(ref _isPhotoImage2Enabled, value); }
		}

		public bool IsTakeLeftPhotoButtonEnabled
		{
			get { return _isTakeLeftPhotoButtonEnabled; }
			set { SetProperty(ref _isTakeLeftPhotoButtonEnabled, value); }
		}

		public bool IsTakeLeftPhotoButtonStackVisible
		{
			get { return _isTakeLeftPhotoButtonStackVisible; }
			set { SetProperty(ref _isTakeLeftPhotoButtonStackVisible, value); }
		}

		public bool IsTakeRightPhotoButtonEnabled
		{
			get { return _isTakeRightPhotoButtonEnabled; }
			set { SetProperty(ref _isTakeRightPhotoButtonEnabled, value); }
		}

		public bool IsTakeRightPhotoButtonStackVisible
		{
			get { return _isTakeRightPhotoButtonStackVisible; }
			set { SetProperty(ref _isTakeRightPhotoButtonStackVisible, value); }
		}

		public string PageTitle
		{
			get { return _pageTitle; }
			set { SetProperty(ref _pageTitle, value); }
		}

		public string ScoreButton1Text
		{
			get { return _scoreButton1Text; }
			set { SetProperty(ref _scoreButton1Text, value); }
		}

		public string ScoreButton2Text
		{
			get { return _scoreButton2Text; }
			set { SetProperty(ref _scoreButton2Text, value); }
		}

		public bool IsCalculatingPhoto1Score
		{
			get { return _isCalculatingPhoto1Score; }
			set { SetProperty(ref _isCalculatingPhoto1Score, value); }
		}

		public bool IsCalculatingPhoto2Score
		{
			get { return _isCalculatingPhoto2Score; }
			set { SetProperty(ref _isCalculatingPhoto2Score, value); }
		}

		public bool IsResetButtonEnabled
		{
			get { return _isResetButtonEnabled; }
			set { SetProperty(ref _isResetButtonEnabled, value); }
		}

		public bool IsScore1ButtonEnabled
		{
			get { return _isScore1ButtonEnabled; }
			set { SetProperty(ref _isScore1ButtonEnabled, value); }
		}

		public bool IsScore2ButtonEnabled
		{
			get { return _isScore2ButtonEnabled; }
			set { SetProperty(ref _isScore2ButtonEnabled, value); }
		}

		public bool IsScore1ButtonVisable
		{
			get { return _isScore1ButtonVisable; }
			set { SetProperty(ref _isScore1ButtonVisable, value); }
		}

		public bool IsScore2ButtonVisable
		{
			get { return _isScore2ButtonVisable; }
			set { SetProperty(ref _isScore2ButtonVisable, value); }
		}

		public bool HasUserAcknowledgedPopUp { get; set; } = false;
		public bool UserResponseToAlert { get; set; }
		#endregion

		#region Methods
		public void SetPhotoImage1(string photo1ImageString)
		{
			Photo1ImageSource = photo1ImageString;

			var allEmotionsString = "";
			allEmotionsString += $"Anger: 0%\n";
			allEmotionsString += $"Contempt: 0%\n";
			allEmotionsString += $"Disgust: 0%\n";
			allEmotionsString += $"Fear: 0%\n";
			allEmotionsString += $"Happiness: 100%\n";
			allEmotionsString += $"Neutral: 0%\n";
			allEmotionsString += $"Sadness: 0%\n";
			allEmotionsString += $"Surprise: 0%";

			_photo1Results = allEmotionsString;
			ScoreButton1Text = "Score: 100%";

			SetEmotion(4);

			IsTakeLeftPhotoButtonEnabled = false;
			IsTakeLeftPhotoButtonStackVisible = false;

			OnRevealPhotoImage1WithAnimation();
			OnRevealScoreButton1WithAnimation();
		}

		public void SetPhotoImage2(string photo2ImageString)
		{
			Photo2ImageSource = photo2ImageString;

			var allEmotionsString = "";
			allEmotionsString += $"Anger: 0%\n";
			allEmotionsString += $"Contempt: 0%\n";
			allEmotionsString += $"Disgust: 0%\n";
			allEmotionsString += $"Fear: 0%\n";
			allEmotionsString += $"Happiness: 100%\n";
			allEmotionsString += $"Neutral: 0%\n";
			allEmotionsString += $"Sadness: 0%\n";
			allEmotionsString += $"Surprise: 0%";

			_photo2Results = allEmotionsString;
			ScoreButton2Text = "Score: 100%";

			SetEmotion(4);

			IsTakeRightPhotoButtonEnabled = false;
			IsTakeRightPhotoButtonStackVisible = false;

			OnRevealPhotoImage2WithAnimation();
			OnRevealScoreButton2WithAnimation();
		}

		async Task ExecuteTakePhoto1ButtonPressed()
		{
			IsTakeRightPhotoButtonEnabled = false;
			IsScore1ButtonEnabled = false;

			Insights.Track(InsightsConstants.PhotoButton1Tapped);

			if (!(await DisplayPopUpAlertAboutEmotion(_player1NameText)))
			{
				IsTakeRightPhotoButtonEnabled = true;
				IsScore1ButtonEnabled = true;
				return;
			}

			var imageMediaFile = await GetMediaFileFromCamera("FaceOff", "PhotoImage1");

			if (imageMediaFile == null)
			{
				IsTakeRightPhotoButtonEnabled = true;
				IsScore1ButtonEnabled = true;
				return;
			}

			await WaitForEventToBeSubscribed(RevealPhotoImage1WithAnimation);

			OnRevealPhotoImage1WithAnimation();

			Insights.Track(InsightsConstants.PhotoTaken);

			IsTakeLeftPhotoButtonEnabled = false;
			IsTakeLeftPhotoButtonStackVisible = false;

			ScoreButton1Text = CalculatingScore;


			Photo1ImageSource = ImageSource.FromStream(() =>
			{
				return GetPhotoStream(imageMediaFile, false);
			});

			IsCalculatingPhoto1Score = true;
			IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

			await WaitForAnimationsToFinish((int)Math.Ceiling(AnimationConstants.PhotoImageAninmationTime * 2.5));

			await WaitForEventToBeSubscribed(RevealScoreButton1WithAnimation);

			OnRevealScoreButton1WithAnimation();

			var emotionArray = await GetEmotionResultsFromMediaFile(imageMediaFile, false);

			var emotionScore = GetPhotoEmotionScore(emotionArray, 0);

			bool doesEmotionScoreContainErrorMessage = DoesStringContainErrorMessage(emotionScore);

			if (doesEmotionScoreContainErrorMessage)
			{
				if (emotionScore.Contains(ErrorMessage[0]))
					Insights.Track(InsightsConstants.NoFaceDetected);
				else if (emotionScore.Contains(ErrorMessage[1]))
					Insights.Track(InsightsConstants.MultipleFacesDetected);

				ScoreButton1Text = emotionScore;
			}
			else
				ScoreButton1Text = $"Score: {emotionScore}";

			_photo1Results = GetStringOfAllPhotoEmotionScores(emotionArray, 0);

			IsCalculatingPhoto1Score = false;
			IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

			imageMediaFile.Dispose();

			await WaitForAnimationsToFinish((int)Math.Ceiling(AnimationConstants.ScoreButonAninmationTime * 2.5));
		}

		async Task ExecuteTakePhoto2ButtonPressed()
		{
			IsTakeLeftPhotoButtonEnabled = false;
			IsScore2ButtonEnabled = false;

			Insights.Track(InsightsConstants.PhotoButton2Tapped);

			if (!(await DisplayPopUpAlertAboutEmotion(_player2NameText)))
			{
				IsTakeLeftPhotoButtonEnabled = true;
				IsScore2ButtonEnabled = true;
				return;
			}

			var imageMediaFile = await GetMediaFileFromCamera("FaceOff", "PhotoImage2");
			if (imageMediaFile == null)
			{
				IsTakeLeftPhotoButtonEnabled = true;
				IsScore2ButtonEnabled = true;
				return;
			}

			await WaitForEventToBeSubscribed(RevealPhotoImage2WithAnimation);

			OnRevealPhotoImage2WithAnimation();

			IsTakeRightPhotoButtonEnabled = false;
			IsTakeRightPhotoButtonStackVisible = false;

			ScoreButton2Text = CalculatingScore;

			Photo2ImageSource = ImageSource.FromStream(() =>
			{
				return GetPhotoStream(imageMediaFile, false);
			});

			IsCalculatingPhoto2Score = true;
			IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

			await WaitForAnimationsToFinish((int)Math.Ceiling(AnimationConstants.PhotoImageAninmationTime * 2.5));

			await WaitForEventToBeSubscribed(RevealScoreButton2WithAnimation);

			OnRevealScoreButton2WithAnimation();

			var emotionArray = await GetEmotionResultsFromMediaFile(imageMediaFile, false);

			var emotionScore = GetPhotoEmotionScore(emotionArray, 0);

			bool doesEmotionScoreContainErrorMessage = DoesStringContainErrorMessage(emotionScore);

			if (doesEmotionScoreContainErrorMessage)
			{
				if (emotionScore.Contains(ErrorMessage[0]))
					Insights.Track(InsightsConstants.NoFaceDetected);
				else if (emotionScore.Contains(ErrorMessage[1]))
					Insights.Track(InsightsConstants.MultipleFacesDetected);

				ScoreButton2Text = emotionScore;
			}
			else
			{
				ScoreButton2Text = $"Score: {emotionScore}";
			}

			_photo2Results = GetStringOfAllPhotoEmotionScores(emotionArray, 0);

			IsCalculatingPhoto2Score = false;
			IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

			imageMediaFile.Dispose();

			await WaitForAnimationsToFinish((int)Math.Ceiling(AnimationConstants.ScoreButonAninmationTime * 2.5));
		}

		void ExecuteResetButtonPressed()
		{
			Insights.Track(InsightsConstants.ResetButtonTapped);

			SetEmotion();

			Photo1ImageSource = null;
			Photo2ImageSource = null;

			IsTakeLeftPhotoButtonEnabled = true;
			IsTakeLeftPhotoButtonStackVisible = true;

			IsTakeRightPhotoButtonEnabled = true;
			IsTakeRightPhotoButtonStackVisible = true;

			ScoreButton1Text = null;
			ScoreButton2Text = null;

			IsScore1ButtonEnabled = false;
			IsScore2ButtonEnabled = false;

			IsScore1ButtonVisable = false;
			IsScore2ButtonVisable = false;

			_photo1Results = null;
			_photo2Results = null;

			IsPhotoImage1Enabled = false;
			IsPhotoImage2Enabled = false;
		}

		void ExecutePhoto1ScoreButtonPressed()
		{
			Insights.Track(InsightsConstants.ResultsButton1Tapped);
			OnDisplayAllEmotionResultsAlert(_photo1Results);
		}

		void ExecutePhoto2ScoreButtonPressed()
		{
			Insights.Track(InsightsConstants.ResultsButton2Tapped);
			OnDisplayAllEmotionResultsAlert(_photo2Results);
		}

		Stream GetPhotoStream(MediaFile mediaFile, bool disposeMediaFile)
		{
			var stream = mediaFile.GetStream();

			if (disposeMediaFile)
				mediaFile.Dispose();

			return stream;
		}

		async Task<MediaFile> GetMediaFileFromCamera(string directory, string filename)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				OnDisplayNoCameraAvailableAlert();
				return null;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = directory,
				Name = filename,
				DefaultCamera = CameraDevice.Front,
				OverlayViewProvider = DependencyService.Get<ICameraService>()?.GetCameraOverlay()
			});

			return file;
		}

		async Task<Emotion[]> GetEmotionResultsFromMediaFile(MediaFile mediaFile, bool disposeMediaFile)
		{
			if (mediaFile == null)
				return null;

			try
			{
				var emotionClient = new EmotionServiceClient(CognitiveServicesConstants.EmotionApiKey);

				using (var handle = Insights.TrackTime(InsightsConstants.AnalyzeEmotion))
				{
					return await emotionClient.RecognizeAsync(GetPhotoStream(mediaFile, disposeMediaFile));
				}
			}
			catch (Exception e)
			{
				Insights.Report(e);
				return null;
			}
		}

		int GetRandomNumberForEmotion()
		{
			int randomNumber;

			do
			{
				var rnd = new Random();
				randomNumber = rnd.Next(0, _emotionStrings.Length);
			} while (randomNumber == _emotionNumber);

			return randomNumber;
		}

		void SetPageTitle(int emotionNumber)
		{
			PageTitle = _emotionStrings[emotionNumber];
		}

		void SetEmotion(int? emotionNumber = null)
		{
			if (emotionNumber != null && emotionNumber >= 0 && emotionNumber <= _emotionStrings.Length - 1)
				_emotionNumber = (int)emotionNumber;
			else
				_emotionNumber = GetRandomNumberForEmotion();

			SetPageTitle(_emotionNumber);
		}

		string GetPhotoEmotionScore(Emotion[] emotionResults, int emotionResultNumber)
		{
			float rawEmotionScore;

			if (emotionResults == null || emotionResults.Length < 1)
				return ErrorMessage[0];

			if (emotionResults.Length > 1)
			{
				OnDisplayMultipleFacesError();
				return ErrorMessage[1];
			}

			try
			{
				switch (_emotionNumber)
				{
					case 0:
						rawEmotionScore = emotionResults[emotionResultNumber].Scores.Anger;
						break;
					case 1:
						rawEmotionScore = emotionResults[emotionResultNumber].Scores.Contempt;
						break;
					case 2:
						rawEmotionScore = emotionResults[emotionResultNumber].Scores.Disgust;
						break;
					case 3:
						rawEmotionScore = emotionResults[emotionResultNumber].Scores.Fear;
						break;
					case 4:
						rawEmotionScore = emotionResults[emotionResultNumber].Scores.Happiness;
						break;
					case 5:
						rawEmotionScore = emotionResults[emotionResultNumber].Scores.Neutral;
						break;
					case 6:
						rawEmotionScore = emotionResults[emotionResultNumber].Scores.Sadness;
						break;
					case 7:
						rawEmotionScore = emotionResults[emotionResultNumber].Scores.Surprise;
						break;
					default:
						return ErrorMessage[0];
				}

				var emotionScoreAsPercentage = ConvertFloatToPercentage(rawEmotionScore);

				return emotionScoreAsPercentage;
			}
			catch (Exception e)
			{
				Insights.Report(e);
				return ErrorMessage[0];
			}
		}

		string GetStringOfAllPhotoEmotionScores(Emotion[] emotionResults, int emotionResultNumber)
		{
			if (emotionResults == null || emotionResults.Length < 1)
				return ErrorMessage[0];

			string allEmotionsString = "";

			allEmotionsString += $"Anger: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Anger)}\n";
			allEmotionsString += $"Contempt: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Contempt)}\n";
			allEmotionsString += $"Disgust: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Disgust)}\n";
			allEmotionsString += $"Fear: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Fear)}\n";
			allEmotionsString += $"Happiness: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Happiness)}\n";
			allEmotionsString += $"Neutral: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Neutral)}\n";
			allEmotionsString += $"Sadness: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Sadness)}\n";
			allEmotionsString += $"Surprise: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Surprise)}";

			return allEmotionsString;
		}

		string ConvertFloatToPercentage(float floatToConvert)
		{
			return floatToConvert.ToString("#0.##%");
		}

		async Task<bool> DisplayPopUpAlertAboutEmotion(string playerName)
		{
			var alertMessage = new AlertMessageModel
			{
				Title = _emotionStrings[_emotionNumber],
				Message = playerName + ", " + MakeAFaceAlertMessage + _emotionStringsForAlertMessage[_emotionNumber]
			};
			OnDisplayEmotionBeforeCameraAlert(alertMessage);

			while (!HasUserAcknowledgedPopUp)
			{
				await Task.Delay(5);
			}
			HasUserAcknowledgedPopUp = false;

			return UserResponseToAlert;
		}

		bool DoesStringContainErrorMessage(string stringToCheck)
		{
			foreach (string errorMessage in ErrorMessage)
			{
				if (stringToCheck.Contains(errorMessage))
					return true;
			}

			return false;
		}

		async Task WaitForEventToBeSubscribed(EventHandler eventToWaitFor)
		{
			while (eventToWaitFor == null)
				await Task.Delay(100);
		}

		async Task WaitForAnimationsToFinish(int waitTimeInSeconds)
		{
			await Task.Delay(waitTimeInSeconds);
		}

		void OnDisplayAllEmotionResultsAlert(string emotionResults)
		{
			var handle = DisplayAllEmotionResultsAlert;
			handle?.Invoke(null, new TextEventArgs(emotionResults));
		}

		void OnDisplayNoCameraAvailableAlert()
		{
			var handle = DisplayNoCameraAvailableAlert;
			handle?.Invoke(null, EventArgs.Empty);
		}

		void OnDisplayEmotionBeforeCameraAlert(AlertMessageModel alertMessage)
		{
			var handle = DisplayEmotionBeforeCameraAlert;
			handle?.Invoke(null, new AlertMessageEventArgs(alertMessage));
		}

		void OnRevealPhotoImage1WithAnimation()
		{
			var handle = RevealPhotoImage1WithAnimation;
			handle?.Invoke(null, EventArgs.Empty);
		}

		void OnRevealScoreButton1WithAnimation()
		{
			var handle = RevealScoreButton1WithAnimation;
			handle?.Invoke(null, EventArgs.Empty); ;
		}

		void OnRevealPhotoImage2WithAnimation()
		{
			var handle = RevealPhotoImage2WithAnimation;
			handle?.Invoke(null, EventArgs.Empty);
		}

		void OnRevealScoreButton2WithAnimation()
		{
			var handle = RevealScoreButton2WithAnimation;
			handle?.Invoke(null, EventArgs.Empty);
		}

		void OnDisplayMultipleFacesError()
		{
			var handle = DisplayMultipleFacesDetectedAlert;
			handle?.Invoke(null, EventArgs.Empty);
		}
		#endregion
	}
}

