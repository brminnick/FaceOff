namespace FaceOff
{
    public class EmotionPopupResponseModel
    {
        public EmotionPopupResponseModel(bool isPopUpConfirmed, PlayerModel player)
        {
            IsPopUpConfirmed = isPopUpConfirmed;
            Player = player;
        }

        public bool IsPopUpConfirmed { get; }
        public PlayerModel Player { get; }
    }
}
