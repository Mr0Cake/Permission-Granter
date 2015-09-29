using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    interface ControlPermissions
    {
        private bool setReadable(bool readValue);
        private bool setPrintable(bool printValue);

    }
}
