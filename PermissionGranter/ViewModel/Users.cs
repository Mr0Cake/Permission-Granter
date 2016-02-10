using PermissionGranter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel
{
    public class Users:INotifyPropertyChanged
    {
        private ObservableCollection<User> _Users = new ObservableCollection<User>();

        public ObservableCollection<User> UserList
        {
            get { return _Users; }
            set {
                _Users = value;
                NotifyPropertyChanged("UserList");
            }
        }

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value; 
                NotifyPropertyChanged("CurrentUser");
            }
        }
        

        public void GetUsersFromDatabase()
        {
            
            
            
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
