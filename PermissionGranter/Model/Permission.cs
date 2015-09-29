using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public class Permission
    {
        private bool permissionValue = false;
        private Action grantMethod;
        private Action revokeMethod;
        private string description;
        
        public Permission(Action grant, Action revoke, string description, bool permissionValue)
        {
            GrantMethod = grant;
            RevokeMethod = revoke;
            Description = description;
            PermissionValue = permissionValue;
        }
        
        public void setPermission(bool permission)
        {
            GrantMethod.DynamicInvoke();
            PermissionValue = true;
        }

        public void removePermission()
        {
            RevokeMethod.DynamicInvoke();
        }

        public Action RevokeMethod
        {
            get { return revokeMethod; }
            set { revokeMethod = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        
        public Action GrantMethod
        {
            get { return grantMethod; }
            set { grantMethod = value; }
        }
        
        public bool PermissionValue
        {
            get { return permissionValue; }
            set { permissionValue = value; }
        }

        
    }
}
