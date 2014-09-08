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
		public MonoTouch.UIKit.UISearchBar SearchBar { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UISegmentedControl SortColumnSegmentControl { get; private set; }

		[Outlet]
		MonoTouch.UIKit.UILabel SortLabel { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UISwitch SortOrderSwitch { get; private set; }

		[Outlet]
		public MonoTouch.UIKit.UILabel StateProvFilterLabel { get; private set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITapGestureRecognizer ad { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITapGestureRecognizer CityFilterTapGestureRecognizer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITapGestureRecognizer StateFilterTapGestureRecognizer { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ad != null) {
				ad.Dispose ();
				ad = null;
			}
			if (CityFilterTapGestureRecognizer != null) {
				CityFilterTapGestureRecognizer.Dispose ();
				CityFilterTapGestureRecognizer = null;
			}
			if (StateFilterTapGestureRecognizer != null) {
				StateFilterTapGestureRecognizer.Dispose ();
				StateFilterTapGestureRecognizer = null;
			}
		}
	}
}
