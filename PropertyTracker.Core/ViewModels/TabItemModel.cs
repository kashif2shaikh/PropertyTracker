using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core.ViewModels
{
    public class TabItemModel : BaseViewModel
    {
      
        private string _tabTitle;
        public string TabTitle
        {
            get { return _tabTitle; }
            set 
            {
                _tabTitle = value;
                RaisePropertyChanged(() => TabTitle);
            }
        }

        public string TabImageName { get; set; }
        public string TabSelectedImageName { get; set; }

        private string _tabBadgeValue;
        public string TabBadgeValue
        {
            get { return _tabBadgeValue; }
            set 
            {
                _tabBadgeValue = value;
                RaisePropertyChanged(() => TabBadgeValue);
            }
        }
    }
}
