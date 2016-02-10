using PermissionGranter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Services
{
    public interface IUserDataService
    {
        void DeleteUser(User u);
        ObservableCollection<User> GetAllUsers();
        User GetUserDetail(int userID);
        void UpdateUser(User u);
    }
}
