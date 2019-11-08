using DryIoc;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using WateringFlowerpots.Repository;
using WateringFlowerpots.ViewModels;
using WateringFlowerpots.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WateringFlowerpots
{
    public partial class App
    {
        public static IContainer AppContainer { get; set; }

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            
            await NavigationService.NavigateAsync(new System.Uri("/MainTabbedPage?selectedTab=SchedulePage", System.UriKind.Absolute));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<SchedulePage, SchedulePageViewModel>();
            containerRegistry.RegisterForNavigation<NewFlowerpotPage, NewFlowerpotPageViewModel>();
            containerRegistry.RegisterForNavigation<GardenPage>();
            containerRegistry.RegisterForNavigation<NotificationPage>();
            containerRegistry.RegisterForNavigation<ProfilePage>();

            containerRegistry.RegisterInstance<IFlowerpotRepository>(new FlowerpotRepository());

            AppContainer = containerRegistry.GetContainer();
        }
    }
}
