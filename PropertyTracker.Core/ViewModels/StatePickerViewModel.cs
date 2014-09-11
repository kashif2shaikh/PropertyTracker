using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.Services;

namespace PropertyTracker.Core.ViewModels
{
    public class StatePickerViewModel : BaseViewModel
    {
        
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IGeoDataService _geoService;
        private readonly IUserDialogService _dialogService;
        private readonly IMvxMessenger _messenger;
        
        private ObservableCollection<string> _states;
        public ObservableCollection<string> States
        {
            get { return _states; }
            set
            {
                _states = value;                
                RaisePropertyChanged(() => States);
            }
        }

        public StatePickerViewModel(IPropertyTrackerService propertyTrackerService, IGeoDataService geoService, IUserDialogService dialogService, IMvxMessenger messenger)
        {
            _propertyTrackerService = propertyTrackerService;
            _geoService = geoService;
            _dialogService = dialogService;
            _messenger = messenger;
            States = new ObservableCollection<string>();
        }

        public void Init(string state, Guid requestedViewId)
        {
            RequestedByViewInstanceId = requestedViewId;

            if (state != null)
            {
                SelectedState = state;
            }
        }

        public override void Start()
        {
            base.Start();
            LoadStates();
        }

        private void LoadStates()
        {
            var country = _propertyTrackerService.LoggedInUser.Company.Country;
            var countryCode = _geoService.GetCountryCode(country);
            var states = _geoService.GetStatesOrProvinces(countryCode);
            foreach(var state in states)
            {
                States.Add(state);
            }
        }

        private string _selectedState;
        public string SelectedState
        {
            get { return _selectedState; }
            set 
            { 
                _selectedState = value;
                RaisePropertyChanged(() => SelectedState);
            }
        }

        public ICommand StatePickerDoneCommand
        {
            get { return new MvxCommand(SendStatePickerMessage); }
        }

        protected void SendStatePickerMessage()
        {
            var message = new StatePickerMessage(this, SelectedState);
            _messenger.Publish(message);
        }
    }
    public class StatePickerMessage : MvxMessage
    {
        public StatePickerMessage(object sender, string state)
            : base(sender)
        {
            State = state;
        }

        public string State { get; private set; }
    }
}