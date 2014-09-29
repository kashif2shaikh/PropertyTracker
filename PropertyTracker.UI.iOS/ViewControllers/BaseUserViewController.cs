using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using Newtonsoft.Json;
using PropertyTracker.Core.ViewModels;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public abstract partial class BaseUserViewController : MvxTableViewController
    {
        
        public BaseUserViewController(IntPtr handle)
            : base(handle)
        {

        }

        public override void DidReceiveMemoryWarning ()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning ();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public virtual new BaseUserViewModel ViewModel
        {
            get { return (BaseUserViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			
            // Perform any additional setup after loading the view, typically from a nib.

            var set = this.CreateBindingSet<BaseUserViewController, BaseUserViewModel>();
            //set.Bind(TakeButton).To(vm => vm.TakePictureCommand);
            //set.Bind(PhotoImageTapGestureRecognizer).To(vm => vm.ChoosePictureCommand);
            //set.Bind(LoginButton).To(vm => vm.LoginCommand);
            set.Bind<UITextField>(CompanyNameTextField).To (vm => vm.CompanyName);
            set.Bind<UITextField>(FullNameTextField).To(vm => vm.FullName);
            set.Bind<UITextField>(UsernameTextField).To (vm => vm.Username);
            set.Bind<UITextField>(PasswordTextField).To (vm => vm.Password);
            set.Bind<UITextField>(ConfirmPasswordTextField).To(vm => vm.ConfirmPassword);
           // set.Bind(AddButtonItem).To(vm => vm.AddUserCommand);
            set.Bind<UIImageView>(PhotoImageView).To(vm => vm.PhotoDataBytes).WithConversion("InMemoryImage");
            set.Apply ();

            PropertiesTapGestureRecognizer.AddTarget(() =>
            {
                var controller = this.CreateViewControllerFor<PropertyPickerViewModel>(new
                {
					viewOnlyMode = !Editing,
                    jsonSelectedPropertyList = JsonConvert.SerializeObject(ViewModel.Properties),
                    requestedViewId = ViewModel.ViewInstanceId,
					userId = ViewModel.UserId
                }) as PropertyPickerViewController;

                NavigationController.PushViewController(controller, true);
            });

            PhotoImageTapGestureRecognizer.AddTarget(() =>
            {
				if(Editing) {
					if(Runtime.Arch == Arch.DEVICE) {
						ViewModel.TakePictureCommand.Execute(null);
					}
						else if(Runtime.Arch == Arch.SIMULATOR) {
						ViewModel.ChoosePictureCommand.Execute(null);
					}
				}
                    
            });		

            ViewModel.OnPictureEventHandler += (object sender, EventArgs e) => {
            	PlaceholderImageView.Hidden = true;
            };

            ViewModel.OnPictureCancelledEventHandler += (object sender, EventArgs e) => {

            };

            // Dismiss view controller after user added
            //ViewModel.AddUserSuccessEventHandler += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);

           // CancelButtonItem.Clicked += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);

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

			SetEditing (true, false);
        }

		public override void SetEditing (bool editing, bool animated)
		{
			base.SetEditing (editing, animated);

			TableView.Editing = false; // don't show the cells in 'Edit mode' as the TableView inherits the editing flag from TableViewController

			MakeFieldsEditable (editing);
		}

		public void MakeFieldsEditable(bool editable)
        {
			FullNameTextField.UserInteractionEnabled = editable;
			UsernameTextField.UserInteractionEnabled = editable;
			PasswordTextField.UserInteractionEnabled = editable;
			ConfirmPasswordTextField.UserInteractionEnabled = editable; 
        }
    }
}