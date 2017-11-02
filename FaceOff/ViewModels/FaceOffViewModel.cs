using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Plugin.Media;
using Plugin.Connectivity;
using Plugin.Media.Abstractions;

using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

using Xamarin;
using Xamarin.Forms;

namespace FaceOff
{
    public class FaceOffViewModel : BaseViewModel
    {
        #region Constant Fields
        readonly string[] _emotionStringsForAlertMessage = { "angry", "disrespectful", "disgusted", "scared", "happy", "blank", "sad", "surprised" };

        const string _makeAFaceAlertMessage = "take a selfie looking ";
        const string _calculatingScoreMessage = "Analyzing";

        const string _playerNumberNotImplentedExceptionText = "Player Number Not Implemented";

        readonly Dictionary<ErrorMessageType, string> _errorMessageDictionary = new Dictionary<ErrorMessageType, string>
        {
            { ErrorMessageType.ConnectionToCognitiveServicesFailed, "Connection Failed" },
            { ErrorMessageType.InvalidAPIKey, "Invalid API Key"},
            { ErrorMessageType.NoFaceDetected, "No Face Detected" },
            { ErrorMessageType.MultipleFacesDetected, "Multiple Faces Detected" },
            { ErrorMessageType.GenericError, "Error" }
        };

        readonly Dictionary<EmotionType, string> _emotionDictionary = new Dictionary<EmotionType, string>
        {
            { EmotionType.Anger, "Anger" },
            { EmotionType.Contempt, "Contempt" },
            { EmotionType.Disgust, "Disgust"},
            { EmotionType.Fear, "Fear" },
            { EmotionType.Happiness, "Happiness" },
            { EmotionType.Neutral, "Neutral" },
            { EmotionType.Sadness, "Sadness" },
            { EmotionType.Surprise, "Surprise" }
        };
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
        bool _isCalculatingPhoto1Score, _isCalculatingPhoto2Score;
        bool _isScore1ButtonEnabled, _isScore2ButtonEnabled, _isScore1ButtonVisable, _isScore2ButtonVisable;
        bool _isPhotoImage1Enabled, _isPhotoImage2Enabled;
        string _photo1Results, _photo2Results;
        EmotionType _currentEmotionType;
        ICommand _takePhoto1ButtonPressed, _takePhoto2ButtonPressed;
        ICommand _photo1ScoreButtonPressed, _photo2ScoreButtonPressed;
        ICommand _resetButtonPressed;
        Command<EmotionPopupResponseModel> _emotionPopUpAlertResponseCommand;
        #endregion

        #region Constructors
        public FaceOffViewModel()
        {
            IsResetButtonEnabled = false;

            SetRandomEmotion();
        }
        #endregion

        #region Properties
        public Command<EmotionPopupResponseModel> EmotionPopUpAlertResponseCommand => _emotionPopUpAlertResponseCommand ??
            (_emotionPopUpAlertResponseCommand = new Command<EmotionPopupResponseModel>(async response => await ExecuteEmotionPopUpAlertResponseCommand(response).ConfigureAwait(false)));

        public ICommand TakePhoto1ButtonPressed => _takePhoto1ButtonPressed ??
            (_takePhoto1ButtonPressed = new Command(ExecuteTakePhoto1ButtonPressed));

        public ICommand TakePhoto2ButtonPressed => _takePhoto2ButtonPressed ??
            (_takePhoto2ButtonPressed = new Command(ExecuteTakePhoto2ButtonPressed));

        public ICommand ResetButtonPressed => _resetButtonPressed ??
            (_resetButtonPressed = new Command(ExecuteResetButtonPressed));

        public ICommand Photo1ScoreButtonPressed => _photo1ScoreButtonPressed ??
            (_photo1ScoreButtonPressed = new Command(ExecutePhoto1ScoreButtonPressed));

        public ICommand Photo2ScoreButtonPressed => _photo2ScoreButtonPressed ??
            (_photo2ScoreButtonPressed = new Command(ExecutePhoto2ScoreButtonPressed));

        public ImageSource Photo1ImageSource
        {
            get => _photo1ImageSource;
            set => SetProperty(ref _photo1ImageSource, value);
        }

