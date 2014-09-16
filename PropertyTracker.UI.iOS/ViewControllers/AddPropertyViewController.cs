
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using PropertyTracker.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class AddPropertyViewController : BasePropertyViewController
	{
		public AddPropertyViewController(IntPtr handle)
			: base(handle)
		{

		}

		public new AddPropertyViewModel ViewModel
		{
			get { return (AddPropertyViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
			
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var set = this.CreateBindingSet<AddPropertyViewController, AddPropertyViewModel>();
			set.Bind(AddButtonItem).To(vm => vm.AddPropertyCommand);
			set.Apply ();

			// Dismiss controller on successful add or cancel
			ViewModel.AddPropertySuccessEventHandler += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);

			CancelButtonItem.Clicked += (object sender, EventArgs e) => NavigationController.PopViewControllerAnimated (true);	
		}


	}
}

