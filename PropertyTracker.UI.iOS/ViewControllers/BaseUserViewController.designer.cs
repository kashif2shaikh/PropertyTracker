using MonoTouch.Foundation;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    [Register ("BaseUserViewController")]
    partial class BaseUserViewController
    {
        //[Outlet]
        //MonoTouch.UIKit.UIBarButtonItem AddButtonItem { get; set; }

        //[Outlet]
        //MonoTouch.UIKit.UIBarButtonItem CancelButtonItem { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UITextField CompanyNameTextField { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UITextField ConfirmPasswordTextField { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UITextField FullNameTextField { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UITextField PasswordTextField { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UITapGestureRecognizer PhotoImageTapGestureRecognizer { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UIImageView PhotoImageView { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UIImageView PlaceholderImageView { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UILabel PropertiesLabel { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UITapGestureRecognizer PropertiesTapGestureRecognizer { get; set; }

        [Outlet]
        public MonoTouch.UIKit.UITextField UsernameTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
        }
    }
}