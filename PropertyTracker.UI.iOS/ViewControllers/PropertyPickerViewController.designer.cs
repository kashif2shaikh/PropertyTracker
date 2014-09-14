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
	[Register ("PropertyPickerViewController")]
	partial class PropertyPickerViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem GetMorePropertiesButtonItem { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem GetPropertiesButtonItem { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIToolbar PropertyToolBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (GetMorePropertiesButtonItem != null) {
				GetMorePropertiesButtonItem.Dispose ();
				GetMorePropertiesButtonItem = null;
			}

			if (GetPropertiesButtonItem != null) {
				GetPropertiesButtonItem.Dispose ();
				GetPropertiesButtonItem = null;
			}

			if (PropertyToolBar != null) {
				PropertyToolBar.Dispose ();
				PropertyToolBar = null;
			}
		}
	}
}
