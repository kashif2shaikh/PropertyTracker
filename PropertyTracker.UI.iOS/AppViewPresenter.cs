using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.ViewControllers;
using PropertyTracker.UI.iOS.Views;

namespace PropertyTracker.UI.iOS
{
    public class AppViewPresenter : MvxTouchViewPresenter
    {
        private List<IMvxTouchView> _presentedViews = new List<IMvxTouchView>();
        private LoginViewController _loginViewController;
        private MainViewController _mainViewController;

        public AppViewPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {

        }

        protected override UINavigationController CreateNavigationController(UIViewController viewController)
        {
            var navBar = base.CreateNavigationController(viewController);
            navBar.NavigationBarHidden = true;
            return navBar;
        }

        public override void Show(Cirrious.MvvmCross.Touch.Views.IMvxTouchView view)
        {
            
            if (view is LoginViewController)
            {
                _loginViewController = (LoginViewController) view;
            }
              
            else if (view is MainViewController)
            {
                _mainViewController = (MainViewController) view;
            }

            //base.Show(view);

            if (PresentView(view) == false)
            {
                // 
                base.Show(view);
            }
            
            _presentedViews.Add(view);
        }

        private bool PresentView(IMvxTouchView view)
        {
            IPresentView parentView = null;

            // iterate reverse order...most likely latest view added will be target for presenting
            for (int i = _presentedViews.Count - 1; i >= 0; i--)
            {
                parentView = _presentedViews[i] as IPresentView;
                if (parentView != null && parentView.CanPresentView(view))
                {
                    break;
                }
                parentView = null;
            }
            //foreach (var pView in _presentedViews)
            //{
            //    parentView = pView as IPresentView;
            //    if (parentView != null && parentView.CanPresentView(view))
            //    {
            //        break;
            //    }
            //}
            if (parentView != null)
            {
                parentView.PresentView(view);
            }
            return parentView != null;
        }
        
        public override void Close(Cirrious.MvvmCross.ViewModels.IMvxViewModel toClose)
        {
            IMvxTouchView viewToClose = null;
            for (int i = _presentedViews.Count - 1; i >= 0; i--)
            {
                if (_presentedViews[i].ViewModel == toClose)
                {
                    viewToClose = _presentedViews[i];
                }
            }

            //foreach (var view in _presentedViews)
            //{
            //    if (view.ViewModel == toClose)
            //    {
            //        viewToClose = view;
            //    }
            //}
            if (viewToClose != null)
            {
                _presentedViews.Remove(viewToClose);
            }

            base.Close(toClose);
        }
    }
}