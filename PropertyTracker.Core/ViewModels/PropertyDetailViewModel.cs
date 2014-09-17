using System;
using PropertyTracker.Core.Services;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Dto.Models;
using Acr.MvvmCross.Plugins.UserDialogs;
using Newtonsoft.Json;

namespace PropertyTracker.Core.ViewModels
{
	public class PropertyDetailViewModel : BasePropertyViewModel
	{
		public PropertyDetailViewModel (IPropertyTrackerService service, IUserDialogService dialogService, IMvxMessenger messenger) : 
		base(service, dialogService, messenger)
		{

		}

		private Property _property;
		public Property Property {
			get { return _property; }
			set {
				_property = value;
				InitPropertyFields ();
			}
		}

		public void Init(string jsonProperty)
		{
			Property = JsonConvert.DeserializeObject<Property> (jsonProperty);
		}

		private void InitPropertyFields()
		{
			PropertyName = _property.Name;
			City = _property.City;
			State = _property.StateProvince;
			SquareFeet = _property.SquareFeet.ToString();
		}

		public IMvxCommand DeletePropertyCommand
		{
			get { return new MvxCommand(DeleteProperty); }
		}

		public IMvxCommand SavePropertyCommand
		{
			get { return new MvxCommand(SaveProperty, PropertyValidation); }
		}

		public IMvxCommand CancelCommand
		{
			get { return new MvxCommand (CancelAction); }
		}


		private void CancelAction()
		{
			InitPropertyFields ();
		}
			
		private async void SaveProperty()
		{

			Property updatedProperty = new Property()
			{
				Name = PropertyName,
				City = City,
				StateProvince = State,
				SquareFeet = Convert.ToInt32 (SquareFeet),
				CompanyId = _property.CompanyId,
				Country = _property.Country				
			};

			object response = null;

			using (_dialogService.Loading ("Saving Property...")) {
				response = await _propertyTrackerService.UpdateProperty (updatedProperty);
			}

			if (response is bool && (bool)response)
			{
				_dialogService.Alert("Property saved successfully", null, "OK", SavePropertySuccess);

				_property = updatedProperty;

				var message = new PropertiesUpdatedMessage(this) {
					Property = _property
				};
				_messenger.Publish(message);
			}
			else
			{
				var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to save Property";
				_dialogService.Alert(msg, "Request Error", "OK", SavePropertyFailed);	            	            
			}                          
		}

		private async void DeleteProperty()
		{
			object response = null;
			using (_dialogService.Loading ("Deleting Property...")) {
				response = await _propertyTrackerService.DeleteProperty (_property.Id);
			}

			if (response is Property)
			{
				_dialogService.Alert("Property deleted successfully", null, "OK", DeletePropertySuccess);

				var message = new PropertiesUpdatedMessage(this);
				_messenger.Publish(message);
			}
			else
			{
				var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to delete Property";
				_dialogService.Alert(msg, "Request Error", "OK", DeletePropertyFailed);	            	            
			} 
		}

		public event EventHandler SavePropertySuccessEventHandler;
		private void SavePropertySuccess()
		{
			if(SavePropertySuccessEventHandler != null)
				SavePropertySuccessEventHandler (this, EventArgs.Empty);
		}

		public event EventHandler SavePropertyFailedEventHandler;
		private void SavePropertyFailed()
		{
			if(SavePropertyFailedEventHandler != null)
				SavePropertyFailedEventHandler (this, EventArgs.Empty);
		}

		public event EventHandler DeletePropertySuccessEventHandler;
		private void DeletePropertySuccess()
		{
			if(DeletePropertySuccessEventHandler != null)
				DeletePropertySuccessEventHandler (this, EventArgs.Empty);
		}

		public event EventHandler DeletePropertyFailedEventHandler;
		private void DeletePropertyFailed()
		{
			if(DeletePropertyFailedEventHandler != null)
				DeletePropertyFailedEventHandler (this, EventArgs.Empty);
		}
	}
}

