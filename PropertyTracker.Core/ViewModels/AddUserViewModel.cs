using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.Services;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Core.ViewModels
{
	public class AddUserViewModel : BaseUserViewModel
	{
	    public AddUserViewModel(IPropertyTrackerService service, IUserDialogService dialogService, IMvxPictureChooserTask pictureChooserTask, IMvxMessenger messenger) : 
            base(service, dialogService, pictureChooserTask, messenger)
	    {
          
		}

	    public IMvxCommand AddUserCommand
        {
            get { return new MvxCommand(AddUser, AddUserValidation); }
        }

	    private async void AddUser()
	    {
	        User newUser = new User()
	        {
	            Fullname = FullName,
                Username = Username,
                Password = Password,
                Properties = Properties,
                Company = _propertyTrackerService.LoggedInUser.Company,
	        };

	        object response = null;
			object imageResponse = null;
			bool uploadedImage = false;
			using (_dialogService.Loading ("Adding user...")) {
				response = await _propertyTrackerService.AddUser (newUser);
				if(response is User && PhotoDataBytes != null && PhotoDataBytes.Length > 0) {
					imageResponse = await _propertyTrackerService.UploadUserPhoto ((response as User).Id, PhotoDataBytes);
					uploadedImage = true;
				}
			}

	        if (response is User)
	        {
				bool alertDisplayed = false;
				if(uploadedImage) {
					if(imageResponse is ErrorResult) {
						_dialogService.Alert ((imageResponse as ErrorResult).Message, "Photo Upload Failed (User Add Successful)", "OK", AddUserSuccess);
						alertDisplayed = true;
					}
					else if(imageResponse == null) {
						_dialogService.Alert ("Photo Upload Failed (User was added successfully)", "Request Failed", "OK", AddUserSuccess);
						alertDisplayed = true;
					}
				}
					
				if(alertDisplayed == false)
	            	_dialogService.Alert("User added successfully", null, "OK", AddUserSuccess);
	        }
	        else
	        {
	            var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to add new user";
                _dialogService.Alert(msg, "Request Error", "OK", AddUserFailed);	            	            
	        }                          
	    }

        public event EventHandler AddUserSuccessEventHandler;
	    private void AddUserSuccess()
	    {
			if(AddUserSuccessEventHandler != null)
				AddUserSuccessEventHandler (this, EventArgs.Empty);
	    }

        public event EventHandler AddUserFailedEventHandler;
	    private void AddUserFailed()
	    {
			if(AddUserFailedEventHandler != null)
				AddUserFailedEventHandler (this, EventArgs.Empty);
	    }

	    private bool AddUserValidation()
	    {
	        string msg = null;
	        if (String.IsNullOrWhiteSpace(FullName))
	            msg = "Full name is empty";

            else if (String.IsNullOrWhiteSpace(Username))
                msg = "Username is empty";

            else if (String.IsNullOrWhiteSpace(Password))
                msg = "Password is empty";

            else if (String.IsNullOrWhiteSpace(ConfirmPassword))
                msg = "Password confirmation is empty";

            else if (!Password.Equals(ConfirmPassword))
                msg = "Password and confirmation don't match";

            if(msg != null)
	            _dialogService.Alert(msg, "Validation Error");

	        return msg == null;
	    }
	}
}

