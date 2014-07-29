using System;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using PropertyTracker.Core;
using Cirrious.MvvmCross.Touch.Views;

namespace PropertyTracker.UI.iOS
{
	public class Setup : MvxTouchSetup
	{
		public Setup (MvxApplicationDelegate appDelegate, IMvxTouchViewPresenter presenter) : base (appDelegate, presenter)
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
	}
}

