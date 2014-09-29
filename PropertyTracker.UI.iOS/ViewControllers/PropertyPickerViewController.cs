
using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.Common;
using PropertyTracker.UI.iOS.Views;

// Make sure namespace is same in designer.cs - Xamarin skips adding subfolders to namespace!
using PropertyTracker.Dto.Models;
using PropertyTracker.Core.Services;
using Cirrious.CrossCore;
using System.Linq;


namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class PropertyPickerViewController : MvxTableViewController
	{	
		public PropertyPickerViewController(IntPtr handle)
			: base(handle)
		{
		}

		public new PropertyPickerViewModel ViewModel
		{
			get { return (PropertyPickerViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public override void ViewDidLoad()
		{            
			base.ViewDidLoad();

			var source = new PropertyPickerTableSource (TableView, PropertyPickerCell.Key);
					
			TableView.Source = source;
			TableView.AllowsSelection = !ViewModel.ViewOnlyMode;
							
			var set = this.CreateBindingSet<PropertyPickerViewController, PropertyPickerViewModel>();
			set.Bind(source).To(vm => vm.Properties);
			set.Bind(source).For(s => s.SelectedItemIndexList).To(vm => vm.SelectedPropertyIndexList);
            
            /* 
             * Disable batched results. 
             */
            //set.Bind(GetPropertiesButtonItem).To(vm => vm.GetPropertiesCommand);
            //set.Bind(GetMorePropertiesButtonItem).To(vm => vm.GetMorePropertiesCommand);
            PropertyToolBar.Hidden = true;
			
			set.Apply();
           
		    TableView.ReloadData();
		}

	    public override void ViewWillDisappear(bool animated)
	    {
	        base.ViewWillDisappear(animated);

	        ViewModel.PropertyPickerDoneCommand.Execute(null);
	    }
	}

	public class PropertyPickerTableSource : MultipleCheckmarkTableSource
	{
		private readonly IPropertyTrackerService _propertyTrackerService;

		public PropertyPickerTableSource(UITableView tableView, NSString cellIdentifier) : base(tableView, cellIdentifier)
		{
			_propertyTrackerService = Mvx.Resolve<IPropertyTrackerService> ();	
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var property = GetItemAt (indexPath) as Property;

			if(property.Users.Select(u => u.Id).Contains(_propertyTrackerService.LoggedInUser.Id)) {
				// Only allow selection, if Property is assigned to logged in user. Otherwise selection will be disabled.
				base.RowSelected (tableView, indexPath);
			}
		}

		protected override UITableViewCell GetOrCreateCellFor (UITableView tableView, NSIndexPath indexPath, object item)
		{
			var cell = base.GetOrCreateCellFor (tableView, indexPath, item);
			var property = item as Property;
			if(property.Users.Select(u => u.Id).Contains(_propertyTrackerService.LoggedInUser.Id)) {
				// Only allow selection, if Property is assigned to logged in user. Otherwise selection will be disabled.
				//cell.ContentView.BackgroundColor = UIColor.White;
				cell.ContentView.Alpha = (float)1.0;
			}
			else {
				//cell.ContentView.BackgroundColor = UIColor.Gray;
				cell.ContentView.Alpha = (float)0.25;
			}
			return cell;

		}
	}
}