        public ImageSource Photo2ImageSource
        {
            get => _photo2ImageSource;
            set => SetProperty(ref _photo2ImageSource, value);
        }

        public bool IsPhotoImage1Enabled
        {
            get => _isPhotoImage1Enabled;
            set => SetProperty(ref _isPhotoImage1Enabled, value);
        }

        public bool IsPhotoImage2Enabled
        {
            get => _isPhotoImage2Enabled;
            set => SetProperty(ref _isPhotoImage2Enabled, value);
        }

        public bool IsTakeLeftPhotoButtonEnabled
        {
            get => _isTakeLeftPhotoButtonEnabled;
            set => SetProperty(ref _isTakeLeftPhotoButtonEnabled, value);
        }

        public bool IsTakeLeftPhotoButtonStackVisible
        {
            get => _isTakeLeftPhotoButtonStackVisible;
            set => SetProperty(ref _isTakeLeftPhotoButtonStackVisible, value);
        }

        public bool IsTakeRightPhotoButtonEnabled
        {
            get => _isTakeRightPhotoButtonEnabled;
            set => SetProperty(ref _isTakeRightPhotoButtonEnabled, value);
        }

        public bool IsTakeRightPhotoButtonStackVisible
        {
            get => _isTakeRightPhotoButtonStackVisible;
            set => SetProperty(ref _isTakeRightPhotoButtonStackVisible, value);
        }

        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        public string ScoreButton1Text
        {
            get => _scoreButton1Text;
            set => SetProperty(ref _scoreButton1Text, value);
        }

        public string ScoreButton2Text
        {
            get => _scoreButton2Text;
            set => SetProperty(ref _scoreButton2Text, value);
        }

        public bool IsCalculatingPhoto1Score
        {
            get => _isCalculatingPhoto1Score;
            set => SetProperty(ref _isCalculatingPhoto1Score, value);
        }

        public bool IsCalculatingPhoto2Score
        {
            get => _isCalculatingPhoto2Score;
            set => SetProperty(ref _isCalculatingPhoto2Score, value);
        }

        public bool IsResetButtonEnabled
        {
            get => _isResetButtonEnabled;
            set => SetProperty(ref _isResetButtonEnabled, value);
        }

        public bool IsScore1ButtonEnabled
        {
            get => _isScore1ButtonEnabled;
            set => SetProperty(ref _isScore1ButtonEnabled, value);
        }

        public bool IsScore2ButtonEnabled
        {
            get => _isScore2ButtonEnabled;
            set => SetProperty(ref _isScore2ButtonEnabled, value);
        }

        public bool IsScore1ButtonVisable
        {
            get => _isScore1ButtonVisable;
            set => SetProperty(ref _isScore1ButtonVisable, value);
        }

        public bool IsScore2ButtonVisable
        {
            get => _isScore2ButtonVisable;
            set => SetProperty(ref _isScore2ButtonVisable, value);
        }
        #endregion

        #region Events
        public event EventHandler<AlertMessageEventArgs> PopUpAlertAboutEmotionTriggered;
        public event EventHandler<string> DisplayAllEmotionResultsAlert;
        public event EventHandler DisplayMultipleFacesDetectedAlert;
        public event EventHandler DisplayNoCameraAvailableAlert;
        public event EventHandler RevealScoreButton1WithAnimation;
        public event EventHandler RevealScoreButton2WithAnimation;
        public event EventHandler RevealPhotoImage1WithAnimation;
        public event EventHandler RevealPhotoImage2WithAnimation;
        #endregion

        #region Enums
        enum ErrorMessageType { NoFaceDetected, MultipleFacesDetected, ConnectionToCognitiveServicesFailed, InvalidAPIKey, GenericError }
        enum EmotionType { Anger, Contempt, Disgust, Fear, Happiness, Neutral, Sadness, Surprise };
        #endregion

