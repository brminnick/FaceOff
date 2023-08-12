namespace FaceOff.Shared
{
	public class PlayerEmotionModel
	{
		public PlayerEmotionModel(string playerName, EmotionType emotion)
		{
			PlayerName = playerName;
			Emotion = emotion;
		}

		public string PlayerName { get; }
		public EmotionType Emotion { get; }
	}
}