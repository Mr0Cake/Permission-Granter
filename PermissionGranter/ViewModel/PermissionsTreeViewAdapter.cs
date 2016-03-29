using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionGranter.Model;

namespace PermissionGranter.ViewModel
{
    public static class PermissionsTreeViewAdapter
    {

        private static void AddItems(Dictionary<string, HashSet<string>> permissions, MenuItems mu, bool? permission)
        {
            List<CustTreeItems> relevantItems = mu.GetAllItemReferences().Where(x => permissions.Keys.Contains(x.Name)).ToList();
            //IEnumerable<CustTreeItems> relevantItems = toFill.Where(x => permissions.Keys.Contains(x.Name));
            foreach (CustTreeItems c in relevantItems)
            {
                c.setAccess(permission);
                    HashSet<string> perms;
                if ((perms = permissions[c.Name]) != null)
                {
                    if (c.Options.Count == perms.Count && c.Options.All(x => perms.Contains(x.Name)))
                    {
                        c.HasAccess = permission;
                    }
                    else {

                        foreach (Permission p in c.Options)
                        {
                            if (perms.Contains(p.Name))
                                p.Value = permission;
                        }
                    }
                }
                
                //c.setAccess(permission);
                
                
            }
        }

        /// <summary>
        /// Check off the permissions on a treeview. using the permissions owned by 'u'
        /// Permissions on the treeview will be cleared before adding.
        /// </summary>
        /// <param name="tempItems">MenuItems object to fill</param>
        /// <param name="u">PermissionsBase object to base the permissions on</param>
        public static void FillMenuItems(MenuItems tempItems, PermissionsBase u)
        {
            tempItems.ClearItems();
            //List<CustTreeItems> items = tempItems.GetAllItemReferences();
            AddItems(u.OwnedPermissions.AllowPermissions, tempItems, true);
            AddItems(u.OwnedPermissions.DenyPermissions, tempItems, null);
        }


        public static void FillMenuItems(MenuItems tempItems, Dictionary<string, HashSet<string>> perms)
        {
            AddItems(perms,tempItems, true);
        }

        public static void FillPermissions(PermissionsBase u, MenuItems tempItems)
        {
            List<CustTreeItems> items = tempItems.GetAllItemReferences();
            u.OwnedPermissions.AllowPermissions.Clear();
            u.OwnedPermissions.DenyPermissions.Clear();
            foreach(CustTreeItems cti in items)
            {
                //heeft het treeitem options?
                if(cti.Options != null && cti.Options.Count > 0)
                {
                    //ja, vraag eventuele permissies op
                    HashSet<string> allowpermissions;
                    HashSet<string> denypermissions;
                    if(!u.OwnedPermissions.AllowPermissions.TryGetValue(cti.Name,out allowpermissions))
                    {
                        allowpermissions = new HashSet<string>();
                    }
                    if(!u.OwnedPermissions.DenyPermissions.TryGetValue(cti.Name, out denypermissions))
                    {
                        denypermissions = new HashSet<string>();
                    }
                    //om het mijzelf gemakkelijker te maken maak ik de permissies leeg om eventuele hoofdpijn te voorkomen
                    allowpermissions.Clear();
                    denypermissions.Clear();
                    //test if applied
                    HashSet<string> temp;
                    u.OwnedPermissions.AllowPermissions.TryGetValue(cti.Name, out temp);
                    HashSet<string> tempDeny;
                    u.OwnedPermissions.DenyPermissions.TryGetValue(cti.Name, out tempDeny);
                    foreach (Permission p in cti.Options)
                    {
                        if (p.Value == true)
                        {
                            if (!u.OwnedPermissions.AllowPermissions.ContainsKey(cti.Name))
                                u.OwnedPermissions.AllowPermissions.Add(cti.Name, allowpermissions);
                            u.OwnedPermissions.AllowPermissions[cti.Name].Add(p.Name);

                        }
                        
                        if (p.Value == null)
                        {
                            if (!u.OwnedPermissions.DenyPermissions.ContainsKey(cti.Name))
                                u.OwnedPermissions.DenyPermissions.Add(cti.Name, denypermissions);
                            u.OwnedPermissions.DenyPermissions[cti.Name].Add(p.Name);
                        }
                        
                            
                    }

                    ////double check if all values are disabled then disable top item
                    //if()
                }
                else
                {
                    if(cti.HasAccess == true)
                    {
                        if (!u.OwnedPermissions.AllowPermissions.ContainsKey(cti.Name))
                            u.OwnedPermissions.AllowPermissions.Add(cti.Name, null);
                    }
                    if (cti.HasAccess == null)
                    {
                        if (!u.OwnedPermissions.DenyPermissions.ContainsKey(cti.Name))
                            u.OwnedPermissions.DenyPermissions.Add(cti.Name, null);
                    }
                }
            }
        }
    }
}
