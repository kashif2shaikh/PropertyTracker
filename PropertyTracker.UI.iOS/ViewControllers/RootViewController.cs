
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using PropertyTracker.Core;
using PropertyTracker.Core.ViewModels;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class RootViewController : MvxViewController
	{

		public new RootViewModel ViewModel
		{
			get { return (RootViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}

		/* Not loading XIB
		public RootViewController () : base ("RootViewController", null)
		{
		}
		*/

		// Controller loaded from Storyboard
		public RootViewController (IntPtr handle) : base (handle)
		{

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}

