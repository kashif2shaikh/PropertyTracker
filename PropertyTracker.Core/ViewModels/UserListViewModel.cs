using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.PresentationHints;

namespace PropertyTracker.Core.ViewModels
{
    public class UserListViewModel : TabItemModel
    {
        public override void Start()
        {
            TabTitle = "Users";
            TabImageName = "UserListIcon.png";
            TabSelectedImageName = null;
            TabBadgeValue = null;
        }

        public void ChangeStuff()
        {
            TabTitle = "Huh";
            TabBadgeValue = "2";
        }

        public IMvxCommand LogoutCommand
        {
            get { return new MvxCommand(() => ChangePresentation(new LogoutPresentationHint())); }
        }
    }
}
