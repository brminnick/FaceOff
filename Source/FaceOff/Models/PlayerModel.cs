using System.Collections.Generic;

using Plugin.Media.Abstractions;

namespace FaceOff
{
	public class PlayerModel
	{
		readonly PlayerNumberType _playerNumber;
		readonly string _playerName;

		public PlayerModel(PlayerNumberType playerNumber, string playerName)
		{
			_playerNumber = playerNumber;
			_playerName = playerName;
		}

		public PlayerNumberType PlayerNumber { get { return _playerNumber; } }
		public string PlayerName { get { return _playerName; } }
		public MediaFile ImageMediaFile { get; set; }
		public List<Emotion>EmotionResults { get; set; }
	}

	public enum PlayerNumberType
	{
		Player1,
		Player2
	}
}
