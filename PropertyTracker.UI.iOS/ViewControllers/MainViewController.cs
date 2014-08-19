
using System;
using System.Drawing;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using PropertyTracker.Core;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.Views;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class MainViewController : MvxViewController, IPresentView
	{

		public new MainViewModel ViewModel
		{
			get { return (MainViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public MainViewController (IntPtr handle) : base (handle)
		{

		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            this.CreateBinding(LogoutButton).To<MainViewModel>(vm => vm.ShowLoginViewCommand).Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = true;
        }

        /* IPresentView Methods */
	    public bool CanPresentView(IMvxTouchView view)
	    {
            // Don't Present View - let app presenter handle it.
	        return false;
	        //return view.Request.RequestedBy.AdditionalInfo == ViewModel.ViewInstanceId.ToString();
	    }

	    public void PresentView(IMvxTouchView view)
        {
            // We don't really load any views here
            //PresentViewController(view as UIViewController, true, () => { });
        }
	}
}

