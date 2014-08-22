
using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.Views;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public partial class UserListViewController : MvxViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public UserListViewController(IntPtr handle)
            : base(handle)
        {
        }

        public new UserListViewModel ViewModel
        {
            get { return (UserListViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

           

           
            
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //NavigationController.NavigationBarHidden = false;

            NavigationController.NavigationBarHidden = false;
            var logoutButton = new UIBarButtonItem("Logout", UIBarButtonItemStyle.Bordered, null);
            NavigationItem.LeftBarButtonItem = logoutButton;
            NavigationItem.Title = "Users";
            /*
            var source = new MvxStandardTableViewSource(TableView, "TitleText Name");
            TableView.Source = source;
            
            var set = this.CreateBinding(UserListViewController, 
            set.Bind(logoutButton).To<PropertyListViewModel>(vm => vm.LogoutCommand).Apply();
            set.Bind(source).To(vm => vm.Kittens);
            set.Apply();
            

            TableView.ReloadData();
            */
           
        }

        private void Logout()
        {

        }
    }
}