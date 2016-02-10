using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionGranter.Model;
using PermissionGranter.ViewModel.BLL;

namespace PermissionGranter.ViewModel.Services
{
    public class UserDataService : IUserDataService
    {
        private ObservableCollection<User> _Users;

        public ObservableCollection<User> Users
        {
            get { return _Users; }
            set { _Users = value; }
        }


        public void DeleteUser(User u)
        {
            //UserBLL.
        }

        public ObservableCollection<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUserDetail(int userID)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User u)
        {
            throw new NotImplementedException();
        }
    }
}
