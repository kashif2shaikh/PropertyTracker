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
    interface IPresentView
    {
        bool CanPresentView(IMvxTouchView view);
        void PresentView(IMvxTouchView view); 
    }
}