using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

using Plugin.Media.Abstractions;

using Xamarin;

namespace FaceOff
{
    public static class EmotionService
    {
        #region Fields
        readonly static Lazy<EmotionServiceClient> _emotionClient = new Lazy<EmotionServiceClient>(() => new EmotionServiceClient(CognitiveServicesConstants.EmotionApiKey));
        readonly static Lazy<Dictionary<ErrorMessageType, string>> _errorMessageDictionary = new Lazy<Dictionary<ErrorMessageType, string>>(() =>
            new Dictionary<ErrorMessageType, string>{
                { ErrorMessageType.ConnectionToCognitiveServicesFailed, "Connection Failed" },
                { ErrorMessageType.InvalidAPIKey, "Invalid API Key"},
                { ErrorMessageType.NoFaceDetected, "No Face Detected" },
                { ErrorMessageType.MultipleFacesDetected, "Multiple Faces Detected" },
                { ErrorMessageType.GenericError, "Error" }
            });
        #endregion

        #region Events
        public static event EventHandler DisplayMultipleFacesDetectedAlert;
        #endregion

        #region Properties
		public static Dictionary<ErrorMessageType, string> ErrorMessageDictionary => _errorMessageDictionary.Value;
        static EmotionServiceClient EmotionClient => _emotionClient.Value;
        #endregion

        #region Methods
        public static async Task<Emotion[]> GetEmotionResultsFromMediaFile(MediaFile mediaFile, bool disposeMediaFile)
        {
            using (var handle = Insights.TrackTime(InsightsConstants.AnalyzeEmotion))
                return await EmotionClient.RecognizeAsync(MediaService.GetPhotoStream(mediaFile, disposeMediaFile)).ConfigureAwait(false);
        }

        public static async Task<string> GetPhotoEmotionScore(Emotion[] emotionResults, int emotionResultNumber, EmotionType currentEmotionType)
        {
            float rawEmotionScore;

            var isInternetConnectionAvilable = await ConnectionService.IsInternetConnectionAvailable().ConfigureAwait(false);

            if (!isInternetConnectionAvilable)
                return ErrorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];

            if (emotionResults == null || emotionResults.Length < 1)
                return ErrorMessageDictionary[ErrorMessageType.NoFaceDetected];

            if (emotionResults.Length > 1)
            {
                OnDisplayMultipleFacesError();
                return ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected];
            }

            try
            {
                switch (currentEmotionType)
                {
                    case EmotionType.Anger:
                        rawEmotionScore = emotionResults[emotionResultNumber].Scores.Anger;
                        break;
                    case EmotionType.Contempt:
                        rawEmotionScore = emotionResults[emotionResultNumber].Scores.Contempt;
                        break;
                    case EmotionType.Disgust:
                        rawEmotionScore = emotionResults[emotionResultNumber].Scores.Disgust;
                        break;
                    case EmotionType.Fear:
                        rawEmotionScore = emotionResults[emotionResultNumber].Scores.Fear;
                        break;
                    case EmotionType.Happiness:
                        rawEmotionScore = emotionResults[emotionResultNumber].Scores.Happiness;
                        break;
                    case EmotionType.Neutral:
                        rawEmotionScore = emotionResults[emotionResultNumber].Scores.Neutral;
                        break;
                    case EmotionType.Sadness:
                        rawEmotionScore = emotionResults[emotionResultNumber].Scores.Sadness;
                        break;
                    case EmotionType.Surprise:
                        rawEmotionScore = emotionResults[emotionResultNumber].Scores.Surprise;
                        break;
                    default:
                        return ErrorMessageDictionary[ErrorMessageType.GenericError];
                }

                var emotionScoreAsPercentage = ConversionService.ConvertFloatToPercentage(rawEmotionScore);

                return emotionScoreAsPercentage;
            }
            catch (Exception e)
            {
                Insights.Report(e);
                return ErrorMessageDictionary[ErrorMessageType.GenericError];
            }
        }

        public static string GetStringOfAllPhotoEmotionScores(Emotion[] emotionResults, int emotionResultNumber)
        {
            if (emotionResults == null || emotionResults.Length < 1)
                return ErrorMessageDictionary[ErrorMessageType.GenericError];

            var allEmotionsString = new StringBuilder();

            allEmotionsString.AppendLine($"Anger: {ConversionService.ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Anger)}");
            allEmotionsString.AppendLine($"Contempt: {ConversionService.ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Contempt)}");
            allEmotionsString.AppendLine($"Disgust: {ConversionService.ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Disgust)}");
            allEmotionsString.AppendLine($"Fear: {ConversionService.ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Fear)}");
            allEmotionsString.AppendLine($"Happiness: {ConversionService.ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Happiness)}");
            allEmotionsString.AppendLine($"Neutral: {ConversionService.ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Neutral)}");
            allEmotionsString.AppendLine($"Sadness: {ConversionService.ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Sadness)}");
            allEmotionsString.Append($"Surprise: {ConversionService.ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Surprise)}");

            return allEmotionsString.ToString();
        }

        public static bool DoesStringContainErrorMessage(string stringToCheck)
        {
            foreach (KeyValuePair<ErrorMessageType, string> errorMessageDictionaryEntry in EmotionService.ErrorMessageDictionary)
            {
                if (stringToCheck.Contains(errorMessageDictionaryEntry.Value))
                    return true;
            }

            return false;
        }

        static void OnDisplayMultipleFacesError() =>
            DisplayMultipleFacesDetectedAlert?.Invoke(null, EventArgs.Empty);
        #endregion
    }
}
