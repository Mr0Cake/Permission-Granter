using PermissionGranter.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.BLL
{
    public class DelayedDatabaseActions
    {
        public ConcurrentDictionary<PermissionsBase, Action> DeleteFromDatabase = new ConcurrentDictionary<PermissionsBase, Action>();
        public ConcurrentDictionary<GroupAdd, Action> GroupActions = new ConcurrentDictionary<GroupAdd, Action>();
        public ConcurrentDictionary<PermissionsBase, Action> PermissionsActions = new ConcurrentDictionary<PermissionsBase, Action>();

        public DelayedDatabaseActions()
        {

        }

        public void UpdatePermissions(PermissionsBase pb)
        {
            PermissionsBase copy = pb.GetCopy();
            Action remove;
            PermissionsActions.TryRemove(copy, out remove);
            PermissionsActions.TryAdd(copy, () => PermissionsBLL.ReplacePermissions(copy));
        }

        public void CancelUpdatePermissions(PermissionsBase pb)
        {
            PermissionsBase copy = pb.GetCopy();
            Action remove;
            PermissionsActions.TryRemove(copy, out remove);
        }

        public void Execute()
        {
            DeleteFromDatabase.ToList().ForEach(x => x.Value.Invoke());
            GroupActions.ToList().ForEach(x => x.Value.Invoke());
            PermissionsActions.ToList().ForEach(x => x.Value.Invoke());
            DeleteFromDatabase.Clear();
            GroupActions.Clear();
            PermissionsActions.Clear();
        }

        public void Cancel()
        {
            DeleteFromDatabase.Clear();
            GroupActions.Clear();
            PermissionsActions.Clear();
        }

        public void DeletePermissionsBase(PermissionsBase obj)
        {
            PermissionsBase pb = obj.GetCopy();
            CancelChanges(pb);
            if (!DeleteFromDatabase.ContainsKey(pb))
            if(pb is UserGroup)
            {
                DeleteFromDatabase.TryAdd(pb, () => GroupBLL.DeleteUserGroup(pb as UserGroup));
            }else
            if(pb is User)
            {
                DeleteFromDatabase.TryAdd(pb, () => UserBLL.DeleteUser(pb as User));
            }
            
        }


        /// <summary>
        /// Deletes or adds the user "user" from/to UserGroup "group"
        /// </summary>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <param name="AddDelete">true = AddUser, false = RemoveUser</param>
        public void UserGroupDelayedAction(User user, UserGroup group, bool AddDelete)
        {
            GroupAdd ga = new GroupAdd((UserGroup)group.GetCopy(), (User)user.GetCopy());
            ga.AddOrRemove = AddDelete;
            Action a = null;
            if (AddDelete)
            {
                //Add
                a = () => UserBLL.AddUserToGroup(ga.AddUser, ga.Group);
            }
            else
            {
                //delete
                if(group.GroupID != -1)
                a = () => UserBLL.DeleteUserFromGroup(ga.AddUser, ga.Group);
            }
            bool doNothing = false;
            foreach(var v in GroupActions)
            {
                if (v.Key.Equals(ga))
                {
                    if (v.Key.AddOrRemove == AddDelete)
                    {
                        doNothing = true;
                    }
                    break;
                }
            }
            Action ac;
            if (!doNothing)
            {
                GroupActions.TryRemove(ga, out ac);
                if(a!=null)
                if (AddDelete)
                {
                    GroupActions.TryAdd(ga, a);
                }
                else
                {
                    if(group.GroupID != -1 && user.UserID != -1)
                    {
                        GroupActions.TryAdd(ga, a);
                    }
                }
            }
            
        }

        public void CancelChanges(PermissionsBase pb)
        {
            PermissionsBase copy = pb.GetCopy();
            IEnumerable<GroupAdd> toRemove = null;
            if(pb is UserGroup)
            {
                toRemove = GroupActions.Keys.Where(x => x.Group.Equals(pb));
            }
            else if(pb is User)
            {
                toRemove = GroupActions.Keys.Where(x => x.AddUser.Equals(pb));
            }
            
            
            Action v;
            if(toRemove != null)
            foreach (GroupAdd a in toRemove)
            {
                GroupActions.TryRemove(a, out v);
            }
            DeleteFromDatabase.TryRemove(pb, out v);
        }

        public void CancelGrouptoUser(User u, UserGroup ug)
        {
            GroupAdd ga = new GroupAdd(ug.GetCopy() as UserGroup, u.GetCopy() as User);
            Action a;
            GroupActions.TryRemove(ga, out a);
        }

        public void RemoveActions(PermissionsBase pb)
        {
            Action actions;
            DeleteFromDatabase.TryRemove(pb , out actions);
        }

        public class GroupAdd
        {
            public UserGroup Group { get; set; }
            public User AddUser { get; set; } 
            public bool AddOrRemove { get; set; }
            public GroupAdd(UserGroup group, User addUser)
            {
                Group = group;
                AddUser = addUser;
            }
            public override bool Equals(object obj)
            {
                if(obj is GroupAdd)
                {
                    GroupAdd ga2 = obj as GroupAdd;
                    return ga2.AddUser.InstanceID.Equals(AddUser.InstanceID) && ga2.Group.InstanceID.Equals(Group.InstanceID);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return (Group.GetHashCode() + AddUser.GetHashCode());
            }
        }

    }
    
}
