// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace PropertyTracker.UI.iOS
{
	[Register ("AddUserViewController")]
	partial class AddUserViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField ConfirmPasswordTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField FullNameTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField PasswordTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView PhotoImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PropertiesLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITapGestureRecognizer PropertiesTapGestureRecognizer { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField UsernameTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PhotoImageView != null) {
				PhotoImageView.Dispose ();
				PhotoImageView = null;
			}

			if (FullNameTextField != null) {
				FullNameTextField.Dispose ();
				FullNameTextField = null;
			}

			if (UsernameTextField != null) {
				UsernameTextField.Dispose ();
				UsernameTextField = null;
			}

			if (PasswordTextField != null) {
				PasswordTextField.Dispose ();
				PasswordTextField = null;
			}

			if (ConfirmPasswordTextField != null) {
				ConfirmPasswordTextField.Dispose ();
				ConfirmPasswordTextField = null;
			}

			if (PropertiesLabel != null) {
				PropertiesLabel.Dispose ();
				PropertiesLabel = null;
			}

			if (PropertiesTapGestureRecognizer != null) {
				PropertiesTapGestureRecognizer.Dispose ();
				PropertiesTapGestureRecognizer = null;
			}
		}
	}
}
