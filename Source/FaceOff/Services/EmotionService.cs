using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Plugin.Media.Abstractions;

namespace FaceOff
{
	abstract class EmotionService : BaseHttpClientService
	{
		#region Constant Fields
		readonly static Lazy<Dictionary<ErrorMessageType, string>> _errorMessageDictionaryHolder = new Lazy<Dictionary<ErrorMessageType, string>>(() =>
			new Dictionary<ErrorMessageType, string>{
				{ ErrorMessageType.ConnectionToCognitiveServicesFailed, "Connection Failed" },
				{ ErrorMessageType.InvalidAPIKey, "Invalid API Key"},
				{ ErrorMessageType.NoFaceDetected, "No Face Detected" },
				{ ErrorMessageType.MultipleFacesDetected, "Multiple Faces Detected" },
				{ ErrorMessageType.GenericError, "Error" },
				{ ErrorMessageType.DeviceOffline, "Device is Offline"}
			});

		readonly static Lazy<Dictionary<EmotionType, string>> _emotionDictionaryHolder = new Lazy<Dictionary<EmotionType, string>>(() =>
			new Dictionary<EmotionType, string>
			{
				{ EmotionType.Anger, "Anger" },
				{ EmotionType.Contempt, "Contempt" },
				{ EmotionType.Disgust, "Disgust"},
				{ EmotionType.Fear, "Fear" },
				{ EmotionType.Happiness, "Happiness" },
				{ EmotionType.Neutral, "Neutral" },
				{ EmotionType.Sadness, "Sadness" },
				{ EmotionType.Surprise, "Surprise" }
			});
		#endregion

		#region Events
		public static event EventHandler MultipleFacesDetectedAlertTriggered;
		#endregion

		#region Properties
		public static Dictionary<ErrorMessageType, string> ErrorMessageDictionary => _errorMessageDictionaryHolder.Value;
		public static Dictionary<EmotionType, string> EmotionDictionary => _emotionDictionaryHolder.Value;
		#endregion

		#region Methods
		public static EmotionType GetRandomEmotionType(EmotionType currentEmotionType)
		{
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			int randomNumber;

			do
			{
				randomNumber = rnd.Next(0, EmotionDictionary.Count);
			} while (randomNumber == (int)currentEmotionType);

			switch (randomNumber)
			{
				case 0:
					return EmotionType.Anger;
				case 1:
					return EmotionType.Contempt;
				case 2:
					return EmotionType.Disgust;
				case 3:
					return EmotionType.Fear;
				case 4:
					return EmotionType.Happiness;
				case 5:
					return EmotionType.Neutral;
				case 6:
					return EmotionType.Sadness;
				case 7:
					return EmotionType.Surprise;
				default:
					throw new NotSupportedException("Invalid Emotion Type");
			}
		}

		public static async Task<List<Emotion>> GetEmotionResultsFromMediaFile(MediaFile mediaFile, bool disposeMediaFile)
		{
			using (var handle = AnalyticsHelpers.TrackTime(AnalyticsConstants.AnalyzeEmotion))
			{
				var faceApiResponseList = await PostObjectToAPI<List<FaceApiModel>, Stream>($"{CognitiveServicesConstants.FaceApiUrl}/detect?returnFaceAttributes=emotion", MediaService.GetPhotoStream(mediaFile, disposeMediaFile)).ConfigureAwait(false);
				return faceApiResponseList.Select(x => x.FaceAttributes.Emotion).ToList();
			}
		}

		public static async Task<string> GetPhotoEmotionScore(List<Emotion> emotionResults, int emotionResultNumber, EmotionType currentEmotionType)
		{
			double rawEmotionScore;

			var isInternetConnectionAvilable = await ConnectionService.IsInternetConnectionAvailable().ConfigureAwait(false);

			if (!isInternetConnectionAvilable)
				return ErrorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];

