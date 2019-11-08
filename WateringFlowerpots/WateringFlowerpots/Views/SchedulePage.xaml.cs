using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace WateringFlowerpots.Views
{
    public partial class SchedulePage : ContentPage
    {
        public SchedulePage()
        {
            InitializeComponent();

            On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Always);
        }
    }
}
