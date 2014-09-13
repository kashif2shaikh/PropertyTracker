using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.Services;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Core.ViewModels
{
    public class UserDetailViewModel : BaseViewModel
    {
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;
        private readonly IMvxPictureChooserTask _pictureChooserTask;
        private readonly IMvxMessenger _messenger;

        public UserDetailViewModel(IPropertyTrackerService service, IUserDialogService dialogService, IMvxPictureChooserTask pictureChooserTask, IMvxMessenger messenger) 
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;
            _pictureChooserTask = pictureChooserTask;
            _messenger = messenger;
            _properties = new List<Property>();
            RegisterSubscriptions();
        }

        private MvxSubscriptionToken _propertyPickerToken;        
        private void RegisterSubscriptions()
        {
            _propertyPickerToken = _messenger.Subscribe<PropertyPickerMessage>(OnPropertyPickeryMessage);            
        }

        private void OnPropertyPickeryMessage(PropertyPickerMessage msg)
        {
            var pickerView = msg.Sender as PropertyPickerViewModel;

            // Only handle city picker msg if we were expecting it.
            if (pickerView != null && ViewInstanceId.Equals(pickerView.RequestedByViewInstanceId))
            {
                Properties = msg.Properties;
            }
        }
			
        public string CompanyName
        {
            get { return _propertyTrackerService.LoggedInUser.Company.Name; }
        }

        private string _fullname;
        public string FullName
        {
            get { return _fullname; }
            set 
            { 
                _fullname = value;
                RaisePropertyChanged(() => FullName);
            }
        }
        
        private string _username;
        public string Username
        {
            get { return _username; }
            set 
            { 
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set 
            { 
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        private List<Property> _properties;
        public List<Property> Properties
        {
            get { return _properties; }
            set 
            { 
                _properties = value;
                RaisePropertyChanged(() => Properties);
            }
        }
	    
        public IMvxCommand SaveUserCommand
        {
            get { return new MvxCommand(SaveUser, SaveUserValidation); }
        }

        private async void SaveUser()
        {
            User updateUser = new User()
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
            using (_dialogService.Loading ("Saving user...")) {
                response = await _propertyTrackerService.UpdateUser(updateUser);
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
                        _dialogService.Alert ((imageResponse as ErrorResult).Message, "Photo Upload Failed (User Save Successful)", "OK", AddUserSuccess);
                        alertDisplayed = true;
                    }
                    else if(imageResponse == null) {
                        _dialogService.Alert ("Photo Upload Failed (User was saved successfully)", "Request Failed", "OK", AddUserSuccess);
                        alertDisplayed = true;
                    }
                }
					
                if(alertDisplayed == false)
                    _dialogService.Alert("User saved successfully", null, "OK", AddUserSuccess);
            }
            else
            {
                var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to add new user";
                _dialogService.Alert(msg, "Request Error", "OK", SaveUserFailed);	            	            
            }                          
        }

        public event EventHandler SaveUserSuccessEventHandler;
        private void AddUserSuccess()
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

        private MvxCommand _takePictureCommand;
        public ICommand TakePictureCommand
        {
            get
            {
                _takePictureCommand = _takePictureCommand ?? new MvxCommand(DoTakePicture);
                return _takePictureCommand;
            }
        }

        private void DoTakePicture()
        {
            _pictureChooserTask.TakePicture(128, 60, OnPicture, () => { });
        }

        private MvxCommand _choosePictureCommand;
        public ICommand ChoosePictureCommand
        {
            get
            {
                _choosePictureCommand = _choosePictureCommand ?? new MvxCommand(DoChoosePicture);
                return _choosePictureCommand;
            }
        }

        private void DoChoosePicture()
        {
            _pictureChooserTask.ChoosePictureFromLibrary(128, 60, OnPicture, OnPictureCancelled);
        }

        private byte[] _photoDataBytes;
        public byte[] PhotoDataBytes
        {
            get { return _photoDataBytes; }
            set { _photoDataBytes = value; RaisePropertyChanged(() => PhotoDataBytes); }
        }

        public event EventHandler OnPictureEventHandler;
        private void OnPicture(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            PhotoDataBytes = memoryStream.ToArray();

            if(OnPictureEventHandler != null)
            {
                OnPictureEventHandler (this, EventArgs.Empty);
            }
        }

        public event EventHandler OnPictureCancelledEventHandler;
        private void OnPictureCancelled()
        {
            if(OnPictureCancelledEventHandler != null)
            {
                OnPictureCancelledEventHandler (this, EventArgs.Empty);
            }
        }
    }
}