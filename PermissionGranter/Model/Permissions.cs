using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public class Permissions : CanCopy<Permissions>, INotifyPropertyChanged
    {
        public Permissions(Dictionary<string, HashSet<string>> allow, Dictionary<string, HashSet<string>> deny)
        {
            AllowPermissions = allow;
            DenyPermissions = deny;
        }

        public Permissions(Permissions copy)
        {
            foreach (var e in copy.AllowPermissions)
            {
                HashSet<string> copyPerms = new HashSet<string>();
                copyPerms.AddRange(e.Value);
                AllowPermissions.Add(e.Key, copyPerms);
            }
            foreach (var e in copy.DenyPermissions)
            {
                HashSet<string> copyPerms = new HashSet<string>();
                copyPerms.AddRange(e.Value);
                AllowPermissions.Add(e.Key, copyPerms);
            }

        }

        public Permissions()
        {
        }

        //allow
        private Dictionary<string, HashSet<string>> _AllowPermissions = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// Permissions die zijn toegelaten
        /// </summary>
        public Dictionary<string, HashSet<string>> AllowPermissions
        {
            get { return _AllowPermissions; }
            set { _AllowPermissions = value; }
        }


        //deny

        private Dictionary<string, HashSet<string>> _DenyPermissions = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// Permissions die verboden zijn
        /// </summary>
        public Dictionary<string, HashSet<string>> DenyPermissions
        {
            get { return _DenyPermissions; }
            set { _DenyPermissions = value; }
        }

        

        private bool _Changed = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Changed
        {
            get { return _Changed; }
            set
            {
                if (_Changed == value)
                    return;

                _Changed = value;
                OnPropertyChanged("Changed");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                Changed = true;
            }
        }

        




        /// <summary>
        /// add a permission to a control
        /// </summary>
        /// <param name="control">name of the control</param>
        /// <param name="permission">name of the permission to be executed</param>
        /// <param name="allowOrDeny">true = allow list, false = deny list</param>
        public void addPermission(string control, bool allowOrDeny, string permission = "")
        {
            Dictionary<string, HashSet<string>> toadd = allowOrDeny ? AllowPermissions : DenyPermissions;

            if (!string.IsNullOrEmpty(control))
            {
                //make sure control exists
                if (!toadd.ContainsKey(control))
                {
                    toadd.Add(control, new HashSet<string>());
                }
                //do not add if empty
                if (!string.IsNullOrEmpty(permission))
                    toadd[control].Add(permission);
                Changed = true;
            }
            else
            {
                throw new Exception("addPermission control string is empty");
            }
        }


        /// <summary>
        /// Combine 2 permissions into 1 object
        /// </summary>
        /// <param name="perms1"></param>
        /// <param name="perms2"></param>
        /// <returns></returns>
        public static Dictionary<string, HashSet<string>> CombinePermissions(Dictionary<string, HashSet<string>> perms1, Dictionary<string, HashSet<string>> perms2)
        {
            foreach (KeyValuePair<string, HashSet<string>> perms in perms2)
            {
                if (!perms1.TryAdd(perms.Key, perms.Value))
                    perms1[perms.Key].AddRange(perms.Value);
            }
            return perms1;
        }


        //calculate
        /// <summary>
        /// CalculatePermissions will return a Dictionary of permissions that only contains the 
        /// allow Permissions after subtraction of the DenyPermissions.
        /// The order of permission is:
        /// - Group Deny
        /// - User Deny
        /// - Group Allow
        /// - User Allow
        /// </summary>
        /// <param name="usergroup">List of groups a user is added to</param>
        /// <param name="allow">The allow permissions of a user/group</param>
        /// <param name="deny">The deny permissions of a user/group</param>
        /// <returns></returns>
        public static Dictionary<string, HashSet<string>> calculatePermissions(List<UserGroup> usergroup, Dictionary<string, HashSet<string>> allow, Dictionary<string, HashSet<string>> deny)
        {
            //Allow lists
            Dictionary<string, HashSet<string>> AllAllowed = new Dictionary<string, HashSet<string>>();
            Dictionary<string, HashSet<string>> AllDeny = new Dictionary<string, HashSet<string>>();
            foreach (UserGroup ug in usergroup)
            {
                CombinePermissions(AllAllowed, ug.OwnedPermissions.AllowPermissions);
                CombinePermissions(AllDeny, ug.OwnedPermissions.DenyPermissions);
            }
            CombinePermissions(AllAllowed, allow);
            CombinePermissions(AllDeny, deny);

            //Remove all allow permissions inside deny
            AllAllowed.Where(x => AllDeny.ContainsKey(x.Key)).ToList().ForEach(p => AllAllowed[p.Key].Intersect(AllDeny[p.Key]));
            //Remove empty permissions
            AllAllowed.ToList().RemoveAll(x => x.Value == null || x.Value.Count == 0);
            return AllAllowed;
        }

        /// <summary>
        /// Remove permissions
        /// </summary>
        /// <param name="removefrom">List of items you want to keep</param>
        /// <param name="key"></param>
        /// <param name="value">List of items you want to remove</param>
        /// <returns></returns>
        public static HashSet<string> deletePermissions(HashSet<string> removefrom, string key, HashSet<string> value)
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
        public static void addPermissions(Dictionary<string, HashSet<string>> toAdd, Dictionary<string, HashSet<string>> values)
        {
            foreach (KeyValuePair<string, HashSet<string>> kvp in values)
            {
                if (!toAdd.TryAdd(kvp.Key, kvp.Value))
                {
                    toAdd[kvp.Key].AddRange(kvp.Value);
                }
            }
        }

        /// <summary>
        /// Combine 2 or more permission objects
        /// </summary>
        /// <param name="combinePermission">List that will be combined </param>
        public static Permissions combinePermissions(params Permissions[] combinePermissions)
        {
            if (combinePermissions != null && combinePermissions.Count() > 1)
            {
                Permissions basePerm = combinePermissions[0];
                for (int i = 1; i <= combinePermissions.Count(); i++)
                {
                    Permissions.addPermissions(basePerm._AllowPermissions, combinePermissions[i]._AllowPermissions);
                    Permissions.addPermissions(basePerm._DenyPermissions, combinePermissions[i]._DenyPermissions);
                }
                return basePerm;
            }
            else
            {
                throw new Exception("Could not combine permissions, no or only 1 parameters were given");
            }
        }

        public Permissions GetCopy()
        {
            Permissions p = new Permissions();
            p.Changed = Changed;
            foreach (var e in this.AllowPermissions)
            {
                HashSet<string> copyPerms = new HashSet<string>();
                if (e.Value != null)
                {
                    copyPerms.AddRange(e.Value);
                }
                else { copyPerms = null; }
                p.AllowPermissions.Add(e.Key, copyPerms);
            }
            foreach (var e in this.DenyPermissions)
            {
                HashSet<string> copyPerms = new HashSet<string>();
                if (e.Value != null)
                {
                    copyPerms.AddRange(e.Value);
                }
                else { copyPerms = null; }
                p.DenyPermissions.Add(e.Key, copyPerms);
            }
            return p;
        }
    }
}
