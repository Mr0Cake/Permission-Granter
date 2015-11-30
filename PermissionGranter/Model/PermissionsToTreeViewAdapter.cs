using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public static class PermissionsToTreeViewAdapter
    {
        public static void FillMenuItems(MenuItems tempItems, User u)
        {
            tempItems.ClearItems();
            List<CustTreeItems> items = tempItems.GetAllItemReferences();

            //allow permissies
            items.Where(x => u.UserPermissions.AllowPermissions.Keys.Contains(x.Name))
                .ToList().ForEach(winItems => u.UserPermissions.AllowPermissions[winItems.Name]
                .ToList().ForEach(opt => winItems.Options.Where(perm => perm.Name == opt)
                .ToList().ForEach(perm => perm.Value = true)));

            //deny permissies
            items.Where(x => u.UserPermissions.DenyPermissions.Keys.Contains(x.Name))
                .ToList().ForEach(winItems => u.UserPermissions.DenyPermissions[winItems.Name]
                .ToList().ForEach(opt => winItems.Options.Where(perm => perm.Name == opt)
                .ToList().ForEach(perm => perm.Value = null)));
            

        }
    }
}
