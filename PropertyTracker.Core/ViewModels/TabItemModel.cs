using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core.ViewModels
{
    public class TabItemModel : MvxNotifyPropertyChanged
    {
        public Type ViewModelType { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set 
            { 
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string ImageName { get; set; }
        public string SelectedImageName { get; set; }

        private string _badgeValue;
        public string BadgeValue
        {
            get { return _badgeValue; }
            set 
            { 
                _badgeValue = value;
                RaisePropertyChanged(() => BadgeValue);
            }
        }
    }
}
