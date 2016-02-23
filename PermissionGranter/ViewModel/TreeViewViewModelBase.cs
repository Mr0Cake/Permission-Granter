using PermissionGranter.Model;
using PermissionGranter.ViewModel.BLL;
using PermissionGranter.ViewModel.Extensions;
using PermissionGranter.ViewModel.Messages;
using PermissionGranter.ViewModel.Services;
using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PermissionGranter.ViewModel
{
    public abstract class TreeViewViewModelBase:INotifyPropertyChanged
    {
        #region Commands
        public ICommand CopyCommand { get; set; }
        public ICommand PasteCommand { get; set; }

        public ICommand RemoveItemCommand { get; set; }
        public ICommand NewItemCommand { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand RemoveUserFromGroupCommand { get; set; }
        public ICommand AddUserToGroupCommand { get; set; }

        public ICommand WindowClosingCommand { get; set; }
        #endregion


        #region Save/Cancel
        protected bool CanCancel(object obj)
        {
            return true;
        }

        protected virtual void CancelChanges(object obj)
        {
            _SelectedItem = null;
            dbActions.Cancel();
            AllItems.Clear();
            CompleteMenu.ClearItems();
            
                if (this is GroupDetailViewModel)
                    _AllItemsBackup.ForEach(x => AllItems.Add(x.SavedCopy as UserGroup));
                if (this is UserDetailViewModel)
                    _AllItemsBackup.ForEach(x => AllItems.Add(x.SavedCopy as User));
            
            if (this is GroupDetailViewModel)
            {
                Messenger.Default.Send(new CloseGroupDetail());
            }
            else
            {
                Messenger.Default.Send(new CloseUserDetail());
            }

        }

        protected bool CanSaveItem(object obj)
        {
            return true;
        }

        protected virtual void SaveItem(object obj)
        {
            //check current Item
            if (CompleteMenu.Items.FindFirst(x => x.Changed))
            {
                PermissionsTreeViewAdapter.FillPermissions(SelectedItem, CompleteMenu);
                //dbActions.UpdatePermissions(SelectedItem);
            }

            

        }

        public void SavePermissions()
        {
            AllItems.Where(x => x.OwnedPermissions.Changed).ToList().ForEach(p => PermissionsBLL.ReplacePermissions(p));
        }

        #endregion

        #region Copy/Paste

        protected bool CanCopy(object obj)
        {
            return SelectedItem != null;
        }

        protected void CopyItemDetails(object obj)
        {
            Memento<PermissionsBase> copy = new Memento<PermissionsBase>(SelectedItem);
            Messenger.Default.Send(copy);

        }
        
        protected bool CanPaste(object obj)
        {
            return CopyItem != null && SelectedItem != null;
        }

        public Memento<PermissionsBase> CopyItem { get; set; }

        protected void CopyReceived(Memento<PermissionsBase> obj)
        {
            CopyItem = obj;
        }

        protected void PasteItemDetails(object obj)
        {
            SelectedItem.OwnedPermissions = CopyItem.SavedCopy.OwnedPermissions;
            CompleteMenu.ClearItems();
            if (SelectedItem.OwnedPermissions.AllowPermissions.Count > 0 && SelectedItem.OwnedPermissions.DenyPermissions.Count > 0)
            {
                PermissionsTreeViewAdapter.FillMenuItems(CompleteMenu, SelectedItem);
            }
            
               
        }

        #endregion

        #region Create/Delete
        protected bool CanCreate(object obj)
        {
            return true;
        }

        protected void CreateItem(object obj)
        {
            PermissionsBase pb;
            if(this is UserDetailViewModel)
            {
                pb = new User();
            }
            else if(this is GroupDetailViewModel)
            {
                pb = new UserGroup();
            }
            else
            {
                pb = null;
            }
            AllItems.Add(pb);
            SelectedItem = pb;
        }

        protected bool CanRemoveItem(object obj)
        {
            return SelectedItem != null;
        }

        protected void RemoveItem(object obj)
        {
            int id = -2;
            if (this is UserDetailViewModel)
            {
                id = ((User)SelectedItem).UserID;
            }
            else if (this is GroupDetailViewModel)
            {
                id = ((UserGroup)SelectedItem).GroupID;
            }
            
            if(id!=-2)
            if (id != -1)
            {
                //already in database
                dbActions.DeletePermissionsBase(SelectedItem);
            }
            else
            {
                dbActions.Cancel(SelectedItem);
            }

            AllItems.Remove(SelectedItem);
            SelectedItem = null;
            CompleteMenu.ClearItems();
        }




        #endregion

        public DelayedDatabaseActions dbActions = new DelayedDatabaseActions();

        #region propertychanged
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region allItems

        protected List<Memento<PermissionsBase>> _AllItemsBackup = new List<Memento<PermissionsBase>>();

        private ObservableCollection<PermissionsBase> _AllItems;

        public ObservableCollection<PermissionsBase> AllItems
        {
            get
            {
                if (_AllItems == null)
                {
                    List<PermissionsBase> items = new List<PermissionsBase>();
                    _AllItems = new ObservableCollection<PermissionsBase>();
                    if (this is UserDetailViewModel)
                    {
                        items = UserBLL.AllUsers().OfType<PermissionsBase>().ToList();
                    }
                    else if (this is GroupDetailViewModel)
                    {
                        items = GroupBLL.GetAllGroups().OfType<PermissionsBase>().ToList();
                    }
                    items.ForEach(x =>
                    {
                        _AllItems.Add(x);
                        _AllItemsBackup.Add(new Memento<PermissionsBase>(x));
                    });
                }
                return _AllItems;
            }
            set
            {
                if (_AllItems == value)
                    return;

                _AllItems = value;
                OnPropertyChanged("AllItems");
            }
        }

        #endregion

        #region Menu
        private PermissionsBase _SelectedItem;
        public virtual PermissionsBase SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                if (_SelectedItem == value)
                    return;

                //Save previous item's OwnedPermission if it has been changed
                if (_SelectedItem != null && CompleteMenu.Items.FindFirst(x => x.Changed))
                {
                    PermissionsTreeViewAdapter.FillPermissions(_SelectedItem, CompleteMenu);
                    _SelectedItem.OwnedPermissions.Changed = true;

                    //dbActions.UpdatePermissions(_SelectedItem);
                    _CompleteMenu.Items.ToList().ForEach(x => x.Changed = false);
                }

                
                _SelectedItem = value;
                CompleteMenu.ClearItems();
                if(value.OwnedPermissions.AllowPermissions.Count > 0 || value.OwnedPermissions.DenyPermissions.Count > 0)
                PermissionsTreeViewAdapter.FillMenuItems(CompleteMenu, value);
                OnPropertyChanged("CompleteMenu");
                OnPropertyChanged("SelectedItem");
                
            }
        }

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
            set
            {
                _CompleteMenu = value;
                OnPropertyChanged("CompleteMenu");
            }
        }
        #endregion

        public void AddUserToGroup(User u, UserGroup ug)
        {
            if (!u.UserGroupPermissions.Contains(ug) && !ug.GroupUsers.Contains(u))
            {
                if(this is UserDetailViewModel)
                {
                    u.Changed = true;
                }
                else
                {
                    ug.Changed = true;
                }

                u.UserGroupPermissions.Add(ug);
                ug.GroupUsers.Add(u);
                dbActions.UserGroupDelayedAction(u, ug, true);
            }
        }

        /// <summary>
        /// Remove User u from UserGroup ug
        /// this will also remove the database action
        /// </summary>
        /// <param name="u"></param>
        /// <param name="ug"></param>
        public void RemoveUserFromGroup(User u, UserGroup ug)
        {
            if (u.UserGroupPermissions.FindFirst(x => x.ID == ug.ID) && ug.GroupUsers.FindFirst(x => x.ID == u.ID))
            {
                u.UserGroupPermissions.ToList().RemoveAll(x => x.ID == ug.ID);
                ug.GroupUsers.ToList().RemoveAll(x => x.ID == u.ID);
                //if selectedItem is a new item cancel changes, else queue a remove action to the database
                if (SelectedItem.ID == -1)
                {
                    dbActions.removeUserGroupAction(u, ug);
                }
                else
                {
                    dbActions.UserGroupDelayedAction(u, ug, false);
                }
            }
            
        }

    }
}
