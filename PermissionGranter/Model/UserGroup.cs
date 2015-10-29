using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public class UserGroup
    {
        private Dictionary<string, HashSet<string>> _Permissions;

        public Dictionary<string, HashSet<string>> Permissions
        {
            get { return _Permissions; }
            set { _Permissions = value; }
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
