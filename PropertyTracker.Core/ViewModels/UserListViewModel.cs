using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.MvvmCross.ViewModels;
using PropertyTracker.Core.PresentationHints;
using PropertyTracker.Core.Services;
using PropertyTracker.Dto.Models;
using Cirrious.MvvmCross.Plugins.Messenger;
using System.Collections.ObjectModel;

namespace PropertyTracker.Core.ViewModels
{
    public class UserListViewModel : TabItemModel
    {
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;
		private readonly  IMvxMessenger _messenger;

		public UserListViewModel(IPropertyTrackerService service,  IUserDialogService dialogService,  IMvxMessenger messenger)
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;
			_messenger = messenger;

            TabTitle = "Users";
            TabImageName = "UserListIcon.png";
            TabSelectedImageName = null;
            TabBadgeValue = null;

			Users = new ObservableCollection<User>();
			RegisterSubscriptions ();

        }

		private MvxSubscriptionToken _usersUpdatedMessageToken;
		private MvxSubscriptionToken _propertiesUpdatedMessageToken;
		protected void RegisterSubscriptions()
		{
			_usersUpdatedMessageToken = _messenger.Subscribe<UsersUpdatedMessage> (OnUsersUpdatedMessaged);
			_propertiesUpdatedMessageToken = _messenger.Subscribe<PropertiesUpdatedMessage>(OnPropertiesUpdatedMessaged);
		}

		private void OnUsersUpdatedMessaged(UsersUpdatedMessage msg)
		{
			// Users were added/updated, refresh list
			GetUsers ();
		}
		private void  OnPropertiesUpdatedMessaged(PropertiesUpdatedMessage msg)
		{
			// Property was added/removed, refresh list, because user contains property info
			GetUsers ();
		}

		private ObservableCollection<User> _users;
		public ObservableCollection<User> Users
        {
            get { return _users; }
            set 
            { 
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public IMvxCommand LogoutCommand
        {
            get { return new MvxCommand(() => ChangePresentation(new LogoutPresentationHint())); }
        }

        public IMvxCommand GetUsersCommand
        {
            get { return new MvxCommand(GetUsers); }
        }

        public override void Start()
        {
            base.Start();
            GetUsers();
        }

        public async void GetUsers()
        {

            object response = null;
            using (_dialogService.Loading("Getting users..."))            
                response = await _propertyTrackerService.GetUsers();                                                           

            if (response is UserList)
            {
				Users.Clear ();
				foreach(var user in (response as UserList).Users) {
					Users.Add(user);
				}
				//Users = new (response as UserList).Users;
            }
            else
            {
                var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to retreive users";
                _dialogService.Alert(msg, "Request Failed");
            } 
        }


               
    }

	public class UsersUpdatedMessage : MvxMessage
	{
		public UsersUpdatedMessage(object sender)
			: base(sender)
		{

		}

		public User User { get; set;}
	}
}
