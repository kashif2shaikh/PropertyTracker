using System;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
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
			return new StoryboardContainer();
		}

        protected override IMvxTouchViewPresenter CreatePresenter()
        {
            return new MyPresenter(ApplicationDelegate, Window);
        }
	}

    public class MyPresenter : MvxTouchViewPresenter
    {
        public MyPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {

        }

        protected override UINavigationController CreateNavigationController(UIViewController viewController)
        {
            var navBar = base.CreateNavigationController(viewController);
            navBar.NavigationBarHidden = true;
            return navBar;
        }

        public override void Show(Cirrious.MvvmCross.Touch.Views.IMvxTouchView view)
        {
            if (true)
            {
                
            }

            base.Show(view);
        }
    }
}

