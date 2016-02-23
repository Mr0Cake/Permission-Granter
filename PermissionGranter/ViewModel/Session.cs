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
    public class Session:NotifyPropertyChangedBase
    {
        private string _Password;

        public string Password
        {
            get { return string.IsNullOrEmpty(_Password) ? "" : "****************"; }
            set
            {
                if (_Password == value || value.Equals("****************"))
                    return;

                _Password = value;
                OnPropertyChanged("Password");
            }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email == value)
                    return;

                _Email = value;
                OnPropertyChanged("Email");
            }
        }


        private string _ErrorMessage;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                if (_ErrorMessage == value)
                    return;

                _ErrorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }


        public ICommand LoginCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand UserDetail { get; set; }
        public ICommand GroupDetail { get; set; }
        DialogService ds = new DialogService();

        private void openGroupDetail(object obj)
        {
            closeLogin();
            ds.Show("GroupDetail");
        }

        private void openUserdetail(object obj)
        {
            closeLogin();
            ds.Show("UserDetail");
        }

        public Session()
        {
            //Messenger.Default.Register<CloseUserDetail>(this, closeUserDetail);
            Messenger.Default.Register<DetailBase>(this, closewin);

            //Messenger.Default.Register<DoLogin>(this, doLogin);
            //Messenger.Default.Register<CloseApplication>(this, closeApplication);
            UserDetail = new CustomCommand(openUserdetail, (obj) => { return true; });
            GroupDetail = new CustomCommand(openGroupDetail, (obj) => { return true; });
            LoginCommand = new CustomCommand(doLoginAction, (obj) => { return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(_Password); });
            CancelCommand = new CustomCommand(cancel, (obj) => { return true; });
            //Messenger.Default.Register<CloseGroupDetail>(this, closeGroupDetail);

        }

        private void closewin(DetailBase obj)
        {
            if (obj is CloseUserDetail)
            {
                ds.CloseDialog("UserDetail");
                ShowLogin();
            }
            else
            {
                ds.CloseDialog("GroupDetail");
                ShowLogin();
            }
        }

        public void ShowLogin()
        {
            ds.ShowDialog("Login");
        }

        private void cancel(object obj)
        {
            ds.CloseDialog("Login");
            Application.Current.Shutdown();
        }

        private void doLoginAction(object obj)
        {

            User u;
            try {
                if(BLL.UserBLL.ComparePasswords(out u, _Password, Email))
                {
                    if (u != null)
                    {
                        u.OwnedPermissions = BLL.PermissionsBLL.GetPermissionsByUserID(u.ID);
                        ds.CloseDialog("Login");
                        ds.ShowUserwindow(u);
                    }
                }
                else
                {
                    throw new Exception("Gebruiker niet gevonden, of wachtwoord komt niet overeen met de gebruiker.");
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            
        }

        //private bool openstartwindow = false;
        //private void closeApplication(CloseApplication obj)
        //{
        //    if (!openstartwindow)
        //    {
        //        Application.Current.Shutdown();
        //    }
        //    else { openstartwindow = false; }
        //}

        private void closeLogin()
        {
            ds.CloseDialog("Login");
        }

        private void closeUserDetail(CloseUserDetail obj)
        {
            
            ds.CloseDialog("UserDetail");
            ShowLogin();
        }

        private void closeGroupDetail(CloseGroupDetail obj)
        {
            ds.CloseDialog("GroupDetail");
            ShowLogin();
        }

        

    }
}
