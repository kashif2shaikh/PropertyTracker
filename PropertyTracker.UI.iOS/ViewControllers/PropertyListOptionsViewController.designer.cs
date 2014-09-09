// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	[Register ("PropertyListOptionsViewController")]
	partial class PropertyListOptionsViewController
	{
		[Outlet]
		public MonoTouch.UIKit.UILabel CityFilterLabel { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UITapGestureRecognizer CityFilterTapGestureRecognizer { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UISearchBar SearchBar { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UISegmentedControl SortColumnSegmentControl { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UILabel SortLabel { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UISwitch SortOrderSwitch { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UITapGestureRecognizer StateFilterTapGestureRecognizer { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UILabel StateProvFilterLabel { get; private set; }

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
