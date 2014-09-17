using System;
using PropertyTracker.Core.Services;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace PropertyTracker.Core.ViewModels
{
	public class BasePropertyViewModel : BaseViewModel
	{
		protected readonly IPropertyTrackerService _propertyTrackerService;
		protected readonly IUserDialogService _dialogService;
		protected readonly IMvxMessenger _messenger;

		public BasePropertyViewModel (IPropertyTrackerService service, IUserDialogService dialogService, IMvxMessenger messenger)
		{
			_propertyTrackerService = service;
			_dialogService = dialogService;
			_messenger = messenger;

			RegisterSubscriptions ();
		}

		private MvxSubscriptionToken _cityPickerToken;
		private MvxSubscriptionToken _statePickerToken;
		private void RegisterSubscriptions()
		{
			_cityPickerToken = _messenger.Subscribe<CityPickerMessage>(OnCityPickerMessage);
			_statePickerToken = _messenger.Subscribe<StatePickerMessage>(OnStatePickerMessage);    
		}

		private void OnCityPickerMessage(CityPickerMessage msg)
		{
			var cityView = msg.Sender as CityPickerViewModel;

			// Only handle city picker msg if we were expecting it.
			if (cityView != null && ViewInstanceId.Equals (cityView.RequestedByViewInstanceId))
			{                
				City = msg.City;                
			} 
		}

		private void OnStatePickerMessage(StatePickerMessage msg)
		{
			var stateView = msg.Sender as StatePickerViewModel;

			// Only handle state picker msg if we were expecting it
			if (stateView != null && ViewInstanceId.Equals (stateView.RequestedByViewInstanceId))
			{
				State = msg.State;
			}
		}

		public string CompanyName
		{
			get { return _propertyTrackerService.LoggedInUser.Company.Name; }
		}

		private string _propertyName;
		public string PropertyName
		{
			get { return _propertyName; }
			set 
			{ 
				_propertyName = value;
				RaisePropertyChanged(() => PropertyName);
			}
		}

		private string _city;
		public string City
		{
			get { return _city; }
			set 
			{ 
				_city = value;
				RaisePropertyChanged(() => City);
			}
		}

		private string _state;
		public string State
		{
			get { return _state; }
			set 
			{ 
				_state = value;
				RaisePropertyChanged(() => State);
			}
		}

		private string _squareFeet;
		public string SquareFeet
		{
			get { return _squareFeet; }
			set 
			{ 
				_squareFeet = value;
				RaisePropertyChanged(() => SquareFeet);
			}
		}

		protected bool PropertyValidation()
		{
			string msg = null;
			int squareFeet = 0;

			if (String.IsNullOrWhiteSpace(PropertyName))
				msg = "Property name is empty";

			else if (String.IsNullOrWhiteSpace(City))
				msg = "Please select a city";

			else if (String.IsNullOrWhiteSpace(State))
				msg = "Please select a state/province";

			else if (String.IsNullOrWhiteSpace(SquareFeet))
				msg = "Square feet is empty";

			else if (!int.TryParse (SquareFeet, out squareFeet) || squareFeet < 0)
				msg = "Square feet is not valid";


			if(msg != null)
				_dialogService.Alert(msg, "Validation Error");

			return msg == null;
		}

	}
}

