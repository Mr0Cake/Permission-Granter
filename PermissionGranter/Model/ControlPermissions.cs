using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public interface ControlPermissions
    {
        /* new
         * save
         * delete
         * cancel
         * print
         * find
         * help
         * close*/
        public List<Permission> getFullPermissions();

        public void permissionRead();
        public void permissionNew();
        public void permissionSave();
        public void permissionDelete();
        public void permissionCancel();
        public void permissionPrint();
        public void permissionFind();
        public void permissionHelp();
        public void permissionClose();

        public void revokePermissionRead();
        public void revokePermissionNew();
        public void revokePermissionSave();
        public void revokePermissionDelete();
        public void revokePermissionCancel();
        public void revokePermissionPrint();
        public void revokePermissionFind();
        public void revokePermissionHelp();
        public void revokePermissionClose();
    }
}
