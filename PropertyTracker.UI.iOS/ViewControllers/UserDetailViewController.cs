
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public partial class UserDetailViewController : BaseUserViewController
    {
        public UserDetailViewController(IntPtr handle)
            : base(handle)
        {
        }
			        
		public new UserDetailViewModel ViewModel
		{
			get { return (UserDetailViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
			   
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			var set = this.CreateBindingSet<UserDetailViewController, UserDetailViewModel>();
					
			set.Apply ();

			// Use ViewController Built-in edit button item 
			NavigationItem.RightBarButtonItem = EditButtonItem;


			EditButtonItem.Clicked += (object sender, EventArgs e) => {
				if (Editing) {
					// Done button pressed
					ViewModel.SaveUserCommand.Execute ();
				} else {
					// Edit button pressed
					SetEditing(true, false);
				}
			};
						
			// Go back to view mode after save
			ViewModel.SaveUserSuccessEventHandler += (object sender, EventArgs e) => {
				SetEditing (false, false);
			};

			ViewModel.SaveUserFailedEventHandler += (object sender, EventArgs e) => {
				// Continue
			};
				
			// By default view only mode.
			SetEditing (false, false);         
        }

		public override void SetEditing (bool editing, bool animated)
		{
			base.SetEditing (editing, animated);
			if(editing) {
				Title = "Edit User";
				var cancelButton = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, (s, e) => {
					SetEditing(false, false);
					ViewModel.CancelCommand.Execute ();
				});
				NavigationItem.LeftBarButtonItem = cancelButton;
			}
			else {
				Title = "User Details";
				NavigationItem.LeftBarButtonItem = null;
			}
		}
    }
}