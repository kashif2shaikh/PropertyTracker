using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Messenger;
using PropertyTracker.Core.Services;

namespace PropertyTracker.Core.ViewModels
{
    
    public class StatePickerViewModel : BaseViewModel
    {

        private readonly IGeoDataService _geoService;
        public StatePickerViewModel(IGeoDataService geoService)
        {
            _geoService = geoService;
        }
    }

    public class StatePickerMessage : MvxMessage
    {
        public StatePickerMessage(object sender, string state)
            : base(sender)
        {
            State = state;
        }

        public string State { get; private set; }
    }
}
