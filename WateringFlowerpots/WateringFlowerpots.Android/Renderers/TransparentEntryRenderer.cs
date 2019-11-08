using System;
using Android.Content;
using WateringFlowerpots.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(TransparentEntryRenderer))]
namespace WateringFlowerpots.Droid.Renderers
{
    public class TransparentEntryRenderer : EntryRenderer
    {
        public TransparentEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}
