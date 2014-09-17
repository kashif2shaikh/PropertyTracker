
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.UI.iOS.ViewControllers;
using Cirrious.MvvmCross.Binding.BindingContext;
using PropertyTracker.Core.ViewModels;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class PropertyDetailViewController : BasePropertyViewController
	{
		public PropertyDetailViewController(IntPtr handle)
			: base(handle)
		{

		}

		public new PropertyDetailViewModel ViewModel
		{
			get { return (PropertyDetailViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var set = this.CreateBindingSet<PropertyDetailViewController, PropertyDetailViewModel>();
			set.Bind (DeletePropertyButton).To (vm => vm.DeletePropertyCommand);
			set.Apply ();

			// Use ViewController Built-in edit button item 
			NavigationItem.RightBarButtonItem = EditButtonItem;


			EditButtonItem.Clicked += (object sender, EventArgs e) => {
				if (Editing) {
					// Done button pressed
					ViewModel.SavePropertyCommand.Execute ();
				} else {
					// Edit button pressed
					SetEditing(true, true);
				}
			};

			// Go back to view mode after save
			ViewModel.SavePropertySuccessEventHandler += (object sender, EventArgs e) => {
				SetEditing (false, true);
			};

			ViewModel.SavePropertyFailedEventHandler += (object sender, EventArgs e) => {
				// Continue
			};

			ViewModel.DeletePropertySuccessEventHandler += (object sender, EventArgs e) => {
				SetEditing (false, true);
				NavigationController.PopViewControllerAnimated(true);
			};

			ViewModel.DeletePropertyFailedEventHandler += (object sender, EventArgs e) => {
				// Continue
			};


			// By default view only mode.
			SetEditing (false, false);  
		}

		public override void SetEditing (bool editing, bool animated)
		{
			base.SetEditing (editing, animated);
			if(editing) {
				var cancelButton = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, (s, e) => {
					SetEditing(false, true);
					ViewModel.CancelCommand.Execute ();
				});
				NavigationItem.LeftBarButtonItem = cancelButton;
				DeletePropertyCell.Hidden = false;
			}
			else {
				NavigationItem.LeftBarButtonItem = null;
				DeletePropertyCell.Hidden = true;
			}
		}


	}
}

