using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.UI.iOS.Views
{
    public partial class PropertyCell : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("PropertyCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("PropertyCell");

        //private readonly MvxImageViewLoader _imageViewLoader;

      
        public PropertyCell(IntPtr handle)
            : base(handle)
		{
			//_imageViewLoader = new MvxImageViewLoader(() => this.MainImage);

			this.DelayBind(() => {
                var set = this.CreateBindingSet<PropertyCell, Property>();
                set.Bind(this.TextLabel).To(property => property.Name);
                set.Bind(this.DetailTextLabel).To(property => property.City);
                //set.Bind(_imageViewLoader).To (property => property.ImageUrl);
				set.Apply ();
			});
		}

        public static PropertyCell Create()
		{
            return (PropertyCell)Nib.Instantiate(null, null)[0];
		}      
    }
}