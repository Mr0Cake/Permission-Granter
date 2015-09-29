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
    public partial class PrinterControl : PermissionGranterControl
    {

        private bool readPermission = false;
        private bool writePermission = false;
        private delegate void setReadPermissiond(bool permission);
        public void setReadPermission(bool permission)
        {
            readPermission = permission;
        }
        public void setWritePermission(bool permission)
        {
            writePermission = permission;
        }

        public void toggleReadPermission()
        {
            readPermission = !readPermission;
        }
        public void toggleWritePermission()
        {
            writePermission = !writePermission;
        }

        public Dictionary<string, Action> fullPermissions;

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

        
    }
}
