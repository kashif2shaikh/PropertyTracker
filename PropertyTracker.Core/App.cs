using System;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core
{
	public class App : MvxApplication
	{
		public App ()
		{
			//Mvx.RegisterType<ICalculation, Calculation> ();

			// Bootstrap and load the initial root view. Does this work.
			RegisterAppStart<PropertyTracker.Core.RootViewModel>();
		}
	}
}

