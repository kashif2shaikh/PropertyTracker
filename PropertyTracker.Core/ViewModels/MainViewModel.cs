using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.PresentationHints;

namespace PropertyTracker.Core.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
        // While it's desirable to have a collection of ViewModels where ViewModels have implemented an ITabModel interface,
        // this won't work on Android since ViewModels cannot be statically allocated.
	    public ObservableCollection<TabItemModel> Tabs;

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                RaisePropertyChanged(() => SelectedTabIndex);
            }
        }

	    public MainViewModel() : base()
	    {
            // Initialize the tabs
            // Tabs are hard-coded for now - but in future we can pass in say IUser model and show
            // tabs based on the type of user.
	        Tabs = new ObservableCollection<TabItemModel>()
	        {
	            new TabItemModel
	            {
	                ViewModelType = typeof (PropertyListViewModel),
	                Title = "Properties",
	                ImageName = "PropertyListIcon.png",	                
	            },
	            new TabItemModel
	            {
	                ViewModelType = typeof (UserListViewModel),
	                Title = "Users",
	                ImageName = "UserListIcon.png",	                
	            }
	        };

	        SelectedTabIndex = 0;
	    }

	    public void SwitchToTab(int index)
	    {
	        SelectedTabIndex = index;
	        Tabs[0].Title = "SELECTED";
	        Tabs[0].BadgeValue = "1";
	    }
     
        public IMvxCommand LogoutCommand
        {
            get { return new MvxCommand(() => ChangePresentation(new LogoutPresentationHint())); }
        }
	}
}

