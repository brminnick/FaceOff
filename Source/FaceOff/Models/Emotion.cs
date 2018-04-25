using Newtonsoft.Json;

namespace FaceOff
{
    public class Emotion
    {
		[JsonProperty("anger")]
        public double Anger { get; set; }

        [JsonProperty("contempt")]
		public double Contempt { get; set; }

        [JsonProperty("disgust")]
        public double Disgust { get; set; }

        [JsonProperty("fear")]
        public double Fear { get; set; }

        [JsonProperty("happiness")]
        public double Happiness { get; set; }
        
        [JsonProperty("neutral")]
        public double Neutral { get; set; }

        [JsonProperty("sadness")]
		public double Sadness { get; set; }

        [JsonProperty("surprise")]
        public double Surprise { get; set; }
    }
}
