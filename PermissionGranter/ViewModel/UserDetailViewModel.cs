using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PermissionGranter.Model;
using PermissionGranter.ViewModel.Services;
using PermissionGranter.ViewModel.Messages;
using PermissionGranter.ViewModel.Utility;
using System.Collections.ObjectModel;
using PermissionGranter.ViewModel.BLL;
using System.Windows;

namespace PermissionGranter.ViewModel
{
    public class UserDetailViewModel:NotifyPropertyChangedBase
    {
        public ICommand SaveComand { get; set; }
        public ICommand RemoveUser { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteGroup { get; set; }
        public ICommand AddGroup { get; set; }
        public ICommand NewUser { get; set; }
        public ICommand CopyCommand { get; set; }
        public ICommand PasteCommand { get; set; }

        Dictionary<User, List<Action>> DatabaseSaveTasks = new Dictionary<User, List<Action>>();

        private void AddTask(User s, Action a)
        {
            if (!DatabaseSaveTasks.ContainsKey(s))
            {
                DatabaseSaveTasks.Add(s, new List<Action>());
            }
            DatabaseSaveTasks[s].Add(a);
        }

        private User _SelectedUser;

        public MenuItems UserPermissions { get; set; }

        public List<UserGroup> UserGroups { get; set; }

        private MenuItems _CompleteMenu;

        public MenuItems CompleteMenu
        {
            get
            {
                if(_CompleteMenu == null)
                {
                    _CompleteMenu = PermissionsBLL.GetTreeMenu();
                }
                return _CompleteMenu;
            }
            set { _CompleteMenu = value; OnPropertyChanged("CompleteMenu"); }
        }

        private ObservableCollection<User> _AllUsers;

        public ObservableCollection<User> AllUsers
        {
            get {
                _AllUsers = _AllUsers ?? new ObservableCollection<User>();
                //BackupList = new List<Memento<User>>();
                UserBLL.AllUsers().ForEach(
                    user => 
                            {
                                _AllUsers.Add(user);
                                //BackupList.Add(new Memento<User>(user));
                            });
                return _AllUsers; }
            set
            {
                if (_AllUsers == value)
                    return;

                _AllUsers = value;
                OnPropertyChanged("AllUsers");
            }
        }


        public User SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                if (_SelectedUser == value)
                    return;

                //give the previous user permissions
                if(_SelectedUser != null)
                {
                    _SelectedUser.OwnedPermissions.AllowPermissions.Clear();
                    _SelectedUser.OwnedPermissions.DenyPermissions.Clear();
                    PermissionsTreeViewAdapter.FillPermissions(_SelectedUser, CompleteMenu);
                }


                _SelectedUser = value;
                if (value != null)
                {
                    
                    CompleteMenu.ClearItems();
                    PermissionsTreeViewAdapter.FillMenuItems(CompleteMenu, value);
                }
                OnPropertyChanged("SelectedUser");
            }
        }

        //public Memento<User> BackupUser { get; set; }
        //public List<Memento<User>> BackupList { get; set; }
        
        public UserDetailViewModel()
        {
            UserGroups = BLL.GroupBLL.AllGroups;
            SaveComand = new CustomCommand(SaveUsers, CanSaveUser);
            RemoveUser = new CustomCommand(DeleteUser, CanDeleteUser);
            CancelCommand = new CustomCommand(RollBackUser, CanCancel);
            DeleteGroup = new CustomCommand(DeleteUserFromGroup, CanDeleteGroup);
            AddGroup = new CustomCommand(AddUserToGroup, CanAdd);
            NewUser = new CustomCommand(CreateUser, (object o) => { return true; });
            Messenger.Default.Register<GroupChanged>(this, GroupsChanged);
            PasteCommand = new CustomCommand(PasteGroupDetails, CanPaste);
            CopyCommand = new CustomCommand(CopyGroupDetails, (obj) => { return SelectedUser != null; });
            Messenger.Default.Register<Memento<PermissionsBase>>(this, CopyReceived);
        }

        private void CopyGroupDetails(object obj)
        {
            Messenger.Default.Send(new Memento<PermissionsBase>(SelectedGroup));
        }

        private bool _CanPaste = false;
        private bool CanPaste(object obj)
        {
            return _CanPaste;
        }

        public Memento<PermissionsBase> CopyUser { get; set; }
        private void CopyReceived(Memento<PermissionsBase> obj)
        {
            CopyUser = obj;
            _CanPaste = SelectedUser!=null;
        }

        private void PasteGroupDetails(object obj)
        {
            SelectedUser.OwnedPermissions = CopyUser.SavedCopy.OwnedPermissions;
        }

        private List<User> NewUsers = new List<User>();

        private void CreateUser(object obj)
        {
            User u = new User();
            AllUsers.Add(u);
            NewUsers.Add(u);
            SelectedUser = u;
            DatabaseSaveTasks.Add(u, new List<Action> { new Action(() => UserBLL.CreateUser(u)) });
        }

