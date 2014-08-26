
using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.Common;
using PropertyTracker.UI.iOS.Views;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public partial class PropertyListViewController : MvxViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public PropertyListViewController(IntPtr handle)
            : base(handle)
        {
        }

        public new PropertyListViewModel ViewModel
        {
            get { return (PropertyListViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //NavigationController.NavigationBarHidden = false;

            var logoutButton = new UIBarButtonItem("Logout", UIBarButtonItemStyle.Bordered, null);
            NavigationItem.LeftBarButtonItem = logoutButton;
            
            this.SetTitleAndTabBarItem(ViewModel.TabTitle, ViewModel.TabImageName, ViewModel.TabSelectedImageName, ViewModel.TabBadgeValue);

            var set = this.CreateBindingSet<PropertyListViewController, PropertyListViewModel>();
            set.Bind(logoutButton).To(vm => vm.LogoutCommand);
            set.Bind(TabBarItem).For(v => v.Title).To(vm => vm.TabTitle);
            set.Bind(TabBarItem).For(v => v.BadgeValue).To(vm => vm.TabBadgeValue);
            //set.Bind(Title).To(vm => vm.TabTitle);
            //set.Bind(NavigationItem).For(v => v.Title).To(vm => vm.TabTitle);

            set.Apply();

            //NavigationItem.Title;

            
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

           //NavigationController.Nav
           //NavigationItem.BackBarButtonItem = new UIBarButtonItem("Logout", UIBarButtonItemStyle.Plain, (o, e) => { });
            // NavigationItem.BackBarButtonItem.Title = "Logout";

            //var navigationController = NavigationController;
           // var item = NavigationItem;

            //NavigationController.NavigationBar.PushNavigationItem(item, true);

            //item.LeftBarButtonItem = new UIBarButtonItem("Logout A", UIBarButtonItemStyle.Bordered, (o, e) => { });
            
            // We want to just share MainViewController's navigation bar.

            //NavigationController.NavigationBarHidden = false;
            //Console.WriteLine("Property Navigation Controller:" + NavigationController);
            

        }

        private void Logout()
        {
           
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //ViewModel.ChangeStuff();

        }
    }
}