using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.Services;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Core.ViewModels
{
    public class CityPickerViewModel : BaseViewModel
    {
        
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IGeoDataService _geoService;
        private readonly IUserDialogService _dialogService;
        private readonly IMvxMessenger _messenger;
        
        private ObservableCollection<string> _cities;
        public ObservableCollection<string> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;                
                RaisePropertyChanged(() => Cities);
            }
        }

        public CityPickerViewModel(IPropertyTrackerService propertyTrackerService, IGeoDataService geoService, IUserDialogService dialogService, IMvxMessenger messenger)
        {
            _propertyTrackerService = propertyTrackerService;
            _geoService = geoService;
            _dialogService = dialogService;
            _messenger = messenger;
            Cities = new ObservableCollection<string>();
        }

		public void Init(string city, Guid requestedViewId)
        {		
			if(requestedViewId != null)
			{
				RequestedByViewInstanceId = requestedViewId;
			}

            if (city != null)
            {
                SelectedCity = city;
            }				
        }

        public override void Start()
        {
            base.Start();
            LoadCities();
        }

        private void LoadCities()
        {
            var country = _propertyTrackerService.LoggedInUser.Company.Country;
            var countryCode = _geoService.GetCountryCode(country);
            var cities = _geoService.GetCities(countryCode);
            foreach(var city in cities)
            {
                Cities.Add(city);
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set 
            { 
                _selectedCity = value;
                RaisePropertyChanged(() => SelectedCity);
            }
        }

        public ICommand CityPickerDoneCommand
        {
            get { return new MvxCommand(SendCityPickerMessage); }
        }

        protected void SendCityPickerMessage()
        {            
            var message = new CityPickerMessage(this, SelectedCity);
            _messenger.Publish(message);
        }
    }

    public class CityPickerMessage : MvxMessage
    {
        public CityPickerMessage(object sender, string city) : base(sender)
        {
            City = city;
        }

        public string City { get; private set; }        
    }


}
