using System;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core.ViewModels
{
	public class MainViewModel : MvxViewModel
	{
		public MainViewModel ()
		{

		}

        public IMvxCommand GoCommand
        {
            get { return new MvxCommand(() => ShowViewModel<LoginViewModel>()); }
        }
	}
}

