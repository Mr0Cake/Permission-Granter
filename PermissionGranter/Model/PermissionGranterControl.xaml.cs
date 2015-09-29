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
    public partial class PermissionGranterControl : UserControl, ControlPermissions
    {
        abstract List<Delegate> fullPermissions;
        abstract List<PermissionGranterControl> children;

        public PermissionGranterControl()
        {
            InitializeComponent();
        }
    }
}
