using System;
using System.Collections.Generic;

namespace FaceOff.Shared
{
    public static class EmotionConstants
    {
        readonly static Lazy<Dictionary<EmotionType, string>> _emotionDictionaryHolder = new Lazy<Dictionary<EmotionType, string>>(() =>
                new Dictionary<EmotionType, string>
                {
                { EmotionType.Anger, "Anger" },
                { EmotionType.Contempt, "Contempt" },
                { EmotionType.Disgust, "Disgust"},
                { EmotionType.Fear, "Fear" },
                { EmotionType.Happiness, "Happiness" },
                { EmotionType.Neutral, "Neutral" },
                { EmotionType.Sadness, "Sadness" },
                { EmotionType.Surprise, "Surprise" }
        });

        public static Dictionary<EmotionType, string> EmotionDictionary => _emotionDictionaryHolder.Value;
    }
}
