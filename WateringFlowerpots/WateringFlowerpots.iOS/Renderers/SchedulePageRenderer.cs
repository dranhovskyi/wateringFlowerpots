using System;
using UIKit;
using WateringFlowerpots.iOS.Renderers;
using WateringFlowerpots.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SchedulePage), typeof(SchedulePageRenderer))]
namespace WateringFlowerpots.iOS.Renderers
{
    public class SchedulePageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBar.PrefersLargeTitles = true;
            NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Automatic;
        }
    }
}
