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
	[Register ("AddUserViewController")]
	partial class AddUserViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem AddButtonItem { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem CancelButtonItem { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField CompanyNameTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField ConfirmPasswordTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField FullNameTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField PasswordTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITapGestureRecognizer PhotoImageTapGestureRecognizer { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView PhotoImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView PlaceholderImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PropertiesLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITapGestureRecognizer PropertiesTapGestureRecognizer { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField UsernameTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AddButtonItem != null) {
				AddButtonItem.Dispose ();
				AddButtonItem = null;
			}

			if (CancelButtonItem != null) {
				CancelButtonItem.Dispose ();
				CancelButtonItem = null;
			}

			if (CompanyNameTextField != null) {
				CompanyNameTextField.Dispose ();
				CompanyNameTextField = null;
			}

			if (ConfirmPasswordTextField != null) {
				ConfirmPasswordTextField.Dispose ();
				ConfirmPasswordTextField = null;
			}

			if (FullNameTextField != null) {
				FullNameTextField.Dispose ();
				FullNameTextField = null;
			}

			if (PasswordTextField != null) {
				PasswordTextField.Dispose ();
				PasswordTextField = null;
			}

			if (PhotoImageTapGestureRecognizer != null) {
				PhotoImageTapGestureRecognizer.Dispose ();
				PhotoImageTapGestureRecognizer = null;
			}

			if (PhotoImageView != null) {
				PhotoImageView.Dispose ();
				PhotoImageView = null;
			}

			if (PlaceholderImageView != null) {
				PlaceholderImageView.Dispose ();
				PlaceholderImageView = null;
			}

			if (PropertiesLabel != null) {
				PropertiesLabel.Dispose ();
				PropertiesLabel = null;
			}

			if (PropertiesTapGestureRecognizer != null) {
				PropertiesTapGestureRecognizer.Dispose ();
				PropertiesTapGestureRecognizer = null;
			}

			if (UsernameTextField != null) {
				UsernameTextField.Dispose ();
				UsernameTextField = null;
			}
		}
	}
}
