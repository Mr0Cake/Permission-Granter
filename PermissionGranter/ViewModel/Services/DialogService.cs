using PermissionGranter.Model;
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

        public void ShowDialog(string window)
        {
            Type t = Type.GetType("PermissionGranter.View." + window);
            windowList[window] = (Window)Activator.CreateInstance(t);
            windowList[window].ShowDialog();
            
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
                //If window is closing .Close() will throw an exception, I don't care
                try
                {
                    //windowList[window].Close();
                    windowList[window].Hide();
                }
                catch (Exception) { }
                windowList[window] = null;
            }
        }

        public void ShowUserwindow(User u)
        {
            StartWindow w = new StartWindow();
            w.WindowUser = u;
            windowList["StartWindow"] = w;
            w.Show();
        }
    }
}
