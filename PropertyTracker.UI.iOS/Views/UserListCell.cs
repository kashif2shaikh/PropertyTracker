using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.UI.iOS.Views
{
    public partial class UserListCell : MvxTableViewCell
    {
        // Kashif: This Cell is defined within the UserListController as a Dynamic Prototype Cell.
        // There is NO XIB!
        public static readonly UINib Nib = UINib.FromName("UserListCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("UserListCell");

		private readonly MvxImageViewLoader _imageViewLoader;

        public UserListCell (IntPtr handle) : base (handle)
        {
			_imageViewLoader = new MvxImageViewLoader(() => PhotoImageView);

            this.DelayBind(() => {
                var set = this.CreateBindingSet<UserListCell, User>();
				set.Bind(FullNameLabel).To(user => user.Fullname);
				set.Bind(UsernameLabel).To(user => user.Username);                   
				set.Bind(_imageViewLoader).To (user => user.PhotoUrl);
                set.Apply ();
            });
        }

        public static UserListCell Create ()
        {
            return (UserListCell)Nib.Instantiate (null, null) [0];
        }
    }
}