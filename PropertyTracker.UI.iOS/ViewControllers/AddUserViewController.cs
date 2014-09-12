
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
			//set.Bind(PhotoImageTapGestureRecognizer).To(vm => vm.ChoosePictureCommand);
			//set.Bind(LoginButton).To(vm => vm.LoginCommand);
			set.Bind(CompanyNameTextField).To (vm => vm.CompanyName);
            set.Bind(FullNameTextField).To(vm => vm.FullName);
            set.Bind(UsernameTextField).To (vm => vm.Username);
			set.Bind(PasswordTextField).To (vm => vm.Password);
		    set.Bind(ConfirmPasswordTextField).To(vm => vm.ConfirmPassword);
		    set.Bind(AddButtonItem).To(vm => vm.AddUserCommand);
			set.Bind(PhotoImageView).To(vm => vm.PhotoDataBytes).WithConversion("InMemoryImage");
			set.Apply ();

			PropertiesTapGestureRecognizer.AddTarget(() => {
				var controller = this.CreateViewControllerFor<PropertyPickerViewModel>(new 
					{
						requestedViewId = ViewModel.ViewInstanceId
					}) as PropertyPickerViewController;

				NavigationController.PushViewController(controller, true);	
			});

			PhotoImageTapGestureRecognizer.AddTarget(() => {
				ViewModel.ChoosePictureCommand.Execute (null);
			});				

			ViewModel.OnPictureEventHandler += (object sender, EventArgs e) => {
				PlaceholderImageView.Hidden = true;
			};

			ViewModel.OnPictureCancelledEventHandler += (object sender, EventArgs e) => {

			};

			// Dismiss view controller after user added
			ViewModel.AddUserSuccessEventHandler += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);

			CancelButtonItem.Clicked += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);

			FullNameTextField.ShouldReturn += (textField) => {
				textField.ResignFirstResponder ();
				UsernameTextField.BecomeFirstResponder ();
				return true; 
			};

			UsernameTextField.ShouldReturn += (textField) => {
				textField.ResignFirstResponder ();
				PasswordTextField.BecomeFirstResponder ();
				return true; 
			};

			PasswordTextField.ShouldReturn += (textField) => {
				textField.ResignFirstResponder ();
				ConfirmPasswordTextField.BecomeFirstResponder ();
				return true; 
			};

			// Done button pressed
			ConfirmPasswordTextField.ShouldReturn += (textField) => { 
				textField.ResignFirstResponder ();
				return true; 
			};
		}
	}
}

