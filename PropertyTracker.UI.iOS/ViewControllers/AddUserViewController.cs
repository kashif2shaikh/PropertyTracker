
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using PropertyTracker.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace PropertyTracker.UI.iOS
{
	public partial class AddUserViewController : MvxTableViewController
	{
		public AddUserViewController(IntPtr handle)
			: base(handle)
		{

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public new AddUserViewModel ViewModel
		{
			get { return (AddUserViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.

			var set = this.CreateBindingSet<AddUserViewController, AddUserViewModel>();
			//set.Bind(LoginButton).To(vm => vm.LoginCommand);
			//set.Bind (UsernameTextField).To (vm => vm.Username);
			//set.Bind (PasswordTextField).To (vm => vm.Password);
			set.Apply ();
		}
	}
}

