using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.BLL.BO
{
    public struct MenuItem
    {
        public string ParentName;
        public string Name;
        public string PermissionName;
        public string PermissionDescription;
        public List<MenuItem> items;
    }
}
