
using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using PropertyTracker.Core;
using PropertyTracker.Core.ViewModels;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class MainViewController : MvxViewController
	{

		public new MainViewModel ViewModel
		{
			get { return (MainViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}

		
        /* Not needed - this is required when loading controller from XIB 
		public MainViewController () : base ("MainViewController", null)
		{

		}
        */
        
		

		// Controller loaded from Storyboard
		public MainViewController (IntPtr handle) : base (handle)
		{

		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            this.CreateBinding(LoginButton).To<MainViewModel>(vm => vm.GoCommand).Apply();            
        }

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}



        partial void LoginButton_TouchUpInside(UIButton sender)
        {

        }
	}
}

