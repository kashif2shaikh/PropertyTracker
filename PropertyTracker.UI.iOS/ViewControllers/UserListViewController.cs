
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
using PropertyTracker.Dto.Models;
using Newtonsoft.Json;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.Messenger;


namespace PropertyTracker.UI.iOS.ViewControllers
{
    public partial class UserListViewController : MvxTableViewController
    {
		private  IMvxMessenger _messenger;
              
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
			base.ViewDidLoad ();

			NavigationController.NavigationBarHidden = false;
			var logoutButton = new UIBarButtonItem ("Logout", UIBarButtonItemStyle.Bordered, null);
			NavigationItem.LeftBarButtonItem = logoutButton;

			var addUserButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, (o, e) => {
				var controller = this.CreateViewControllerFor<AddUserViewModel> () as AddUserViewController;
				NavigationController.PushViewController (controller, true);
			});
			NavigationItem.RightBarButtonItem = addUserButton;
          
			var source = new MvxStandardTableViewSource (TableView, UserListCell.Key);

			TableView.Source = source;

			this.SetTitleAndTabBarItem (ViewModel.TabTitle, ViewModel.TabImageName, ViewModel.TabSelectedImageName, ViewModel.TabBadgeValue);
            
			var set = this.CreateBindingSet<UserListViewController, UserListViewModel> ();
			set.Bind (source).To (vm => vm.Users);
			set.Bind (logoutButton).To (vm => vm.LogoutCommand);
			set.Bind (TabBarItem).For (v => v.Title).To (vm => vm.TabTitle);
			set.Bind (TabBarItem).For (v => v.BadgeValue).To (vm => vm.TabBadgeValue);
			//set.Bind(Title).To(vm => vm.TabTitle);
			//set.Bind(NavigationItem).For(v => v.Title).To(vm => vm.TabTitle);            
			set.Apply ();

			source.SelectedItemChanged += (object sender, EventArgs e) => {
				var controller = this.CreateViewControllerFor<UserDetailViewModel> (new
					{
						jsonUser = JsonConvert.SerializeObject(source.SelectedItem as User)
					}) as UserDetailViewController;
				NavigationController.PushViewController (controller, true);
				//var forceLoadView = controller.View; // force load view so we get ViewDidLoad and ViewModel initialized.
				//controller.ViewModel.User = source.SelectedItem as User;
			};

			// Data is fetched after
			TableView.ReloadData ();

			RegisterSubscriptions ();
			//ViewModel.Users.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => TableView.ReloadData ();
		}
			
		private MvxSubscriptionToken _usersUpdatedMessageToken;
		protected void RegisterSubscriptions()
		{
			_messenger = Mvx.Resolve<IMvxMessenger> ();
			_usersUpdatedMessageToken = _messenger.Subscribe<UsersUpdatedMessage> (OnUsersUpdatedMessaged);   
		}

		private void OnUsersUpdatedMessaged(UsersUpdatedMessage msg)
		{
			// Need to purge image cache 
			var imageCache = Mvx.Resolve<IMvxImageCache<UIImage>> ();
			imageCache.PurgeImage (msg.User.PhotoUrl);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

		}
    }

}