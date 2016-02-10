using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Services
{
    public class ViewModelLocator
    {
        private static UserDetailViewModel _UserDetailViewModel = new UserDetailViewModel();

        public static UserDetailViewModel UserDetails
        {
            get { return _UserDetailViewModel; }
        }

        private static GroupDetailViewModel _GroupDetailViewModel = new GroupDetailViewModel();

        public static GroupDetailViewModel GroupDetails
        {
            get { return _GroupDetailViewModel; }
        }

        private static Session _UserSession;

        public static Session UserSession
        {
            get { return _UserSession; }
            set { _UserSession = value; }
        }

    }
}
