using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Amazon.Runtime;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

using AsyncAwaitBestPractices;

using Plugin.Media.Abstractions;

using Xamarin.Forms;
using Xamarin.Essentials;

using FaceOff.Shared;
using System.IO;
using Amazon;

namespace FaceOff
{
    static class EmotionService
    {
        #region Constant Fields
        readonly static Lazy<AmazonRekognitionClient> _rekognitionClientHolder =
            new Lazy<AmazonRekognitionClient>(() => new AmazonRekognitionClient(new BasicAWSCredentials(AwsConstants.AccessKey, AwsConstants.AccessSecret), RegionEndpoint.USWest2));

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
        #endregion

        #region Events
        public static event EventHandler MultipleFacesDetectedAlertTriggered
        {
            add => _multipleFacesDetectedAlertTriggeredEventManager.AddEventHandler(value);
            remove => _multipleFacesDetectedAlertTriggeredEventManager.RemoveEventHandler(value);
        }
        #endregion

        #region Properties
        public static Dictionary<ErrorMessageType, string> ErrorMessageDictionary => _errorMessageDictionaryHolder.Value;
        static AmazonRekognitionClient RekognitionClient => _rekognitionClientHolder.Value;
        #endregion

        #region Methods
        public static EmotionType GetRandomEmotionType(EmotionType currentEmotionType)
        {
            var rnd = new Random((int)DateTime.UtcNow.Ticks);
            int randomNumber;

            do
            {
                randomNumber = rnd.Next(0, EmotionConstants.EmotionDictionary.Count);
            } while (randomNumber == (int)currentEmotionType || randomNumber == (int)EmotionType.UNKNOWN);

            return (EmotionType)randomNumber;
        }

        public static async Task<List<Emotion>> GetEmotionResultsFromMediaFile(MediaFile mediaFile)
        {
            Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = true);

            try
            {
                using (var imageStream = mediaFile.GetStream())
                using (var imageMemoryStream = new MemoryStream())
                using (var handle = AnalyticsService.TrackTime(AnalyticsConstants.AnalyzeEmotion))
                {
                    imageStream.CopyTo(imageMemoryStream);

                    var detectFacesRequest = new DetectFacesRequest
                    {
                        Attributes = new List<string> { { "ALL" } },
                        Image = new Amazon.Rekognition.Model.Image { Bytes = imageMemoryStream }
                    };
                    var detectFacesResponse = await RekognitionClient.DetectFacesAsync(detectFacesRequest).ConfigureAwait(false);

                    if (detectFacesResponse.FaceDetails.Count > 1)
                    {
                        OnMultipleFacesDetectedAlertTriggered();
                        throw new Exception("Multiple Faces Detected");
                    }

                    return detectFacesResponse.FaceDetails.SelectMany(x => x.Emotions).ToList();
                }
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = false);
            }
        }

        public static string GetPhotoEmotionScore(List<Emotion> emotionResults, EmotionType currentEmotionName)
        {
            var isInternetConnectionAvilable = Connectivity.NetworkAccess.Equals(NetworkAccess.Internet);

            if (!isInternetConnectionAvilable)
                return ErrorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];

            if (emotionResults is null || emotionResults.Count < 1)
                return ErrorMessageDictionary[ErrorMessageType.NoFaceDetected];

            try
            {
                var rawEmotionScore = emotionResults.First(x => x.Type.Equals(EmotionConstants.EmotionDictionary[currentEmotionName])).Confidence;

                var emotionScoreAsPercentage = rawEmotionScore.ConvertToPercentage();

                return emotionScoreAsPercentage;
            }
            catch (Exception e)
            {
                AnalyticsService.Report(e);
                return ErrorMessageDictionary[ErrorMessageType.GenericError];
            }
        }

        public static string GetStringOfAllPhotoEmotionScores(List<Emotion> emotionResults)
        {
            if (emotionResults is null || emotionResults.Count < 1)
                return ErrorMessageDictionary[ErrorMessageType.GenericError];

            var allEmotionsString = new StringBuilder();

            if (emotionResults.Any(x => x.Type.Equals(EmotionType.UNKNOWN)))
            {
                allEmotionsString.Append($"Unknown: {emotionResults.FirstOrDefault(x => x.Type.Equals(EmotionConstants.EmotionDictionary[EmotionType.UNKNOWN]))?.Confidence.ConvertToPercentage()}");
            }
            else
            {
                allEmotionsString.AppendLine($"Angry: {emotionResults.FirstOrDefault(x => x.Type.Equals(EmotionConstants.EmotionDictionary[EmotionType.ANGRY]))?.Confidence.ConvertToPercentage()}");
                allEmotionsString.AppendLine($"Calm: {emotionResults.FirstOrDefault(x => x.Type.Equals(EmotionConstants.EmotionDictionary[EmotionType.CALM]))?.Confidence.ConvertToPercentage()}");
                allEmotionsString.AppendLine($"Confused: {emotionResults.FirstOrDefault(x => x.Type.Equals(EmotionConstants.EmotionDictionary[EmotionType.CONFUSED]))?.Confidence.ConvertToPercentage()}");
                allEmotionsString.AppendLine($"Disgusted: {emotionResults.FirstOrDefault(x => x.Type.Equals(EmotionConstants.EmotionDictionary[EmotionType.DISGUSTED]))?.Confidence.ConvertToPercentage()}");
                allEmotionsString.AppendLine($"Happy: {emotionResults.FirstOrDefault(x => x.Type.Equals(EmotionConstants.EmotionDictionary[EmotionType.HAPPY]))?.Confidence.ConvertToPercentage()}");
                allEmotionsString.AppendLine($"Sad: {emotionResults.FirstOrDefault(x => x.Type.Equals(EmotionConstants.EmotionDictionary[EmotionType.SAD]))?.Confidence.ConvertToPercentage()}");
                allEmotionsString.Append($"Surprised: {emotionResults.FirstOrDefault(x => x.Type.Equals(EmotionConstants.EmotionDictionary[EmotionType.SURPRISED]))?.Confidence.ConvertToPercentage()}");
            }

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
            _multipleFacesDetectedAlertTriggeredEventManager.HandleEvent(null, EventArgs.Empty, nameof(MultipleFacesDetectedAlertTriggered));
        #endregion
    }
}
