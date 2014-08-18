using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyTracker.Core.Services
{
    interface IPropertyTrackerService
    {
        bool LoggedIn { get; }
    }

    public class PropertyTrackerService : IPropertyTrackerService
    {
        public bool LoggedIn { get; private set; }

        public PropertyTrackerService()
        {
            LoggedIn = false;
        }


        


    }
}
