using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    interface DefaultPermissions
    {
        public string PermissionGroupName();
        public Dictionary<string, Action> getFullPermissions();

        public void permissionNew();
        public void permissionSave();
        public void permissionDelete();
        public void permissionCancel();
        public void permissionPrint();
        public void permissionFind();
        public void permissionHelp();
        public void permissionClose();
    }
}
