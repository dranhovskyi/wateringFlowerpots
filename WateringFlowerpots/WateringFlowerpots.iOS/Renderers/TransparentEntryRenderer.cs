using UIKit;
using WateringFlowerpots.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(TransparentEntryRenderer))]
namespace WateringFlowerpots.iOS.Renderers
{
    public class TransparentEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.ClipsToBounds = true;

                Control.BorderStyle = UITextBorderStyle.None;

                Control.Layer.MasksToBounds = true;
                Control.Layer.BorderWidth = 1;
                Control.Layer.BorderColor = Color.Transparent.ToCGColor();
            }
        }
    }
}
