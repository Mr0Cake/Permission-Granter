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
using PermissionGranter.ViewModel.Extensions;

namespace PermissionGranter.ViewModel
{
    public class UserDetailViewModel:TreeViewViewModelBase
    {

        public UserDetailViewModel()
        {
            
            
            NewItemCommand = new CustomCommand(CreateItem, CanCreate);
            RemoveItemCommand = new CustomCommand(RemoveItem, CanRemoveItem);

            CancelCommand = new CustomCommand(CancelChanges, CanCancel);
            SaveCommand = new CustomCommand(SaveItem, CanSaveItem);

            RemoveUserFromGroupCommand = new CustomCommand(DeleteUserFromGroup, CanDeleteGroup);
            AddUserToGroupCommand = new CustomCommand(AddUserToGroup, CanAdd);
            
            PasteCommand = new CustomCommand(PasteItemDetails, CanPaste);
            CopyCommand = new CustomCommand(CopyItemDetails, CanCopy);
            WindowClosingCommand = new CustomCommand(CancelChanges, CanCancel);
            Messenger.Default.Register<Memento<PermissionsBase>>(this, CopyReceived);

            Messenger.Default.Register<GroupChanged>(this, GroupsChanged);
            
        }


        protected override void CancelChanges(object obj)
        {
            base.CancelChanges(obj);
            UserGroups.Clear();
            SelectedGroup = null;
            ToAdd = null;
        }

        private bool CanAdd(object obj)
        {
            return ToAdd != null && !UserGroups.FindFirst(x => x.ID == ToAdd.ID) && SelectedItem != null;
        }

        private void AddUserToGroup(object obj)
        {
            if(SelectedItem != null && ToAdd != null)
            {
                AddUserToGroup(SelectedItem as User, ToAdd);
                UserGroups.Add(ToAdd);
                //AllItems[AllItems.IndexOf(SelectedItem)].Changed = true;
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
            RemoveUserFromGroup(SelectedItem as User, SelectedGroup);
            UserGroups.Remove(SelectedGroup);
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

        private ObservableCollection<UserGroup> _UserGroups;

        public ObservableCollection<UserGroup> UserGroups
        {
            get
            {
                if (_UserGroups == null)
                    _UserGroups = new ObservableCollection<UserGroup>();
                return _UserGroups;
            }
            set
            {
                if (_UserGroups == value)
                    return;

                _UserGroups = value;
                OnPropertyChanged("UserGroups");
            }
        }


        public override PermissionsBase SelectedItem
        {
            get { return base.SelectedItem; }
            set
            {
                if (base.SelectedItem == value || value == null)
                    return;

                base.SelectedItem = value;

                UserGroups.Clear();
                User ug = SelectedItem as User;
                //Check if User has no groups
                if (ug.UserGroupPermissions == null || (ug.UserGroupPermissions != null && ug.UserGroupPermissions.Count == 0))
                {
                    //Check in database if user has groups
                    //Make sure you only do this the fist time
                    if (ug.ID > 0 && !ug.CheckedInDatabase)
                    {
                        List<UserGroup> dbGroups = GroupBLL.GetGroupsByUserID(ug.ID);
                        //Compare ID of dbgroups with allgroups and then add the UserGroup in AllGroups to UserGroups.
                        //Equals Method is only using GUID and I need the GUID that has been generated in allgroups.
                        AllGroups.Where(x => dbGroups.FindFirst(z => z.ID == x.ID)).ToList().ForEach(p => UserGroups.Add(p));
                        ug.CheckedInDatabase = true;
                    }
                    //if above statement is false the user probably doesn't have a usergroup assigned
                }
                else
                {
                    //the User has usergrouppermissions and they should have the same GUID as in AllGroups
                    ug.UserGroupPermissions.Where(x => AllGroups.FindFirst(z => z.ID == x.ID)).ToList().ForEach(z => UserGroups.Add(z));
                }

                OnPropertyChanged("UserGroups");
            }
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

        protected override void SaveItem(object obj)
        {
            base.SaveItem(obj);
            
            List<User> AllUserGroupItems = AllItems.Cast<User>().ToList();
            if (AllItems.FindFirst(x => x.Changed || x.OwnedPermissions.Changed))
            {
                if (AllItems.FindFirst(x => !x.IsCorrect))
                {
                    if (MessageBox.Show("U heeft enkele gebruikers niet correct ingevuld, klik op ok om de foutieve gebruikers niet op te slaan, op annuleren om deze te verbeteren.", "Fout", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            foreach (User ug in AllUserGroupItems)
                            {
                                if (!ug.IsCorrect)
                                {
                                    //remove database actions
                                    dbActions.Cancel(ug);
                                    dbActions.removeUserGroupAction(ug);
                                    //remove user from groups
                                    AllGroups.ToList().Where(x => ug.UserGroupPermissions.Contains(x)).ToList().ForEach(z => z.GroupUsers.Remove(ug));
                                    //check new item
                                    if (ug.ID == -1)
                                    {
                                        //remove item
                                        AllItems.Remove(ug);
                                    }
                                    //item has id, undo changes
                                    else
                                    {
                                        User OldUser = _AllItemsBackup.Find(x => x.SavedCopy.Equals(ug)).SavedCopy as User;
                                        if (OldUser != null && OldUser.ID != -1)
                                        {
                                            //replace item
                                            AllItems[AllItems.IndexOf(ug)] = OldUser;
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
                                        UserBLL.UpdateUser(ug);
                                }
                            }
                            dbActions.Execute();
                            SavePermissions();
                            MessageBox.Show("Database is opgeslagen.", "Opgeslagen", MessageBoxButton.OK);
                        });
                    }
                }
                else
                {
                    
                    AllUserGroupItems.Where(x => x.Changed || x.ID == -1).ToList().ForEach(p => BLL.UserBLL.UpdateUser(p));
                    dbActions.Execute(AllItems.ToList());
                    SavePermissions();
                    MessageBox.Show("Database is opgeslagen.", "Opgeslagen", MessageBoxButton.OK);
                    
                }

                Task.Factory.StartNew(() =>
                {
                foreach (User u in AllUserGroupItems.Where(x => x.OwnedPermissions.Changed).Where(z => z.Email.Contains('@')))
                    {
                        User old = _AllItemsBackup.Where(x => x.SavedCopy.Equals(u)).First().SavedCopy as User;
                        dbActions.ChangesToPermission(old, u);
                    }

                    foreach(var p in dbActions.NotificationMail)
                    {
                        StringBuilder sb = new StringBuilder();
                        p.Value.ForEach(s => sb.AppendLine(s));
                        SendMail.Mail(p.Key.Email, sb.ToString());
                    }

                    AllItems.ToList().ForEach(x => { x.Changed = false; x.OwnedPermissions.Changed = false; });
                });

                MessageBox.Show("Database wordt geupdate.", "Opslaan", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Er is niets veranderd.", "Geen verandering", MessageBoxButton.OK);
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
        

        private void Close()
        {
            Messenger.Default.Send(new CloseUserDetail());
        }

    }
}
