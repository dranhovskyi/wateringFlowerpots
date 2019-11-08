using System;
using System.Collections.Generic;
using System.Windows.Input;
using Prism.Navigation;
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

        public List<string> DaysOfTheWeek { get; set; }

        private readonly INavigationService navigationService;
        private readonly IFlowerpotRepository flowerpotRepository;

        public NewFlowerpotPageViewModel(INavigationService navigationService,
            IFlowerpotRepository flowerpotRepository)
            : base(navigationService)
        {
            this.navigationService = navigationService;
            this.flowerpotRepository = flowerpotRepository;

            CloseModalCommand = new Command(CloseModalAsync);
            SaveCommand = new Command(SaveCommandAsync);
            DaysOfTheWeek = new List<string>(CustomHelpers.GetWeekdays(DayOfWeek.Monday));
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        private async void SaveCommandAsync(object obj)
        {
            var image = await ImagePath.GetBase64FromImageByteArrayAsync();
            await flowerpotRepository.AddNewFlowerpotAsync(image, flowerpotName, volumeCount, selectedDayOfTheWeek);
            await navigationService.GoBackAsync();
        }

        private async void CloseModalAsync(object obj)
        {
            await navigationService.GoBackAsync();
        }        
    }
}
