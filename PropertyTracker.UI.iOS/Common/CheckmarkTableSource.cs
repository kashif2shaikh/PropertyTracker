using System;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public class CheckmarkTableSource : MvxStandardTableViewSource
    {            
        public CheckmarkTableSource(UITableView tableView, string bindingText)
            : base(tableView, bindingText)
        {
               
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var oldSelectedItem = SelectedItem;
            base.RowSelected(tableView, indexPath);
            if (SelectedItem.Equals(oldSelectedItem))
            {
                // if same item is selected, de-select
                SelectedItem = String.Empty;
            }                
            TableView.ReloadData();                                
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = GetCell(tableView, indexPath);

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