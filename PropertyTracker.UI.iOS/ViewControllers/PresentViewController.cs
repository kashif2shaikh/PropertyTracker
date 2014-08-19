using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.Views;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public abstract class PresentViewController : MvxViewController, IPresentView
    {
        public abstract bool CanPresentView(IMvxTouchView view);
        public abstract void PresentView(IMvxTouchView view);

        // We implement our own ShowView - in case 
        public void ShowView<TViewModel>(string info) where TViewModel : IMvxViewModel
        {
            var viewDispatcher = Mvx.Resolve<IMvxViewDispatcher>();
            //var request = MvxViewModelRequest.GetDefaultRequest(typeof(T));
            //request.ParameterValues = ((object)parameter).ToSimplePropertyDictionary();
            var request = new MvxViewModelRequest<TViewModel>(null, null, new MvxRequestedBy(MvxRequestedByType.UserAction, info));
            viewDispatcher.ShowViewModel(request);
        }
    }
}