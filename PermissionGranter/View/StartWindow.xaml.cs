using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using PermissionGranter.ViewModel.Utility;
using System.Reflection;
using PermissionGranter.ViewModel.BLL;
using PermissionGranter.ViewModel.Extensions;

namespace PermissionGranter.View
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window, INotifyPropertyChanged
    {
        private PermissionsBase _windowUser;
        public ICommand Command { get; set; }

        public PermissionsBase WindowUser
        {
            get { return _windowUser; }
            set
            {
                _windowUser = value;
                //MenuItems fill = new MenuItems();
                createUserItems = PermissionsBLL.GetTreeMenu();
                PermissionsTreeViewAdapter.FillMenuItems(createUserItems, _windowUser);
                removeItems();
                //mnu.ItemsSource = fill;
                NotifyPropertyChanged("createUserItems");
                NotifyPropertyChanged("WindowUser");
            }
        }

        public void removeItems()
        {
            for(int i = createUserItems.Items.Count-1; i>-1 ; i--)
            {
                if(createUserItems.Items[i].HasAccess == false || createUserItems.Items[i].HasAccess == null)
                {
                    createUserItems.Items.RemoveAt(i);
                }
                else
                {
                    createUserItems.Items[i].RemoveNoPermissions();
                }
            }
        }

        private Dictionary<string, Tuple<string, Action>> _ExecutableActions;
        public Dictionary<string, Tuple<string, Action>> ExecutableActions
        {
            get
            {
                return _ExecutableActions;
            }

            set
            {
                _ExecutableActions = value;
            }
        }

        public MenuItems createUserItems { get; set; }


        public StartWindow()
        {
            InitializeComponent();
            Command = new CustomCommand(doCommand, (object obj) => { return true; });
            createUserItems = new MenuItems();
            methods.Add("New", GetMethodInfo(New));
            methods.Add("Save", GetMethodInfo(Save));
            methods.Add("Delete", GetMethodInfo(Delete));
            methods.Add("Cancel", GetMethodInfo(Cancel));
            methods.Add("Print", GetMethodInfo(Print));
            methods.Add("Find", GetMethodInfo(Find));
            methods.Add("Help", GetMethodInfo(Help));
            methods.Add("Close", GetMethodInfo(Close));

        }

        private void doCommand(object obj)
        {
            
        }

        private void item_Click(object sender, RoutedEventArgs args)
        {
           
            MenuItem mu = sender as MenuItem;
            //Type get = sender.GetType();
            if(mu!= null && mu.Items.Count == 0)
            {
                CustTreeItems dc = mu.DataContext as CustTreeItems;
                Description = "";
                ItemName = "";


                ItemName = dc.Name;
                if (dc.Options.Count > 0)
                {
                    LoadButtons(dc);
                }
                else
                {
                    spButtons.Children.Clear();
                }
            }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description == value)
                    return;

                _Description = value;
                NotifyPropertyChanged("Description");
            }
        }


        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName == value)
                    return;

                _ItemName = value;
                NotifyPropertyChanged("ItemName");
            }
        }

        Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>
        {
            
        };

        private MethodInfo GetMethodInfo(Action<string> a)
        {
            return a.Method;
        }

        private void LoadButtons(CustTreeItems Permissions)
        {
            
            spButtons.Children.Clear();
            //string type = "Word";
            //MessageBox.Show("cleared");
            foreach (var s in Permissions.Options.Where(x=> x.Value == true))
            {
                object[] parameters = new object[] { s.Description };
                MethodInfo methodinfo = methods[s.Name];
                Button button = new Button();
                button.Content = s.Name;
                button.Click += new RoutedEventHandler((x, e) => methodinfo.Invoke(this, parameters));
                spButtons.Children.Add(button);
                    
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        //Actions
        #region actions

        
        
        public void New(string text)
        {
            Description = text;
        }

        public void Save(string text)
        {
            Description = text;
        }

        public void Delete(string text)
        {
            Description = text;
        }

        public void Cancel(string text)
        {
            Description = text;
        }

        public void Print(string text)
        {
            Description = text;
        }

        public void Find(string text)
        {
            Description = text;
        }

        public void Help(string text)
        {
            Description = text;
        }

        public void Close(string text)
        {
            Application.Current.Shutdown();
        }
        #endregion

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
