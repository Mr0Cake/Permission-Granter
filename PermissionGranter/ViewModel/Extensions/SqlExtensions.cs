using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Extensions
{
    public static class SqlExtensions
    {
        public static SqlParameter SetParameter(this object o, string name)
        {
            return new SqlParameter(name, o);
        }

        //public static SqlParameter SetParameter(this int o, string name)
        //{
        //    return new SqlParameter(name, o);
        //}
    }
}
