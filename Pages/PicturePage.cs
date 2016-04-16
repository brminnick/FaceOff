using Plugin.Media;
using Xamarin.Forms;

namespace FaceOff
{
	public class PicturePage : ContentPage
	{

		public PicturePage()
		{
			var viewModel = new PictureViewModel();
			BindingContext = viewModel;

			#region Create Photo 1 Button
			var takePhoto1Button = new Button
			{
				Text = "Take Photo 1",
				BackgroundColor = Color.Red,
				Style = StylesConstants.ButtonStyle
			};
			takePhoto1Button.SetBinding(Button.CommandProperty, "TakePhoto1ButtonPressed");
			takePhoto1Button.SetBinding(Button.IsEnabledProperty, "IsTakePhoto1ButtonEnabled");
			takePhoto1Button.SetBinding(Button.IsVisibleProperty, "IsTakePhoto1ButtonEnabled");
			#endregion

			#region Create Photo 2 Button
			var takePhoto2Button = new Button
			{
				Text = "Take Photo 2",
				BackgroundColor = Color.Blue,
				Style = StylesConstants.ButtonStyle
			};
			takePhoto2Button.SetBinding(Button.CommandProperty, "TakePhoto2ButtonPressed");
			takePhoto2Button.SetBinding(Button.IsEnabledProperty, "IsTakePhoto2ButtonEnabled");
			takePhoto2Button.SetBinding(Button.IsVisibleProperty, "IsTakePhoto2ButtonEnabled");
			#endregion

			#region Create Photo Image Containers
			var photoImage1 = new Image();
			photoImage1.SetBinding(Image.SourceProperty, "Photo1ImageSource");
			photoImage1.SetBinding(Image.IsVisibleProperty, "IsPhotoImage1Enabled");

			var photoImage2 = new Image();
			photoImage2.SetBinding(Image.SourceProperty, "Photo2ImageSource");
			photoImage1.SetBinding(Image.IsVisibleProperty, "IsPhotoImage2Enabled");
			#endregion
		
			#region Create Reset Button
			var resetButton = new Button
			{
				Text = "Reset",
				Style = StylesConstants.ButtonStyle,
				BackgroundColor = Color.Silver
			};
			resetButton.SetBinding(Button.CommandProperty, "ResetButtonPressed");
			#endregion

			#region Create Relative Laout
			var buttonImageRelativeLayout = new RelativeLayout();
			buttonImageRelativeLayout.Children.Add(photoImage1,
				Constraint.RelativeToParent(parent =>
				{
					return parent.X;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Y;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 7 / 8;
				})
			);
			buttonImageRelativeLayout.Children.Add(takePhoto1Button,
				Constraint.RelativeToParent(parent =>
				{
					return parent.X;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Y;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 7 / 8;
				})
			);
			buttonImageRelativeLayout.Children.Add(photoImage2,
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Y;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 7 / 8;
				})
			);
			buttonImageRelativeLayout.Children.Add(takePhoto2Button,
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Y;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 7 / 8;
				})
			);
			buttonImageRelativeLayout.Children.Add(resetButton,
				Constraint.RelativeToParent(parent =>
				{
					return parent.X;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 7 / 8;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Width;
				}),
				Constraint.RelativeToParent(parent =>
				{
					return parent.Height * 1 / 8;
				})
			);
			#endregion

			#region Set Page Content and Padding
			Padding = new Thickness(0, Device.OnPlatform(30, 10, 10), 0, 5);
			Content = buttonImageRelativeLayout;
			#endregion
		}

	}
}

