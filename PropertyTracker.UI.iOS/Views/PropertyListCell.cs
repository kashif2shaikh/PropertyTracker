
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Dto.Models;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace PropertyTracker.UI.iOS.Views
{
	public partial class PropertyListCell : MvxTableViewCell
	{
		// Kashif: This Cell is defined within the PropertyListViewController as a Dynamic Prototype Cell.
		// There is NO XIB!
		public static readonly UINib Nib = UINib.FromName ("PropertyListCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PropertyListCell");

		public PropertyListCell (IntPtr handle) : base (handle)
		{
			//_imageViewLoader = new MvxImageViewLoader(() => this.MainImage);

			this.DelayBind(() => {
				var set = this.CreateBindingSet<PropertyListCell, Property>();
				set.Bind(PropertyName).To(property => property.Name);
				set.Bind(PropertyCity).To(property => property.City);
				set.Bind(PropertyStateProvince).To(property => property.StateProvince);
				//set.Bind(_imageViewLoader).To (property => property.ImageUrl);
				set.Apply ();
			});
		}

		public static PropertyListCell Create ()
		{
			return (PropertyListCell)Nib.Instantiate (null, null) [0];
		}
	}
}

