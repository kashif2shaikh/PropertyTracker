
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
    public partial class LoginViewController : MvxViewController, IPresentView
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
            this.CreateBinding(LoginButton).To<LoginViewModel>(vm => vm.LoginCommand).Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = false;
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