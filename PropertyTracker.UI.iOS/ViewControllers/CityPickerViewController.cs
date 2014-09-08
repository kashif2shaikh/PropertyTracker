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
       
        public override void ViewDidLoad()
        {          
            base.ViewDidLoad();
            
            // Use custom table source so I can put checkmarks on cells
            var source = new CustomTableSource(TableView, "TitleText");         
            TableView.Source = source;
            TableView.AllowsSelection = true;
            
            // reference: to allow multiple selection
            //TableView.AllowsMultipleSelection = true;
            //set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.CitySelectedCommand);           

            var set = this.CreateBindingSet<CityPickerViewController, CityPickerViewModel>();
            set.Bind(source).To(vm => vm.Cities);            
            set.Bind(source).For(s => s.SelectedItem).To(vm => vm.SelectedCity);
            set.Apply();
           
            TableView.ReloadData();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewModel.CityPickerDoneCommand.Execute(null);
        }

        class CustomTableSource : MvxStandardTableViewSource
        {            
            public CustomTableSource(UITableView tableView, string bindingText)
                : base(tableView, bindingText)
            {
               
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                base.RowSelected(tableView, indexPath);
                TableView.ReloadData();                                
            }
      
            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
				var cell = base.GetOrCreateCellFor(tableView, indexPath, item);
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.Accessory =  item.Equals(SelectedItem) ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;				
				return cell;
            }
        }
    }
}