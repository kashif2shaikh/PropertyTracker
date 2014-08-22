using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PropertyTracker.UI.iOS.ViewControllers
{

    public static class ViewControllerExtensions
    {
        public static void LoadViewModel<TViewModel>(this IMvxTouchView view) where TViewModel : IMvxViewModel 
        {
            
        }
        
    }


    
}