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
    public class PaginatedPropertyListModel //: MvxNotifyPropertyChanged
    {
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;

		public ObservableCollection<Property> Properties { get; set;}

		public int CurrentPage { get; set;}

		public int PageSize { get; set;}

		public string NameFilter { get; set;}

		public string CityFilter { get; set;}

		public string StateFilter { get; set;}

		public string SortColumn { get; set;}

		public bool SortAscending { get; set;}

		public int TotalPages { get; set; }

		public PaginatedPropertyListModel(IPropertyTrackerService service,  IUserDialogService dialogService)
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;

			Reset ();
        }

		public void Reset()
		{
			CurrentPage = 0;
			PageSize = PropertyListRequest.DefaultPageSize;
			NameFilter = "";
			CityFilter = "";
			StateFilter = "";
			SortColumn = PropertyListRequest.NameColumn;
			SortAscending = true;
		}
			                							     
        public PaginatedPropertyList LastResult;
       

		public async void GetProperties()
		{
			CurrentPage = 0;

			var requestParams = new PropertyListRequest
			{
				CurrentPage = CurrentPage,
				PageSize = PageSize,
				NameFilter = NameFilter,
				CityFilter = CityFilter,
				StateFilter = StateFilter,
				SortColumn = SortColumn,
				SortAscending = SortAscending,
			};
			
			var result = await GetPropertiesAsync(requestParams);
			if(result != null)
			{
				Properties.Clear ();
				foreach (var p in result.Properties)
				{
					Properties.Add(p);
				}
				LastResult = result;
				TotalPages = LastResult.TotalPages;
			}
		}				

		public async void GetMoreProperties( )
		{
			CurrentPage += 1;

			var requestParams = new PropertyListRequest
			{
				CurrentPage = CurrentPage,
				PageSize = PageSize,
				NameFilter = NameFilter,
				CityFilter = CityFilter,
				StateFilter = StateFilter,
				SortColumn = SortColumn,
				SortAscending = SortAscending,
			};

			requestParams.CurrentPage += 1;
			var result = await GetPropertiesAsync(requestParams);
			if(result != null) 
			{
				foreach (var p in result.Properties)
				{
					Properties.Add(p);
				}
				LastResult = result;
			}

		}

        private async Task<PaginatedPropertyList> GetPropertiesAsync(PropertyListRequest requestParams)
        {
            PaginatedPropertyList response = null;

            
            //if(LastResult != null && requestParams.CurrentPage >= TotalPages) {
            //    _dialogService.Alert("", "All properties have been loaded", "OK");
            //    return null;
            //}
			
                      
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
