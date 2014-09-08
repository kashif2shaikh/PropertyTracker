using System;
using System.Drawing;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using PropertyTracker.Core.ViewModels;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public class CityPickerViewController : MvxTableViewController
    {
        // Controller does not have a XIB/Storyboard.
        public CityPickerViewController()
        {

        }

        public new CityPickerViewModel ViewModel
        {
            get { return (CityPickerViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public ICommand CheckmarkCommand
        {
            get { return new MvxCommand<UITableViewCell>(item => item.Accessory = UITableViewCellAccessory.Checkmark); }
        }

        public override void ViewDidLoad()
        {          
            base.ViewDidLoad();
            //var source = new MvxStandardTableViewSource(TableView, "Cell");
            var source = new CustomTableSource(TableView, UITableViewCellStyle.Default, new NSString("CityPickerCell"), "TitleText", UITableViewCellAccessory.None);
            TableView.Source = source;
            TableView.AllowsSelection = true;
            //TableView.AllowsMultipleSelection = true;
            //source.SelectionChangedCommand = CheckmarkCommand;

            var set = this.CreateBindingSet<CityPickerViewController, CityPickerViewModel>();
            set.Bind(source).To(vm => vm.Cities);
            set.Apply();

            TableView.ReloadData();
        }

        // Need to override to to show checkmark on cell
        class CustomTableSource : MvxStandardTableViewSource
        {
            public CustomTableSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, string bindingText, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
                : base(tableView, style, cellIdentifier, bindingText, tableViewCellAccessory)
            {

            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {                
                UITableViewCell cell = GetCell(tableView, indexPath);
                cell.Accessory = UITableViewCellAccessory.Checkmark;                
            }

            public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
            {             
                UITableViewCell cell = GetCell(tableView, indexPath);
                cell.Accessory = UITableViewCellAccessory.None;
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                return base.GetOrCreateCellFor(tableView, indexPath, item);
            }
        }
    }
}