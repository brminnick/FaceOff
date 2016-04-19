using System.IO;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Microsoft.ProjectOxford.Emotion;

using Xamarin.Forms;
using System;
using Microsoft.ProjectOxford.Emotion.Contract;
using Xamarin;

namespace FaceOff
{
	public class PictureViewModel : BaseViewModel
	{
		#region Constant Fields
		readonly string[] _emotionStrings = { "Anger", "Contempt", "Disgust", "Fear", "Happiness", "Neutral", "Sadness", "Surprise" };
		#endregion

		#region Fields
		ImageSource _photo1ImageSource, _photo2ImageSource;
		string _photo1LabelText, _photo2LabelText;
		bool _isTakeLeftPhotoButtonEnabled = false;
		bool _isTakeRightPhotoButtonEnabled = true;
		string _pageTitle;
		int _emotionNumber;
		bool _isCalculatingPhoto1Score, _isCalculatingPhoto2Score;
		#endregion

		#region Constructors
		public PictureViewModel()
		{
			SetEmotion();

			TakePhoto1ButtonPressed = new Command(async () =>
			{
				Insights.Track(InsightsConstants.PhotoButton1Tapped);

				var imageMediaFile = await GetMediaFileFromCamera("FaceOff", "PhotoImage1");
				if (imageMediaFile == null)
					return;

				IsTakeLeftPhotoButtonEnabled = false;

				Photo1LabelText = "Calculating Score";

				Photo1ImageSource = ImageSource.FromStream(() =>
				{
					return GetPhotoStream(imageMediaFile, false);
				});

				IsCalculatingPhoto1Score = true;
				var emotionArray = await GetEmotionResultsFromMediaFile(imageMediaFile, false);
				Photo1LabelText = $"Score: {GetPhotoEmotionScore(emotionArray, 0)}";
				IsCalculatingPhoto1Score = false;

				imageMediaFile.Dispose();
			});

			TakePhoto2ButtonPressed = new Command(async () =>
			{
				Insights.Track(InsightsConstants.PhotoButton2Tapped);

				var imageMediaFile = await GetMediaFileFromCamera("FaceOff", "PhotoImage2");
				if (imageMediaFile == null)
					return;

				IsTakeRightPhotoButtonEnabled = false;
				IsTakeLeftPhotoButtonEnabled = true;

				Photo2LabelText = "Calculating Score";

				Photo2ImageSource = ImageSource.FromStream(() =>
				{
					return GetPhotoStream(imageMediaFile, false);
				});

				IsCalculatingPhoto2Score = true;
				var emotionArray = await GetEmotionResultsFromMediaFile(imageMediaFile, false);
				Photo2LabelText = $"Score: {GetPhotoEmotionScore(emotionArray, 0)}";
				IsCalculatingPhoto2Score = false;

				imageMediaFile.Dispose();
			});

			ResetButtonPressed = new Command(() =>
			{
				Insights.Track(InsightsConstants.ResetButtonTapped);

				SetEmotion();

				Photo1ImageSource = null;
				Photo2ImageSource = null;
				IsTakeLeftPhotoButtonEnabled = false;
				IsTakeRightPhotoButtonEnabled = true;
				Photo1LabelText = null;
				Photo2LabelText = null;
			});
		}
		#endregion

		#region Properties
		public Command TakePhoto1ButtonPressed { get; protected set; }
		public Command TakePhoto2ButtonPressed { get; protected set; }
		public Command ResetButtonPressed { get; protected set; }
		public Command SubmitButtonPressed { get; protected set; }

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
			get { return !IsTakeLeftPhotoButtonEnabled; }
		}
		public bool IsPhotoImage2Enabled
		{
			get { return !IsTakeRightPhotoButtonEnabled; }
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
				OnPropertyChanged("IsPhotoImage1Enabled");
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
				OnPropertyChanged("IsPhotoImage2Enabled");
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

		public string Photo1LabelText
		{
			get
			{
				return _photo1LabelText;
			}
			set
			{
				_photo1LabelText = value;
				OnPropertyChanged("Photo1LabelText");
			}
		}

		public string Photo2LabelText
		{
			get
			{
				return _photo2LabelText;
			}
			set
			{
				_photo2LabelText = value;
				OnPropertyChanged("Photo2LabelText");
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
				OnPropertyChanged("IsResetButtonEnabled");
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
				OnPropertyChanged("IsResetButtonEnabled");
			}
		}

		public bool IsResetButtonEnabled
		{
			get 
			{
				return !(IsCalculatingPhoto2Score || IsCalculatingPhoto1Score);
			}
		}
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
				//Todo Handle case when no camera is available
				return null;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = directory,
				Name = filename
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

				using (var handle = Insights.TrackTime(InsightsConstants.CalculateEmotion))
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
			var errorMessage = "Error, No Face Detected";
			float rawEmotionScore;

			if (emotionResults == null || emotionResults.Length < 1)
				return errorMessage;

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
						return errorMessage;
				}

				var emotionScoreAsPercentage = ConvertFloatToPercentage(rawEmotionScore);
#if DEBUG
				emotionScoreAsPercentage += $"\n{GetPhotoEmotionScoreDebug(emotionResults, emotionResultNumber)}";
#endif
				return emotionScoreAsPercentage;
			}
			catch (Exception e)
			{
				Insights.Report(e);
				return errorMessage;
			}
		}

		string GetPhotoEmotionScoreDebug(Emotion[] emotionResults, int emotionResultNumber)
		{
			string allEmotionsString = "";

			allEmotionsString += $"Anger: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Anger)}\n";
			allEmotionsString += $"Disgust: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Disgust)}\n";
			allEmotionsString += $"Fear: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Fear)}\n";
			allEmotionsString += $"Happiness: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Happiness)}\n";
			allEmotionsString += $"Neutral: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Neutral)}\n";
			allEmotionsString += $"Sadness: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Sadness)}\n";
			allEmotionsString += $"Surprise: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Surprise)}\n";

			return allEmotionsString;
		}


		string ConvertFloatToPercentage(float floatToConvert)
		{
			return floatToConvert.ToString("#0.##%");

		}

		#endregion
	}
}

