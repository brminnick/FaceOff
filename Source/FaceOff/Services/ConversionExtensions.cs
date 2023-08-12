namespace FaceOff
{
	static class ConversionExtensions
	{
		public static string ConvertToPercentage(this double doubleToConvert) => doubleToConvert.ToString("#0.##%");
	}
}