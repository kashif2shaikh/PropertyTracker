using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PropertyTracker.UI.iOS.Common
{
    public static class MvxTouchViewExtensions
    {
        public static IMvxTouchView CreateViewControllerFor(this IMvxCanCreateTouchView view, Type viewModelType)
        {
            var request = new MvxViewModelRequest(viewModelType, null, null, MvxRequestedBy.UserAction);
            return view.CreateViewControllerFor(request);
        }
    }
}