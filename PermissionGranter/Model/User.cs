using System;
using System.Collections.Generic;
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
    public class User: INotifyPropertyChanged
    {
        #region userInfo
        

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set 
            { 
                _LastName = value;
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
                NotifyPropertyChanged("FirstName");
            }
        }

        private string _Function;

        public string Function
        {
            get { return _Function; }
            set
            {
                _Function = value;
                NotifyPropertyChanged("Function");
            }
        }

        #endregion

        #region userPermissions
        //usergroups
        private List<UserGroup> _UserGroupPermissions = new List<UserGroup>();

        /// <summary>
        /// Array van Usergroups waar de gebruiker toe behoord.
        /// </summary>
        public List<UserGroup> UserGroupPermissions
        {
            get { return _UserGroupPermissions; }
            set { _UserGroupPermissions = value; }
        }

        //userAccessPermissions
        private Permissions _UserPermissions = new Permissions();

        /// <summary>
        /// Permissions die gezet zijn specifiek voor de gebruiker
        /// </summary>
        public Permissions UserPermissions
        {
            get { return _UserPermissions; }
            set { _UserPermissions = value; }
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

        //create a new Permissions object with allowed permissions and denied permissions combined
        private Permissions calculatePermissions()
        {
            if (UserGroupPermissions.Count() > 0)
            {
                Permissions p = UserPermissions;
                foreach (UserGroup usergroupPerms in UserGroupPermissions)
                    p = Permissions.combinePermissions(p, usergroupPerms.GroupPermissions);
                p.CalculatePermissions();
                this.UserCalculatedPermission = p.CalculatedPermissions;
                return p;
            }
            else
            {
                return UserPermissions;
            }
        }


        
        
        #endregion

        
        public User(string lastname, string firstname, string function)
        {
            LastName = lastname;
            FirstName = firstname;
            Function = function;
        
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
