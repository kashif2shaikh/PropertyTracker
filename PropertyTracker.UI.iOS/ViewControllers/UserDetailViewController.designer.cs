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
	[Register ("UserDetailViewController")]
	partial class UserDetailViewController : BaseUserViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem CancelButtonItem { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem EditButtonItem { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CancelButtonItem != null) {
				CancelButtonItem.Dispose ();
				CancelButtonItem = null;
			}

			if (EditButtonItem != null) {
				EditButtonItem.Dispose ();
				EditButtonItem = null;
			}
		}
	}
}
