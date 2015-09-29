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
        public User(string lastname, string firstname, string function)
        {
            this.lastName = lastname;
            this.firstName = firstname;
            this.function = function;
        }

    }
}
