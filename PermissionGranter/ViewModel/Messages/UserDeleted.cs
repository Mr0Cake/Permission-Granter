using PermissionGranter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Messages
{
    public class UserDeleted
    {
        private User DeletedUser = null;

        public UserDeleted(User _DeletedUser)
        {
            this.DeletedUser = _DeletedUser;
        }
    }
}
