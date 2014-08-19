using System;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.UIKit;
using PropertyTracker.Core;
using Cirrious.MvvmCross.Touch.Views;

namespace PropertyTracker.UI.iOS
{
	public class Setup : MvxTouchSetup
	{
        public Setup(MvxApplicationDelegate appDelegate, UIWindow window)
            : base(appDelegate, window)
		{
			// nothing to
		}

		protected override Cirrious.MvvmCross.ViewModels.IMvxApplication CreateApp()
		{
			return new App();
		}

		//Override to allow view controllers to be loaded from Storyboards.
		protected override IMvxTouchViewsContainer CreateTouchViewsContainer()
		{
			return new AppViewsContainer();
		}

        protected override IMvxTouchViewPresenter CreatePresenter()
        {
            return new AppViewPresenter(ApplicationDelegate, Window);
        }
	}
}

