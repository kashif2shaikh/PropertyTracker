using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using FluentValidation.Results;
using Newtonsoft.Json;
using PropertyTracker.Core.Services;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Core.ViewModels
{
    public class PropertyPickerViewModel : BaseViewModel
    {
        
        private readonly IPropertyTrackerService _propertyTrackerService;       
        private readonly IUserDialogService _dialogService;
        private readonly IMvxMessenger _messenger;       
        private readonly PaginatedPropertyListModel _listModel;


		public bool ViewOnlyMode {get; set;}

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

        private List<Property> _selectedProperties; 

		private List<int> _selectedPropertyIndexList;
		public List<int> SelectedPropertyIndexList
        {
            get { return _selectedPropertyIndexList; }
            set
            {
                _selectedPropertyIndexList = value;
                RaisePropertyChanged(() => SelectedPropertyIndexList);
            }
        }

        public PropertyPickerViewModel(IPropertyTrackerService propertyTrackerService,  IUserDialogService dialogService, IMvxMessenger messenger)
        {
            _propertyTrackerService = propertyTrackerService;            
            _dialogService = dialogService;
            _messenger = messenger;

            _properties = new ObservableCollection<Property>();          
			_selectedPropertyIndexList = new List<int> ();
            _listModel = new PaginatedPropertyListModel(_propertyTrackerService, _dialogService)
            {
                Properties = _properties,
                // Return all properties as picker won't work if selected items cross over multiple batches!
                PageSize = PropertyListRequest.NoLimitForPageSize
            };
        }

        public void Init(bool viewOnlyMode, string jsonSelectedPropertyList, Guid requestedViewId)
        {		            
			ViewOnlyMode = viewOnlyMode;
            RequestedByViewInstanceId = requestedViewId;
            _selectedProperties = JsonConvert.DeserializeObject<List<Property>>(jsonSelectedPropertyList);                     
        }

        private void CreateSelectedPropertyIndexList()
        {
            if (_selectedProperties != null && Properties != null)
            {
                var pindex = 0;
                foreach (var prop in Properties)
                {
                    if (_selectedProperties.FirstOrDefault(p => p.Id == prop.Id) != null)
                    {
                        SelectedPropertyIndexList.Add(pindex);
                    }
                    pindex++;
                }
                _selectedProperties = null;
            }
        }

        public override async void Start()
        {
            base.Start();
            await _listModel.GetProperties();
            CreateSelectedPropertyIndexList();
        }

        /*
        public IMvxCommand GetPropertiesCommand
        {
            get
            {
                return new MvxCommand(() => _listModel.GetProperties());
            }
        }

        public IMvxCommand GetMorePropertiesCommand
        {
            get { return new MvxCommand(() => _listModel.GetMoreProperties()); }
        }
        */

        public ICommand PropertyPickerDoneCommand
        {
			get { return new MvxCommand(SendPropertyPickerMessage); }
        }

        protected void SendPropertyPickerMessage()
        {
            var properties = SelectedPropertyIndexList.Select(index => Properties.ElementAt(index)).ToList();

            var message = new PropertyPickerMessage(this, properties);
            _messenger.Publish(message);
        }
    }

    public class PropertyPickerMessage : MvxMessage
    {
        public PropertyPickerMessage(object sender, List<Property> properties)
            : base(sender)
        {
            Properties = properties;
        }

        public List<Property> Properties { get; private set; }
    }
}