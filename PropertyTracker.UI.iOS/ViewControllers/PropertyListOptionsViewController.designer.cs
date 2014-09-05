// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	[Register ("PropertyListOptionsViewController")]
	partial class PropertyListOptionsViewController
	{
		[Outlet]
		public MonoTouch.UIKit.UILabel CityFilterLabel { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UISearchBar SearchBar { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UISegmentedControl SortColumnSegmentControl { get; private set; }

		[Outlet]
		MonoTouch.UIKit.UILabel SortLabel { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UISwitch SortOrderSwitch { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UILabel StateProvFilterLabel { get; private set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CityFilterLabel != null) {
				CityFilterLabel.Dispose ();
				CityFilterLabel = null;
			}

			if (SearchBar != null) {
				SearchBar.Dispose ();
				SearchBar = null;
			}

			if (SortColumnSegmentControl != null) {
				SortColumnSegmentControl.Dispose ();
				SortColumnSegmentControl = null;
			}

			if (SortOrderSwitch != null) {
				SortOrderSwitch.Dispose ();
				SortOrderSwitch = null;
			}

			if (StateProvFilterLabel != null) {
				StateProvFilterLabel.Dispose ();
				StateProvFilterLabel = null;
			}

			if (SortLabel != null) {
				SortLabel.Dispose ();
				SortLabel = null;
			}
		}
	}
}
