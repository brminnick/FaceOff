using System;
using System.ComponentModel;
using System.Linq;
using FaceOff.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(EntryCustomRederer))]
namespace FaceOff.iOS
{
    public class EntryCustomRederer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null && Control != null)
                Control.AllEditingEvents -= HandleAllEditingEvents;

            if (e.NewElement != null && Control != null)
                Control.AllEditingEvents += HandleAllEditingEvents;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null)
            {
                Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                Control.Layer.BorderWidth = 0.25f;
                Control.Layer.CornerRadius = 5;
            }
        }

        void HandleAllEditingEvents(object sender, EventArgs e)
        {
            if (Control.Subviews.OfType<UIButton>().FirstOrDefault() is UIButton clearButton
                && clearButton.CurrentImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate) is UIImage clearButtonImage)
            {
                var userInterfaceStyle = Xamarin.Essentials.Platform.GetCurrentUIViewController().TraitCollection.UserInterfaceStyle;

                switch (userInterfaceStyle)
                {
                    case UIUserInterfaceStyle.Light:
                        clearButton.SetImage(clearButtonImage, UIControlState.Normal);
                        clearButton.TintColor = UIColor.DarkGray;
                        break;

                    case UIUserInterfaceStyle.Dark:
                        clearButton.SetImage(clearButtonImage, UIControlState.Normal);
                        clearButton.TintColor = UIColor.LightGray;
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}
