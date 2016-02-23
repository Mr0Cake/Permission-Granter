using PermissionGranter.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.BLL
{
    /// <summary>
    /// The purpose of this class is to save all actions to the database, and execute them when the user wants to save.
    /// Also when the user cancels their actions you wont need to go back to the database.
    /// </summary>
    public class DelayedDatabaseActions
    {
        //public ConcurrentDictionary<PermissionsBase, Action> DeleteFromDatabase = new ConcurrentDictionary<PermissionsBase, Action>();
        //public ConcurrentDictionary<GroupAdd, Action> GroupActions = new ConcurrentDictionary<GroupAdd, Action>();
        //public ConcurrentDictionary<PermissionsBase, Action> PermissionsActions = new ConcurrentDictionary<PermissionsBase, Action>();

        public HashSet<PermissionsBase> DeleteObjects = new HashSet<PermissionsBase>();
        public HashSet<PermissionsBase> AddObjects = new HashSet<PermissionsBase>();
        public HashSet<GroupAction> GroupActions = new HashSet<GroupAction>();
        public Dictionary<User, List<string>> NotificationMail = new Dictionary<User, List<string>>();

        public DelayedDatabaseActions()
        {

        }

        
        public void AddMessage(User u, string message)
        {
            List<string> messages;
            if(!NotificationMail.TryGetValue(u, out messages))
            {
                NotificationMail.Add(u, new List<string> { message });
            }
            else
            {
                NotificationMail[u].Add(message);
            }
        }

        //public Dictionary<string, HashSet<string>> getChanges(Dictionary<string, HashSet<string>> permissions1, Dictionary<string, HashSet<string>> permissions2)
        //{
        //    Dictionary<string, HashSet<string>> output = new Dictionary<string, HashSet<string>>();
        //    if(permissions1.Count == 0 && permissions2.Count > 0)
        //    {
        //        return permissions2;
        //    }
        //    if(permissions2.Count == 0 && permissions1.Count > 0)
        //    {
        //        return permissions1;
        //    }


        //    return output;
        //}

        /// <summary>
        /// Using a list of current items add or delete items
        /// </summary>
        /// <param name="currentItems"></param>
        public void Execute(List<PermissionsBase> currentItems)
        {
            DeleteObjects.ToList().ForEach(x =>
            {
                    
                if (x.ID > -1)
                {
                    if(x is UserGroup)
                    {
                        GroupBLL.DeleteUserGroup(x as UserGroup);
                    }
                    else
                    {
                        UserBLL.DeleteUser(x as User);
                    }
                }
            });
            if(currentItems.Count>0)
            AddObjects.Where(x => currentItems.Contains(x)).ToList().ForEach(addobj =>
            {
                int index = currentItems.IndexOf(addobj);
                if(currentItems[index].ID > -1)
                if (addobj is UserGroup)
                {
                    GroupBLL.EditUserGroup(currentItems[index] as UserGroup);
                }
                else
                {
                    UserBLL.UpdateUser(currentItems[index] as User);
                        AddMessage(currentItems[index] as User, "Welkom " + (currentItems[index] as User).FirstName);
                }
            });

            if (currentItems.Count > 0)
            {
                GroupActions.ToList().ForEach(action =>
                {
                    if(currentItems.First() is UserGroup)
                    {
                        UserGroup ug = currentItems[currentItems.IndexOf(action.Group)] as UserGroup;
                        if(ug.ID > -1 && action.AddUser.ID > -1 )
                        {
                            StringBuilder sb = new StringBuilder();
                            if (action.AddOrRemove)
                            {
                                UserBLL.AddUserToGroup(action.AddUser, ug);
                                ChangesToGroup(action.AddUser, ug, action.AddOrRemove);
                            }
                            else
                            {
                                UserBLL.DeleteUserFromGroup(action.AddUser, ug);
                                ChangesToGroup(action.AddUser, ug, action.AddOrRemove);
                            }
                        }
                    }
                    else
                    {
                        User ug = currentItems[currentItems.IndexOf(action.AddUser)] as User;
                        if (ug.ID > -1 && action.Group.ID > -1)
                        {
                            if (action.AddOrRemove)
                            {
                                UserBLL.AddUserToGroup(ug, action.Group);
                                ChangesToGroup(ug, action.Group, action.AddOrRemove);
                            }
                            else
                            {
                                UserBLL.DeleteUserFromGroup(ug, action.Group);
                                //UserBLL.DeleteUserFromGroup(action.AddUser, ug);
                                ChangesToGroup(ug, action.Group, action.AddOrRemove);
                            }
                        }
                    }
                });
            }
                
        }

        private string getDel(bool deleted)
        {
            return deleted ? "verwijderd" : "toegevoegd";
        }

        public void ChangesToGroup(User AddUser, UserGroup ug, bool deleted)
        {
            StringBuilder sb = new StringBuilder();
            string del = getDel(deleted);
            sb.Append("U bent "+del+" aan de groep: ").Append(ug.Name).Append(Environment.NewLine);
            sb.Append("Volgende permissies zijn gewijzigd: \r\n");
            sb.Append("AllowPermissies: \r\n");
            ug.OwnedPermissions.AllowPermissions.ToList().ForEach(x =>
            {
                if (AddUser.OwnedPermissions.AllowPermissions.ContainsKey(x.Key))
                {
                    sb.Append(x.Key).Append(" Gewijzigd");
                    x.Value.Where(p => !AddUser.OwnedPermissions.AllowPermissions[x.Key].Contains(p)).ToList().ForEach(z => sb.AppendLine("\t").Append(z).Append(" "+ getDel(!deleted)));
                    AddUser.OwnedPermissions.AllowPermissions[x.Key].Where(p => !x.Value.Contains(p)).ToList().ForEach(z => sb.AppendLine("\t").Append(z).Append(" "+ getDel(!deleted)));

                }
                else
                {
                    sb.Append(x.Key).Append(del);
                    x.Value.ToList().ForEach(p => sb.AppendLine("\t").Append(p));
                }
            });
            sb.Append("DenyPermissies: \r\n");
            ug.OwnedPermissions.DenyPermissions.ToList().ForEach(x =>
            {
                if (AddUser.OwnedPermissions.DenyPermissions.ContainsKey(x.Key))
                {
                    sb.Append(x.Key).Append(" Gewijzigd");
                    x.Value.Where(p => !AddUser.OwnedPermissions.DenyPermissions[x.Key].Contains(p)).ToList().ForEach(z => sb.AppendLine("\t").Append(z).Append(" " + getDel(!deleted)));
                    AddUser.OwnedPermissions.DenyPermissions[x.Key].Where(p => !x.Value.Contains(p)).ToList().ForEach(z => sb.AppendLine("\t").Append(z).Append(" " + getDel(!deleted)));

                }
                else
                {
                    sb.Append(x.Key).Append(del);
                    x.Value.ToList().ForEach(p => sb.AppendLine("\t").Append(p));
                }
            });
            AddMessage(AddUser, sb.ToString());
        }

        public void ChangesToPermission(User OldUser, User NewUser)
        {
            StringBuilder sb = new StringBuilder();
            if(OldUser.OwnedPermissions.AllowPermissions.Count == 0 && NewUser.OwnedPermissions.AllowPermissions.Count > 0)
            {
                sb.Append("Volgende allow permissies zijn toegevoegd:\r\n");
                foreach(var v in NewUser.OwnedPermissions.AllowPermissions)
                {
                    sb.AppendLine(v.Key);
                    foreach(string a in NewUser.OwnedPermissions.AllowPermissions[v.Key])
                    {
                        sb.AppendLine("\t" + a);
                    }
                }
            }
            sb.AppendLine();
            if (OldUser.OwnedPermissions.AllowPermissions.Count > 0 && NewUser.OwnedPermissions.AllowPermissions.Count == 0)
            {
                sb.Append("Volgende allow permissies zijn verwijderd:\r\n");
                foreach (var v in NewUser.OwnedPermissions.AllowPermissions)
                {
                    sb.AppendLine(v.Key);
                    foreach (string a in NewUser.OwnedPermissions.AllowPermissions[v.Key])
                    {
                        sb.AppendLine("\t" + a);
                    }
                }
            }
            sb.AppendLine();
            if (OldUser.OwnedPermissions.DenyPermissions.Count == 0 && NewUser.OwnedPermissions.DenyPermissions.Count > 0)
            {
                sb.Append("Volgende deny permissies zijn toegevoegd:\r\n");
                foreach (var v in NewUser.OwnedPermissions.AllowPermissions)
                {
                    sb.AppendLine(v.Key);
                    foreach (string a in NewUser.OwnedPermissions.AllowPermissions[v.Key])
                    {
                        sb.AppendLine("\t" + a);
                    }
                }
            }
            sb.AppendLine();
            if (OldUser.OwnedPermissions.DenyPermissions.Count > 0 && NewUser.OwnedPermissions.DenyPermissions.Count == 0)
            {
                sb.Append("Volgende deny permissies zijn verwijderd:\r\n");
                foreach (var v in NewUser.OwnedPermissions.AllowPermissions)
                {
                    sb.AppendLine(v.Key);
                    foreach (string a in NewUser.OwnedPermissions.AllowPermissions[v.Key])
                    {
                        sb.AppendLine("\t" + a);
                    }
                }
            }
            sb.AppendLine();

            sb.AppendLine("AllowPermissions");
            if (OldUser.OwnedPermissions.AllowPermissions.Count > 0 && NewUser.OwnedPermissions.AllowPermissions.Count > 0)
            {
                if(OldUser != NewUser)
                {
                    if(OldUser.OwnedPermissions.AllowPermissions.Count > 0 && NewUser.OwnedPermissions.AllowPermissions.Count > 0)
                    {
                        StringBuilder gewijzigd = new StringBuilder();
                        OldUser.OwnedPermissions.AllowPermissions.ToList().ForEach(x =>
                        {
                            if (NewUser.OwnedPermissions.AllowPermissions.ContainsKey(x.Key))
                            {
                                gewijzigd.Append(x.Key).Append(" Gewijzigd\r\n");
                                x.Value.Where(p => !NewUser.OwnedPermissions.AllowPermissions[x.Key].Contains(p)).ToList().ForEach(z => gewijzigd.AppendLine("\t").Append(z).Append(" verwijderd"));
                                NewUser.OwnedPermissions.AllowPermissions[x.Key].Where(p => !x.Value.Contains(p)).ToList().ForEach(z => gewijzigd.AppendLine("\t").Append(z).Append(" toegevoegd"));

                            }
                            else
                            {
                                sb.Append(x.Key).Append(" Verwijderd\r\n");
                                x.Value.ToList().ForEach(p => sb.AppendLine("\t").Append(p));
                            }
                        });
                        NewUser.OwnedPermissions.AllowPermissions.ToList().ForEach(x =>
                        {
                            if (OldUser.OwnedPermissions.AllowPermissions.ContainsKey(x.Key))
                            {
                                gewijzigd.Append(x.Key).Append(" Gewijzigd\r\n");
                                x.Value.Where(p => !OldUser.OwnedPermissions.AllowPermissions[x.Key].Contains(p)).ToList().ForEach(z => gewijzigd.AppendLine("\t").Append(z).Append(" verwijderd"));
                                OldUser.OwnedPermissions.AllowPermissions[x.Key].Where(p => !x.Value.Contains(p)).ToList().ForEach(z => gewijzigd.AppendLine("\t").Append(z).Append(" toegevoegd"));

                            }
                            else
                            {
                                sb.Append(x.Key).Append(" Toegevoegd\r\n");
                                x.Value.ToList().ForEach(p => sb.AppendLine("\t").Append(p));
                            }
                        });
                        sb.AppendLine().Append(gewijzigd);
                    }
                }
            }
            sb.AppendLine();
            sb.AppendLine("DenyPermissions");

            if (OldUser.OwnedPermissions.DenyPermissions.Count > 0 && NewUser.OwnedPermissions.DenyPermissions.Count > 0)
            {
                if (OldUser != NewUser)
                {
                    if (OldUser.OwnedPermissions.DenyPermissions.Count > 0 && NewUser.OwnedPermissions.DenyPermissions.Count > 0)
                    {
                        StringBuilder gewijzigd = new StringBuilder();
                        OldUser.OwnedPermissions.DenyPermissions.ToList().ForEach(x =>
                        {
                            if (NewUser.OwnedPermissions.DenyPermissions.ContainsKey(x.Key))
                            {
                                gewijzigd.Append(x.Key).Append(" Gewijzigd\r\n");
                                x.Value.Where(p => !NewUser.OwnedPermissions.DenyPermissions[x.Key].Contains(p)).ToList().ForEach(z => gewijzigd.AppendLine("\t").Append(z).Append(" verwijderd"));
                                NewUser.OwnedPermissions.DenyPermissions[x.Key].Where(p => !x.Value.Contains(p)).ToList().ForEach(z => gewijzigd.AppendLine("\t").Append(z).Append(" toegevoegd"));

                            }
                            else
                            {
                                sb.Append(x.Key).Append(" Verwijderd\r\n");
                                x.Value.ToList().ForEach(p => sb.AppendLine("\t").Append(p));
                            }
                        });
                        NewUser.OwnedPermissions.DenyPermissions.ToList().ForEach(x =>
                        {
                            if (OldUser.OwnedPermissions.DenyPermissions.ContainsKey(x.Key))
                            {
                                gewijzigd.Append(x.Key).Append(" Gewijzigd\r\n");
                                x.Value.Where(p => !OldUser.OwnedPermissions.DenyPermissions[x.Key].Contains(p)).ToList().ForEach(z => gewijzigd.AppendLine("\t").Append(z).Append(" verwijderd"));
                                OldUser.OwnedPermissions.DenyPermissions[x.Key].Where(p => !x.Value.Contains(p)).ToList().ForEach(z => gewijzigd.AppendLine("\t").Append(z).Append(" toegevoegd"));

                            }
                            else
                            {
                                sb.Append(x.Key).Append(" Toegevoegd\r\n");
                                x.Value.ToList().ForEach(p => sb.AppendLine("\t").Append(p));
                            }
                        });
                        sb.AppendLine().Append(gewijzigd);
                    }
                }
            }
            sb.AppendLine();

            AddMessage(NewUser, sb.ToString());
        }

        //public int getID(List<PermissionsBase> currentItems, PermissionsBase oldItem)
        //{

        //}

        /// <summary>
        /// Delete and add the objects
        /// does nothing if ID is not set
        /// </summary>
        public void Execute()
        {
            DeleteObjects.ToList().ForEach(x =>
            {
                if (x.ID > -1)
                {
                    if (x is UserGroup)
                    {
                        GroupBLL.DeleteUserGroup(x as UserGroup);
                    }
                    else
                    {
                        UserBLL.DeleteUser(x as User);
                    }
                }
            });
            AddObjects.ToList().ForEach(addobj =>
            {
                if (addobj.ID > -1)
                    if (addobj is UserGroup)
                    {
                        GroupBLL.EditUserGroup(addobj as UserGroup);
                    }
                    else
                    {
                        UserBLL.UpdateUser(addobj as User);
                    }
            });
        }

        public void Cancel()
        {
            DeleteObjects.Clear();
            AddObjects.Clear();
            GroupActions.Clear();
        }

        public void Cancel(PermissionsBase pb)
        {
            DeleteObjects.Remove(pb);
            AddObjects.Remove(pb);
            if(pb is UserGroup)
            {
                GroupActions.RemoveWhere(x => x.Group.Equals(pb));
            }
            else
            {
                GroupActions.RemoveWhere(x => x.AddUser.Equals(pb));
            }
        }

        public void AddPermissionsBase(PermissionsBase obj)
        {
            PermissionsBase pb = obj.GetCopy();
            AddObjects.Add(pb);
            DeleteObjects.Remove(pb);
        }

        public void DeletePermissionsBase(PermissionsBase obj)
        {
            PermissionsBase pb = obj.GetCopy();
            AddObjects.Remove(pb);
            DeleteObjects.Add(obj);
        }

        public void removeUserGroupAction(PermissionsBase pb)
        {
            if(pb is User)
            {
                GroupActions.RemoveWhere(x => x.AddUser.Equals(pb));
            }
            else
            {
                GroupActions.RemoveWhere(x => x.Group.Equals(pb));
            }
        }

        public void removeUserGroupAction(User u, UserGroup ug)
        {
            GroupAction ga = new GroupAction(ug, u);
            GroupActions.Remove(ga);
        }


        /// <summary>
        /// Deletes or adds the user "user" from/to UserGroup "group"
        /// </summary>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <param name="AddDelete">true = AddUser, false = RemoveUser</param>
        public void UserGroupDelayedAction(User user, UserGroup group, bool AddDelete)
        {
            GroupAction ga = new GroupAction((UserGroup)group.GetCopy(), (User)user.GetCopy());
            ga.AddOrRemove = AddDelete;

            if (AddDelete)
            {
                //Add
                GroupActions.Add(ga);
            }
            else
            {
                //delete
                GroupActions.Remove(ga);
                GroupActions.Add(ga);
            }
        }


        public void RemoveActions(PermissionsBase pb)
        {
            DeleteObjects.Add(pb);
        }

        public class GroupAction
        {
            public UserGroup Group { get; set; }
            public User AddUser { get; set; } 
            public bool AddOrRemove { get; set; }
            public GroupAction(UserGroup group, User addUser)
            {
                Group = group;
                AddUser = addUser;
            }
            public override bool Equals(object obj)
            {
                if(obj is GroupAction)
                {
                    GroupAction ga2 = obj as GroupAction;
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
