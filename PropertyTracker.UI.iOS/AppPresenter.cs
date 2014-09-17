using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.UIKit;
using PropertyTracker.Core.PresentationHints;
using PropertyTracker.Core.Services;
using PropertyTracker.UI.iOS.ViewControllers;
using PropertyTracker.UI.iOS.Views;

namespace PropertyTracker.UI.iOS
{
    public class AppPresenter : MvxTouchViewPresenter
    {        
        private List<IMvxTouchView> _viewPresenters = new List<IMvxTouchView>();
        private List<IMvxTouchView> _presentedViews = new List<IMvxTouchView>(); 
        private LoginViewController _loginViewController;
        private MainViewController _mainViewController;
        
        public AppPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            
        }

        protected override UINavigationController CreateNavigationController(UIViewController viewController)
        {
            var navBar = base.CreateNavigationController(viewController);
            navBar.NavigationBarHidden = true;
            return navBar;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);

            this.Show(view);
        }

        public override void Show(Cirrious.MvvmCross.Touch.Views.IMvxTouchView view)
        {
            
            // When initializing views for the first time, we will setup 
            if (view is LoginViewController)
            {              
                _loginViewController = (LoginViewController) view;
                base.Show(view);             
            }
              
            else if (view is MainViewController)
            {                
                _mainViewController = (MainViewController) view;
                base.Show(view);
            }
            else
            {
                if (PresentView(view) == false)
                {
                    // No views can handle presenting this view...just push it on the navigation stack as last resort.
                    base.Show(view);
                }
            }
            _viewPresenters.Add(view);

            var viewPresenter = view as IViewPresenter;
            if (viewPresenter != null)
            {
                viewPresenter.ViewPresenterAdded();
            }
        }

        // TODO: There is no CLOSE Method for a view - any view changes come down as presentation hints
        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is LogoutPresentationHint)
            {
                var service = Mvx.Resolve<IPropertyTrackerService>(); 
                service.Logout();
                
                CloseUpTo(_loginViewController);
                return;
            }
            base.ChangePresentation(hint);
        }

        // TODO: We have to make sure views are opened and closed through Presenter - if view is opened
        private bool PresentView(IMvxTouchView view)
        {
            IViewPresenter parentView = null;

            // iterate reverse order...most likely latest view added will be target for presenting
            for (int i = _viewPresenters.Count - 1; i >= 0; i--)
            {
                parentView = _viewPresenters[i] as IViewPresenter;
                if (parentView != null && parentView.CanPresentView(view))
                {
                    break;
                }
                parentView = null;
            }
          
            if (parentView != null)
            {
                parentView.PresentView(view);
                _presentedViews.Add(view);
            }
            return parentView != null;
        }
        
        public override void Close(Cirrious.MvvmCross.ViewModels.IMvxViewModel toClose)
        {            
            for (int i = _viewPresenters.Count - 1; i >= 0; i--)
            {
                if (_viewPresenters[i].ViewModel == toClose)
                {
                    CloseViewController(_viewPresenters[i]);
                    break;
                }
            }            
        }

        private void CloseViewController(IMvxTouchView viewController)
        {          
            if (viewController != null)
            {              
                _viewPresenters.Remove(viewController);

                bool viewClosed = false;
                var viewPresenter = viewController as IViewPresenter;
                if (viewPresenter != null)
                {                    
                    viewPresenter.ViewPresenterRemoved();
                }
                
                if (_loginViewController != null && _loginViewController == viewController)
                {
                    base.Close(_loginViewController.ViewModel);
                    _loginViewController = null;
                }
                else if (_mainViewController != null && _mainViewController == viewController)
                {
                    base.Close(_mainViewController.ViewModel);
                    _mainViewController = null;
                }
                else if (_presentedViews.Contains(viewController))
                {
                    // This view is already presented - we don't handle the close for this. They should
                    // have removed themselves on ViewPresenterRemoved
                    _presentedViews.Remove(viewController);
                }
                else
                {
                    // Close another view that was not presented, was not login, or not main view controller
                    base.Close(viewController.ViewModel);                    
                }
            }           
        }


        private void CloseUpTo(IMvxTouchView viewController)
        {
            for (int i = _viewPresenters.Count - 1; i >= 0; i--)
            {
                if (_viewPresenters[i] != viewController)
                {
                    CloseViewController(_viewPresenters[i]);
                }
            }   
        }
    }
}