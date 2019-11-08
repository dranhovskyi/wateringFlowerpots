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

        private List<FlowerpotGroup> flowerpots;
        public List<FlowerpotGroup> Flowerpots
        {
            get { return flowerpots; }
            set { SetProperty(ref flowerpots, value); }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { SetProperty(ref isActive, value, RaiseIsActiveChanged); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value, RaiseIsActiveChanged); }
        }

        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

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
            RefreshCommand.Execute(null);
        }

        private async Task ShowAddModalAsync()
        {
            await navigationService.NavigateAsync("NewFlowerpotPage", useModalNavigation: true);
        }

        private async Task RefreshCommandAsync()
        {
            var allFlowerpots = await flowerpotRepository.GetAllFlowerpotAsync();
            Flowerpots = GetGroupedAndSortedList(allFlowerpots);

            IsRefreshing = false;
        }

        private async Task ItemTappedCommandAsync(object obj)
        {
            if (obj is Flowerpot flowerpot)
            {
                var action = await dialogService.DisplayActionSheetAsync("Select an action", "Cancel", null, "Delete");
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
            DateTime todayDateTime = DateTime.Today;
            DayOfWeek today = todayDateTime.DayOfWeek;
            DayOfWeek tomorrow = todayDateTime.DayOfWeek + 1;

            var todayList = new List<Flowerpot>();
            var tomorrowList = new List<Flowerpot>();
            var comingSoon = new List<Flowerpot>();

            foreach (var flowerpot in flowerpots)
            {
                if (flowerpot.DayOfTheWeek == today.ToString())
                {
                    flowerpot.Date = todayDateTime;
                    todayList.Add(flowerpot);
                }
                else if (flowerpot.DayOfTheWeek == tomorrow.ToString())
                {
                    flowerpot.Date = todayDateTime.AddDays(1);
                    tomorrowList.Add(flowerpot);
                }
                else
                {
                    int daysUntilTuesday = ((int)Enum.Parse(typeof(DayOfWeek), flowerpot.DayOfTheWeek) - (int)today + 7) % 7;
                    DateTime nextDayOfTheWeek = todayDateTime.AddDays(daysUntilTuesday);
                    flowerpot.Date = nextDayOfTheWeek;
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
                comingSoon = comingSoon.AsParallel().OrderBy(p => p.Date).ToList();
                listOfGorups.Add(new FlowerpotGroup("Coming soon", comingSoon));
            }

            return listOfGorups;
        }
    }
}