			if (emotionResults == null || emotionResults.Count < 1)
				return ErrorMessageDictionary[ErrorMessageType.NoFaceDetected];

			if (emotionResults.Count > 1)
			{
				OnMultipleFacesDetectedAlertTriggered();
				return ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected];
			}

			try
			{
				switch (currentEmotionType)
				{
					case EmotionType.Anger:
						rawEmotionScore = emotionResults[emotionResultNumber].Anger;
						break;
					case EmotionType.Contempt:
						rawEmotionScore = emotionResults[emotionResultNumber].Contempt;
						break;
					case EmotionType.Disgust:
						rawEmotionScore = emotionResults[emotionResultNumber].Disgust;
						break;
					case EmotionType.Fear:
						rawEmotionScore = emotionResults[emotionResultNumber].Fear;
						break;
					case EmotionType.Happiness:
						rawEmotionScore = emotionResults[emotionResultNumber].Happiness;
						break;
					case EmotionType.Neutral:
						rawEmotionScore = emotionResults[emotionResultNumber].Neutral;
						break;
					case EmotionType.Sadness:
						rawEmotionScore = emotionResults[emotionResultNumber].Sadness;
						break;
					case EmotionType.Surprise:
						rawEmotionScore = emotionResults[emotionResultNumber].Surprise;
						break;
					default:
						return ErrorMessageDictionary[ErrorMessageType.GenericError];
				}

				var emotionScoreAsPercentage = ConversionService.ConvertDoubleToPercentage(rawEmotionScore);

				return emotionScoreAsPercentage;
			}
			catch (Exception e)
			{
				AnalyticsHelpers.Report(e);
				return ErrorMessageDictionary[ErrorMessageType.GenericError];
			}
		}

		public static string GetStringOfAllPhotoEmotionScores(List<Emotion> emotionResults, int emotionResultNumber)
		{
			if (emotionResults == null || emotionResults.Count < 1)
				return ErrorMessageDictionary[ErrorMessageType.GenericError];

			var allEmotionsString = new StringBuilder();

			allEmotionsString.AppendLine($"Anger: {ConversionService.ConvertDoubleToPercentage(emotionResults[emotionResultNumber].Anger)}");
			allEmotionsString.AppendLine($"Contempt: {ConversionService.ConvertDoubleToPercentage(emotionResults[emotionResultNumber].Contempt)}");
			allEmotionsString.AppendLine($"Disgust: {ConversionService.ConvertDoubleToPercentage(emotionResults[emotionResultNumber].Disgust)}");
			allEmotionsString.AppendLine($"Fear: {ConversionService.ConvertDoubleToPercentage(emotionResults[emotionResultNumber].Fear)}");
			allEmotionsString.AppendLine($"Happiness: {ConversionService.ConvertDoubleToPercentage(emotionResults[emotionResultNumber].Happiness)}");
			allEmotionsString.AppendLine($"Neutral: {ConversionService.ConvertDoubleToPercentage(emotionResults[emotionResultNumber].Neutral)}");
			allEmotionsString.AppendLine($"Sadness: {ConversionService.ConvertDoubleToPercentage(emotionResults[emotionResultNumber].Sadness)}");
			allEmotionsString.Append($"Surprise: {ConversionService.ConvertDoubleToPercentage(emotionResults[emotionResultNumber].Surprise)}");

			return allEmotionsString.ToString();
		}

		public static bool DoesStringContainErrorMessage(string stringToCheck)
		{
			foreach (KeyValuePair<ErrorMessageType, string> errorMessageDictionaryEntry in ErrorMessageDictionary)
			{
				if (stringToCheck.Contains(errorMessageDictionaryEntry.Value))
					return true;
			}

			return false;
		}

		static void OnMultipleFacesDetectedAlertTriggered() =>
			MultipleFacesDetectedAlertTriggered?.Invoke(null, EventArgs.Empty);
		#endregion
	}
}
