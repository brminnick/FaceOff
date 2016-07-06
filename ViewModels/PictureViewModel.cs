using System;
using System.IO;
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

		const string ErrorMessage = "No Face Detected";
		const string MakeAFaceAlertMessage = "take a selfie looking ";
		const string CalculatingScore = "Analyzing";
		#endregion

		#region Fields
		ImageSource _photo1ImageSource, _photo2ImageSource;
		string _scoreButton1Text, _scoreButton2Text;
		bool _isTakeLeftPhotoButtonEnabled = true;
		bool _isTakeLeftPhotoButtonVisible = true;
		bool _isTakeRightPhotoButtonEnabled = true;
		bool _isTakeRightPhotoButtonVisible = true;
		bool _isResetButtonEnabled;
		string _pageTitle;
		int _emotionNumber;
		bool _isCalculatingPhoto1Score, _isCalculatingPhoto2Score;
		bool _isScore1ButtonEnabled, _isScore2ButtonEnabled, _isScore1ButtonVisable, _isScore2ButtonVisable;
		bool _isPhotoImage1Enabled, _isPhotoImage2Enabled;
		string _photo1Results, _photo2Results;

		public event EventHandler RotateImage;
		public event EventHandler DisplayEmtionBeforeCameraAlert;
		public event EventHandler DisplayAllEmotionResultsAlert;
		public event EventHandler DisplayNoCameraAvailableAlert;
		public event EventHandler RevealScoreButton1WithAnimation;
		public event EventHandler RevealScoreButton2WithAnimation;
		public event EventHandler RevealPhotoImage1WithAnimation;
		public event EventHandler RevealPhotoImage2WithAnimation;
		#endregion

		#region Constructors
		public PictureViewModel()
		{
			IsResetButtonEnabled = false;

			SetEmotion();

			TakePhoto1ButtonPressed = new Command(async () =>
			{
				IsTakeRightPhotoButtonEnabled = false;
				IsScore1ButtonEnabled = false;

				Insights.Track(InsightsConstants.PhotoButton1Tapped);

				if (!(await DisplayPopUpAlertAboutEmotion(1)))
					return;

				var imageMediaFile = await GetMediaFileFromCamera("FaceOff", "PhotoImage1");

				if (imageMediaFile == null)
					return;

				RevealPhotoImage1WithAnimation(this, new EventArgs());

				Insights.Track (InsightsConstants.PhotoTaken);

				IsTakeLeftPhotoButtonEnabled = false;
				IsTakeLeftPhotoButtonVisible = false;

				ScoreButton1Text = CalculatingScore;


				Photo1ImageSource = ImageSource.FromStream(() =>
				{
					return GetPhotoStream(imageMediaFile, false);
				});

				IsCalculatingPhoto1Score = true;
				IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

				//Yeild to the UI Thread to ensure the PhotoImageAnimation has completed
				await Task.Delay((int)(AnimationConstants.PhotoImageAninmationTime * 2.5));
				
				RevealScoreButton1WithAnimation(this, new EventArgs());

				var emotionArray = await GetEmotionResultsFromMediaFile(imageMediaFile, false);

				var emotionScore = GetPhotoEmotionScore(emotionArray, 0);
				if (emotionScore.Contains(ErrorMessage))
				{
					Insights.Track(InsightsConstants.NoFaceDetected);
					ScoreButton1Text = ErrorMessage;
				}
				else
					ScoreButton1Text = $"Score: {emotionScore}";

				_photo1Results = GetStringOfAllPhotoEmotionScores(emotionArray, 0);

				IsCalculatingPhoto1Score = false;
				IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

				imageMediaFile.Dispose();

				//Yeild to the UI Thread to ensure the ScoreButtonAnimation has completed
				await Task.Delay((int)(AnimationConstants.ScoreButonAninmationTime * 2.5));
			});

			TakePhoto2ButtonPressed = new Command(async () =>
			{
				IsTakeLeftPhotoButtonEnabled = false;
				IsScore2ButtonEnabled = false;

				Insights.Track(InsightsConstants.PhotoButton2Tapped);

				if (!(await DisplayPopUpAlertAboutEmotion(2)))
					return;

				var imageMediaFile = await GetMediaFileFromCamera("FaceOff", "PhotoImage2");
				if (imageMediaFile == null)
					return;

				RevealPhotoImage2WithAnimation(this, new EventArgs());

				IsTakeRightPhotoButtonEnabled = false;
				IsTakeRightPhotoButtonVisible = false;

				ScoreButton2Text = CalculatingScore;

				Photo2ImageSource = ImageSource.FromStream(() =>
				{
					return GetPhotoStream(imageMediaFile, false);
				});

				IsCalculatingPhoto2Score = true;
				IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

				//Yeild to the UI Thread to ensure the PhotoImageAnimation has completed
				await Task.Delay((int)(AnimationConstants.PhotoImageAninmationTime * 2.5));
				
				RevealScoreButton2WithAnimation(this, new EventArgs());

				var emotionArray = await GetEmotionResultsFromMediaFile(imageMediaFile, false);

				var emotionScore = GetPhotoEmotionScore(emotionArray, 0);
				if (emotionScore.Contains(ErrorMessage))
				{
					Insights.Track(InsightsConstants.NoFaceDetected);
					ScoreButton2Text = ErrorMessage;
				}
				else
					ScoreButton2Text = $"Score: {emotionScore}";

				_photo2Results = GetStringOfAllPhotoEmotionScores(emotionArray, 0);

				IsCalculatingPhoto2Score = false;
				IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

				imageMediaFile.Dispose();

				//Yeild to the UI Thread to ensure the ScoreButtonAnimation has completed
				await Task.Delay((int)(AnimationConstants.ScoreButonAninmationTime * 2.5));
			});

			ResetButtonPressed = new Command(() =>
			{
				Insights.Track(InsightsConstants.ResetButtonTapped);

				SetEmotion();

				Photo1ImageSource = null;
				Photo2ImageSource = null;

				IsTakeLeftPhotoButtonEnabled = true;
				IsTakeLeftPhotoButtonVisible = true;

				IsTakeRightPhotoButtonEnabled = true;
				IsTakeRightPhotoButtonVisible = true;

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
			});

			Photo1ScoreButtonPressed = new Command(() =>
			{
				Insights.Track(InsightsConstants.ResultsButton1Tapped);
				DisplayAllEmotionResultsAlert(_photo1Results, new EventArgs());
			});

			Photo2ScoreButtonPressed = new Command(() =>
			{
				Insights.Track(InsightsConstants.ResultsButton2Tapped);
				DisplayAllEmotionResultsAlert(_photo2Results, new EventArgs());
			});
		}
		#endregion

		#region Properties
		public Command TakePhoto1ButtonPressed { get; protected set; }
		public Command TakePhoto2ButtonPressed { get; protected set; }
		public Command ResetButtonPressed { get; protected set; }
		public Command SubmitButtonPressed { get; protected set; }
		public Command Photo1ScoreButtonPressed { get; protected set; }
		public Command Photo2ScoreButtonPressed { get; protected set; }

		public ImageSource Photo1ImageSource
		{
			get
			{
				return _photo1ImageSource;
			}
			set
			{
				_photo1ImageSource = value;
				OnPropertyChanged("Photo1ImageSource");
			}
		}

		public ImageSource Photo2ImageSource
		{
			get
			{
				return _photo2ImageSource;
			}
			set
			{
				_photo2ImageSource = value;
				OnPropertyChanged("Photo2ImageSource");
			}
		}

		public bool IsPhotoImage1Enabled
		{
			get 
			{ 
				return _isPhotoImage1Enabled; 
			}
			set
			{
				_isPhotoImage1Enabled = value;
				OnPropertyChanged("IsPhotoImage1Enabled");
			}
		}

		public bool IsPhotoImage2Enabled
		{
			get
			{
				return _isPhotoImage2Enabled;
			}
			set
			{
				_isPhotoImage2Enabled = value;
				OnPropertyChanged("IsPhotoImage2Enabled");
			}
		}

		public bool IsTakeLeftPhotoButtonEnabled
		{
			get
			{
				return _isTakeLeftPhotoButtonEnabled;
			}
			set
			{
				_isTakeLeftPhotoButtonEnabled = value;
				OnPropertyChanged("IsTakeLeftPhotoButtonEnabled");
			}
		}

		public bool IsTakeLeftPhotoButtonVisible
		{
			get
			{
				return _isTakeLeftPhotoButtonVisible;
			}
			set
			{
				_isTakeLeftPhotoButtonVisible = value;
				OnPropertyChanged("IsTakeLeftPhotoButtonVisible");
			}
		}

		public bool IsTakeRightPhotoButtonEnabled
		{
			get
			{
				return _isTakeRightPhotoButtonEnabled;
			}
			set
			{
				_isTakeRightPhotoButtonEnabled = value;
				OnPropertyChanged("IsTakeRightPhotoButtonEnabled");
			}
		}

		public bool IsTakeRightPhotoButtonVisible
		{
			get
			{
				return _isTakeRightPhotoButtonVisible;
			}
			set
			{
				_isTakeRightPhotoButtonVisible = value;
				OnPropertyChanged("IsTakeRightPhotoButtonVisible");
			}
		}

		public string PageTitle
		{
			get
			{
				return _pageTitle;
			}
			set
			{
				_pageTitle = value;
				OnPropertyChanged("PageTitle");
			}
		}

		public string ScoreButton1Text
		{
			get
			{
				return _scoreButton1Text;
			}
			set
			{
				_scoreButton1Text = value;
				OnPropertyChanged("ScoreButton1Text");
			}
		}

		public string ScoreButton2Text
		{
			get
			{
				return _scoreButton2Text;
			}
			set
			{
				_scoreButton2Text = value;
				OnPropertyChanged("ScoreButton2Text");
			}
		}

		public bool IsCalculatingPhoto1Score
		{
			get
			{
				return _isCalculatingPhoto1Score;
			}
			set
			{
				_isCalculatingPhoto1Score = value;
				OnPropertyChanged("IsCalculatingPhoto1Score");
			}
		}

		public bool IsCalculatingPhoto2Score
		{
			get
			{
				return _isCalculatingPhoto2Score;
			}
			set
			{
				_isCalculatingPhoto2Score = value;
				OnPropertyChanged("IsCalculatingPhoto2Score");
			}
		}

		public bool IsResetButtonEnabled
		{
			get
			{
				return _isResetButtonEnabled;
			}
			set
			{
				_isResetButtonEnabled = value;
				OnPropertyChanged("IsResetButtonEnabled");
			}
		}

		public bool IsScore1ButtonEnabled
		{
			get
			{
				return _isScore1ButtonEnabled;
			}
			set
			{
				_isScore1ButtonEnabled = value;
				OnPropertyChanged("IsScore1ButtonEnabled");
			}
		}

		public bool IsScore2ButtonEnabled
		{
			get
			{
				return _isScore2ButtonEnabled;
			}
			set
			{
				_isScore2ButtonEnabled = value;
				OnPropertyChanged("IsScore2ButtonEnabled");
			}
		}

		public bool IsScore1ButtonVisable
		{
			get
			{
				return _isScore1ButtonVisable;
			}
			set
			{
				_isScore1ButtonVisable = value;
				OnPropertyChanged("IsScore1ButtonVisable");
			}
		}

		public bool IsScore2ButtonVisable
		{
			get
			{
				return _isScore2ButtonVisable;
			}
			set
			{
				_isScore2ButtonVisable = value;
				OnPropertyChanged("IsScore2ButtonVisable");
			}
		}

		public bool UserHasAcknowledgedPopUp { get; set; } = false;
		public bool UserResponseToAlert { get; set; }

		#endregion

		#region Methods
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
				DisplayNoCameraAvailableAlert(this, new EventArgs());
				return null;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = directory,
				Name = filename,
				DefaultCamera = CameraDevice.Front
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

		void SetEmotion()
		{
			_emotionNumber = GetRandomNumberForEmotion();
			SetPageTitle(_emotionNumber);
		}

		string GetPhotoEmotionScore(Emotion[] emotionResults, int emotionResultNumber)
		{
			float rawEmotionScore;

			if (emotionResults == null || emotionResults.Length < 1)
				return ErrorMessage;

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
						return ErrorMessage;
				}

				var emotionScoreAsPercentage = ConvertFloatToPercentage(rawEmotionScore);

				return emotionScoreAsPercentage;
			}
			catch (Exception e)
			{
				Insights.Report(e);
				return ErrorMessage;
			}
		}

		string GetStringOfAllPhotoEmotionScores(Emotion[] emotionResults, int emotionResultNumber)
		{
			if (emotionResults == null || emotionResults.Length < 1)
				return ErrorMessage;

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

		void RotateImageToVertical(int degreesOfClockwiseRotation, int imageNumberToRotate)
		{
			var parameters = new RotatableImageParameters
			{
				DegreesOfClockwiseRotation = degreesOfClockwiseRotation,
				ImageNumberToRotate = imageNumberToRotate

			};
			RotateImage(parameters, new EventArgs());
		}

		async Task<bool> DisplayPopUpAlertAboutEmotion(int playerNumber)
		{
			var alertMessage = new AlertMessage
			{
				Title = _emotionStrings[_emotionNumber],
				Message = "Player " + playerNumber + ", " + MakeAFaceAlertMessage + _emotionStringsForAlertMessage[_emotionNumber]
			};
			DisplayEmtionBeforeCameraAlert(alertMessage, new EventArgs());

			while (!UserHasAcknowledgedPopUp)
			{
				await Task.Delay(5);
			}
			UserHasAcknowledgedPopUp = false;

			return UserResponseToAlert;
		}

		void DisplayPopUpAlertShowingAllEmotionResults(string emotionResults)
		{
			DisplayEmtionBeforeCameraAlert(emotionResults, new EventArgs());
		}

		#endregion
	}
}

