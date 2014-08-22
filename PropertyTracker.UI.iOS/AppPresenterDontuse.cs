using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.CoreAnimation;
using MonoTouch.UIKit;
using PropertyTracker.Core.PresentationHints;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.ViewControllers;
using PropertyTracker.UI.iOS.Views;

namespace PropertyTracker.UI.iOS
{
    public class AppPresenterDontuse : MvxTouchViewPresenter
    {
        private List<IMvxTouchView> _presentedViews = new List<IMvxTouchView>();
        private LoginViewController _loginViewController;
        private MainViewController _mainViewController;
        private PropertyListViewController _propertyListViewController;
        private UserListViewController _userListViewController;
        
        public AppPresenterDontuse(UIApplicationDelegate applicationDelegate, UIWindow window)
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
            // Don't create view controller if it already exists, we will switch to it.
            var view = ControllerForRequest(request);
            if (view == null)
            {
                view = this.CreateViewControllerFor(request);
            }

            this.Show(view);
        }

        private bool RequestIsViewType<TClass>(MvxViewModelRequest request)
        {
            return request.ViewModelType == typeof (TClass);
        }

        private IMvxTouchView ControllerForRequest(MvxViewModelRequest request)
        {
            if (RequestIsViewType<LoginViewModel>(request) && _loginViewController != null)
            {
                return _loginViewController;
            }
            else if (RequestIsViewType<MainViewModel>(request) && _mainViewController != null)
            {
                return _mainViewController;
            }
            else if (RequestIsViewType<PropertyListViewModel>(request) && _propertyListViewController != null)
            {
                return _propertyListViewController;
            }
            else if (RequestIsViewType<UserListViewModel>(request) && _userListViewController != null)
            {
                return _userListViewController;
            }
            return null;

        }

        public override void Show(Cirrious.MvvmCross.Touch.Views.IMvxTouchView view)
        {            
            // When initializing views for the first time, we will setup 
            if (view is LoginViewController && _loginViewController == null)
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
                    // No views can handle presenting this view...just present it modally as last resort.
                    base.Show(view);
                }
            }
            _presentedViews.Add(view);
        }

        private UINavigationController CreateTabFor<TViewModel>(int index, string title, string imageName) where TViewModel : MvxViewModel
        {
            var navController = new UINavigationController();
            var viewController = this.CreateViewControllerFor<TViewModel>() as UIViewController;
            SetTitleAndTabBarItem(viewController, index, title, imageName);
            navController.PushViewController(viewController, false);
            return navController;
        }

        private void SetTitleAndTabBarItem(UIViewController viewController, int index, string title, string imageName)
        {
            viewController.Title = title;
            viewController.TabBarItem.Title = title;
            //viewController.TabBarItem.Image = UIImage.FromBundle(imageName);
            //screen.TabBarItem = new UITabBarItem(title, UIImage.FromBundle("Images/Tabs/" + imageName + ".png"),
            //                                     _createdSoFarCount);
            //_createdSoFarCount++;
        }

        // TODO: There is no CLOSE Method for a view - any view changes come down as presentation hints
        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is LogoutPresentationHint)
            {
                Close(_mainViewController.ViewModel);
                return;
            }
            base.ChangePresentation(hint);
        }

        // TODO: We have to make sure views are opened and closed through Presenter - if view is opened
        private bool PresentView(IMvxTouchView view)
        {
            IViewPresenter parentView = null;

            // iterate reverse order...most likely latest view added will be target for presenting
            for (int i = _presentedViews.Count - 1; i >= 0; i--)
            {
                parentView = _presentedViews[i] as IViewPresenter;
                if (parentView != null && parentView.CanPresentView(view))
                {
                    break;
                }
                parentView = null;
            }
          
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

            IViewPresenter presentedViewToClose = null;
            if (viewToClose != null)
            {
                /*
                presentedViewToClose = viewToClose as IViewPresenter;
                if (presentedViewToClose != null && presentedViewToClose.CanClosePresentedView())
                {
                    presentedViewToClose.ClosePresentedView();
                }
                else
                {
                    presentedViewToClose = null;
                }
                */
                _presentedViews.Remove(viewToClose);

                if (_loginViewController != null && _loginViewController.ViewModel == toClose)
                {
                    _loginViewController = null;
                }
                else if (_loginViewController != null && _loginViewController.ViewModel == toClose)
                {
                    _loginViewController = null;
                }
            }

            if (presentedViewToClose == null)
            {
                // If presented view was not closed, we close it ourselves.
                // NOTE: Thise will only close (pop-off) the top-most controller on the navigation stack
                base.Close(toClose);    
            }
            
        }
    }
}