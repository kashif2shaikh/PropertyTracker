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
	[Register ("PropertyCell")]
	partial class PropertyCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel PropertyCity { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PropertyName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PropertyStateProvince { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PropertyName != null) {
				PropertyName.Dispose ();
				PropertyName = null;
			}

			if (PropertyCity != null) {
				PropertyCity.Dispose ();
				PropertyCity = null;
			}

			if (PropertyStateProvince != null) {
				PropertyStateProvince.Dispose ();
				PropertyStateProvince = null;
			}
		}
	}
}
