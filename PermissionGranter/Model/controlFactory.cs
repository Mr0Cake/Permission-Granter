using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    class ControlFactory
    {
        public static PermissionGranterControl createPermissionControlFromUser(PermissionGranterControl control, User user)
        {
            foreach (Delegate d in user.permissions)
            {
                d.DynamicInvoke();
            }
            

            return control;
        }
    }
}
