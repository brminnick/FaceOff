using Xamarin.Forms;

namespace FaceOff
{
    public class DarkBlueLabel : Label
    {
        public DarkBlueLabel(in string text)
        {
            Text = text;
            TextColor = Color.FromHex("2C7797");
        }
    }
}
