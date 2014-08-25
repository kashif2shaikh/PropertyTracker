
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using PropertyTracker.Core;
using PropertyTracker.Core.ViewModels;
using PropertyTracker.UI.iOS.Views;
using MonoTouch.ObjCRuntime;
using PropertyTracker.UI.iOS.Common;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    [Register("MainViewController")]
    public sealed class MainViewController : MvxTabBarViewController, IViewPresenter
    {
        private PropertyListViewController _propertyListViewController;
        private UserListViewController _userListViewController;

        private new MainViewModel ViewModel
        {
            get { return (MainViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        
        public MainViewController()
            
        {
            // This is hack for inheriting from tab bar controller and making it work under Mvx
            ViewDidLoad();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Same hack 
            if (ViewModel == null)
                return;

            EdgesForExtendedLayout = UIRectEdge.None;

            NavigationItem.HidesBackButton = true;
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem("GO BACK", UIBarButtonItemStyle.Plain, (o, e) => { });
            NavigationItem.Title = "Main View";

            var viewControllers = new List<UIViewController>();
            
            foreach (var tabType in ViewModel.Tabs)
            {
                var tabView = this.CreateViewControllerFor(tabType) as UIViewController;
                viewControllers.Add(tabView.EmbedWithNavigation());                              
            }
            

            this.ViewControllers = viewControllers.ToArray();
            this.CustomizableViewControllers = new UIViewController[] { };
            this.SelectedViewController = ViewControllers[0];

            // Tab view controllers are loaded on demand when tab is selected. We instead want to force load controllers, 
            // so that ViewDidLoad is called, where the tab view controllers initialize their tab items and fire off 
            // network requests, so that views are pre-loaded when tapped on.
            //
            // TODO: We will need a way for the ViewModel to refresh the UI or automatically refresh based on time
            //
            this.ForceLoadViewControllers();

            // Create a binding from target property in view controller to source property in view model
            var bindingSet = this.CreateBindingSet<MainViewController, MainViewModel>();            
            bindingSet.Bind(this).For(v => v.SelectedIndex).To(vm => vm.SelectedTabIndex);
            bindingSet.Apply();                      
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBarHidden = true;

            Console.WriteLine("MainView NavController" + NavigationController);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            //ViewModel.SwitchToTab(1);
        }

        /* IViewPresenter Methods */
        public void ViewPresenterAdded()
        {
          
        }

        public void ViewPresenterRemoved()
        {

        }

        public bool CanPresentView(IMvxTouchView view)
        {
            return false;
            //return (view.Request.RequestedBy.AdditionalInfo == ViewModel.ViewInstanceId.ToString()) ;       
        }

        public void PresentView(IMvxTouchView view)
        {
           // Don't load any views through Presenter
        }        
    }
}