        private bool CanAdd(object obj)
        {
            if(ToAdd != null)
            {
                return true;
            }
            return false;
        }

        private void AddUserToGroup(object obj)
        {
            if (!SelectedUser.UserGroupPermissions.Contains(ToAdd))
            {
                SelectedUser.UserGroupPermissions.Add(ToAdd);
                AddTask(SelectedUser,() => BLL.UserBLL.AddUserToGroup(SelectedUser, SelectedGroup));
            }

        }

        private UserGroup _ToAdd;

        public UserGroup ToAdd
        {
            get { return _ToAdd; }
            set
            {
                if (_ToAdd == value)
                    return;

                _ToAdd = value;
                OnPropertyChanged("ToAdd");
            }
        }

        


        private UserGroup _SelectedGroup;

        public UserGroup SelectedGroup
        {
            get { return _SelectedGroup; }
            set
            {
                if (_SelectedGroup == value)
                    return;

                _SelectedGroup = value;
                OnPropertyChanged("SelectedGroup");
            }
        }


        private bool CanDeleteGroup(object obj)
        {
            if (_SelectedGroup != null)
            {
                return true;
            }
            return false;
        }

        private void DeleteUserFromGroup(object obj)
        {
            SelectedUser.UserGroupPermissions.Remove(SelectedGroup);
            if (!NewUsers.Contains(SelectedUser))
            {
                AddTask(SelectedUser, () => BLL.UserBLL.DeleteUserFromGroup(SelectedUser, SelectedGroup));
            }
            
            
            SelectedGroup = null;
        }

        private void GroupsChanged(GroupChanged obj)
        {
            AllGroups.Clear();
            BLL.GroupBLL.GetAllGroups().ToList().ForEach(x => AllGroups.Add(x));
        }

        private ObservableCollection<UserGroup> _AllGroups;

        public ObservableCollection<UserGroup> AllGroups
        {
            get
            {
                if(_AllGroups == null)
                {
                    _AllGroups = new ObservableCollection<UserGroup>();
                    //BackupList = new List<Memento<User>>();
                    BLL.GroupBLL.GetAllGroups().ToList().ForEach(x =>_AllGroups.Add(x));
                }
                return _AllGroups;
            }
            set { _AllGroups = value; }
        }


        private void SelectedUserChanged(User obj)
        {
            SelectedUser = obj;
        }

        private bool _CanCancel = true;

        public bool CanCancelUser
        {
            get { return _CanCancel; }
            set { _CanCancel = value; }
        }


        private bool CanCancel(object obj)
        {
            return CanCancelUser;
        }

        private void RollBackUser(object obj)
        {
            DatabaseSaveTasks.Clear();
            Close();
        }

        private bool _CanSave = true;

        public bool CanSave
        {
            get { return _CanSave; }
            set { _CanSave = value; }
        }


        private bool CanSaveUser(object obj)
        {
            return CanSave;
        }

        private void SaveUsers(object obj)
        {
            bool errors = false;
            foreach(User u in AllUsers)
            {
                if (!u.IsCorrect)
                {
                    errors = true;
                    break;
                }
            }
            if (!errors)
            {
                DatabaseSaveTasks.ToList().ForEach(x => x.Value.ForEach(z => Task.Factory.StartNew(z)));
                BLL.UserBLL.UpdateUser(SelectedUser);
                Messenger.Default.Send<UsersUpdated>(new UsersUpdated());
            }
            else
            {
                if((MessageBox.Show("U heeft enkele gebruikers niet correct ingevuld, als u op OK klikt worden deze gebruikers niet opgeslagen.", "Fout", MessageBoxButton.OKCancel)
                ) == MessageBoxResult.OK)
                {
                    
                    foreach(var a in DatabaseSaveTasks)
                    {
                        if (a.Key.IsCorrect)
                        {
                            if (!NewUsers.Contains(a.Key))
                            {
                                a.Value.ForEach(x => Task.Factory.StartNew(x));
                            }
                            else
                            {
                                UserBLL.CreateUser(a.Key);
                                a.Value.ForEach(x => Task.Factory.StartNew(x));
                            }
                        }
                    }
                }
            }
            
        }

        private bool _CanDelete = true;

        public bool CanDelete
        {
            get { return _CanDelete; }
            set { _CanDelete = value; }
        }


        private bool CanDeleteUser(object obj)
        {
            return CanDelete;
        }

        private void DeleteUser(object obj)
        {
            if (!NewUsers.Contains(SelectedUser))
            {

                BLL.UserBLL.DeleteUser(SelectedUser);
            }
            else
            {
                NewUsers.Remove(SelectedUser);
            }
            AllUsers.Remove(SelectedUser);
            SelectedUser = null;
        }

        private void Close()
        {
            Messenger.Default.Send(new CloseUserDetail());
        }

    }
}
