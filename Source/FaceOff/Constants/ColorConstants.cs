using Xamarin.Forms;

namespace FaceOff
{
	static class ColorConstants
	{
		const string _lightBlueHex = "91E2F4";
		const string _mediumBlueHex = "1FAECE";
		const string _darkBlueHex = "3192B3";
		const string _darkestBlueHex = "2C7797";

		public static Color ActivityIndicatorColor { get; } = Color.FromHex(_darkestBlueHex);
		public static Color ButtonBackgroundColor { get; } = Color.FromHex(_darkBlueHex);
		public static Color ButtonTextColor { get; } = Color.White;
		public static Color LabelTextColor { get; } = Color.FromHex(_darkestBlueHex);
		public static Color NavigationBarBackgroundColor { get; } = Color.FromHex(_mediumBlueHex);
		public static Color NaviagtionBarTextColor { get; } = Color.White;
		public static Color PageBackgroundColor { get; } = Color.FromHex(_lightBlueHex);
	}
}