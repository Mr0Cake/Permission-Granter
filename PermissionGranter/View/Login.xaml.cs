using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PermissionGranter.Model;
using PermissionGranter.ViewModel;
using PermissionGranter.ViewModel.Services;
using PermissionGranter.ViewModel.Messages;

namespace PermissionGranter.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        

        public Login()
        {
            InitializeComponent();
        }

        

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //string email = TxtEmail.Text;
            //User u = new User();
            //u.Email = email;
            //string password = TxtPassword.Password;
            //string salt;
            //int iterations = 10;
            //ViewModel.PasswordEncryption.EncryptPassword(ref password, iterations, out salt);
            //u.Password = password;

            //ViewModel.BLL.UserBLL.CreateUser(u);
            //_LoginUser = u;
            //this.DialogResult = true;
            //this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
