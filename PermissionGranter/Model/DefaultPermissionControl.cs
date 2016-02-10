using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public interface DefaultPermissionControl
    {
        Dictionary<string, Tuple<string, Action>> ExecutableActions { get; set; }
        string MenuName { get; set; }
        void permissionNew();
        void permissionSave();
        void permissionDelete();
        void permissionCancel();
        void permissionPrint();
        void permissionFind();
        void permissionHelp();
        void permissionClose();
    }

    
}
