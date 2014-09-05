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
using PropertyTracker.Dto.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;

namespace PropertyTracker.Core.ViewModels
{


    public class PropertyListViewModel : TabItemModel
    {
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;

    

		private PropertyListRequest _requestParams { get; set; }
        private PaginatedPropertyListModel _listModel { get; set; }
        
        public PropertyListViewModel(IPropertyTrackerService service,  IUserDialogService dialogService) : base()
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;

            TabTitle = "Properties";
            TabImageName = "PropertyListIcon.png";
            TabSelectedImageName = null;
            TabBadgeValue = null;

			_properties = new ObservableCollection<Property> ();
			_listModel = new PaginatedPropertyListModel(service, dialogService);
			_listModel.Properties = _properties;
        }

		private void Reset()
		{
			_listModel.Reset ();

			RaisePropertyChanged(() => Properties);
			RaisePropertyChanged(() => CurrentPage);
			RaisePropertyChanged(() => PageSize);
			RaisePropertyChanged(() => NameFilter);
			RaisePropertyChanged(() => CityFilter);
			RaisePropertyChanged(() => StateFilter);
			RaisePropertyChanged(() => SortColumn);
			RaisePropertyChanged(() => SortAscending);
		}

       
        public override void Start()
        {
            base.Start();
			_listModel.GetProperties ();
        }

		private ObservableCollection<Property> _properties;
		public ObservableCollection<Property> Properties
		{
			get { return _properties; }
			set 
			{ 
				_properties = value;
				_listModel.Properties = _properties;
				RaisePropertyChanged(() => Properties);
			}
		}

		private int CurrentPage
		{
			get { return _listModel.CurrentPage; }
			set 
			{ 
				_listModel.CurrentPage = value;
				RaisePropertyChanged(() => CurrentPage);
			}
		}
						
		public int PageSize
		{
			get { return _listModel.PageSize; }
			set 
			{ 
				_listModel.PageSize = value;
				RaisePropertyChanged(() => PageSize);
				_listModel.GetProperties ();
			}
		}
			
		public string NameFilter
		{
			get { return _listModel.NameFilter; }
			set 
			{ 
				_listModel.NameFilter = value;
				RaisePropertyChanged(() => NameFilter);
				GetPropertiesAfterDelay ();
			}
		}

		// We don't want to hammer the server for each keystroke! 
		// Put half-second delay before we start searching 
		private CancellationTokenSource _tokenSource = null;
		private async void GetPropertiesAfterDelay()
		{
			if(_tokenSource != null)
			{
				_tokenSource.Cancel ();
				_tokenSource = null;
			}
			_tokenSource = new CancellationTokenSource ();
			try
			{
				await Task.Delay (500, _tokenSource.Token);
				_listModel.GetProperties();
			}
			catch (TaskCanceledException e)
			{

			}
		}
			
		public string CityFilter
		{
			get { return _listModel.CityFilter; }
			set 
			{ 
				_listModel.CityFilter = value;
				RaisePropertyChanged(() => CityFilter);
				_listModel.GetProperties ();
			}
		}
				
		public string StateFilter
		{
			get { return _listModel.StateFilter; }
			set 
			{ 
				_listModel.StateFilter = value;
				RaisePropertyChanged(() => StateFilter);
				_listModel.GetProperties ();
			}
		}
			
		public string SortColumn
		{
			get { return _listModel.SortColumn; }
			set 
			{ 
				_listModel.SortColumn = value;
				RaisePropertyChanged(() => SortColumn);
				_listModel.GetProperties ();
			}
		}
			
		public bool SortAscending
		{
			get { return _listModel.SortAscending; }
			set 
			{ 
				_listModel.SortAscending = value;
				RaisePropertyChanged(() => SortAscending);
				_listModel.GetProperties ();
			}
		}

		public IMvxCommand GetPropertiesCommand
		{
			get { return new MvxCommand (() => {
					 Reset ();
					_listModel.GetProperties ();
				}); }
		}

		public IMvxCommand GetMorePropertiesCommand
		{
			get { return new MvxCommand (() => _listModel.GetMoreProperties ());; }
		}


        public IMvxCommand LogoutCommand
        {
            get { return new MvxCommand(() => ChangePresentation(new LogoutPresentationHint())); }
        }
    }
}
