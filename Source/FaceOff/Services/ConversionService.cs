namespace FaceOff
{
    public static class ConversionService
    {
        public static string ConvertFloatToPercentage(float floatToConvert) => floatToConvert.ToString("#0.##%");
    }
}
