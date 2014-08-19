using System;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel () : base()
		{

		}

        public IMvxCommand ShowLoginViewCommand
        {
            get { return new MvxCommand(() => ShowViewModel<LoginViewModel>()); }
        }
	}
}

