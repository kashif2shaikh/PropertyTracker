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
	public class PaginatedPropertyListUpdatedEventArgs : EventArgs
	{
		public PaginatedPropertyList LastResult { get; set; }
	}

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

		public List<int> UserIdListFilter { get; set; }

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
		public event EventHandler<PaginatedPropertyListUpdatedEventArgs> PropertyListUpdatedHandler;
       

		public async Task GetProperties()
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
				UserIdListFilter = UserIdListFilter,
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
				OnPropertyListUpdated (result);
			}
		}

		private void OnPropertyListUpdated(PaginatedPropertyList result)
		{
			if(PropertyListUpdatedHandler != null) {
				var args = new PaginatedPropertyListUpdatedEventArgs () {
					LastResult = result
				};
				PropertyListUpdatedHandler (this, args);
			}
		}

		public async Task GetMoreProperties( )
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
				
			var result = await GetPropertiesAsync(requestParams);
			if(result != null) 
			{
				foreach (var p in result.Properties)
				{
					Properties.Add(p);
				}
				LastResult = result;
				OnPropertyListUpdated (result);
			}

		}

        private async Task<PaginatedPropertyList> GetPropertiesAsync(PropertyListRequest requestParams)
        {
            object response = null;
		                      
            using (_dialogService.Loading("Getting properties..."))
                response = await _propertyTrackerService.GetProperties(requestParams);

            if (response is PaginatedPropertyList)
            {
                return response as PaginatedPropertyList;
            }

            var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to retreive properties";
            _dialogService.Alert(msg, "Request Failed");
            return null;
        }
    }
}
