using System;
using Xamarin.Forms;

namespace FaceOff
{
	public static class StylesConstants
	{
		public static Style ButtonStyle = new Style(typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex("3192B3") },
				new Setter { Property = Button.TextColorProperty, Value = Color.White }
			}
		};

		public static Style StackLayoutStyle = new Style(typeof(StackLayout))
		{
			Setters = {
				new Setter { Property = StackLayout.SpacingProperty, Value = 50 }
			}
		};

		public static Style LabelStyle = new Style(typeof(Label))
		{
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("2C7797") }
			}
		};
	}
}

