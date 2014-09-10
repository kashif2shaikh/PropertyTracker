using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.Services;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Core.ViewModels
{
	public class AddUserViewModel : BaseViewModel
	{
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;
        private readonly IMvxPictureChooserTask _pictureChooserTask;

        public AddUserViewModel(IPropertyTrackerService service, IUserDialogService dialogService, IMvxPictureChooserTask pictureChooserTask) 
		{
            _propertyTrackerService = service;
            _dialogService = dialogService;
            _pictureChooserTask = pictureChooserTask;
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
            using (_dialogService.Loading("Adding user..."))
                response = await _propertyTrackerService.AddUser(newUser);

	        if (response is User)
	        {
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
			AddUserSuccessEventHandler (this, EventArgs.Empty);
	    }

        public event EventHandler AddUserFailedEventHandler;
	    private void AddUserFailed()
	    {
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
            _pictureChooserTask.TakePicture(400, 95, OnPicture, () => { });
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
            _pictureChooserTask.ChoosePictureFromLibrary(400, 95, OnPicture, () => { });
        }

        private byte[] _photoDataBytes;
        public byte[] PhotoDataBytes
        {
            get { return _photoDataBytes; }
            set { _photoDataBytes = value; RaisePropertyChanged(() => PhotoDataBytes); }
        }


        private void OnPicture(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            PhotoDataBytes = memoryStream.ToArray();
        }



	}
}

