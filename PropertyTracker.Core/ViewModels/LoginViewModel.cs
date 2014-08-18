using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {        
        public string Username
        {
            get { return Username; }
            set { RaisePropertyChanged(() => Username); }
        }

        public string Password 
        {
            get { return Username; }
            set {  RaisePropertyChanged(() => Password); } 
        }

        public LoginViewModel()
        {
           
            
        }
    }
}
