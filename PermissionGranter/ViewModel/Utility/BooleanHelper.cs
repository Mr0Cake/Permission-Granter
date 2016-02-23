using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Utility
{
    public static class BooleanHelper
    {
        public static bool NotNull(params object[] objects)
        {
            if(objects != null && objects.Length > 0)
            {
                foreach (object obj in objects)
                {
                    if(obj == null)
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }
    }
}
