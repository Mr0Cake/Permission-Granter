using PermissionGranter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using PermissionGranter.ViewModel;
using PermissionGranter.ViewModel.BLL;

namespace PermissionGranter
{
    /// <summary>
    /// Interaction logic for CreateUsers.xaml
    /// </summary>
    public partial class CreateUsers : UserControl, DefaultPermissionControl
    {

        public string MenuName { get; set; }

        private ObservableCollection<User> _Users = new ObservableCollection<User>();

        public ObservableCollection<User> Users
        {
            get { return _Users; }
            set 
            {
                _Users = value;
            }
        }
        
        
        public CreateUsers()
        {
            InitializeComponent();
            this.MenuName = this.GetType().Name;
            lstUsers.ItemsSource = Users;
            ExecutableActions = new Dictionary<string, Tuple<string, Action>>();
            ExecutableActions.Add("Print", Tuple.Create("Print the current document.",new Action(() => permissionPrint())));
            ExecutableActions.Add("New", Tuple.Create("Create a new document.", new Action(() => permissionNew())));
            ExecutableActions.Add("Find", Tuple.Create("Look for given string in documents.", new Action(() => permissionFind())));
            ExecutableActions.Add("Save", Tuple.Create("Save document to hard-drive.", new Action(() => permissionSave())));
            ExecutableActions.Add("Delete", Tuple.Create("Delete the current document.", new Action(() => permissionDelete())));
            ExecutableActions.Add("Help", Tuple.Create("Open a new window that displays help information.", new Action(() => permissionHelp())));
            ExecutableActions.Add("Close", Tuple.Create("Close the current document.", new Action(() => permissionClose())));
            ExecutableActions.Add("Cancel", Tuple.Create("Undo changes in the current document and reload the last save.", new Action(() => permissionDelete())));
          
            createUserItems = PermissionsBLL.GetTreeMenu();
            
            trvUser.ItemsSource = createUserItems;
            mnu.ItemsSource = createUserItems;
            
        }

        public MenuItems createUserItems { get; set; }

        

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            User tempUser = new User(txtLasttName.Text, txtFirstName.Text);
            Random r = new Random();
            HashSet<string> actions = new HashSet<string>();
            HashSet<string> deny = new HashSet<string>();
            deny.Add("Find");
            for(int i = 0; i<3; i++)
            {
                int ran = r.Next(0, 3);
                string input = getKeyInt(ExecutableActions, ran);
                actions.Add(input);
            }
            string type = "Word";
            tempUser.OwnedPermissions.AllowPermissions.Add(type, actions);
            tempUser.OwnedPermissions.DenyPermissions.Add(type, deny);
            tempUser.OwnedPermissions.CalculatePermissions();
            Users.Add(tempUser);
            CheckBox cb = new CheckBox();
            

        }

        private string getKeyInt(Dictionary<string, Tuple<string,Action>> dic, int pos)
        {
            string output = "";
            int i = 0;
            foreach(string s in dic.Keys)
            {
                if(i++ == pos)
                {
                    return s;
                }
            }
            return output;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ExecutableActions[(sender as Button).Content.ToString()].Item2.Invoke();
        }

        private User previousUser = null;
        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //save dictionary
            if (previousUser != null)
                PermissionsTreeViewAdapter.FillPermissions(previousUser, createUserItems);
            //vul buttons
            loadButtons((sender as ListBox).SelectedItem);
            //vul treeview
            PermissionsTreeViewAdapter.FillMenuItems(createUserItems, ((sender as ListBox).SelectedItem) as User);
            
            
            previousUser = (sender as ListBox).SelectedItem as User;
        }

        private void loadButtons(object selectedItem)
        {
            User u = selectedItem as User;
            spButtons.Children.Clear();
            //string type = "Word";
            //MessageBox.Show("cleared");
            foreach (string s in u.UserCalculatedPermission["Word"])
            {
                Action b = null;
                //PermissionsFull.TryGetValue(s, out b);
                Tuple<string, Action> descAction;
                ExecutableActions.TryGetValue(s, out descAction);
                if (descAction != null)
                {
                    b = descAction.Item2;
                    if (b != null)
                    {
                        Button button = new Button();
                        button.Content = s;
                        button.Click += new RoutedEventHandler((x, e) => b.Invoke());
                        spButtons.Children.Add(button);
                    }
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


        public void permissionNew()
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void permissionSave()
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void permissionDelete()
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void permissionCancel()
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void permissionPrint()
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void permissionFind()
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void permissionHelp()
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void permissionClose()
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        private List<DefaultPermissionControl> _Children = new List<DefaultPermissionControl>();

        public List<DefaultPermissionControl> Children()
        {
            return _Children;
        }
        
    }
}
