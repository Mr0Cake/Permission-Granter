using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PermissionGranter.View;
using PermissionGranter.ViewModel;

namespace PermissionGranter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        
        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // TODO: Parse commandline arguments and other startup work 
            //if(ViewModel.Services.ViewModelLocator.UserSession == null)
            ViewModel.Services.ViewModelLocator.UserSession = new Session(); 
        }
    }
}
