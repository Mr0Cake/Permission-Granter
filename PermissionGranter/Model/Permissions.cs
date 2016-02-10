using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public class Permissions:CanCopy<Permissions>, INotifyPropertyChanged
    {
        public Permissions(Dictionary<string, HashSet<string>> allow, Dictionary<string, HashSet<string>> deny)
        {
            AllowPermissions = allow;
            DenyPermissions = deny;
            CalculatePermissions();
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
        private Dictionary<string, HashSet<string>> _AllowPermissions = new Dictionary<string,HashSet<string>>();

        /// <summary>
        /// Permissions die zijn toegelaten
        /// </summary>
        public Dictionary<string, HashSet<string>> AllowPermissions
        {
            get { return _AllowPermissions; }
            set { _AllowPermissions = value; }
        }


        //deny

        private Dictionary<string, HashSet<string>> _DenyPermissions = new Dictionary<string,HashSet<string>>();

        /// <summary>
        /// Permissions die verboden zijn
        /// </summary>
        public Dictionary<string, HashSet<string>> DenyPermissions
        {
            get { return _DenyPermissions; }
            set { _DenyPermissions = value; }
        }

        private Dictionary<string, HashSet<string>> _CalculatedPermissions = new Dictionary<string,HashSet<string>>();

        /// <summary>
        /// Permissions die verboden zijn
        /// </summary>
        public Dictionary<string, HashSet<string>> CalculatedPermissions
        {
            get 
            {
                CalculatePermissions();
                return _CalculatedPermissions; 
            }
            set { _CalculatedPermissions = value; }
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
        /// Fills the CalculatedPermissions. 
        /// It will remove all AllowPermissions that have been denied by DenyPermision
        /// 
        /// </summary>
        public void CalculatePermissions()
        {
            Dictionary<string, HashSet<string>> CalcPermissions = new Dictionary<string, HashSet<string>>(_AllowPermissions);
            _CalculatedPermissions =
            CalcPermissions.AsEnumerable().Where(x => _DenyPermissions.ContainsKey(x.Key)).Where(x => x.Value.RemoveRange(_DenyPermissions[x.Key]) && x.Value.Count>0).ToDictionary(x => x.Key, y => y.Value);
            //Allow permissies als enumerable, waar de key van de allow dictionary voorkomt in deny, waar x.Value=HashSet<string>.Verwijder de permissies van deny (altijd true) dus alle KeyValuePairs worden meegenomen tenzij deze leeg is geraakt, naar nieuwe dictionary
        }



        

        /// <summary>
        /// add a permission to a control
        /// </summary>
        /// <param name="control">name of the control</param>
        /// <param name="permission">name of the permission to be executed</param>
        /// <param name="allowOrDeny">true = allow list, false = deny list</param>
        public void addPermission(string control, bool allowOrDeny, string permission="")
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
                if(!string.IsNullOrEmpty(permission))
                toadd[control].Add(permission);
                Changed = true;
            }
            else
            {
                throw new Exception("addPermission control string is empty");
            }
        }



        //calculate
        /// <summary>
        /// CalculatePermissions will return a Dictionary of permissions that only contains the 
        /// unrestricted allow Permissions.
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
                addPermissions(aperms, ug.OwnedPermissions.AllowPermissions);
                addPermissions(dperms, ug.OwnedPermissions.DenyPermissions);
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
                basePerm.CalculatePermissions();
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
