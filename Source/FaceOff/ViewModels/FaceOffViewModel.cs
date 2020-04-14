using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using FaceOff.Shared;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FaceOff
{
    public class FaceOffViewModel : BaseViewModel
    {
        readonly Lazy<string[]> _emotionStringsForAlertMessageHolder = new Lazy<string[]>(() =>
            new string[] { "angry", "disrespectful", "disgusted", "scared", "happy", "blank", "sad", "surprised" });

        const string _makeAFaceAlertMessage = "take a selfie looking";
        const string _calculatingScoreMessage = "Analyzing";

        const string _playerNumberNotImplentedExceptionText = "Player Number Not Implemented";

        readonly WeakEventManager<GameInitializedEventArgs> _gameInitializedEventManager = new WeakEventManager<GameInitializedEventArgs>();
        readonly WeakEventManager<string> _emotionResultsGatheredEventManager = new WeakEventManager<string>();
        readonly WeakEventManager<PlayerNumberType> _generateEmotionResultsCompletedEventManager = new WeakEventManager<PlayerNumberType>();
        readonly WeakEventManager<PlayerNumberType> _generateEmotionResultsStartedEventManager = new WeakEventManager<PlayerNumberType>();

        ImageSource? _photo1ImageSource, _photo2ImageSource;

        bool _isTakeLeftPhotoButtonEnabled = true;
        bool _isTakeLeftPhotoButtonStackVisible = true;
        bool _isTakeRightPhotoButtonEnabled = true;
        bool _isTakeRightPhotoButtonStackVisible = true;
        bool _isCalculatingPhoto1Score, _isCalculatingPhoto2Score;
        bool _isScore1ButtonEnabled, _isScore2ButtonEnabled;
        EmotionType _currentEmotionType;

        string _photo1Results = string.Empty,
            _photo2Results = string.Empty,
            _pageTitle = string.Empty,
            _scoreButton1Text = string.Empty,
            _scoreButton2Text = string.Empty;

        ICommand? _resetButtonTapped, _emotionPopUpAlertResponseCommand, _takePhoto1ButtonTapped,
            _takePhoto2ButtonTapped, _photo1ScoreButtonTapped, _photo2ScoreButtonTapped;

        public FaceOffViewModel() => SetRandomEmotion();

        public event EventHandler<GameInitializedEventArgs> GameInitialized
        {
            add => _gameInitializedEventManager.AddEventHandler(value);
            remove => _gameInitializedEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<string> EmotionResultsGathered
        {
            add => _emotionResultsGatheredEventManager.AddEventHandler(value);
            remove => _emotionResultsGatheredEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<PlayerNumberType> GenerateEmotionsResultsCompleted
        {
            add => _generateEmotionResultsCompletedEventManager.AddEventHandler(value);
            remove => _generateEmotionResultsCompletedEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<PlayerNumberType> GenerateEmotionResultsStarted
        {
            add => _generateEmotionResultsStartedEventManager.AddEventHandler(value);
            remove => _generateEmotionResultsStartedEventManager.RemoveEventHandler(value);
        }

        public ICommand EmotionPopUpAlertResponseCommand => _emotionPopUpAlertResponseCommand ??= new AsyncCommand<EmotionPopupResponseModel>(ExecuteEmotionPopUpAlertResponseCommand);
        public ICommand TakePhoto1ButtonTapped => _takePhoto1ButtonTapped ??= new Command(ExecuteTakePhoto1ButtonTapped);
        public ICommand TakePhoto2ButtonTapped => _takePhoto2ButtonTapped ??= new Command(ExecuteTakePhoto2ButtonTapped);
        public ICommand ResetButtonTapped => _resetButtonTapped ??= new Command(ExecuteResetButtonTapped);
        public ICommand Photo1ScoreButtonTapped => _photo1ScoreButtonTapped ??= new Command(ExecutePhoto1ScoreButtonTapped);
        public ICommand Photo2ScoreButtonTapped => _photo2ScoreButtonTapped ??= new Command(ExecutePhoto2ScoreButtonTapped);

        public bool IsResetButtonEnabled => !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

        public ImageSource? Photo1ImageSource
        {
            get => _photo1ImageSource;
            set => SetProperty(ref _photo1ImageSource, value);
        }

        public ImageSource? Photo2ImageSource
        {
            get => _photo2ImageSource;
            set => SetProperty(ref _photo2ImageSource, value);
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
            set => SetProperty(ref _isCalculatingPhoto1Score, value, () => OnPropertyChanged(nameof(IsResetButtonEnabled)));
        }

        public bool IsCalculatingPhoto2Score
        {
            get => _isCalculatingPhoto2Score;
            set => SetProperty(ref _isCalculatingPhoto2Score, value, () => OnPropertyChanged(nameof(IsResetButtonEnabled)));
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

        string[] EmotionStringsForAlertMessage => _emotionStringsForAlertMessageHolder.Value;

        #region UITest Backdoor Methods
#if DEBUG
        public Task SubmitPhoto(EmotionType emotion, PlayerModel player)
        {
            _currentEmotionType = emotion;
            SetPageTitle(_currentEmotionType);

            return ExecuteGetPhotoResultsWorkflow(player);
        }
#endif
        #endregion
        void ExecuteTakePhoto1ButtonTapped() =>
            ExecutePopUpAlert(new PlayerModel(PlayerNumberType.Player1, PreferencesService.Player1Name));

        void ExecuteTakePhoto2ButtonTapped() =>
            ExecutePopUpAlert(new PlayerModel(PlayerNumberType.Player2, PreferencesService.Player2Name));

        void ExecutePopUpAlert(PlayerModel playerModel)
        {
            logPhotoButtonTapped(playerModel.PlayerNumber);
            DisableButtons(playerModel.PlayerNumber);

            var title = EmotionConstants.EmotionDictionary[_currentEmotionType];
            var message = $"{playerModel.PlayerName}, {_makeAFaceAlertMessage} {EmotionStringsForAlertMessage[(int)_currentEmotionType]}";
            OnGameInitialized(title, message, playerModel);

            static void logPhotoButtonTapped(PlayerNumberType playerNumber)
            {
                switch (playerNumber)
                {
                    case PlayerNumberType.Player1:
                        AnalyticsService.Track(AnalyticsConstants.PhotoButton1Tapped);
                        break;
                    case PlayerNumberType.Player2:
                        AnalyticsService.Track(AnalyticsConstants.PhotoButton2Tapped);
                        break;
                    default:
                        throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
                }
            }
        }

        Task ExecuteEmotionPopUpAlertResponseCommand(EmotionPopupResponseModel response)
        {
            var player = response.Player;

            if (response.IsPopUpConfirmed)
                return ExecuteTakePhotoWorkflow(player);

            EnableButtons(player.PlayerNumber);
            return Task.CompletedTask;
        }

        async Task ExecuteTakePhotoWorkflow(PlayerModel player)
        {
            player.ImageMediaFile = await MediaService.GetMediaFileFromCamera("FaceOff", player.PlayerNumber).ConfigureAwait(false);

            if (player.ImageMediaFile is null)
                EnableButtons(player.PlayerNumber);
            else
                await ExecuteGetPhotoResultsWorkflow(player).ConfigureAwait(false);
        }

        async Task ExecuteGetPhotoResultsWorkflow(PlayerModel player)
        {
            AnalyticsService.Track(AnalyticsConstants.PhotoTaken);

            ConfigureUIForPendingEmotionResults(player);

            var results = await GenerateEmotionResults(player).ConfigureAwait(false);

            ConfigureUIForFinalizedEmotionResults(player, results);
        }

        void ConfigureUIForPendingEmotionResults(PlayerModel player)
        {
            OnGenerateEmotionResultsStarted(player.PlayerNumber);

            SetIsEnabledForOppositePhotoButton(true, player.PlayerNumber);

            SetIsEnabledForCurrentPhotoButton(false, player.PlayerNumber);
            SetIsVisibleForCurrentPhotoStack(false, player.PlayerNumber);

            SetScoreButtonText(_calculatingScoreMessage, player.PlayerNumber);

            SetPhotoImageSource(player.ImageMediaFile, player.PlayerNumber);

            SetIsCalculatingPhotoScore(true, player.PlayerNumber);
        }

        void ConfigureUIForFinalizedEmotionResults(PlayerModel player, string results)
        {
            OnGenerateEmotionsResultsCompleted(player.PlayerNumber);

            SetPhotoResultsText(results, player.PlayerNumber);

            SetIsCalculatingPhotoScore(false, player.PlayerNumber);

            SetIsEnabledForCurrentPlayerScoreButton(true, player.PlayerNumber);
        }

        async Task<string> GenerateEmotionResults(PlayerModel player)
        {
            List<Emotion> emotionArray = Enumerable.Empty<Emotion>().ToList();
            string emotionScore;

            try
            {
                emotionArray = await EmotionService.GetEmotionResultsFromMediaFile(player.ImageMediaFile).ConfigureAwait(false);
                emotionScore = EmotionService.GetPhotoEmotionScore(emotionArray, 0, _currentEmotionType);
            }
            catch (APIErrorException e) when (e.Response.StatusCode is System.Net.HttpStatusCode.Unauthorized)
            {
                AnalyticsService.Report(e);

                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.InvalidAPIKey];
            }
            catch (HttpRequestException e) when (e.Message.Contains("offline"))
            {
                AnalyticsService.Report(e);

                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.DeviceOffline];
            }
            catch (Exception e)
            {
                AnalyticsService.Report(e);

                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];
            }

            var doesEmotionScoreContainErrorMessage = EmotionService.DoesStringContainErrorMessage(emotionScore);

            if (doesEmotionScoreContainErrorMessage)
            {
                var errorMessageKey = EmotionService.ErrorMessageDictionary.First(x => x.Value.Contains(emotionScore)).Key;

                switch (errorMessageKey)
                {
                    case ErrorMessageType.NoFaceDetected:
                        AnalyticsService.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.NoFaceDetected]);
                        break;
                    case ErrorMessageType.MultipleFacesDetected:
                        AnalyticsService.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected]);
                        break;
                    case ErrorMessageType.GenericError:
                        AnalyticsService.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected]);
                        break;
                }

                SetScoreButtonText(emotionScore, player.PlayerNumber);
            }
            else
                SetScoreButtonText($"Score: {emotionScore}", player.PlayerNumber);

            return EmotionService.GetStringOfAllPhotoEmotionScores(emotionArray, 0);
        }

        void ExecuteResetButtonTapped()
        {
            AnalyticsService.Track(AnalyticsConstants.ResetButtonTapped);

            SetRandomEmotion();

            Photo1ImageSource = null;
            Photo2ImageSource = null;

            IsTakeLeftPhotoButtonEnabled = true;
            IsTakeLeftPhotoButtonStackVisible = true;

            IsTakeRightPhotoButtonEnabled = true;
            IsTakeRightPhotoButtonStackVisible = true;

            ScoreButton1Text = string.Empty;
            ScoreButton2Text = string.Empty;

            IsScore1ButtonEnabled = false;
            IsScore2ButtonEnabled = false;

            _photo1Results = string.Empty;
            _photo2Results = string.Empty;
        }

        void ExecutePhoto1ScoreButtonTapped()
        {
            AnalyticsService.Track(AnalyticsConstants.ResultsButton1Tapped);
            OnEmotionResultsGathered(_photo1Results);
        }

        void ExecutePhoto2ScoreButtonTapped()
        {
            AnalyticsService.Track(AnalyticsConstants.ResultsButton2Tapped);
            OnEmotionResultsGathered(_photo2Results);
        }

        void SetRandomEmotion() => SetEmotion(EmotionService.GetRandomEmotionType(_currentEmotionType));


        void SetEmotion(EmotionType emotionType)
        {
            _currentEmotionType = emotionType;

            SetPageTitle(_currentEmotionType);
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
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
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
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
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
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
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
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
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
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetPhotoImageSource(MediaFile? imageMediaFile, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    Photo1ImageSource = ImageSource.FromStream(() => imageMediaFile?.GetStream());
                    break;
                case PlayerNumberType.Player2:
                    Photo2ImageSource = ImageSource.FromStream(() => imageMediaFile?.GetStream());
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
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
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
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
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetPageTitle(EmotionType emotionType) =>
            PageTitle = EmotionConstants.EmotionDictionary[emotionType];

        Task WaitForAnimationsToFinish(int waitTimeInMilliseconds) => Task.Delay(waitTimeInMilliseconds);

        void EnableButtons(PlayerNumberType playerNumber) =>
            SetIsEnabledForButtons(true, playerNumber);

        void DisableButtons(PlayerNumberType playerNumber) =>
            SetIsEnabledForButtons(false, playerNumber);

        void OnEmotionResultsGathered(string emotionResults) =>
            _emotionResultsGatheredEventManager.HandleEvent(this, emotionResults, nameof(EmotionResultsGathered));

        void OnGenerateEmotionResultsStarted(PlayerNumberType playerNumber) =>
            _generateEmotionResultsStartedEventManager.HandleEvent(this, playerNumber, nameof(GenerateEmotionResultsStarted));

        void OnGenerateEmotionsResultsCompleted(PlayerNumberType playerNumber) =>
            _generateEmotionResultsCompletedEventManager.HandleEvent(this, playerNumber, nameof(GenerateEmotionsResultsCompleted));

        void OnGameInitialized(string title, string message, PlayerModel player) =>
            _gameInitializedEventManager.HandleEvent(this, new GameInitializedEventArgs(title, message, player), nameof(GameInitialized));
    }
}

