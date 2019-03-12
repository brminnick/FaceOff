using System;
using System.Collections.Generic;

namespace FaceOff.Shared
{
    public static class EmotionConstants
    {
        readonly static Lazy<Dictionary<EmotionType, string>> _emotionDictionaryHolder = new Lazy<Dictionary<EmotionType, string>>(() =>
                new Dictionary<EmotionType, string>
                {
                { EmotionType.ANGRY, "Angry" },
                { EmotionType.CALM, "Calm" },
                { EmotionType.CONFUSED, "Confused" },
                { EmotionType.DISGUSTED, "Disgusted"},
                { EmotionType.HAPPY, "Happy" },
                { EmotionType.SAD, "Sad" },
                { EmotionType.SURPRISED, "Surprised" },
                { EmotionType.UNKNOWN, "Unknown" },
        });

        public static Dictionary<EmotionType, string> EmotionDictionary => _emotionDictionaryHolder.Value;
    }
}
