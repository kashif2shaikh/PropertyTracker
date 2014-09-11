
using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.Common;
using PropertyTracker.UI.iOS.Views;

// Make sure namespace is same in designer.cs - Xamarin skips adding subfolders to namespace!
namespace PropertyTracker.UI.iOS.ViewControllers
{
    public partial class UserListViewController : MvxTableViewController
    {
        private const string UserCellId = "UserCell";
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

            NavigationController.NavigationBarHidden = false;
            var logoutButton = new UIBarButtonItem("Logout", UIBarButtonItemStyle.Bordered, null);
            NavigationItem.LeftBarButtonItem = logoutButton;

			var addUserButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, (o, e) => {
				var controller = this.CreateViewControllerFor<AddUserViewModel>() as AddUserViewController;
				NavigationController.PushViewController(controller, true);
			});
			NavigationItem.RightBarButtonItem = addUserButton;
          
			var source = new MvxStandardTableViewSource (TableView, UserListCell.Key);

			//var source = new CustomTableSource(TableView, UITableViewCellStyle.Subtitle, new NSString(UserCellId), "TitleText Fullname;DetailText Username;ImageUrl PhotoUrl;",
            //    UITableViewCellAccessory.DisclosureIndicator);

            TableView.Source = source;

            this.SetTitleAndTabBarItem(ViewModel.TabTitle, ViewModel.TabImageName, ViewModel.TabSelectedImageName, ViewModel.TabBadgeValue);
            
            var set = this.CreateBindingSet<UserListViewController, UserListViewModel>();
            set.Bind(source).To(vm => vm.Users);
            set.Bind(logoutButton).To(vm => vm.LogoutCommand);
            set.Bind(TabBarItem).For(v => v.Title).To(vm => vm.TabTitle);
            set.Bind(TabBarItem).For(v => v.BadgeValue).To(vm => vm.TabBadgeValue);
            //set.Bind(Title).To(vm => vm.TabTitle);
            //set.Bind(NavigationItem).For(v => v.Title).To(vm => vm.TabTitle);            
            set.Apply();

            // Data is fetched after
            TableView.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

           
        }       	        
    }

    // Need to subclass so we can customize UIImageView. We could also have created a new User Cell XIB, but that would be way more
    // monkey-work.
    class CustomTableSource : MvxStandardTableViewSource
    {
        public CustomTableSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, string bindingText,
            UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None) : base(tableView,style,cellIdentifier,bindingText,tableViewCellAccessory)
        {
            
        }
        
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = base.GetOrCreateCellFor(tableView, indexPath, item);
            cell.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            cell.ImageView.ClipsToBounds = true;
            var frame = cell.ImageView.Frame;
            frame.Width = 64;
            frame.Height = 64;
            cell.ImageView.Frame = frame;
            return cell;
        }
    }
}