using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PropertyTracker.UI.iOS.Views
{
    interface IViewPresenter
    {
        // Tell ViewPresenter they have been added or removed from the Presenter, and
        // should be used to cleanup or initialize more things.
        void ViewPresenterAdded();
        void ViewPresenterRemoved();

        // Present views (e.g. child view controllers) requested by ViewPresenter
        bool CanPresentView(IMvxTouchView view);
        void PresentView(IMvxTouchView view);
    }
}