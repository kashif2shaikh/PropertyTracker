using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PropertyTracker.UI.iOS.Common
{
    // To use this - make sure your TableView as AllowsSelection set (
    public class MultipleCheckmarkTableSource : MvxStandardTableViewSource
    {
        // This should be bound to your view model
        public List<int> SelectedItemIndexList { get; set; }
  
        public MultipleCheckmarkTableSource(UITableView tableView, NSString cellIdentifier) : base(tableView, cellIdentifier)
        {
			SelectedItemIndexList = new List<int> ();               
        }
        
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);
            if (!SelectedItemIndexList.Contains(indexPath.Row))
            {
                SelectedItemIndexList.Add(indexPath.Row);
            }
            else
            {
                SelectedItemIndexList.Remove(indexPath.Row);
            }
            
            TableView.ReloadRows(new[] { indexPath }, UITableViewRowAnimation.None);

            // Row is marked 'internally' as selected. Mark it as de-selected.             
            TableView.DeselectRow(indexPath, false);
        }
               
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = base.GetOrCreateCellFor(tableView, indexPath, item);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            cell.Accessory = SelectedItemIndexList.Contains(indexPath.Row) ? cell.Accessory = UITableViewCellAccessory.Checkmark :  cell.Accessory = UITableViewCellAccessory.None;                        

            return cell;
        }
        
    }
}