using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PermissionGranter.Model
{
    /// <summary>
    /// Interaction logic for PermissionGranterControl.xaml
    /// </summary>
    public partial class PermissionGranterControl : UserControl, DefaultPermissions
    {
        

        public PermissionGranterControl()
        {
            InitializeComponent();
        }

        public Dictionary<string, Action> getFullPermissions()
        {
            throw new NotImplementedException();
        }

        public void permissionRead()
        {
            throw new NotImplementedException();
        }

        public void permissionNew()
        {
            throw new NotImplementedException();
        }

        public void permissionSave()
        {
            throw new NotImplementedException();
        }

        public void permissionDelete()
        {
            throw new NotImplementedException();
        }

        public void permissionCancel()
        {
            throw new NotImplementedException();
        }

        public void permissionPrint()
        {
            throw new NotImplementedException();
        }

        public void permissionFind()
        {
            throw new NotImplementedException();
        }

        public void permissionHelp()
        {
            throw new NotImplementedException();
        }

        public void permissionClose()
        {
            throw new NotImplementedException();
        }

        public string PermissionGroupName()
        {
            throw new NotImplementedException();
        }
    }
}
