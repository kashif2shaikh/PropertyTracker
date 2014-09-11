using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PropertyTracker.UI.iOS.Common
{
    public class MultipleCheckmarkTableSource : MvxStandardTableViewSource
    {
        // This should be bound to your view model
        public List<int> SelectedItemIndexList { get; set; }
  
        public MultipleCheckmarkTableSource(UITableView tableView, string bindingText)
            : base(tableView, bindingText)
        {
               
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            SelectedItemIndexList.Add(indexPath.Row);
            TableView.ReloadRows(new[]{indexPath}, UITableViewRowAnimation.None);           
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            SelectedItemIndexList.Remove(indexPath.Row);
            TableView.ReloadRows(new []{indexPath}, UITableViewRowAnimation.None);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = base.GetOrCreateCellFor(tableView, indexPath, item);
            //cell.SelectionStyle = UITableViewCellSelectionStyle.None;            
            cell.Accessory =  SelectedItemIndexList.Contains(indexPath.Row) ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;				
            return cell;
        }
    }
}