using System;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core
{
	public class App : MvxApplication
	{
		public App ()
		{
            CreatableTypes()
            .EndingWith("Service")
            .AsInterfaces()
            .RegisterAsLazySingleton();

			//Mvx.RegisterType<ICalculation, Calculation> ();

			// Bootstrap and load the initial root view.
			RegisterAppStart<PropertyTracker.Core.ViewModels.LoginViewModel>();
		}
	}
}

