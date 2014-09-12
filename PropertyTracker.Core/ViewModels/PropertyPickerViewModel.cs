using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using FluentValidation.Results;
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

		private ObservableCollection<int> _selectedPropertyIndexList;
		public ObservableCollection<int> SelectedPropertyIndexList
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
			_selectedPropertyIndexList = new ObservableCollection<int> ();
            _listModel = new PaginatedPropertyListModel(_propertyTrackerService, _dialogService)
            {
                Properties = _properties
            };
        }

        public void Init(Guid requestedViewId)
        {		            
            RequestedByViewInstanceId = requestedViewId;            

            // TODO - figure out how we can add list of items.


        }

        public override void Start()
        {
            base.Start();
            _listModel.GetProperties();
        }

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