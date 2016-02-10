using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionGranter.Model;
using PermissionGranter.View;
using System.Windows;
using PermissionGranter.ViewModel.Services;
using System.Windows.Input;
using PermissionGranter.ViewModel.Utility;
using PermissionGranter.ViewModel.Messages;

namespace PermissionGranter.ViewModel
{
    public class Session
    {
        public ICommand UserDetail { get; set; }
        public ICommand GroupDetail
        {
            get; set;
        }
        static DialogService ds = new DialogService();

        private static void openGroupDetail(object obj)
        {
            ds.ShowDialog("GroupDetail");
        }

        private static void openUserdetail(object obj)
        {
            ds.ShowDialog("UserDetail");
        }

        public Session()
        {
            Messenger.Default.Register<CloseGroupDetail>(this, closeGroupDetail);
            Messenger.Default.Register<CloseUserDetail>(this, closeUserDetail);
            Messenger.Default.Register<DoLogin>(this, doLogin);
            Messenger.Default.Register<CloseApplication>(this, closeApplication);
            UserDetail = new CustomCommand(openUserdetail, (obj) => { return true; });
            GroupDetail = new CustomCommand(openGroupDetail, (obj) => { return true; });
            //ds.Show("Login");
            ds.Show("GroupDetail");
            //ds.Show("UserDetail");
            
        }
        private bool openstartwindow = false;
        private void closeApplication(CloseApplication obj)
        {
            if (!openstartwindow)
            {
                Application.Current.Shutdown();
            }
            else { openstartwindow = false; }
        }

        private void closeUserDetail(CloseUserDetail obj)
        {
            
            ds.CloseDialog("UserDetail");
        }

        private void doLogin(DoLogin obj)
        {
            openstartwindow = true;
            ds.CloseDialog("Login");
            ds.ShowDialog("StartWindow");
        }

        private void closeGroupDetail(CloseGroupDetail obj)
        {
            ds.CloseDialog("GroupDetail");
        }

        public User LoadUserPermission(string email)
        {
            User u = new User();


            return u;
        }


        private string _Test;

        public string Test
        {
            get { return _Test; }
            set { _Test = value; }
        }

    }
}
