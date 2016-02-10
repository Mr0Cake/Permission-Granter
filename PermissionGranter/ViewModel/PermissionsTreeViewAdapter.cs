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

        private static void AddItems(Dictionary<string, HashSet<string>> permissions, List<CustTreeItems> toFill, bool? permission)
        {
            IEnumerable<CustTreeItems> relevantItems = toFill.Where(x => permissions.Keys.Contains(x.Name));
            foreach (CustTreeItems c in relevantItems)
            {
                
                HashSet<string> perms;
                if ((perms = permissions[c.Name]) != null)
                {
                    //perms.ToList().ForEach(options => c.Options.Where(perm => perm.Name == options).ToList().ForEach(perm => perm.setAccess(permission)));
                    foreach (var p in perms)
                    {
                        c.Options.Where(option => option.Name == p).First().setAccess(permission);
                    }
                }
                if (permission != null)
                {
                    c.setAccess(permission);
                }
                else
                {
                    if(c.Options.All(option => option.Value == null))
                    {
                        c.setAccess(null);
                    }
                }
            }
        }

        public static void FillMenuItems(MenuItems tempItems, PermissionsBase u)
        {
            tempItems.ClearItems();
            List<CustTreeItems> items = tempItems.GetAllItemReferences();
            AddItems(u.OwnedPermissions.AllowPermissions, items, true);
            AddItems(u.OwnedPermissions.DenyPermissions, items, null);


            //set HasAccess if all items are the same
            //items.Where(x => x.Options != null && x.Options.Count > 0)
            //    .ToList().Where(y => y.Options.ToList()
            //        .All(z => y.Options.First().Value == z.Value))
            //        .ToList().ForEach(p => p.HasAccess = p.Options.First().Value);

            //traceer de permissies terug naar boven (als de items allemaal gelijk zijn dan is hasaccess van
            //het parent item gelijk)
            //items.Where(parent => parent.Items.Count>0).Where(z=>z.Items.All(item => item.HasAccess == z.Items.First().HasAccess))
            //    .ToList().ForEach(item => item.HasAccess = item.Items.First().HasAccess);
            //items.Where(parent => parent.Items.Count > 0).Where(z => z.Items.All(item => item.HasAccess == z.Items.First().HasAccess))
            //    .ToList().ForEach(item => item.HasAccess = item.Items.First().HasAccess);
            //items.Where(parent => parent.Items.Count > 0).Where(z => z.Items.All(item => item.HasAccess == z.Items.First().HasAccess))
            //    .ToList().ForEach(item => item.HasAccess = item.Items.First().HasAccess);
            //3 keer omdat ik even geen zin heb in een ingewikkelde functie die terugloopt naar het hoogste niveau
        }

        public static void FillPermissions(PermissionsBase u, MenuItems tempItems)
        {
            List<CustTreeItems> items = tempItems.GetAllItemReferences();

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

                        //if(p.Value == false)
                        //{
                        //    if (u.OwnedPermissions.DenyPermissions.ContainsKey(cti.Name))
                        //        u.OwnedPermissions.DenyPermissions[cti.Name].Remove(p.Name);
                        //    if (u.OwnedPermissions.AllowPermissions.ContainsKey(cti.Name))
                        //        u.OwnedPermissions.AllowPermissions[cti.Name].Remove(p.Name);
                        //}
                            
                    }
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

                    //if (cti.HasAccess == false)
                    //{
                    //    if (u.OwnedPermissions.DenyPermissions.ContainsKey(cti.Name))
                    //        u.OwnedPermissions.DenyPermissions.Remove(cti.Name);
                    //    if (u.OwnedPermissions.AllowPermissions.ContainsKey(cti.Name))
                    //        u.OwnedPermissions.AllowPermissions.Remove(cti.Name);
                    //}
                }
            }
        }
    }
}
