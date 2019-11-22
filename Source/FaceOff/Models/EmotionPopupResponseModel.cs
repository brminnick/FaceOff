namespace FaceOff
{
    public class EmotionPopupResponseModel
    {
        public EmotionPopupResponseModel(bool isPopUpConfirmed, PlayerModel player) =>
            (IsPopUpConfirmed, Player) = (isPopUpConfirmed, player);

        public bool IsPopUpConfirmed { get; }
        public PlayerModel Player { get; }
    }
}
