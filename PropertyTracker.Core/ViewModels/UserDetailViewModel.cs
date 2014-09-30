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
using Newtonsoft.Json;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using System.Threading.Tasks;

namespace PropertyTracker.Core.ViewModels
{
	public class UserDetailViewModel : BaseUserViewModel
	{
		private bool _newPicture ;
		private byte[] LastPhotoDataBytes;

		public UserDetailViewModel(IPropertyTrackerService service, IUserDialogService dialogService, IMvxPictureChooserTask pictureChooserTask, IMvxMessenger messenger) : 
            base(service, dialogService, pictureChooserTask, messenger)
	    {
          
		}

	    public IMvxCommand SaveUserCommand
        {
            get { return new MvxCommand(SaveUser, SaveUserValidation); }
        }

		public IMvxCommand CancelCommand
		{
			get { return new MvxCommand (CancelAction); }
		}

		private User _user;
		public User User {
			get { return _user; }
			set {
				_user = value;
				InitUserFields ();
			}
		}

		public void Init(string jsonUser)
		{
			User = JsonConvert.DeserializeObject<User> (jsonUser);
		}

		private void InitUserFields()
		{
			UserId = _user.Id;
			FullName = _user.Fullname;
			Username = _user.Username;
			Password = "";
			ConfirmPassword = "";
			Properties = _user.Properties;
		}

		public async override void Start ()
		{
			base.Start ();

			object response = await _propertyTrackerService.DownloadUserPhoto (_user);
			if(response is byte[]) {
				PhotoDataBytes = response as byte[];
			}
		}

		private void CancelAction()
		{
			InitUserFields ();
			if(_newPicture) {
				PhotoDataBytes = LastPhotoDataBytes;
				LastPhotoDataBytes = null;
				_newPicture = false;
			}
		}

		protected override void OnPicture(Stream pictureStream)
		{
			LastPhotoDataBytes = PhotoDataBytes;
			_newPicture = true;
			base.OnPicture (pictureStream);
		}
				
	    private async void SaveUser()
	    {
	        User savedUser = new User()
	        {
				Id = UserId,
	            Fullname = FullName,
                Username = Username,
				Password = String.IsNullOrWhiteSpace(Password) ? null : Password,
                Properties = Properties,
                Company = _propertyTrackerService.LoggedInUser.Company,
	        };


	        object response = null;
			object imageResponse = null;
			bool uploadedImage = false;
			using (_dialogService.Loading ("Saving user...")) {
				response = await _propertyTrackerService.UpdateUser (savedUser);

				if(response is bool && (bool)response && _newPicture && PhotoDataBytes != null && PhotoDataBytes.Length > 0) {
					imageResponse = await _propertyTrackerService.UploadUserPhoto (_user.Id, PhotoDataBytes);
					uploadedImage = true;
				}
			}

			if (response is bool && (bool)response)
			{
				bool alertDisplayed = false;
				if(uploadedImage) {
					if(imageResponse is ErrorResult) {
						_dialogService.Alert ((imageResponse as ErrorResult).Message, "Photo Upload Failed (User Save Successful)", "OK", SaveUserSuccess);
						alertDisplayed = true;
					}
					else if(imageResponse == null) {
						_dialogService.Alert ("Photo Upload Failed (User was saved successfully)", "Request Failed", "OK", SaveUserSuccess);
						alertDisplayed = true;
					}
				}

				if(alertDisplayed == false)
					_dialogService.Alert("User saved successfully", null, "OK", SaveUserSuccess);
					
				UserUpdated (savedUser);
			}
			else
			{
				var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to save user";
				_dialogService.Alert(msg, "Request Error", "OK", SaveUserFailed);	            	            
			}                                
	    }

		private void UserUpdated(User savedUser)
		{
			// Photo url does not change if photo is updated, it is a fixed path to photo resource for user
			savedUser.PhotoUrl = _user.PhotoUrl;
			_user = savedUser;
		  
			var message = new UsersUpdatedMessage(this) {
				User = _user
			};
			_messenger.Publish(message);

			LastPhotoDataBytes = null;
			_newPicture = false;
		}

        public event EventHandler SaveUserSuccessEventHandler;
	    private void SaveUserSuccess()
	    {
			if(SaveUserSuccessEventHandler != null)
				SaveUserSuccessEventHandler (this, EventArgs.Empty);
	    }

        public event EventHandler SaveUserFailedEventHandler;
	    private void SaveUserFailed()
	    {
			if(SaveUserFailedEventHandler != null)
				SaveUserFailedEventHandler (this, EventArgs.Empty);
	    }

		private bool SaveUserValidation()
	    {
	        string msg = null;
	        if (String.IsNullOrWhiteSpace(FullName))
	            msg = "Full name is empty";

            else if (String.IsNullOrWhiteSpace(Username))
                msg = "Username is empty";
				
            if (!String.IsNullOrWhiteSpace(Password))
			{
				if (String.IsNullOrWhiteSpace(ConfirmPassword))
					msg = "Password confirmation is empty";

				else if (!Password.Equals(ConfirmPassword))
					msg = "Password and confirmation don't match";
			}                 		          

            if(msg != null)
	            _dialogService.Alert(msg, "Validation Error");

	        return msg == null;
	    }
	}
}

