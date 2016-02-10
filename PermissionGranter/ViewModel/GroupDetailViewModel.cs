using PermissionGranter.Model;
using PermissionGranter.ViewModel.BLL;
using PermissionGranter.ViewModel.Messages;
using PermissionGranter.ViewModel.Services;
using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PermissionGranter.ViewModel.Extensions;

namespace PermissionGranter.ViewModel
{
    public class GroupDetailViewModel:NotifyPropertyChangedBase
    {
        //public Dictionary<PermissionsBase, List<Action>> SaveToDatabase = new Dictionary<PermissionsBase, List<Action>>();
        public Dictionary<UserGroup, Dictionary<User, Action>> AddUsertoGroup = new Dictionary<UserGroup, Dictionary<User, Action>>();
        //public List<UserGroup> NewGroups = new List<UserGroup>();
        //public List<Memento<PermissionsBase>> GroupsBackup = new List<Memento<PermissionsBase>>();

        public DelayedDatabaseActions dbActions = new DelayedDatabaseActions();

        public ICommand AddGroup { get; set; }
        public ICommand RemoveGroup { get; set; }
        public ICommand AddUserToGroup { get; set; }
        public ICommand RemoveUserFromGroup { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CopyCommand { get; set; }
        public ICommand PasteCommand { get; set; }

        


        public GroupDetailViewModel()
        {
            
            AddGroup = new CustomCommand(CreateGroup, CanCreate);
            RemoveGroup = new CustomCommand(DeleteGroup, CanDeleteGroup);
            AddUserToGroup = new CustomCommand(AddUsers, CanAdd);
            SaveCommand = new CustomCommand(SaveGroup, CanSaveGroup);
            CancelCommand = new CustomCommand(CancelChanges, CanCancel);
            RemoveUserFromGroup = new CustomCommand(DeleteFromGroup, CanDelete);


            PasteCommand = new CustomCommand(PasteGroupDetails, CanPaste);
            CopyCommand = new CustomCommand(CopyGroupDetails, (obj) => { return SelectedGroup != null; });
            Messenger.Default.Register<Memento<PermissionsBase>>(this, CopyReceived);
            Messenger.Default.Register<UsersUpdated>(this, UpdateUsers);
        }

        private void DeleteFromGroup(object obj)
        {
            if(SelectedGroup.GroupID != -1)
            dbActions.UserGroupDelayedAction(SelectedUser, SelectedGroup, false);
            SelectedUser.UserGroupPermissions.Remove(SelectedGroup);
            SelectedGroup.GroupUsers.Remove(SelectedUser);
        }

        private bool CanDelete(object obj)
        {
            return SelectedUser != null;
        }

        private void DeleteGroup(object obj)
        {
            if (SelectedGroup.GroupID != -1)
            {
                //already in database
                dbActions.DeletePermissionsBase(SelectedGroup);
            }
            else
            {
                dbActions.CancelChanges(SelectedGroup);
            }
            AllGroups.Remove(SelectedGroup);
            SelectedGroup = null;
        }

        private void CopyGroupDetails(object obj)
        {
            Memento<PermissionsBase> copy = new Memento<PermissionsBase>(SelectedGroup);
            Messenger.Default.Send(copy);
        }

        private bool _CanPaste = false;
        private bool CanPaste(object obj)
        {
            return _CanPaste;
        }

        private void CopyReceived(Memento<PermissionsBase> obj)
        {
            CopyGroup = obj;
            _CanPaste = SelectedGroup != null;
        }

        private void PasteGroupDetails(object obj)
        {
            SelectedGroup.OwnedPermissions = CopyGroup.SavedCopy.OwnedPermissions;
        }

        private void UpdateUsers(UsersUpdated obj)
        {
            AllUsers.Clear();
            AllUsers = null;
            //BLL.UserBLL.AllUsers().ForEach(x => _AllUsers.Add(x));
        }

        private bool CanCreate(object obj)
        {
            return true;
        }

        private void CreateGroup(object obj)
        {
            UserGroup NewGroup = new UserGroup();
            AllGroups.Add(NewGroup);
            SelectedGroup = NewGroup;
            
        }

        public Memento<PermissionsBase> CopyGroup { get; set; }
        
        private void AddUsers(object obj)
        {
            if(SelectedGroup != null)
            {
                if(SelectedAddUser != null)
                {
                    dbActions.UserGroupDelayedAction(SelectedAddUser, SelectedGroup, true);
                    SelectedGroup.GroupUsers.Add(SelectedAddUser);
                    SelectedAddUser.UserGroupPermissions.Add(SelectedGroup);
                }
            }
        }

        private bool CanAdd(object obj)
        {
            return SelectedAddUser != null;
        }

        private bool CanDeleteGroup(object obj)
        {
            return SelectedGroup != null;
        }

        private bool CanCancel(object obj)
        {
            return true;
        }

        private bool CanSaveGroup(object obj)
        {
            return true;
        }

       

        

        private void CancelChanges(object obj)
        {
            _SelectedGroup = null;
            dbActions.Cancel();
            AllGroups.Clear();
            _AllGroupsBackup.ForEach(x => AllGroups.Add(x.SavedCopy as UserGroup)) ;
            Close();
        }

        private void Close()
        {
            Messenger.Default.Send(new CloseGroupDetail());
        }

        private void SaveGroup(object obj)
        {
            //check current group
            if(CompleteMenu.Items.FindFirst(x => x.Changed))
            {
                PermissionsTreeViewAdapter.FillPermissions(SelectedGroup, CompleteMenu);
                dbActions.UpdatePermissions(SelectedGroup);
            }
            if (AllGroups.FindFirst(x => x.Changed))
            {
                if(MessageBox.Show("U heeft enkele gebruikersgroepen niet correct ingevuld, klik op ok om de foutieve groepen niet op te slaan, op annuleren om deze te verbeteren.", "Fout", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    AllGroups.Where(z => !z.IsCorrect && z.GroupID == -1).ToList().ForEach(p =>  p.GroupUsers.ToList().ForEach(user => { user.UserGroupPermissions.Remove(p); dbActions.CancelChanges(p); }));
                    AllGroups.Where(z => !z.IsCorrect && z.GroupID != -1).ToList().ForEach(p => p.GroupUsers.ToList().ForEach(user => { user.UserGroupPermissions.Remove(p); dbActions.UserGroupDelayedAction(user, p, false); }));
                    AllGroups.Where(z => z.IsCorrect && z.GroupID == -1).ToList().ForEach(p => BLL.GroupBLL.CreateUserGroup(p));
                    AllGroups.Where(x => x.IsCorrect && x.Changed).ToList().ForEach(p => BLL.GroupBLL.CreateUserGroup(p));
                    dbActions.Execute();
                }
            }
            else
            {
                AllGroups.Where(z => z.IsCorrect && z.GroupID == -1).ToList().ForEach(p => BLL.GroupBLL.CreateUserGroup(p));
                AllGroups.Where(x => x.IsCorrect && x.Changed).ToList().ForEach(p => BLL.GroupBLL.CreateUserGroup(p));
                dbActions.Execute();
            }
        }
        

        private ObservableCollection<UserGroup> _UserGroups;

        public ObservableCollection<UserGroup> UserGroups
        {
            get { return _UserGroups; }
            set { _UserGroups = value; }
        }

        private ObservableCollection<User> _GroupUsers = new ObservableCollection<User>();

        public ObservableCollection<User> GroupUsers
        {
            get { return _GroupUsers; }
            set
            {
                _GroupUsers = value;
                OnPropertyChanged("GroupUsers");
            }
        }

        private List<Memento<PermissionsBase>> _AllUsersBackup = new List<Memento<PermissionsBase>>();

        private ObservableCollection<User> _AllUsers;

        public ObservableCollection<User> AllUsers
        {
            get
            {
                if(_AllUsers == null)
                {
                    _AllUsers = new ObservableCollection<User>();
                    _AllUsersBackup.Clear();
                    BLL.UserBLL.AllUsers().ForEach(x => { _AllUsers.Add(x); _AllUsersBackup.Add(new Memento<PermissionsBase>(x)); });
                    
                }
                return _AllUsers;

            }
            set { _AllUsers = value; }
        }

        private UserGroup _SelectedGroup;

        

        public UserGroup SelectedGroup
        {
            get { return _SelectedGroup; }
            set
            {
                if (_SelectedGroup == value)
                    return;
                Task waitfortask = null;
                if (SelectedGroup != null && CompleteMenu.Items.FindFirst(x => x.Changed))
                {
                    //deze had ik graag in een task gestopt
                    waitfortask = Task.Factory.StartNew(() =>
                    {
                        PermissionsTreeViewAdapter.FillPermissions(SelectedGroup, CompleteMenu);
                        dbActions.UpdatePermissions(SelectedGroup);
                    });

                }

                

                //BackupGroup = new Memento<UserGroup>(value);
                GroupUsers.Clear();
                AllUsers.ToList().Where(x => x.UserGroupPermissions.Contains(value)).ToList().ForEach(z => GroupUsers.Add(z));
                //BLL.UserBLL.SelectUsersByGroupName(_SelectedGroup.GroupName).ToList().ForEach(x => GroupUsers.Add(x));
                if(waitfortask != null)
                Task.WaitAll(waitfortask);
                CompleteMenu.ClearItems();
                Task.Factory.StartNew(() =>
                PermissionsTreeViewAdapter.FillMenuItems(CompleteMenu, value));
                _SelectedGroup = value;
                OnPropertyChanged("SelectedGroup");
            }
        }

        private User _SelectedAddUser;

        public User SelectedAddUser
        {
            get { return _SelectedAddUser; }
            set
            {
                if (_SelectedAddUser == value)
                    return;

                _SelectedAddUser = value;
                OnPropertyChanged("SelectedAddUser");
            }
        }

        private User _SelectedUser;

        public User SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                if (_SelectedUser == value)
                    return;

                _SelectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        private MenuItems _CompleteMenu;

        public MenuItems CompleteMenu
        {
            get
            {
                if (_CompleteMenu == null)
                {
                    _CompleteMenu = PermissionsBLL.GetTreeMenu();
                    _CompleteMenu.Items.ToList().ForEach(x => x.Changed = false);
                }
                return _CompleteMenu;
            }
            set { _CompleteMenu = value; OnPropertyChanged("CompleteMenu"); }
        }

        private List<Memento<PermissionsBase>> _AllGroupsBackup = new List<Memento<PermissionsBase>>();

        private ObservableCollection<UserGroup> _AllGroups;

        public ObservableCollection<UserGroup> AllGroups
        {
            get
            {
                if(_AllGroups == null)
                {
                    _AllGroups = new ObservableCollection<UserGroup>();
                    _AllGroups.Clear();
                    GroupBLL.GetAllGroups().ForEach(x => { _AllGroups.Add(x); _AllGroupsBackup.Add(new Memento<PermissionsBase>(x)); });
                }
                return _AllGroups;
            }
            set { _AllGroups = value; }
        }

    }
}
