using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
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
                    jsonSelectedPropertyList = JsonConvert.SerializeObject(ViewModel.Properties),
                    requestedViewId = ViewModel.ViewInstanceId
                }) as PropertyPickerViewController;

                NavigationController.PushViewController(controller, true);
            });

            PhotoImageTapGestureRecognizer.AddTarget(() =>
            {
                if(_editMode)
                    ViewModel.ChoosePictureCommand.Execute(null);
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

            EditMode = false;
        }

        public void MakeFieldsEditable()
        {
            EditMode = true;
        }

        public void MakeFieldsReadOnly()
        {
            EditMode = false;
        }

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                FullNameTextField.UserInteractionEnabled = _editMode;
                UsernameTextField.UserInteractionEnabled = _editMode;
                PasswordTextField.UserInteractionEnabled = _editMode;
                ConfirmPasswordTextField.UserInteractionEnabled = _editMode; 
            }
            
        }
        
	   
    }
}