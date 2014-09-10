using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.Services;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;               

        public LoginViewModel(IPropertyTrackerService service, IUserDialogService dialogService)
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;
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


        public IMvxCommand LoginCommand
        {
            //get { return new MvxCommand(() => ShowViewModel<MainViewModel>()); }
            get
            {
                return new MvxCommand(Login);
            }
        }

        private IMvxCommand LoginFailedAlert
        {
            get
            {
                return new MvxCommand(() => _dialogService.Alert("Invalid credentials", "Login Failed", "OK"));
            }
        }

        private async void Login()
        {
            if (_propertyTrackerService.LoggedIn)
                ShowViewModel<MainViewModel>();

            object response = null;
            
            using (_dialogService.Loading("Logging in..."))
                response = await _propertyTrackerService.Login(Username, Password);

            if (response is LoginResponse)
                ShowViewModel<MainViewModel>();
            else
            {
                var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to Login";                                    
                _dialogService.Alert(msg, "Request Failed");
            }                            
        }       
    }
}
