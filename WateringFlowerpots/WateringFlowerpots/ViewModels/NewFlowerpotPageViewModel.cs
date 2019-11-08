using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Prism.Services;
using WateringFlowerpots.Extensions;
using WateringFlowerpots.Helpers;
using WateringFlowerpots.Repository;
using Xamarin.Forms;

namespace WateringFlowerpots.ViewModels
{
    public class NewFlowerpotPageViewModel : ViewModelBase
    {
        public ICommand CloseModalCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public List<string> DaysOfTheWeek { get; set; }

        private string flowerpotName;
        public string FlowerpotName
        {
            get { return flowerpotName; }
            set { SetProperty(ref flowerpotName, value); }
        }

        private int volumeCount;
        public int VolumeCount
        {
            get { return volumeCount; }
            set { SetProperty(ref volumeCount, value); }
        }

        private string selectedDayOfTheWeek;
        public string SelectedDayOfTheWeek
        {
            get { return selectedDayOfTheWeek; }
            set { SetProperty(ref selectedDayOfTheWeek, value); }
        }

        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set { SetProperty(ref imagePath, value); }
        }

        private readonly INavigationService navigationService;
        private readonly IPageDialogService pageDialogService;
        private readonly IFlowerpotRepository flowerpotRepository;

        public NewFlowerpotPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IFlowerpotRepository flowerpotRepository)
            : base(navigationService)
        {
            this.navigationService = navigationService;
            this.pageDialogService = pageDialogService;
            this.flowerpotRepository = flowerpotRepository;

            CloseModalCommand = new Command(async () => await CloseModalAsync());
            SaveCommand = new Command(async () => await SaveCommandAsync());
            DaysOfTheWeek = new List<string>(CustomHelpers.GetWeekdays(DayOfWeek.Monday));
        }

        private async Task SaveCommandAsync()
        {
            if (ValidateFlowerpot())
            {
                var image = await ImagePath.GetBase64FromImageByteArrayAsync();
                await flowerpotRepository.AddNewFlowerpotAsync(image, flowerpotName, volumeCount, selectedDayOfTheWeek);
                await navigationService.GoBackAsync();
            }
            else
            {
                await pageDialogService.DisplayAlertAsync("Validation warning",
                    "Please fill all required data", "OK");
            }
        }

        private async Task CloseModalAsync()
        {
            await navigationService.GoBackAsync();
        }

        private bool ValidateFlowerpot()
        {
            if (string.IsNullOrEmpty(ImagePath) ||
                string.IsNullOrWhiteSpace(FlowerpotName) ||
                string.IsNullOrEmpty(SelectedDayOfTheWeek))
            {
                return false;
            }

            return true;
        }       
    }
}
