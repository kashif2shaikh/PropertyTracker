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

        public UserListViewModel(IPropertyTrackerService service,  IUserDialogService dialogService)
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

            object response = null;
            using (_dialogService.Loading("Getting users..."))            
                response = await _propertyTrackerService.GetUsers();                                                           

            if (response is UserList)
            {
                Users = (response as UserList).Users;
            }
            else
            {
                var msg = response is ErrorResult ? (response as ErrorResult).Message : "Failed to retreive users";
                _dialogService.Alert(msg, "Request Failed");
            } 
        }
               
    }
}
