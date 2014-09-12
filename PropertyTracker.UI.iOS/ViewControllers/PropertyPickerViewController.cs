
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

			var source = new MultipleCheckmarkTableSource (TableView, PropertyPickerCell.Key);
					
			TableView.Source = source;
			TableView.AllowsSelection = true;
			//TableView.AllowsMultipleSelection = true;
				
			var set = this.CreateBindingSet<PropertyPickerViewController, PropertyPickerViewModel>();
			set.Bind(source).To(vm => vm.Properties);
			set.Bind(source).For(s => s.SelectedItemIndexList).To(vm => vm.SelectedPropertyIndexList);
            set.Bind(GetPropertiesButtonItem).To(vm => vm.GetPropertiesCommand);
            set.Bind(GetMorePropertiesButtonItem).To(vm => vm.GetMorePropertiesCommand);
			
			set.Apply();

			// Reload clicked - run command and hide search options
			/*
		    GetPropertiesButtonItem.Clicked += (sender, args) =>
		    {
		        //HideSearchOptions(true);
                //TableView.ReloadData();
		    };
            */

		    TableView.ReloadData();
		}
	}
}