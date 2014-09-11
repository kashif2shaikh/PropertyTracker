// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace PropertyTracker.UI.iOS.Views
{
	[Register ("UserListCell")]
	partial class UserListCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel FullNameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView PhotoImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel UsernameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PhotoImageView != null) {
				PhotoImageView.Dispose ();
				PhotoImageView = null;
			}

			if (FullNameLabel != null) {
				FullNameLabel.Dispose ();
				FullNameLabel = null;
			}

			if (UsernameLabel != null) {
				UsernameLabel.Dispose ();
				UsernameLabel = null;
			}
		}
	}
}
