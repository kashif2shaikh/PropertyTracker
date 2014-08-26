using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.Services;

namespace PropertyTracker.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IPropertyTrackerService _propertyTrackerService;

        public LoginViewModel(IPropertyTrackerService service)
        {
            _propertyTrackerService = service;
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

        private void Login()
        {
            if (!_propertyTrackerService.LoggedIn)
            {
				_propertyTrackerService.Login(Username, Password).Wait ();
				//Console.WriteLine("Finished login:" + response);
            }           
        }

        /*
        async void Login()
        {

            BTProgressHUD.Show("Logging in...");

            var success = await WebService.Shared.Login(username, password);
            if (success)
            {
                var canContinue = await WebService.Shared.PlaceOrder(WebService.Shared.CurrentUser, true);
                if (!canContinue.Success)
                {
                    new UIAlertView("Sorry", "Only one shirt per person. Edit your cart and try again.", null, "OK").Show();
                    BTProgressHUD.Dismiss();
                    return;
                }
            }

            BTProgressHUD.Dismiss();

            if (success)
            {
                LoginSucceeded();
            }
            else
            {
                var alert = new UIAlertView("Could Not Log In", "Please verify your Xamarin account credentials and try again", null, "OK");
                alert.Show();
                alert.Clicked += delegate
                {
                    LoginView.PasswordField.SelectAll(this);
                    LoginView.PasswordField.BecomeFirstResponder();
                };
            }
        }
        */
    }
}
