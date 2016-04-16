using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FaceOff.Model
{
   public class EmotionRoot
    {
        [JsonProperty("faceRectangle")]
        public FaceRectangle Rectangle { get; set; } = new FaceRectangle();
        public Scores Scores { get; set; } = new Scores();
    }

    public class FaceRectangle
    {
        [JsonProperty("left")]
        public int Left { get; set; }
        [JsonProperty("top")]
        public int Top { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class Scores
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
