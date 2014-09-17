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
	[Register ("PropertyDetailViewController")]
	partial class PropertyDetailViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton DeletePropertyButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableViewCell DeletePropertyCell { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DeletePropertyCell != null) {
				DeletePropertyCell.Dispose ();
				DeletePropertyCell = null;
			}

			if (DeletePropertyButton != null) {
				DeletePropertyButton.Dispose ();
				DeletePropertyButton = null;
			}
		}
	}
}
