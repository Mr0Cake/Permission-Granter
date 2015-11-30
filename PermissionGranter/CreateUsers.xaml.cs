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
            PermissionsFull = new Dictionary<string, Action>();
            PermissionsFull.Add("Print", new Action(() => permissionPrint()));
            PermissionsFull.Add("New", new Action(() => permissionNew()));
            PermissionsFull.Add("Find", new Action(() => permissionFind()));
            PermissionsFull.Add("Save", new Action(() => permissionSave()));
            PermissionsFull.Add("Delete", new Action(() => permissionDelete()));
            //MenuItems.Add(this);
            //CreateTreeView();
            createUserItems = new MenuItems();
            createUserItems.Items.Add(new CustTreeItems(this.MenuName, true));

            foreach (string s in PermissionsFull.Keys)
                createUserItems.Items[0].Options.Add(new Permission(createUserItems.Items[0],s, true));
            trvUser.ItemsSource = createUserItems;
        }

        public MenuItems createUserItems { get; set; }

        

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            User tempUser = new User(txtLasttName.Text, txtFirstName.Text, txtFunction.Text);
            Random r = new Random();
            HashSet<string> actions = new HashSet<string>();
            HashSet<string> deny = new HashSet<string>();
            deny.Add("Find");
            for(int i = 0; i<3; i++)
            {
                int ran = r.Next(0, 3);
                string input = getKeyInt(PermissionsFull, ran);
                actions.Add(input);
            }
            string type = this.GetType().Name;
            tempUser.UserPermissions.AllowPermissions.Add(type, actions);
            tempUser.UserPermissions.DenyPermissions.Add(type, deny);
            tempUser.UserPermissions.CalculatePermissions();
            Users.Add(tempUser);

        }

        private string getKeyInt(Dictionary<string, Action> dic, int pos)
        {
            string output = "";
            int i = 0;
            foreach(KeyValuePair<string, Action> kvp in dic)
            {
                if(i++ == pos)
                {
                    return kvp.Key;
                }
            }
            return output;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            PermissionsFull[(sender as Button).Content.ToString()].Invoke();
        }

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadButtons((sender as ListBox).SelectedItem);
            PermissionsToTreeViewAdapter.FillMenuItems(createUserItems, ((sender as ListBox).SelectedItem) as User);
            //TODO SAVE TO DICTIONARY WHEN CHANGE

        }

        private void loadButtons(object selectedItem)
        {
            User u = selectedItem as User;
            spButtons.Children.Clear();
            string type = this.GetType().Name;
            MessageBox.Show("cleared");
            foreach (string s in u.UserCalculatedPermission[type])
            {
                Action b = null;
                PermissionsFull.TryGetValue(s, out b);
                if (b != null)
                {
                    Button button = new Button();
                    button.Content = s;
                    button.Click += new RoutedEventHandler((x,e) => b.Invoke());
                    spButtons.Children.Add(button);
                }
            }
        }

        private Dictionary<string,Action> _PermissionsFull;

        public Dictionary<string,Action> PermissionsFull
        {
            get { return _PermissionsFull; }
            set { _PermissionsFull = value; }
        }

        

        public Dictionary<string, Action> getFullPermissions()
        {
            return PermissionsFull;
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
