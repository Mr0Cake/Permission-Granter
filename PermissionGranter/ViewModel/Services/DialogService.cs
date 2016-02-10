using PermissionGranter.View;
using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PermissionGranter.ViewModel.Services
{
    public class DialogService
    {
        
        static Dictionary<string, Window> windowList = new Dictionary<string, Window>();

        public DialogService()
        {
        }

        public bool? ShowDialog(string window)
        {
            Type t = Type.GetType("PermissionGranter.View." + window);
            windowList[window] = (Window)Activator.CreateInstance(t);
            windowList[window].ShowDialog();
            return windowList[window].DialogResult;
        }

        public void Show(string window)
        {
            Type t = Type.GetType("PermissionGranter.View." + window);
            windowList[window] = (Window)Activator.CreateInstance(t);
            windowList[window].Show();
        }

        public void CloseDialog(string window)
        {
            if (windowList[window] != null)
            {
                windowList[window].Close();
                windowList[window] = null;
            }
        }
    }
}
