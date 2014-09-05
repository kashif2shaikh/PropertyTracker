
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PropertyTracker.UI.iOS.ViewControllers
{
	public partial class PropertyListOptionsViewController : UITableViewController
	{
		public enum SortColumn {Name, City, State};
	
//		public PropertyListOptionsViewController () : base ("PropertyListOptionsViewController", null)
//		{
//		}

		public PropertyListOptionsViewController(IntPtr handle) : base(handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			//CityFilterLabel.Text = "San Diego";
			//StateProvFilterLabel.Text = "California";




			// Perform any additional setup after loading the view, typically from a nib.
		}


	}
}

