
using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.Views;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public partial class LoginViewController : MvxViewController, IViewPresenter
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public LoginViewController(IntPtr handle)
            : base(handle)
        {
        }

        public LoginViewController()
        {
            
        }

        public new LoginViewModel ViewModel
        {
            get { return (LoginViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        /* View LifeCycle Methods */
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

			var set = this.CreateBindingSet<LoginViewController, LoginViewModel>();
			set.Bind(LoginButton).To(vm => vm.LoginCommand);
			set.Bind (UsernameTextField).To (vm => vm.Username);
			set.Bind (PasswordTextField).To (vm => vm.Password);
			set.Apply ();		     
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //NavigationController.NavigationBarHidden = false;
        }

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear (animated);
			ViewModel.Username = "kshaikh";
			ViewModel.Password = "1234";
		}

        /* IViewPresenter Methods */
        public void ViewPresenterAdded()
        {
           // nothing to do
        }

        public void ViewPresenterRemoved()
        {
            
        }

        public bool CanPresentView(IMvxTouchView view)
        {
            // We don't load any views!
            return false;            
        }

        public void PresentView(IMvxTouchView view)
        {
            
        }       
    }
}