        #region Methods
#if DEBUG
        public void SetPhotoImage1ToHappyForUITest(string photo1ImageString)
        {
            Photo1ImageSource = photo1ImageString;

            var allEmotionsStringBuilder = new StringBuilder();
            allEmotionsStringBuilder.AppendLine($"Anger: 0%");
            allEmotionsStringBuilder.AppendLine($"Contempt: 0%");
            allEmotionsStringBuilder.AppendLine($"Disgust: 0%");
            allEmotionsStringBuilder.AppendLine($"Fear: 0%");
            allEmotionsStringBuilder.AppendLine($"Happiness: 100%");
            allEmotionsStringBuilder.AppendLine($"Neutral: 0%");
            allEmotionsStringBuilder.AppendLine($"Sadness: 0%");
            allEmotionsStringBuilder.AppendLine($"Surprise: 0%");

            _photo1Results = allEmotionsStringBuilder.ToString();
            ScoreButton1Text = "Score: 100%";

            SetEmotion(EmotionType.Happiness);

            IsTakeLeftPhotoButtonEnabled = false;
            IsTakeLeftPhotoButtonStackVisible = false;

            OnRevealPhotoImage1WithAnimation();
            OnRevealScoreButton1WithAnimation();
        }

        public void SetPhotoImage2ToHappyForUITest(string photo2ImageString)
        {
            Photo2ImageSource = photo2ImageString;

            var allEmotionsStringBuilder = new StringBuilder();
            allEmotionsStringBuilder.AppendLine($"Anger: 0%");
            allEmotionsStringBuilder.AppendLine($"Contempt: 0%");
            allEmotionsStringBuilder.AppendLine($"Disgust: 0%");
            allEmotionsStringBuilder.AppendLine($"Fear: 0%");
            allEmotionsStringBuilder.AppendLine($"Happiness: 100%");
            allEmotionsStringBuilder.AppendLine($"Neutral: 0%");
            allEmotionsStringBuilder.AppendLine($"Sadness: 0%");
            allEmotionsStringBuilder.AppendLine($"Surprise: 0%");

            _photo2Results = allEmotionsStringBuilder.ToString();
            ScoreButton2Text = "Score: 100%";

            SetEmotion(EmotionType.Happiness);

            IsTakeRightPhotoButtonEnabled = false;
            IsTakeRightPhotoButtonStackVisible = false;

            OnRevealPhotoImage2WithAnimation();
            OnRevealScoreButton2WithAnimation();
        }
#endif

        void ExecuteTakePhoto1ButtonPressed() =>
            ExecutePopUpAlert(new PlayerModel(PlayerNumberType.Player1, Settings.Player1Name));

        void ExecuteTakePhoto2ButtonPressed() =>
            ExecutePopUpAlert(new PlayerModel(PlayerNumberType.Player2, Settings.Player2Name));

        void ExecutePopUpAlert(PlayerModel playerModel)
        {
            LogPhotoButtonTapped(playerModel.PlayerNumber);

            DisableButtons(playerModel.PlayerNumber);

            var title = _emotionDictionary[_currentEmotionType];
            var message = playerModel.PlayerName + ", " + _makeAFaceAlertMessage + _emotionStringsForAlertMessage[(int)_currentEmotionType];
            OnPopUpAlertAboutEmotionTriggered(title, message, playerModel);
        }

        async Task ExecuteEmotionPopUpAlertResponseCommand(EmotionPopupResponseModel response)
        {
            var player = response.Player;

            if (response.IsPopUpConfirmed)
                await ExecuteTakePhotoWorkflow(player).ConfigureAwait(false);
            else
                EnableButtons(player.PlayerNumber);
        }

        async Task ExecuteTakePhotoWorkflow(PlayerModel player)
        {
            player.ImageMediaFile = await GetMediaFileFromCamera("FaceOff", player.PlayerNumber).ConfigureAwait(false);

            if (player.ImageMediaFile == null)
                EnableButtons(player.PlayerNumber);
            else
                await ExecuteGetPhotoResultsWorkflow(player).ConfigureAwait(false);
        }

        async Task ExecuteGetPhotoResultsWorkflow(PlayerModel player)
        {
            Insights.Track(InsightsConstants.PhotoTaken);

            await ConfigureUIForPendingEmotionResults(player).ConfigureAwait(false);

            var results = await GenerateEmotionResults(player).ConfigureAwait(false);

            await ConfigureUIForFinalizedEmotionResults(player, results).ConfigureAwait(false);
        }

