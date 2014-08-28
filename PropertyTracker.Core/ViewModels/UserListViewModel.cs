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

namespace PropertyTracker.Core.ViewModels
{
    public class UserListViewModel : TabItemModel
    {
        private readonly IPropertyTrackerService _propertyTrackerService;
        private readonly IUserDialogService _dialogService;    

        public UserListViewModel(IPropertyTrackerService service,  IUserDialogService dialogService) : base()
        {
            _propertyTrackerService = service;
            _dialogService = dialogService;

            TabTitle = "Users";
            TabImageName = "UserListIcon.png";
            TabSelectedImageName = null;
            TabBadgeValue = null;

            Users = new List<User>();
        }

        private List<User> _users;
        public List<User> Users
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
            UserList response = null;

            using (_dialogService.Loading("Getting users..."))
                response = await _propertyTrackerService.GetUsers();

            if (response != null)
                Users = response.Users;
            else
                _dialogService.Alert("Failed to retreive users", "Request Failed", "OK");

        }
        
        
    }
}
