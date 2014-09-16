using System;
using PropertyTracker.Core.Services;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Dto.Models;
using Acr.MvvmCross.Plugins.UserDialogs;

namespace PropertyTracker.Core.ViewModels
{
	public class AddPropertyViewModel : BasePropertyViewModel
	{
		public AddPropertyViewModel (IPropertyTrackerService service, IUserDialogService dialogService, IMvxMessenger messenger) : 
		base(service, dialogService, messenger)
		{

		}

		public IMvxCommand AddPropertyCommand
		{
			get { return new MvxCommand(AddProperty, AddPropertyValidation); }
		}

		private async void AddProperty()
		{

			Property newProperty = new Property()
			{
				Name = PropertyName,
				City = City,
				StateProvince = State,
				SquareFeet = Convert.ToInt32 (SquareFeet),
				CompanyId = _propertyTrackerService.LoggedInUser.Company.Id,
				Country = _propertyTrackerService.LoggedInUser.Company.Country				
			};

			object response = null;

			using (_dialogService.Loading ("Adding Property...")) {
				response = await _propertyTrackerService.AddProperty (newProperty);
			}

			if (response is Property)
			{
				_dialogService.Alert("Property added successfully", null, "OK", AddPropertySuccess);

				var message = new PropertiesUpdatedMessage(this) {
					Property = response as Property
				};
				_messenger.Publish(message);
			}
			else
			{
				var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to add new Property";
				_dialogService.Alert(msg, "Request Error", "OK", AddPropertyFailed);	            	            
			}                          
		}

		public event EventHandler AddPropertySuccessEventHandler;
		private void AddPropertySuccess()
		{
			if(AddPropertySuccessEventHandler != null)
				AddPropertySuccessEventHandler (this, EventArgs.Empty);
		}

		public event EventHandler AddPropertyFailedEventHandler;
		private void AddPropertyFailed()
		{
			if(AddPropertyFailedEventHandler != null)
				AddPropertyFailedEventHandler (this, EventArgs.Empty);
		}

		private bool AddPropertyValidation()
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

