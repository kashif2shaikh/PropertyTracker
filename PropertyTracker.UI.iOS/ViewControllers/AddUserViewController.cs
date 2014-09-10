
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using PropertyTracker.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace PropertyTracker.UI.iOS.ViewControllers
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
            //set.Bind(TakeButton).To(vm => vm.TakePictureCommand);
            //set.Bind(ChooseButton).To(vm => vm.ChoosePictureCommand);
			//set.Bind(LoginButton).To(vm => vm.LoginCommand);
			set.Bind(CompanyNameTextField).To (vm => vm.CompanyName);
            set.Bind(FullNameTextField).To(vm => vm.FullName);
            set.Bind(UsernameTextField).To (vm => vm.Username);
			set.Bind(PasswordTextField).To (vm => vm.Password);
		    set.Bind(ConfirmPasswordTextField).To(vm => vm.ConfirmPassword);
		    set.Bind(AddButtonItem).To(vm => vm.AddUserCommand);
			set.Apply ();


			// Dismiss view controller after user added
			ViewModel.AddUserSuccessEventHandler += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);

			CancelButtonItem.Clicked += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);
		}
	}
}

