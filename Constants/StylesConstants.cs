using System;
using Xamarin.Forms;

namespace FaceOff
{
	public static class StylesConstants
	{
		public static Style ButtonStyle = new Style(typeof(Label))
		{
			Setters = {
				new Setter { Property = Button.BorderColorProperty, Value = Color.Black },
				new Setter { Property = Button.BorderWidthProperty, Value = 2 },
				new Setter { Property = Button.BorderRadiusProperty, Value = 10 }
			}
		};
	}
}

