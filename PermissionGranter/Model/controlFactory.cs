using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    class ControlFactory
    {
        public static PermissionGranterControl applyUserPermissions(User user, PermissionGranterControl control)
        {
            
            return control;
        }
    }
}
