using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Common.Contract;

using Plugin.Media.Abstractions;

using Xamarin;
using Xamarin.Forms;

namespace FaceOff
{
    public static class EmotionService
    {
        #region Constant Fields
        readonly static Lazy<EmotionServiceClient> _emotionClientHolder = new Lazy<EmotionServiceClient>(() => new EmotionServiceClient(CognitiveServicesConstants.EmotionApiKey));

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

        static EmotionServiceClient EmotionClient => _emotionClientHolder.Value;
        #endregion

        #region Methods
        public static EmotionType GetRandomEmotionType(EmotionType currentEmotionType)
        {
            var rnd = new Random();
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

        public static async Task<Emotion[]> GetEmotionResultsFromMediaFile(MediaFile mediaFile, bool disposeMediaFile)
        {
            Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = true);

            try
            {
                using (var handle = Insights.TrackTime(InsightsConstants.AnalyzeEmotion))
                    return await EmotionClient.RecognizeAsync(MediaService.GetPhotoStream(mediaFile, disposeMediaFile));
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = false);
            }
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
                OnMultipleFacesDetectedAlertTriggered();
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
