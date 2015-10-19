using PermissionGranter.Model;
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

namespace PermissionGranter.View
{
    /// <summary>
    /// Interaction logic for PrinterControl.xaml
    /// </summary>
    public partial class PrinterControl : PermissionGranterControl,PermissionControl
    {

        

        public Dictionary<string, Action> fullPermissions;
        public List<Permission> fullPermissionsList = new List<Permission>();

        public List<Permission> getFullPermissions()
        {
            return this.fullPermissionsList;
        }
        public PrinterControl()
        {
            InitializeComponent();
            fullPermissions.Add("readPermission", new Action(() => toggleReadPermission()));
        }

        public void fillPermission()
        {
            
            fullPermissions.Add("readPermission", new Action(() => setReadPermission(true)));
            fullPermissions.Add("writePermission", new Action(() => setWritePermission(true)));
        }



        public void permissionRead()
        {
        //give access to read
            readPermission = true;
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
    }
}
