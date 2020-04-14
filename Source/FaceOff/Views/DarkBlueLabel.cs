using Xamarin.Forms;

namespace FaceOff
{
    public class DarkBlueLabel : Label
    {
        public DarkBlueLabel(in string text)
        {
            Text = text;
            TextColor = ColorConstants.LabelTextColor;
        }
    }
}
