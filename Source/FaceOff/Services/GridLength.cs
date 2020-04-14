namespace Xamarin.Forms.Markup
{
    static class GridLengths
    {
        public static GridLength StarGridLength(double value) => new GridLength(value, GridUnitType.Star);
        public static GridLength StarGridLength(int value) => StarGridLength((double)value);

        public static GridLength AbsoluteGridLength(double value) => new GridLength(value, GridUnitType.Absolute);
        public static GridLength AbsoluteGridLength(int value) => AbsoluteGridLength((double)value);
    }
}
