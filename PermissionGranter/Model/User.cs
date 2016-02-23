using PermissionGranter.ViewModel;
using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    /// <summary>
    /// Beschrijft een gebruiker en zijn permissies
    /// Een gebruiker kan worden toegevoegd aan meerdere groepen
    /// De permissies van de groep worden dan aangezet
    /// Permissies kunnen geweigerd worden, deze worden uitgezet na berekening van de groep en user permissies
    /// Dus een weigering van een permissie heeft voorang op de toegang van een permissie
    /// </summary>
    public class User: PermissionsBase
    {
        #region userInfo

        public override bool IsCorrect
        {
            get { return    !string.IsNullOrEmpty(LastName) && LastName.Length < 30 && 
                            !string.IsNullOrEmpty(FirstName) && FirstName.Length < 30 &&
                            !string.IsNullOrEmpty(Email) && Email.Length < 80 &&
                            !string.IsNullOrEmpty(Password) && Password.Length < 50; }
        }


        public override int ID
        {
            get
            {
                return UserID;
            }
        }

        public override string Name
        {
            get { return FirstName + " " + LastName; }
        }


        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set 
            { 
                _LastName = value;
                NotifyPropertyChanged("Output");
                NotifyPropertyChanged("Iscorrect");
                NotifyPropertyChanged("LastName");
                NotifyPropertyChanged("Name");
            }
        }

        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                NotifyPropertyChanged("Output");
                NotifyPropertyChanged("Iscorrect");
                NotifyPropertyChanged("FirstName");
                NotifyPropertyChanged("Name");
            }
        }


        public int UserID { get; set; }

        public void SetPassword(string pw)
        {
            _Password = pw;
        }

        private string GetPassword()
        {
            string password = Password;
            string salt = InstanceID.ToString();
            PasswordEncryption.EncryptPassword(ref password, 0, out salt);
            if (string.IsNullOrEmpty(Salt))
            {
                Salt = salt;
            }
            return password;
        }

        private string _EncryptedPassword;

        public string EncryptedPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_EncryptedPassword))
                    _EncryptedPassword = GetPassword();
                return _EncryptedPassword;
            }
            set
            {
                _Password = value;
            }
        }

        public bool PasswordChanged { get; set; }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                //_Password = value;

                string salt = InstanceID.ToString() ;
                string password = value;
                PasswordEncryption.EncryptPassword(ref password, 0, out salt);
                _Password = password;
                Salt = salt;
                PasswordChanged = true;
                NotifyPropertyChanged("Iscorrect");
                NotifyPropertyChanged("Password");
            }
        }

        public string Salt { get; set; }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                NotifyPropertyChanged("Iscorrect");
                NotifyPropertyChanged("Email");
            }
        }

        //public override bool Equals(object obj)
        //{
        //    return base.Equals(obj);
        //}



        #endregion

        #region userPermissions
        //usergroups
        private ObservableCollection<UserGroup> _UserGroupPermissions = new ObservableCollection<UserGroup>();

        /// <summary>
        /// Array van Usergroups waar de gebruiker toe behoord.
        /// </summary>
        public ObservableCollection<UserGroup> UserGroupPermissions
        {
            get
            {
                if((_UserGroupPermissions == null || _UserGroupPermissions.Count==0 )&& ID != -1)
                {
                    ViewModel.BLL.GroupBLL.GetGroupsByUserID(this.UserID).ToList().ForEach(x => _UserGroupPermissions.Add(x));
                }
                return _UserGroupPermissions;
            }
            set
            {
                _UserGroupPermissions = value;
                NotifyPropertyChanged("UserGroupPermissions");
            }
        }
        

        //calculatePermission
        private Dictionary<string, HashSet<string>> _UserCalculatedPermission;

        /// <summary>
        /// Permissions die nog over blijven na toevoegen van grouppermissions, userpermissions
        /// en na verwijdering van denypermissions
        /// </summary>
        public Dictionary<string, HashSet<string>> UserCalculatedPermission
        {
            get 
            {
                if (_UserCalculatedPermission == null)
                {
                    Permissions p = calculatePermissions();
                    _UserCalculatedPermission = p.CalculatedPermissions;
                    //_UserCalculatedPermission = calculatePermissions().CalculatedPermissions;
                }
                return _UserCalculatedPermission; 
            }
            set { _UserCalculatedPermission = value; }
        }
        

        //combineer allow en deny van alle groepen en voeg samen in 1 permissions object
        private Permissions calculatePermissions()
        {
            if (UserGroupPermissions.Count() > 0)
            {
                Permissions p = OwnedPermissions;
                foreach (UserGroup usergroupPerms in UserGroupPermissions)
                    p = Permissions.combinePermissions(p, usergroupPerms.OwnedPermissions);
                p.CalculatePermissions();
                this.UserCalculatedPermission = p.CalculatedPermissions;
                return p;
            }
            else
            {
                return OwnedPermissions;
            }
        }


        
        
        #endregion


        public User()
        {
            UserID = -1;
            Changed = false;
            InstanceID = Guid.NewGuid();
        }

        public User(int userID)
        {
            InstanceID = Guid.NewGuid();
            UserID = userID;
            Changed = false;
        }
        
        public User(int userID, string lastname, string firstname):this(userID)
        {
            LastName = lastname;
            FirstName = firstname;
        }

        public User(string lastname, string firstname):this()
        {
            LastName = lastname;
            FirstName = firstname;

        }

        public string Output
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

        public override PermissionsBase GetCopy()
        {
            User u = new User(this.LastName, this.FirstName);
            u.InstanceID = InstanceID;
            u.Email = Email;
            u.Password = Password;
            u.UserID = UserID;
            u.Salt = Salt;
            u.OwnedPermissions = this.OwnedPermissions.GetCopy();
            u.UserGroupPermissions = new ObservableCollection<UserGroup>();
            foreach (UserGroup group in UserGroupPermissions)
            {
                u.UserGroupPermissions.Add(group.GetCopy() as UserGroup);
            }
            return u;
        }
    }
}
