using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PermissionGranter.ViewModel.Messages
{
    public class CloseWindow
    {
        public Window ToClose { get; set; }
        public CloseWindow(Window w)
        {
            ToClose = w;
        }
    }
}
