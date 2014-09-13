
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Newtonsoft.Json;
using PropertyTracker.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class AddUserViewController : BaseUserViewController
	{	    
		public AddUserViewController(IntPtr handle)
			: base(handle)
		{

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

            // Most bindings are done by the base user view controller
			var set = this.CreateBindingSet<AddUserViewController, AddUserViewModel>();

		    set.Bind(AddButtonItem).To(vm => vm.AddUserCommand);
			
			set.Apply ();

			// Dismiss view controller after user added
			ViewModel.AddUserSuccessEventHandler += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);

			CancelButtonItem.Clicked += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);	
		}
	}
}

