using PermissionGranter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Messages
{
    public class DoLogin
    {
        private User _LoginUser;

        public User LoginUser
        {
            get { return _LoginUser; }
            set { _LoginUser = value; }
        }


        public DoLogin(User u)
        {
            _LoginUser = u;
        }
    }
}
