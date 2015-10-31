using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    /// <summary>
    /// Representeerd de permissies voor een bepaalde groep
    /// </summary>
    public class UserGroup
    {

        public UserGroup(Permissions perms, string name, string description)
        {
            GroupPermissions = perms;
            GroupName = name;
            Description = description;
        }

        private Permissions _GroupPermissions;

        public Permissions GroupPermissions
        {
            get { return _GroupPermissions; }
            set { _GroupPermissions = value; }
        }

        private string _GroupName;

        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        
        
        
    }
}
