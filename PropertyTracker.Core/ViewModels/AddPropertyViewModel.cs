using System;
using PropertyTracker.Core.Services;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Dto.Models;
using Acr.MvvmCross.Plugins.UserDialogs;
using System.Collections.Generic;

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
			get { return new MvxCommand(AddProperty, PropertyValidation); }
		}

		private async void AddProperty()
		{
			// By default we add and associate property with the logged in user.
			User user = _propertyTrackerService.LoggedInUser;

			Property newProperty = new Property()
			{
				Name = PropertyName,
				City = City,
				StateProvince = State,
				SquareFeet = Convert.ToInt32 (SquareFeet),
				CompanyId = user.Company.Id,
				Country = user.Company.Country,
				Users = new List<User>{ user } 
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




	}
}