        async Task ConfigureUIForPendingEmotionResults(PlayerModel player)
        {
            RevealPhoto(player.PlayerNumber);

            SetIsEnabledForCurrentPhotoButton(false, player.PlayerNumber);
            SetIsVisibleForCurrentPhotoStack(false, player.PlayerNumber);

            SetScoreButtonText(_calculatingScoreMessage, player.PlayerNumber);

            SetPhotoImageSource(player.ImageMediaFile, player.PlayerNumber);

            SetIsCalculatingPhotoScore(true, player.PlayerNumber);

            SetResetButtonIsEnabled();

            await WaitForAnimationsToFinish((int)Math.Ceiling(AnimationConstants.PhotoImageAninmationTime * 2.5)).ConfigureAwait(false);

            RevealPhotoButton(player.PlayerNumber);
        }

        async Task ConfigureUIForFinalizedEmotionResults(PlayerModel player, string results)
        {
            SetPhotoResultsText(results, player.PlayerNumber);

            SetIsCalculatingPhotoScore(false, player.PlayerNumber);

            SetResetButtonIsEnabled();

            player.ImageMediaFile.Dispose();

            await WaitForAnimationsToFinish((int)Math.Ceiling(AnimationConstants.ScoreButonAninmationTime * 2.5)).ConfigureAwait(false);
        }

        async Task<string> GenerateEmotionResults(PlayerModel player)
        {
            Emotion[] emotionArray;
            string emotionScore;
            try
            {
                emotionArray = await GetEmotionResultsFromMediaFile(player.ImageMediaFile, false).ConfigureAwait(false);

                emotionScore = await GetPhotoEmotionScore(emotionArray, 0).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Insights.Report(e);

                emotionArray = null;

                if ((e is ClientException clientException) && (clientException.HttpStatus.Equals(System.Net.HttpStatusCode.Unauthorized)))
                    emotionScore = _errorMessageDictionary[ErrorMessageType.InvalidAPIKey];
                else
                    emotionScore = _errorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];
            }

            var doesEmotionScoreContainErrorMessage = DoesStringContainErrorMessage(emotionScore);

            if (doesEmotionScoreContainErrorMessage)
            {
                var errorMessageKey = _errorMessageDictionary.FirstOrDefault(x => x.Value.Contains(emotionScore)).Key;

                switch (errorMessageKey)
                {
                    case ErrorMessageType.NoFaceDetected:
                        Insights.Track(_errorMessageDictionary[ErrorMessageType.NoFaceDetected]);
                        break;
                    case ErrorMessageType.MultipleFacesDetected:
                        Insights.Track(_errorMessageDictionary[ErrorMessageType.MultipleFacesDetected]);
                        break;
                    case ErrorMessageType.GenericError:
                        Insights.Track(_errorMessageDictionary[ErrorMessageType.MultipleFacesDetected]);
                        break;
                }

                SetScoreButtonText(emotionScore, player.PlayerNumber);
            }
            else
                SetScoreButtonText($"Score: {emotionScore}", player.PlayerNumber);

            return GetStringOfAllPhotoEmotionScores(emotionArray, 0);
        }

        void ExecuteResetButtonPressed()
        {
            Insights.Track(InsightsConstants.ResetButtonTapped);

            SetRandomEmotion();

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

        async Task<MediaFile> GetMediaFileFromCamera(string directory, PlayerNumberType playerNumber)
        {
            await CrossMedia.Current.Initialize().ConfigureAwait(false);

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                OnDisplayNoCameraAvailableAlert();
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Small,
                Directory = directory,
                Name = playerNumber.ToString(),
                DefaultCamera = CameraDevice.Front,
                OverlayViewProvider = DependencyService.Get<ICameraService>()?.GetCameraOverlay()
            }).ConfigureAwait(false);

