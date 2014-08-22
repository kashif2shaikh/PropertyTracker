
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
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

            
            
            
            foreach (var tab in ViewModel.Tabs)
            {
                var tabView = this.CreateViewControllerForTab(tab) as MvxViewController;
                viewControllers.Add(EmbedWithNavigation(tabView as UIViewController));
                // TODO look into binding the tab item
                var tabBindingSet = tabView.CreateBindingSet<MvxViewController, TabItemModel>();
                tabBindingSet.Bind(tabView).For(tv => tv.TabBarItem.Title).To(tm => tm.Title);
                tabBindingSet.Bind(tabView).For(tv => tv.TabBarItem.BadgeValue).To(tm => tm.BadgeValue);
                tabBindingSet.Apply();
                //tabView.Bind(tabView, tv => tv.Title, (TabItemModel tm) => tm.Title);
            }
            

            this.ViewControllers = viewControllers.ToArray();
            this.CustomizableViewControllers = new UIViewController[] { };
            this.SelectedViewController = ViewControllers[0];

            var bindingSet = this.CreateBindingSet<MainViewController, MainViewModel>();

            // Create a binding from target property in view controller to source property in view model
            bindingSet.Bind(this).For(v => v.SelectedIndex).To(vm => vm.SelectedTabIndex);
            bindingSet.Apply();
            

            /*

            // Perform any additional setup after loading the view, typically from a nib.
            //this.CreateBinding(LogoutButton).To<MainViewModel>(vm => vm.ShowLoginViewCommand).Apply();

            var propertyListNavController = CreateTabFor<PropertyListViewModel>(0, "Properties", "PropertyTabIcon.png");
            var userListNavController = CreateTabFor<UserListViewModel>(1, "Users", "UserTabIcon.png");

            _propertyListViewController = (PropertyListViewController)propertyListNavController.TopViewController;
            _userListViewController = (UserListViewController)userListNavController.TopViewController;
            
            ViewModel.PropertyListView = _propertyListViewController.ViewModel;
            ViewModel.UserListView = _userListViewController.ViewModel;

            var viewControllers = new UIViewController[]
                                  {
                                      propertyListNavController,
                                      userListNavController                                   
                                  };

            this.ViewControllers = viewControllers;
            this.CustomizableViewControllers = new UIViewController[] { };
            this.SelectedViewController = ViewControllers[0];
             * 
            */

            //var nextViewController = segue.DestinationViewController as DetailViewController;
            //nextViewController.Request = new MvxViewModelInstanceRequest(new DetailViewModel() { Menu = menu });
        }

        private UINavigationController EmbedWithNavigation(UIViewController viewController)
        {
            var navController = new UINavigationController();
            navController.PushViewController(viewController, false);
            return navController;
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
            ViewModel.SwitchToTab(1);
        }

        /*
        private void PresentTabViews()
        {
           // var propertyListNavController = CreateTabFor(_propertyListViewController, 0, "Properties", "PropertyTabIcon.png");
          //  var userListNavController = CreateTabFor(_userListViewController, 1, "Users", "UserTabIcon.png");
            
            //ViewModel.PropertyListView = _propertyListViewController.ViewModel;
            //ViewModel.UserListView = _userListViewController.ViewModel;

            var viewControllers = new UIViewController[]
                                  {
                                      propertyListNavController,
                                      userListNavController               
                                  };

            this.ViewControllers = viewControllers;
            this.CustomizableViewControllers = new UIViewController[] { };
            this.SelectedViewController = ViewControllers[0];
        }
        */
      

        /*
        public IMvxTouchView CreateViewControllerFor(Type viewModelType)
        {
            var request = new MvxViewModelRequest(viewModelType, null, null, MvxRequestedBy.UserAction);
            return this.CreateViewControllerFor(request);
        }
        */


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

