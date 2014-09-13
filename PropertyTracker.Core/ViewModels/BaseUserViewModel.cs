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
    public abstract class BaseUserViewModel : BaseViewModel
    {
        protected IPropertyTrackerService _propertyTrackerService;
        protected IUserDialogService _dialogService;
        protected IMvxPictureChooserTask _pictureChooserTask;
        protected IMvxMessenger _messenger;
        private MvxSubscriptionToken _propertyPickerToken;
        private string _fullname;
        private string _username;
        private string _password;
        private string _confirmPassword;
        protected List<Property> _properties;
        private MvxCommand _takePictureCommand;
        private MvxCommand _choosePictureCommand;
        private byte[] _photoDataBytes;

        protected BaseUserViewModel(IPropertyTrackerService service, IUserDialogService dialogService, IMvxPictureChooserTask pictureChooserTask, IMvxMessenger messenger)
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;
            _pictureChooserTask = pictureChooserTask;
            _messenger = messenger;
            _properties = new List<Property>();
            RegisterSubscriptions();
        }

        public string CompanyName
        {
            get { return _propertyTrackerService.LoggedInUser.Company.Name; }
        }

        public string FullName
        {
            get { return _fullname; }
            set 
            { 
                _fullname = value;
                RaisePropertyChanged(() => FullName);
            }
        }

        public string Username
        {
            get { return _username; }
            set 
            { 
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set 
            { 
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        public List<Property> Properties
        {
            get { return _properties; }
            set 
            { 
                _properties = value;
                RaisePropertyChanged(() => Properties);
            }
        }

        public ICommand TakePictureCommand
        {
            get
            {
                _takePictureCommand = _takePictureCommand ?? new MvxCommand(DoTakePicture);
                return _takePictureCommand;
            }
        }

        public ICommand ChoosePictureCommand
        {
            get
            {
                _choosePictureCommand = _choosePictureCommand ?? new MvxCommand(DoChoosePicture);
                return _choosePictureCommand;
            }
        }

        public byte[] PhotoDataBytes
        {
            get { return _photoDataBytes; }
            set { _photoDataBytes = value; RaisePropertyChanged(() => PhotoDataBytes); }
        }

        protected void RegisterSubscriptions()
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

        private void DoTakePicture()
        {
            _pictureChooserTask.TakePicture(128, 60, OnPicture, () => { });
        }

        private void DoChoosePicture()
        {
            _pictureChooserTask.ChoosePictureFromLibrary(128, 60, OnPicture, OnPictureCancelled);
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