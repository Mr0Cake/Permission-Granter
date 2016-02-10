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

        public bool IsCorrect
        {
            get { return    !string.IsNullOrEmpty(LastName) && LastName.Length < 30 && 
                            !string.IsNullOrEmpty(FirstName) && FirstName.Length < 30 &&
                            !string.IsNullOrEmpty(Email) && Email.Length < 80 &&
                            !string.IsNullOrEmpty(Password) && Password.Length < 50; }
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
            }
        }


        public int UserID { get; set; }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                string salt = base.InstanceID.ToString() ;
                string password = value;
                PasswordEncryption.EncryptPassword(ref password, 0, out salt);
                _Password = password;

                NotifyPropertyChanged("Iscorrect");
                NotifyPropertyChanged("Password");
            }
        }

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


        
        

        #endregion

        #region userPermissions
        //usergroups
        private IList<UserGroup> _UserGroupPermissions = null;

        /// <summary>
        /// Array van Usergroups waar de gebruiker toe behoord.
        /// </summary>
        public IList<UserGroup> UserGroupPermissions
        {
            get { return _UserGroupPermissions ?? (_UserGroupPermissions = ViewModel.BLL.GroupBLL.GetGroupsByUserID(this.UserID)); }
            set { _UserGroupPermissions = value; }
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
            u.OwnedPermissions = this.OwnedPermissions.GetCopy();
            u.UserGroupPermissions = new List<UserGroup>();
            foreach (UserGroup group in UserGroupPermissions)
            {
                u.UserGroupPermissions.Add(group.GetCopy() as UserGroup);
            }
            return u;
        }
    }
}
