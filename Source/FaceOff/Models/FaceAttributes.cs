using Newtonsoft.Json;

namespace FaceOff
{
    public class FaceAttributes
    {
		[JsonProperty("emotion")]
        public Emotion Emotion { get; set; }
    }
}
