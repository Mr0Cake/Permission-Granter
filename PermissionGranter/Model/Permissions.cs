using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public class Permissions
    {
        public Permissions(Dictionary<string, HashSet<string>> allow, Dictionary<string, HashSet<string>> deny)
        {
            AllowPermissions = allow;
            DenyPermissions = deny;
        }

        //allow
        private Dictionary<string, HashSet<string>> _AllowPermissions;

        /// <summary>
        /// Permissions die zijn toegelaten
        /// </summary>
        public Dictionary<string, HashSet<string>> AllowPermissions
        {
            get { return _AllowPermissions; }
            set { _AllowPermissions = value; }
        }


        //deny

        private Dictionary<string, HashSet<string>> _DenyPermissions;

        /// <summary>
        /// Permissions die verboden zijn
        /// </summary>
        public Dictionary<string, HashSet<string>> DenyPermissions
        {
            get { return _CalculatedPermissions; }
            set { _CalculatedPermissions = value; }
        }

        private Dictionary<string, HashSet<string>> _CalculatedPermissions;

        /// <summary>
        /// Permissions die verboden zijn
        /// </summary>
        public Dictionary<string, HashSet<string>> CalculatedPermissions
        {
            get { return _CalculatedPermissions; }
            set { _CalculatedPermissions = value; }
        }


        //calculate
        /// <summary>
        /// CalculatePermissions will return a Dictionary of permissions that removed the deny permissions from the allowpermissions. 
        /// 
        /// </summary>
        /// <param name="usergroup">List of groups a user is added to</param>
        /// <param name="allow">The allow permissions of a user/group</param>
        /// <param name="deny">The deny permissions of a user/group</param>
        /// <returns></returns>
        public static Dictionary<string, HashSet<string>> calculatePermissions(List<UserGroup> usergroup, Dictionary<string, HashSet<string>> allow, Dictionary<string, HashSet<string>> deny)
        {
            Dictionary<string, HashSet<string>> aperms = new Dictionary<string, HashSet<string>>();
            Dictionary<string, HashSet<string>> dperms = new Dictionary<string, HashSet<string>>();
            foreach (UserGroup ug in usergroup)
            {
                addPermissions(aperms, ug.GroupPermissions.AllowPermissions);
                addPermissions(dperms, ug.GroupPermissions.DenyPermissions);
            }
            List<string> lstKeys = new List<string>();
            addPermissions(aperms, allow);
            addPermissions(dperms, deny);
            

            List<string> lstKeysToRemove = new List<string>();
            foreach (KeyValuePair<string, HashSet<string>> kvp in dperms)
            {
                HashSet<string> stringvalue;
                if (aperms.TryGetValue(kvp.Key, out stringvalue))
                {
                    stringvalue = deletePermissions(stringvalue, kvp.Key, kvp.Value);
                }
                if (stringvalue.Count == 0)
                    lstKeysToRemove.Add(kvp.Key);

            }
            //Remove empty lists from dictionary
            foreach (string s in lstKeysToRemove)
            {
                aperms.Remove(s);
            }

            return aperms;
        }

        /// <summary>
        /// Remove permissions
        /// </summary>
        /// <param name="removefrom">List of items you want to keep</param>
        /// <param name="key"></param>
        /// <param name="value">List of items you want to remove</param>
        /// <returns></returns>
        private static HashSet<string> deletePermissions(HashSet<string> removefrom, string key, HashSet<string> value)
        {

            foreach (string s in value)
            {
                if (removefrom.Contains(s))
                {
                    removefrom.Remove(s);
                }
            }
            return value;
        }

        /// <summary>
        /// Add permissions to another list of unique permissions
        /// </summary>
        /// <param name="toAdd">List that will be added to </param>
        /// <param name="values">List that will be added from</param>
        private static void addPermissions(Dictionary<string, HashSet<string>> toAdd, Dictionary<string, HashSet<string>> values)
        {
            foreach (KeyValuePair<string, HashSet<string>> kvp in values)
            {
                if (!toAdd.TryAdd(kvp.Key, kvp.Value))
                {
                    toAdd[kvp.Key].AddRange(kvp.Value);
                }
            }
        }



    }
}
