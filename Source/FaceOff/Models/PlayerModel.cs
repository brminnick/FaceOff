using System.Collections.Generic;
using Amazon.Rekognition.Model;

using Plugin.Media.Abstractions;

namespace FaceOff
{
    public class PlayerModel
	{
        public PlayerModel(PlayerNumberType playerNumber, string playerName)
		{
            PlayerNumber = playerNumber;
            PlayerName = playerName;
        }

        public PlayerNumberType PlayerNumber { get; }
        public string PlayerName { get; }
        public MediaFile ImageMediaFile { get; set; }
		public List<Emotion>EmotionResults { get; set; }
	}

	public enum PlayerNumberType
	{
		Player1,
		Player2
	}
}
