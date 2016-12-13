using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceOff
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class WelcomePage : ContentPage
	{
		public WelcomePage()
		{
			InitializeComponent();
			BindingContext = new WelcomeViewModel();
		}
	}
}
