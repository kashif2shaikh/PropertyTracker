using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
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
            get { return new MvxCommand(() => ShowViewModel<MainViewModel>()); }
        }
    }
}
