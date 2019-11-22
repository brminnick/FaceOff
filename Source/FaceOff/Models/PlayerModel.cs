using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media.Abstractions;

namespace FaceOff
{
    public class PlayerModel
    {
        public PlayerModel(PlayerNumberType playerNumber, string playerName) =>
            (PlayerNumber, PlayerName) = (playerNumber, playerName);

        public PlayerNumberType PlayerNumber { get; }
        public string PlayerName { get; }
        public MediaFile? ImageMediaFile { get; set; }
        public List<Emotion>? EmotionResults { get; set; }
    }
}
