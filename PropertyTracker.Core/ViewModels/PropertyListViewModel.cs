using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.PresentationHints;
using PropertyTracker.Core.Services;
using PropertyTracker.Core.ViewModels;

namespace PropertyTracker.Core.ViewModels
{


    public class PropertyListViewModel : TabItemModel
    {
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;
    
        public PaginatedPropertyListModel PaginatedPropertyListModel { get; set; }
        
        public PropertyListViewModel(IPropertyTrackerService service,  IUserDialogService dialogService) : base()
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;

            TabTitle = "Properties";
            TabImageName = "PropertyListIcon.png";
            TabSelectedImageName = null;
            TabBadgeValue = null;

            PaginatedPropertyListModel = new PaginatedPropertyListModel(service, dialogService);
        }

        public void ChangeStuff()
        {
            TabTitle = "Blah";
            TabBadgeValue = "1";
        }

        public override void Start()
        {
            base.Start();
            PaginatedPropertyListModel.GetProperties();
        }

        public IMvxCommand LogoutCommand
        {
            get { return new MvxCommand(() => ChangePresentation(new LogoutPresentationHint())); }
        }
    }
}
