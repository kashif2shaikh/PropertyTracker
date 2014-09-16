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
	[Register ("AddPropertyViewController")]
	partial class AddPropertyViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem AddButtonItem { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem CancelButtonItem { get; set; }


		
		void ReleaseDesignerOutlets ()
		{


			if (CancelButtonItem != null) {
				CancelButtonItem.Dispose ();
				CancelButtonItem = null;
			}

			if (AddButtonItem != null) {
				AddButtonItem.Dispose ();
				AddButtonItem = null;
			}
		}
	}
}