            return file;
        }

        async Task<Emotion[]> GetEmotionResultsFromMediaFile(MediaFile mediaFile, bool disposeMediaFile)
        {
            if (mediaFile == null)
                return null;

            var emotionClient = new EmotionServiceClient(CognitiveServicesConstants.EmotionApiKey);

            using (var handle = Insights.TrackTime(InsightsConstants.AnalyzeEmotion))
            {
                return await emotionClient.RecognizeAsync(GetPhotoStream(mediaFile, disposeMediaFile)).ConfigureAwait(false);
            }

        }

        EmotionType GetRandomEmotionType()
        {
            var rnd = new Random();
            int randomNumber;

            do
            {
                randomNumber = rnd.Next(0, _emotionDictionary.Count);
            } while (randomNumber == (int)_currentEmotionType);

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
                    throw new Exception("Invalid Emotion Type");
            }
        }

        void SetPageTitle(EmotionType emotionType)
        {
            PageTitle = _emotionDictionary[emotionType];
        }

        void SetRandomEmotion()
        {
            _currentEmotionType = GetRandomEmotionType();

            SetPageTitle(_currentEmotionType);
        }

        void SetEmotion(EmotionType emotionType)
        {
            _currentEmotionType = emotionType;

            SetPageTitle(_currentEmotionType);
        }

        async Task<string> GetPhotoEmotionScore(Emotion[] emotionResults, int emotionResultNumber)
        {
            float rawEmotionScore;

            var isInternetConnectionAvilable = await IsInternetConnectionAvailable().ConfigureAwait(false);

            if(!isInternetConnectionAvilable)
                return _errorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];

            if (emotionResults == null || emotionResults.Length < 1)
                return _errorMessageDictionary[ErrorMessageType.NoFaceDetected];

            if (emotionResults.Length > 1)
            {
                OnDisplayMultipleFacesError();
                return _errorMessageDictionary[ErrorMessageType.MultipleFacesDetected];
            }

            try
            {
                switch (_currentEmotionType)
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
                        return _errorMessageDictionary[ErrorMessageType.GenericError];
                }

                var emotionScoreAsPercentage = ConvertFloatToPercentage(rawEmotionScore);

                return emotionScoreAsPercentage;
            }
            catch (Exception e)
            {
                Insights.Report(e);
                return _errorMessageDictionary[ErrorMessageType.GenericError];
            }
        }

        async Task<bool> IsInternetConnectionAvailable() =>
            CrossConnectivity.Current.IsConnected &&
            await CrossConnectivity.Current.IsRemoteReachable("google.com").ConfigureAwait(false);

        string GetStringOfAllPhotoEmotionScores(Emotion[] emotionResults, int emotionResultNumber)
        {
            if (emotionResults == null || emotionResults.Length < 1)
                return _errorMessageDictionary[ErrorMessageType.GenericError];

            var allEmotionsString = new StringBuilder();

            allEmotionsString.AppendLine($"Anger: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Anger)}");
            allEmotionsString.AppendLine($"Contempt: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Contempt)}");
            allEmotionsString.AppendLine($"Disgust: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Disgust)}");
            allEmotionsString.AppendLine($"Fear: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Fear)}");
            allEmotionsString.AppendLine($"Happiness: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Happiness)}");
            allEmotionsString.AppendLine($"Neutral: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Neutral)}");
            allEmotionsString.AppendLine($"Sadness: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Sadness)}");
            allEmotionsString.Append($"Surprise: {ConvertFloatToPercentage(emotionResults[emotionResultNumber].Scores.Surprise)}");

            return allEmotionsString.ToString();
        }

        bool DoesStringContainErrorMessage(string stringToCheck)
        {
            foreach (KeyValuePair<ErrorMessageType, string> errorMessageDictionaryEntry in _errorMessageDictionary)
            {
                if (stringToCheck.Contains(errorMessageDictionaryEntry.Value))
                    return true;
            }

            return false;
        }

        void RevealPhoto(PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    OnRevealPhotoImage1WithAnimation();
                    break;
                case PlayerNumberType.Player2:
                    OnRevealPhotoImage2WithAnimation();
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsEnabledForOppositePhotoButton(bool isEnabled, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsTakeRightPhotoButtonEnabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsTakeLeftPhotoButtonEnabled = isEnabled;
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsEnabledForCurrentPlayerScoreButton(bool isEnabled, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsScore1ButtonEnabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsScore2ButtonEnabled = isEnabled;
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsEnabledForButtons(bool isEnabled, PlayerNumberType playerNumber)
        {
            SetIsEnabledForOppositePhotoButton(isEnabled, playerNumber);
            SetIsEnabledForCurrentPlayerScoreButton(isEnabled, playerNumber);
        }

        void SetIsEnabledForCurrentPhotoButton(bool isEnabled, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsTakeLeftPhotoButtonEnabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsTakeRightPhotoButtonEnabled = isEnabled;
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetScoreButtonText(string scoreButtonText, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    ScoreButton1Text = scoreButtonText;
                    break;
                case PlayerNumberType.Player2:
                    ScoreButton2Text = scoreButtonText;
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsVisibleForCurrentPhotoStack(bool isVisible, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsTakeLeftPhotoButtonStackVisible = isVisible;
                    break;
                case PlayerNumberType.Player2:
                    IsTakeRightPhotoButtonStackVisible = isVisible;
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetPhotoImageSource(MediaFile imageMediaFile, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    Photo1ImageSource = ImageSource.FromStream(() =>
                    {
                        return GetPhotoStream(imageMediaFile, false);
                    });
                    break;
                case PlayerNumberType.Player2:
                    Photo2ImageSource = ImageSource.FromStream(() =>
                    {
                        return GetPhotoStream(imageMediaFile, false);
                    });
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsCalculatingPhotoScore(bool isCalculatingScore, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsCalculatingPhoto1Score = isCalculatingScore;
                    break;
                case PlayerNumberType.Player2:
                    IsCalculatingPhoto2Score = isCalculatingScore;
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void RevealPhotoButton(PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    OnRevealScoreButton1WithAnimation();
                    break;
                case PlayerNumberType.Player2:
                    OnRevealScoreButton2WithAnimation();
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetPhotoResultsText(string results, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    _photo1Results = results;
                    break;
                case PlayerNumberType.Player2:
                    _photo2Results = results;
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        void LogPhotoButtonTapped(PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    Insights.Track(InsightsConstants.PhotoButton1Tapped);
                    break;
                case PlayerNumberType.Player2:
                    Insights.Track(InsightsConstants.PhotoButton2Tapped);
                    break;
                default:
                    throw new Exception(_playerNumberNotImplentedExceptionText);
            }
        }

        string ConvertFloatToPercentage(float floatToConvert) => floatToConvert.ToString("#0.##%");

        async Task WaitForAnimationsToFinish(int waitTimeInSeconds) =>
            await Task.Delay(waitTimeInSeconds).ConfigureAwait(false);

        void EnableButtons(PlayerNumberType playerNumber) =>
            SetIsEnabledForButtons(true, playerNumber);

        void DisableButtons(PlayerNumberType playerNumber) =>
            SetIsEnabledForButtons(false, playerNumber);

        void SetResetButtonIsEnabled() =>
            IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

        void OnDisplayAllEmotionResultsAlert(string emotionResults) =>
            DisplayAllEmotionResultsAlert?.Invoke(this, emotionResults);

        void OnDisplayNoCameraAvailableAlert() =>
            DisplayNoCameraAvailableAlert?.Invoke(this, EventArgs.Empty);

        void OnRevealPhotoImage1WithAnimation() =>
            RevealPhotoImage1WithAnimation?.Invoke(this, EventArgs.Empty);

        void OnRevealScoreButton1WithAnimation() =>
            RevealScoreButton1WithAnimation?.Invoke(this, EventArgs.Empty);

        void OnRevealPhotoImage2WithAnimation() =>
            RevealPhotoImage2WithAnimation?.Invoke(this, EventArgs.Empty);

        void OnRevealScoreButton2WithAnimation() =>
            RevealScoreButton2WithAnimation?.Invoke(this, EventArgs.Empty);

        void OnDisplayMultipleFacesError() =>
            DisplayMultipleFacesDetectedAlert?.Invoke(this, EventArgs.Empty);

        void OnPopUpAlertAboutEmotionTriggered(string title, string message, PlayerModel player) =>
            PopUpAlertAboutEmotionTriggered?.Invoke(this, new AlertMessageEventArgs(title, message, player));
        #endregion
    }
}

