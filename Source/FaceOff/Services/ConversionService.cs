namespace FaceOff
{
    public static class ConversionService
    {
		public static string ConvertDoubleToPercentage(double doubleToConvert) => doubleToConvert.ToString("#0.##%");
    }
}
