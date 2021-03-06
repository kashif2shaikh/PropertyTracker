﻿using System;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;

// Create a new Container to load view controllers from Storyboard. 
// For our convention, we mirror XIB-based views, where there will be *one* Storyboard for each View Controller, 
// this view controller will be marked as the "Initial" view controller.
//
// Eg. to load "SampleViewController", your storyboard should be called "SampleViewController.storyboard", and should
// only have a single view controller
//
// Thanks to http://stackoverflow.com/a/22128810 for the idea.
//
//
// NOTE: This container will fallback to load from XIB if storyboard is not found for a view.
//

using Cirrious.CrossCore.Exceptions;
using MonoTouch.Foundation;


namespace PropertyTracker.UI.iOS
{
	public class AppContainer : MvxTouchViewsContainer
	{
		public AppContainer () : base()
		{

		}

		protected  override IMvxTouchView CreateViewOfType(Type viewType, MvxViewModelRequest request)
		{
			// If View is "SampleViewController", then Storyboard should be "SampleViewController.storyboard"
			// If we can't find storyboard, then proceed to load XIB..
			//
			// NOTE: You must have this ctor defined for Storyboard: 
			//
			//   public YourViewController (IntPtr handle) : base (handle)
			//
            // As indicated here: http://developer.xamarin.com/guides/ios/user_interface/introduction_to_storyboards/
            //

		    IMvxTouchView viewController = null;

			if (NSBundle.MainBundle.PathForResource (viewType.Name, "storyboardc") != null) {
				// If storyboard exists for this view, load it.
				System.Console.WriteLine ("Creating view from Storyboard {0}.storyboard", viewType.Name);
				var storyboard = UIStoryboard.FromName(viewType.Name, null);
				viewController = (IMvxTouchView)storyboard.InstantiateInitialViewController();
			}
			else {
				System.Console.WriteLine ("Storyboard {0}.storyboard does not exist: creating view of type instead", viewType.Name);
				viewController = (IMvxTouchView)base.CreateViewOfType(viewType, request);
			}

			if(viewController == null) {
				System.Console.WriteLine ("Can't create view for type:" + viewType.Name);
				throw new Exception ("Can't create view for type:" + viewType.Name);
			}									
			return viewController;
		}
	}
}

