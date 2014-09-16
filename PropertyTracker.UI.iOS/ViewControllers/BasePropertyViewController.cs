
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using PropertyTracker.UI.iOS.ViewControllers;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class BasePropertyViewController : MvxTableViewController
	{
		public BasePropertyViewController(IntPtr handle)
			: base(handle)
		{

		}

		public virtual new BasePropertyViewModel ViewModel
		{
			get { return (BasePropertyViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var set = this.CreateBindingSet<BasePropertyViewController, BasePropertyViewModel>();
			set.Bind<UITextField>(CompanyNameTextField).To (vm => vm.CompanyName);
			set.Bind<UITextField>(PropertyNameTextField).To(vm => vm.PropertyName);
			set.Bind<UITextField>(CityTextField).To (vm => vm.City);
			set.Bind<UITextField>(StateTextField).To (vm => vm.State);
			set.Bind<UITextField>(SquareFeetTextField).To(vm => vm.SquareFeet);		
			set.Apply ();

			CityTextField.ShouldBeginEditing +=  (textField) => {
				var controller = this.CreateViewControllerFor<CityPickerViewModel>(new 
					{
						city = CityTextField.Text, 
						requestedViewId = ViewModel.ViewInstanceId
					}) as CityPickerViewController;
				NavigationController.PushViewController(controller, true);

				return false;
			};

			StateTextField.ShouldBeginEditing +=  (textField) => {
				var controller = this.CreateViewControllerFor<StatePickerViewModel>(new
					{
						state = StateTextField.Text,
						requestedViewId = ViewModel.ViewInstanceId
					}) as StatePickerViewController;
				NavigationController.PushViewController(controller, true);

				return false;
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
			PropertyNameTextField.UserInteractionEnabled = editable;
			CityTextField.UserInteractionEnabled = editable;
			StateTextField.UserInteractionEnabled = editable;
			SquareFeetTextField.UserInteractionEnabled = editable;
		}
	}
}

