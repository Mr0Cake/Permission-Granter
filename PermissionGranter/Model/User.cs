using System;
using System.Collections.Generic;
using System.Linq;
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
    public class User
    {
        #region userInfo
        private string lastName { public get; set; }
        private string firstName { public get; set; }
        private string function { public get; set; }




        #endregion

        #region userPermissions
        //usergroups
        private UserGroup[] _UserGroupPolicies;

        /// <summary>
        /// Array van Usergroups waar de gebruiker toe behoord.
        /// </summary>
        public UserGroup[] UserGroupPolicies
        {
            get { return _UserGroupPolicies; }
            set { _UserGroupPolicies = value; }
        }

        //userAccessPermissions
        private Permissions _UserPermissions;

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
            get { return _UserCalculatedPermission; }
            set { _UserCalculatedPermission = value; }
        }

        private Dictionary<string, HashSet<string>> calculatePermissions()
        {
            Dictionary<string, HashSet<string>> perms = new Dictionary<string, HashSet<string>>();
            foreach (UserGroup ug in UserGroupPolicies)
            {
                foreach (KeyValuePair<string, HashSet<string>> kvp in ug.Permissions)
                    perms.TryAdd(kvp.Key,kvp.Value);
            }
            List<string> lstKeys = new List<string>();
            foreach(KeyValuePair<string,HashSet<string>> kvpU in UserPermissions){
                if (!perms.TryAdd(kvpU.Key, kvpU.Value)) 
                {
                    lstKeys.Add(kvpU.Key);
                }
            }
            foreach (string s in lstKeys)
            {
                perms[s].AddRange(UserPermissions[s]);
            }

            List<string> lstKeysToRemove = new List<string>();
            foreach(string s in UserDenyPermission.Keys)
            {
                HashSet<string> stringvalue;
                if (perms.TryGetValue(s, out stringvalue)) 
                {
                    stringvalue = deletePermissions(s,stringvalue);
                }
                if (stringvalue.Count == 0)
                    lstKeysToRemove.Add(s);

            }

            foreach (string s in lstKeysToRemove)
            {
                perms.Remove(s);
            }

            return perms;
        }

        private HashSet<string> deletePermissions(string key, HashSet<string> value)
        {
            foreach (string s in UserDenyPermission[key])
            {
                if (value.Contains(s))
                {
                    value.Remove(s);
                }
            }
            return value;
        }
        

        
        
        #endregion

        
        

        

        


        public User(string lastname, string firstname, string function)
        {
            this.lastName = lastname;
            this.firstName = firstname;
            this.function = function;
        }


    }
}
