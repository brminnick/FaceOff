namespace FaceOff
{
    public static class ConversionExtensions
    {
        public static string ConvertToPercentage(this float doubleToConvert)
        {
            var truncatedNumber = System.Math.Truncate(doubleToConvert * 100) / 100;
            return $"{truncatedNumber}%";
        }
    }
}
