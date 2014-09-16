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
	[Register ("BasePropertyViewController")]
	partial class BasePropertyViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField CityTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField CompanyNameTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField PropertyNameTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField SquareFeetTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField StateTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CityTextField != null) {
				CityTextField.Dispose ();
				CityTextField = null;
			}

			if (CompanyNameTextField != null) {
				CompanyNameTextField.Dispose ();
				CompanyNameTextField = null;
			}

			if (PropertyNameTextField != null) {
				PropertyNameTextField.Dispose ();
				PropertyNameTextField = null;
			}

			if (SquareFeetTextField != null) {
				SquareFeetTextField.Dispose ();
				SquareFeetTextField = null;
			}

			if (StateTextField != null) {
				StateTextField.Dispose ();
				StateTextField = null;
			}
		}
	}
}
