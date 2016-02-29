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
    public class GroupDetailViewModel:TreeViewViewModelBase
    {
        

        public GroupDetailViewModel()
        {
            //item acions
            NewItemCommand = new CustomCommand(CreateItem, CanCreate);
            RemoveItemCommand = new CustomCommand(RemoveItem, CanRemoveItem);

            //save/cancel actions
            SaveCommand = new CustomCommand(SaveItem, CanSaveItem);
            CancelCommand = new CustomCommand(CancelChanges, CanCancel);

            //add/remove user from group
            RemoveUserFromGroupCommand = new CustomCommand(DeleteFromGroup, CanDelete);
            AddUserToGroupCommand = new CustomCommand(AddUsers, CanAdd);

            //Paste/copy OwnedPermisisons
            PasteCommand = new CustomCommand(PasteItemDetails, CanPaste);
            CopyCommand = new CustomCommand(CopyItemDetails, CanCopy);
            WindowClosingCommand = new CustomCommand(CancelChanges, CanCancel);

            //copy between group and user, receive a copy
            Messenger.Default.Register<Memento<PermissionsBase>>(this, CopyReceived);

            //Check if Userlist has been updated
            Messenger.Default.Register<UsersUpdated>(this, UpdateUsers);
        }

        protected override void CancelChanges(object obj)
        {
            base.CancelChanges(obj);
            GroupUsers.Clear();
            SelectedAddUser = null;
            SelectedUser = null;
        }

        private void DeleteFromGroup(object obj)
        {
            RemoveUserFromGroup(SelectedUser, SelectedItem as UserGroup);
            GroupUsers.Remove(SelectedUser);
            SelectedUser = null;
        }

        private bool CanDelete(object obj)
        {
            return SelectedUser != null;
        }
        
        private void UpdateUsers(UsersUpdated obj)
        {
            AllUsers.Clear();
            AllUsers = null;
        }

        
        private void AddUsers(object obj)
        {
            AddUserToGroup(SelectedAddUser, SelectedItem as UserGroup);
            GroupUsers.Add(SelectedAddUser);
        }

        private bool CanAdd(object obj)
        {
            return SelectedAddUser != null && !GroupUsers.FindFirst(x => x.ID == SelectedAddUser.ID) && SelectedItem != null;
        }
        

        //private void Close()
        //{
        //    //dit werkt niet
        //    Messenger.Default.Send(new CloseGroupDetail());
        //}

        protected override void SaveItem(object obj)
        {
            base.SaveItem(obj);
            List<UserGroup> AllUserGroupItems = AllItems.Cast<UserGroup>().ToList();
            if (AllItems.FindFirst(x => x.Changed || x.OwnedPermissions.Changed))
            {
                if (AllItems.FindFirst(x => !x.IsCorrect))
                {
                    if (MessageBox.Show("U heeft enkele gebruikersgroepen niet correct ingevuld, klik op ok om de foutieve groepen niet op te slaan, op annuleren om deze te verbeteren.", "Fout", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        foreach (UserGroup ug in AllUserGroupItems)
                        {
                            if (!ug.IsCorrect)
                            {
                                dbActions.Cancel(ug);
                                dbActions.removeUserGroupAction(ug);
                                AllUsers.ToList().Where(x => ug.GroupUsers.Contains(x)).ToList().ForEach(z => z.UserGroupPermissions.Remove(ug));
                                if (ug.GroupID == -1)
                                {
                                    AllItems.Remove(ug);
                                }
                                else
                                {
                                    UserGroup OldGroup = _AllItemsBackup.Find(x => x.SavedCopy.Equals(ug)).SavedCopy as UserGroup;
                                    if (OldGroup != null && OldGroup.ID != -1)
                                    {
                                        AllItems[AllItems.IndexOf(ug)] = OldGroup;
                                    }
                                    else
                                    {
                                        AllItems.Remove(ug);
                                    }
                                }
                            }
                            else
                            {
                                //if (ug.Changed && ug.ID != -1)
                                if (ug.Changed)
                                    GroupBLL.EditUserGroup(ug);
                            }
                        }
                        dbActions.Execute();
                        SavePermissions();
                    }
                }
                else
                {
                    AllUserGroupItems.Where(x => x.Changed || x.ID == -1).ToList().ForEach(p => BLL.GroupBLL.EditUserGroup(p));
                    dbActions.Execute(AllItems.ToList());
                    SavePermissions();
                }

                MessageBox.Show("Database wordt geupdate.", "Opslaan", MessageBoxButton.OK);
                
            }
            else
            {
                MessageBox.Show("Er is niets veranderd.", "Geen verandering", MessageBoxButton.OK);
            }


        }
        

        private ObservableCollection<User> _GroupUsers = new ObservableCollection<User>();

        public ObservableCollection<User> GroupUsers
        {
            get
            {
                return _GroupUsers;
            }
            set
            {
                if (_GroupUsers == value)
                    return;
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
        
        public override PermissionsBase SelectedItem
        {
            get { return base.SelectedItem; }
            set
            {
                if (base.SelectedItem == value || value == null)
                    return;

                base.SelectedItem = value;

                //GroupUsers.Clear();
                //UserGroup ug = SelectedItem as UserGroup;
                //if(ug.GroupUsers!=null && ug.GroupUsers.Count > 0)
                //AllUsers.ToList().Where(x => x.UserGroupPermissions.Contains(value)).ToList().ForEach(z => GroupUsers.Add(z));

                GroupUsers.Clear();
                UserGroup ug = SelectedItem as UserGroup;
                //Check if User has no groups
                if (ug.GroupUsers == null || (ug.GroupUsers != null && ug.GroupUsers.Count == 0))
                {
                    //Check in database if group has users
                    //Make sure you only do this the fist time
                    if (ug.ID > 0 && !ug.CheckedInDatabase)
                    {
                        List<int> dbGroupUsers = GroupBLL.GetUsersByGroupID(ug.ID);
                        //Compare ID of dbgroups with allgroups and then add the UserGroup in AllGroups to UserGroups.
                        //Equals Method is only using GUID and I need the GUID that has been generated in allgroups.
                        AllUsers.Where(x => dbGroupUsers.FindFirst(z => z == x.ID)).ToList().ForEach(p => { GroupUsers.Add(p); ug.GroupUsers.Add(p); });
                        ug.CheckedInDatabase = true;
                    }
                    //if above statement is false the user probably doesn't have a usergroup assigned
                }
                else
                {
                    //the User has usergrouppermissions and they should have the same GUID as in AllGroups
                    ug.GroupUsers.Where(x => AllUsers.Contains(x)).ToList().ForEach(z => GroupUsers.Add(z));
                }
                OnPropertyChanged("GroupUsers");
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
        
    }
}
