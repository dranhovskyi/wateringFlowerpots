using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism;
using Prism.Navigation;
using Prism.Services;
using WateringFlowerpots.Models;
using WateringFlowerpots.Repository;
using Xamarin.Forms;

namespace WateringFlowerpots.ViewModels
{
    public class SchedulePageViewModel : ViewModelBase, IActiveAware
    {
        public event EventHandler IsActiveChanged;
        public ICommand RefreshCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand ItemTappedCommand { get; set; }

        public List<FlowerpotGroup> Flowerpots
        {
            get { return flowerpots; }
            set { SetProperty(ref flowerpots, value); }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { SetProperty(ref isActive, value, RaiseIsActiveChanged); }
        }

        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool isActive;
        private List<FlowerpotGroup> flowerpots;
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly IFlowerpotRepository flowerpotRepository;

        public SchedulePageViewModel(INavigationService navigationService,
            IPageDialogService dialogService,
            IFlowerpotRepository flowerpotRepository)
            : base(navigationService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.flowerpotRepository = flowerpotRepository;

            AddNewCommand = new Command(async() => await ShowAddModalAsync());
            RefreshCommand = new Command(async() => await RefreshCommandAsync());
            ItemTappedCommand = new Command(async (obj) => await ItemTappedCommandAsync(obj));
            Flowerpots = new List<FlowerpotGroup>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        private async Task ShowAddModalAsync()
        {
            await navigationService.NavigateAsync("NewFlowerpotPage", useModalNavigation: true);
        }

        private async Task RefreshCommandAsync()
        {
            var allFlowerpots = await flowerpotRepository.GetAllFlowerpotAsync();
            Flowerpots = GetGroupedAndSortedList(allFlowerpots);
        }

        private async Task ItemTappedCommandAsync(object obj)
        {
            if (obj is Flowerpot flowerpot)
            {
                var action = await dialogService.DisplayActionSheetAsync("Select an action", "Cancel", "Delete");
                switch (action)
                {
                    case "Delete":
                        await flowerpotRepository.DeleteFlowerpotAsync(flowerpot.Id);
                        await RefreshCommandAsync();
                        break;
                }
            }
        }

        private List<FlowerpotGroup> GetGroupedAndSortedList(List<Flowerpot> flowerpots)
        {
            //TODO: Date of waterfill
            DayOfWeek today = DateTime.Today.DayOfWeek;
            DayOfWeek tomorrow = DateTime.Today.DayOfWeek + 1;

            var todayList = new List<Flowerpot>();
            var tomorrowList = new List<Flowerpot>();
            var comingSoon = new List<Flowerpot>();

            foreach (var flowerpot in flowerpots)
            {
                if (flowerpot.DayOfTheWeek == today.ToString())
                {
                    todayList.Add(flowerpot);
                }
                else if (flowerpot.DayOfTheWeek == tomorrow.ToString())
                {
                    tomorrowList.Add(flowerpot);
                }
                else
                {
                    comingSoon.Add(flowerpot);
                }
            }

            var listOfGorups = new List<FlowerpotGroup>();

            if (todayList.Any())
            {
                listOfGorups.Add(new FlowerpotGroup("Today", todayList));
            }
            if (tomorrowList.Any())
            {
                listOfGorups.Add(new FlowerpotGroup("Tomorrow", tomorrowList));
            }
            if (comingSoon.Any())
            {
                listOfGorups.Add(new FlowerpotGroup("Coming soon", comingSoon));
            }

            return listOfGorups;
        }
    }
}
