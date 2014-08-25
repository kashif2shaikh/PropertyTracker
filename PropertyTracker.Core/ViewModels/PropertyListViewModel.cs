using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.PresentationHints;

namespace PropertyTracker.Core.ViewModels
{
    public class PropertyListViewModel : TabItemModel
    {
        public PropertyListViewModel() : base()
        {
            TabTitle = "Properties";
            TabImageName = "PropertyListIcon.png";
            TabSelectedImageName = null;
            TabBadgeValue = null;
        }

        public void ChangeStuff()
        {
            TabTitle = "Blah";
            TabBadgeValue = "1";
        }

        public IMvxCommand LogoutCommand
        {
            get { return new MvxCommand(() => ChangePresentation(new LogoutPresentationHint())); }
        }
    }
}
