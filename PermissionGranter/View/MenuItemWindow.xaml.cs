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
using System.Windows.Shapes;

namespace PermissionGranter.View
{
    /// <summary>
    /// Interaction logic for MenuItemWindow.xaml
    /// </summary>
    public partial class MenuItemWindow : Window
    {
        public MenuItemWindow(params string[] buttons)
        {
            InitializeComponent();
            foreach (string s in buttons)
            {
                //Action b = null;
                Button button = new Button();
                button.Content = s;
                //button.Click += new RoutedEventHandler((x, e) => b.Invoke());
                ButtonPanel.Children.Add(button);
                
            }
            Button closebutton = new Button();
            closebutton.Content = "Close";
            closebutton.Click += new RoutedEventHandler((x, e) => this.Close());
            ButtonPanel.Children.Add(closebutton);
        }
    }
}
