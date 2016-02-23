using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    /// <summary>
    /// Representeerd de permissies voor een bepaalde groep
    /// </summary>
    public class UserGroup : PermissionsBase
    {

        public UserGroup()
        {
            GroupID = -1;
            InstanceID = Guid.NewGuid();
        }

        public UserGroup(Permissions perms, string name, string description):this()
        {
            OwnedPermissions = perms;
            GroupName = name;
            Description = description;
        }

        public override int ID
        {
            get
            {
                return GroupID;
            }
        }

        public override string Name
        {
            get
            {
                return GroupName;
            }
        }

        public override bool IsCorrect
        {
            get
            {
                return !string.IsNullOrEmpty(GroupName) && GroupName.Length < 50 && !string.IsNullOrEmpty(Description) && Description.Length < 250;
            }
        }


        private int _GroupID;

        public int GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value; }
        }


        private string _GroupName;

        public string GroupName
        {
            get { return _GroupName; }
            set
            {
                
                _GroupName = value;
                NotifyPropertyChanged("GroupName");
                NotifyPropertyChanged("IsCorrect");
                NotifyPropertyChanged("Name");

            }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set
            {
                
                _Description = value;
                NotifyPropertyChanged("IsCorrect");
                NotifyPropertyChanged("Description");

            }
        }

        private int _DummyUser;

        public int DummyUser
        {
            get { return _DummyUser; }
            set
            {
                if (_DummyUser == value)
                    return;

                _DummyUser = value;
                NotifyPropertyChanged("DummyUser");
            }
        }


        //public override bool Equals(object obj)
        //{
        //    return base.Equals(obj);
        //}

        public override string ToString()
        {
            return GroupName + " - " + Description;
        }

        private ObservableCollection<User> _GroupUsers;

        public ObservableCollection<User> GroupUsers
        {
            get
            {
                if (_GroupUsers == null && ID > -1)
                {
                    _GroupUsers = new ObservableCollection<User>();

                }
                    
                return _GroupUsers ;
            }
            set { _GroupUsers = value; }
        }


        public override PermissionsBase GetCopy()
        {
            UserGroup copy = new UserGroup();
            copy.InstanceID = InstanceID;
            copy.GroupName = GroupName;
            copy.GroupID = GroupID;
            copy.OwnedPermissions = OwnedPermissions.GetCopy();
            copy.Description = Description;
            copy.DummyUser = DummyUser;
            return copy;
        }
    }
}
