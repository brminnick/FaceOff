using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using FaceOff.Shared;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media.Abstractions;
using Polly;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FaceOff
{
    static class EmotionService
    {
        readonly static Lazy<FaceClient> _faceApiClientHolder =
            new Lazy<FaceClient>(() => new FaceClient(new ApiKeyServiceClientCredentials(CognitiveServicesConstants.FaceApiKey), new HttpClient(), false) { Endpoint = CognitiveServicesConstants.FaceApiBaseUrl });

        readonly static Lazy<Dictionary<ErrorMessageType, string>> _errorMessageDictionaryHolder = new Lazy<Dictionary<ErrorMessageType, string>>(() =>
            new Dictionary<ErrorMessageType, string>{
                { ErrorMessageType.ConnectionToCognitiveServicesFailed, "Connection Failed" },
                { ErrorMessageType.InvalidAPIKey, "Invalid API Key"},
                { ErrorMessageType.NoFaceDetected, "No Face Detected" },
                { ErrorMessageType.MultipleFacesDetected, "Multiple Faces Detected" },
                { ErrorMessageType.GenericError, "Error" },
                { ErrorMessageType.DeviceOffline, "Device is Offline"}
            });

        readonly static WeakEventManager _multipleFacesDetectedAlertTriggeredEventManager = new WeakEventManager();

        public static event EventHandler MultipleFacesDetectedAlertTriggered
        {
            add => _multipleFacesDetectedAlertTriggeredEventManager.AddEventHandler(value);
            remove => _multipleFacesDetectedAlertTriggeredEventManager.RemoveEventHandler(value);
        }

        public static Dictionary<ErrorMessageType, string> ErrorMessageDictionary => _errorMessageDictionaryHolder.Value;
        static FaceClient FaceApiClient => _faceApiClientHolder.Value;

        public static EmotionType GetRandomEmotionType(EmotionType currentEmotionType)
        {
            var rnd = new Random((int)DateTime.UtcNow.Ticks);
            int randomNumber;

            do
            {
                randomNumber = rnd.Next(0, EmotionConstants.EmotionDictionary.Count);
            } while (randomNumber == (int)currentEmotionType);

            return (EmotionType)randomNumber;
        }

        public static async Task<List<Emotion>> GetEmotionResultsFromMediaFile(MediaFile? mediaFile)
        {
            await MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.IsBusy = true).ConfigureAwait(false);

            try
            {
                using var handle = AnalyticsService.TrackTime(AnalyticsConstants.AnalyzeEmotion);
                var faceApiResponseList = await AttemptAndRetry(() => FaceApiClient.Face.DetectWithStreamAsync(mediaFile?.GetStream(),
                                                                            returnFaceAttributes: new List<FaceAttributeType?> { { FaceAttributeType.Emotion } })).ConfigureAwait(false);

                return faceApiResponseList.Select(x => x.FaceAttributes.Emotion).ToList();
            }
            finally
            {
                await MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.IsBusy = false).ConfigureAwait(false);
            }
        }

        public static string GetPhotoEmotionScore(IList<Emotion> emotionResults, int emotionResultNumber, EmotionType currentEmotionType)
        {
            var isInternetConnectionAvilable = Connectivity.NetworkAccess.Equals(NetworkAccess.Internet);

            if (!isInternetConnectionAvilable)
                return ErrorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];

            if (emotionResults is null || emotionResults.Count < 1)
                return ErrorMessageDictionary[ErrorMessageType.NoFaceDetected];

            if (emotionResults.Count > 1)
            {
                OnMultipleFacesDetectedAlertTriggered();
                return ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected];
            }

            try
            {
                double rawEmotionScore = currentEmotionType switch
                {
                    EmotionType.Anger => emotionResults[emotionResultNumber].Anger,
                    EmotionType.Contempt => emotionResults[emotionResultNumber].Contempt,
                    EmotionType.Disgust => rawEmotionScore = emotionResults[emotionResultNumber].Disgust,
                    EmotionType.Fear => emotionResults[emotionResultNumber].Fear,
                    EmotionType.Happiness => emotionResults[emotionResultNumber].Happiness,
                    EmotionType.Neutral => emotionResults[emotionResultNumber].Neutral,
                    EmotionType.Sadness => emotionResults[emotionResultNumber].Sadness,
                    EmotionType.Surprise => emotionResults[emotionResultNumber].Surprise,
                    _ => throw new NotSupportedException($"{nameof(EmotionType)} Not Supported: {currentEmotionType}")
                };

                var emotionScoreAsPercentage = rawEmotionScore.ConvertToPercentage();

                return emotionScoreAsPercentage;
            }
            catch (Exception e)
            {
                AnalyticsService.Report(e);
                return ErrorMessageDictionary[ErrorMessageType.GenericError];
            }
        }

        public static string GetStringOfAllPhotoEmotionScores(IList<Emotion> emotionResults, int emotionResultNumber)
        {
            if (emotionResults is null || emotionResults.Count < 1)
                return ErrorMessageDictionary[ErrorMessageType.GenericError];

            var allEmotionsString = new StringBuilder();

            allEmotionsString.AppendLine($"Anger: {emotionResults[emotionResultNumber].Anger.ConvertToPercentage()}");
            allEmotionsString.AppendLine($"Contempt: {emotionResults[emotionResultNumber].Contempt.ConvertToPercentage()}");
            allEmotionsString.AppendLine($"Disgust: {emotionResults[emotionResultNumber].Disgust.ConvertToPercentage()}");
            allEmotionsString.AppendLine($"Fear: {emotionResults[emotionResultNumber].Fear.ConvertToPercentage()}");
            allEmotionsString.AppendLine($"Happiness: {emotionResults[emotionResultNumber].Happiness.ConvertToPercentage()}");
            allEmotionsString.AppendLine($"Neutral: {emotionResults[emotionResultNumber].Neutral.ConvertToPercentage()}");
            allEmotionsString.AppendLine($"Sadness: {emotionResults[emotionResultNumber].Sadness.ConvertToPercentage()}");
            allEmotionsString.Append($"Surprise: {emotionResults[emotionResultNumber].Surprise.ConvertToPercentage()}");

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

        static Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 3)
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);

            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }

        static void OnMultipleFacesDetectedAlertTriggered() =>
            _multipleFacesDetectedAlertTriggeredEventManager.RaiseEvent(null, EventArgs.Empty, nameof(MultipleFacesDetectedAlertTriggered));
    }
}
