using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.Services;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Core.ViewModels
{
    public class PaginatedPropertyListModel : MvxNotifyPropertyChanged
    {
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;

        public PaginatedPropertyListModel(IPropertyTrackerService service,  IUserDialogService dialogService)
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;

            Properties = new ObservableCollection<Property>();
        }

        private ObservableCollection<Property> _properties;
        public ObservableCollection<Property> Properties
        {
            get { return _properties; }
            set 
            { 
                _properties = value;
                RaisePropertyChanged(() => Properties);
            }
        }
       
        public IMvxCommand GetPropertiesCommand
        {
            get { return new MvxCommand(GetProperties); }
        }
        
        public IMvxCommand GetMorePropertiesCommand
        {
            get { return new MvxCommand(GetMoreProperties); }
        }

        public PropertyListRequest RequestParams { get; set; }
        public PaginatedPropertyList LastResult;

        public async void GetProperties()
        {
            RequestParams = new PropertyListRequest
            {
                CurrentPage = 0,
                PageSize = 25,
                SortColumn = PropertyListRequest.CityColumn,
                SortAscending = true,
                //NameFilter = "Beach",
                //CityFilter = "San Diego",
                //StateFilter = "California"                
            };
            LastResult = await GetProperties(RequestParams);
            Properties = new ObservableCollection<Property>(LastResult.Properties);             
        }

        public async void GetMoreProperties()
        {
            RequestParams.CurrentPage += 1;
            LastResult = await GetProperties(RequestParams);
            foreach (var p in LastResult.Properties)
            {
                Properties.Add(p);
            }
        }

        public async Task<PaginatedPropertyList> GetProperties(PropertyListRequest requestParams)
        {
            PaginatedPropertyList response = null;
                      
            using (_dialogService.Loading("Getting properties..."))
                response = await _propertyTrackerService.GetProperties(requestParams);

            if (response != null)
            {
                return response;
            }
            else
            {
                _dialogService.Alert("Failed to retreive properties", "Request Failed", "OK");
                return null;
            }                
        }
    }
}
