using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public class User
    {
        private string lastName { public get; set; }
        private string firstName { public get; set; }
        private string function { public get; set; }

        private Dictionary<string, string[]> _UserPermissions;

        public Dictionary<string, string[]> UserPermissions
        {
            get { return _UserPermissions; }
            set { _UserPermissions = value; }
        }

        public bool addPermission(PermissionGranterControl pgc, List<Permission> permissions){

            KeyValuePair<string, string[]> kvp = new KeyValuePair<string,string[]> { pgc.GetType().ToString(), permissions.ToArray() };
            return false;
        }
        


        public User(string lastname, string firstname, string function)
        {
            this.lastName = lastname;
            this.firstName = firstname;
            this.function = function;
        }


    }
}
