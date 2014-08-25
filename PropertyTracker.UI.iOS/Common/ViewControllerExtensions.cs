using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PropertyTracker.Core.ViewModels;


namespace PropertyTracker.UI.iOS.Common
{
    public static class ViewControllerExtensions
    {
        public static UIViewController CreateViewControllerForTab<TViewModel>(this IMvxTouchView view, string title, string imageName, string selectedImageName, string badgeValue) where TViewModel : MvxViewModel
        {
            return view.CreateViewControllerForTab(typeof(TViewModel), title, imageName, selectedImageName, badgeValue);            
        }

        public static UIViewController CreateViewControllerForTab(this IMvxTouchView view, Type viewModelType, string title, string imageName, string selectedImageName, string badgeValue)
        {           
            var viewController = view.CreateViewControllerFor(viewModelType) as UIViewController;
            viewController.SetTitleAndTabBarItem(title, imageName, selectedImageName, badgeValue);

            return viewController;
        }

        /*
        public static UIViewController CreateViewControllerForTab(this IMvxTouchView view, TabItemModel tab)
        {
            var viewController = view.CreateViewControllerForTab(tab.ViewModelType, tab.Title, tab.ImageName, tab.SelectedImageName, tab.BadgeValue);
            return viewController;
        }
        */

        public static void SetTitleAndTabBarItem(this UIViewController viewController, string title, string imageName, string selectedImageName = null, string badgeValue = null)
        {
            UIImage image = UIImage.FromBundle(Constants.ImagePath + "/" + imageName);
            UIImage selectedImage = selectedImageName != null ? UIImage.FromBundle(Constants.ImagePath + "/" + selectedImageName) : null;
   
            viewController.Title = title;
            viewController.TabBarItem = new UITabBarItem(title, image, selectedImage);
            viewController.TabBarItem.BadgeValue = badgeValue;                                
        }

        public static UINavigationController EmbedWithNavigation(this UIViewController viewController)
        {
            var navController = new UINavigationController();
            navController.PushViewController(viewController, false);
            return navController;
        }

        public static void ForceLoadViewControllers(this UITabBarController tabBarViewController)
        {
            foreach (var vc in tabBarViewController.ViewControllers)
            {
                UIViewController forceLoadView = vc;
                if (vc is UINavigationController)
                {
                    forceLoadView = (vc as UINavigationController).TopViewController;
                }
                UIView view = forceLoadView.View; // Accessing View property of controller will force load the view and call ViewDidLoad.                                                
            }
        } 

        
    }
}