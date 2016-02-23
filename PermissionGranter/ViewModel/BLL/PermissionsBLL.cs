using PermissionGranter.Model;
using PermissionGranter.ViewModel.BLL.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionGranter.ViewModel.DAL;
using PermissionGranter.ViewModel.Utility;
using PermissionGranter.ViewModel.Extensions;

namespace PermissionGranter.ViewModel.BLL
{
    public class PermissionsBLL
    {
        

        private static void comparePermissions(int currentID, Dictionary<string, HashSet<string>> oldPermissions, Dictionary<string, HashSet<string>> newPermissions, List<Task> tasklist, bool AccessValue)
        {
            if (!oldPermissions.Equals(newPermissions))
            {
                UserControlPermission ucp = new UserControlPermission();
                UserPermission up = new UserPermission();
                HashSet<string> deletedControls = new HashSet<string>();
                //added permissions
                foreach(var delPerms in oldPermissions)
                {
                    if (!newPermissions.Keys.Contains(delPerms.Key))
                    {
                        deletedControls.Add(delPerms.Key);
                        tasklist.Add(Task.Factory.StartNew(() => DeleteControlAccess(currentID, delPerms.Key)));
                    }
                }
                foreach (var permission in newPermissions)
                {
                    if (!deletedControls.Contains(permission.Key) && oldPermissions.ContainsKey(permission.Key))
                    {
                        //delete all permissions from control
                        if (!(permission.Value.Count > 0))
                            tasklist.Add(Task.Factory.StartNew(() => DeleteControlPermissions(currentID, permission.Key)));

                        HashSet<string> DeletePermissions = new HashSet<string>();
                        HashSet<string> AddPermissions = oldPermissions[permission.Key];

                        if (AddPermissions != null)
                        {
                            DeletePermissions.AddRange(oldPermissions[permission.Key]);
                            foreach (string s in permission.Value)
                            {
                                if (!DeletePermissions.Remove(s))
                                {
                                    up.Control = permission.Key;
                                    up.Permission = s;
                                    up.AccessValue = AccessValue;
                                    //add permission
                                    tasklist.Add(Task.Factory.StartNew(() => SetPermissionByUserID(currentID, up)));
                                }
                            }
                        }
                        else { throw new Exception("addpermission = null"); }
                        if (DeletePermissions.Count > 0)
                        {
                            foreach (string s in DeletePermissions)
                            {
                                up.Control = permission.Key;
                                up.Permission = s;
                                tasklist.Add(Task.Factory.StartNew(() => DeleteControlPermissionAccess(currentID, up)));
                            }
                        }
                    }
                    else
                    {
                        ucp.Control = permission.Key;
                        ucp.AccessValue = true;
                        tasklist.Add(Task.Factory.StartNew(() => SetPermissionByUserID(currentID, ucp)));
                        
                        foreach (string s in permission.Value)
                        {
                            up.AccessValue = AccessValue;
                            up.Control = permission.Key;
                            up.Permission = s;
                            tasklist.Add(Task.Factory.StartNew(() => SetPermissionByUserID(currentID, ucp)));
                        }
                    }

                }
            }
        }


        public static void ReplacePermissions(PermissionsBase pb)
        {
            DeletePermissions(pb);
            AddPermissions(pb);
        }

        public static void DeletePermissions(PermissionsBase pb)
        {
            int userid = -1;
            if (pb is User)
                userid = (pb as User).UserID;
            if (pb is UserGroup)
                userid = (pb as UserGroup).DummyUser;
            if(userid != -1)
            {
                DAL.DAL.ExecuteNonQuery("D_AllPermissions_ByUserID", userid.SetParameter("UserID"));
            }
        }

        public static void AddPermissions(PermissionsBase pb)
        {
            if (pb.ID != -1)
            {

                AddPermissionsFromList(pb.ID, pb.OwnedPermissions.AllowPermissions, true);
                AddPermissionsFromList(pb.ID, pb.OwnedPermissions.DenyPermissions, false);
            }
        }

        public static void AddPermissionsFromList(int userid, Dictionary<string, HashSet<string>> perm, bool accessValue)
        {
            foreach (var z in perm)
            {
                if (z.Value == null || (z.Value != null && !(z.Value.Count > 0)))
                {
                    UserControlPermission ucp = new UserControlPermission();
                    ucp.AccessValue = accessValue;
                    ucp.Control = z.Key;
                    SetPermissionByUserID(userid, ucp);
                }

                if (z.Value != null && z.Value.Count > 0)
                {
                    foreach (string action in z.Value)
                    {
                        UserPermission up = new UserPermission();
                        up.AccessValue = accessValue;
                        up.Control = z.Key;
                        up.Permission = action;
                        SetPermissionByUserID(userid, up);
                    }
                }
            }
        }

        public static void CompareChangesAndUpdate (Permissions previous, PermissionsBase current)
        {
            List<Task> tList = new List<Task>();
            int userid = -1;
            if(current is User)
            {
                userid = (current as User).UserID;
            }else if (current is UserGroup)
            {
                userid = (current as UserGroup).DummyUser;
            }
            if (!previous.Equals(current.OwnedPermissions))
            {
                if (!previous.AllowPermissions.Equals(current.OwnedPermissions.AllowPermissions))
                {
                    comparePermissions(userid, previous.AllowPermissions, current.OwnedPermissions.AllowPermissions, tList, true);
                }
                if (!previous.DenyPermissions.Equals(current.OwnedPermissions.DenyPermissions))
                {
                    comparePermissions(userid, previous.DenyPermissions, current.OwnedPermissions.DenyPermissions, tList, false);
                }
                Task.WaitAll(tList.ToArray());
            }
            
        }


        #region Select
        //SELECT METHODS
        public static MenuItems GetTreeMenu()
        {
            MenuItems mu = new MenuItems();
            List<MenuItem> dbmi = new List<MenuItem>();
            Queue<Tuple<string,CustTreeItems>> toAddParent = new Queue<Tuple<string,CustTreeItems>>();
            
            dbmi.AddRange(DAL.DAL.ExecuteDataReader("S_AllControls_ControlName_PermissionName", FillMenuItem));
            
            List<CustTreeItems> menulist = new List<CustTreeItems>();
            Dictionary<string, CustTreeItems> itemsDictionary = new Dictionary<string, CustTreeItems>();
            Dictionary<string, CustTreeItems> addPermissions = new Dictionary<string, CustTreeItems>();

            foreach(var t in dbmi)
            {
                CustTreeItems c;
                if (!itemsDictionary.ContainsKey(t.Name))
                {
                    c = new CustTreeItems();
                    c.Name = t.Name;
                    c.HasAccess = false;
                }
                else
                {
                    c = itemsDictionary[t.Name];
                }
                if (!string.IsNullOrEmpty(t.PermissionName))
                {
                    c.HasPermissions = true;
                    c.Options.Add(new Permission(c, t.PermissionName, t.PermissionDescription, false));
                }
                if (!string.IsNullOrEmpty(t.ParentName))
                {
                    c.HasParent = true;
                    if (itemsDictionary.ContainsKey(t.ParentName))
                    {
                        CustTreeItems parent = itemsDictionary[t.ParentName];
                        c.Parent = parent;
                        int count = parent.Items.Where(x => x.Name == c.Name).Count();
                        if(count == 0)
                        parent.Items.Add(c);

                        itemsDictionary.TryAdd(t.Name, c);
                    }
                    else
                    {
                        toAddParent.Enqueue(Tuple.Create(t.ParentName,c));
                    }
                }
                else
                {
                    itemsDictionary.TryAdd(t.Name, c);
                }
                itemsDictionary.TryAdd(t.Name, c);
            }

            //niet zo efficient maar mijn vermoedelijke efficientere manier liet het afweten
            //als parent in itemsdictionary -> bind 
            //als parent niet in itemsdicionary -> enqueue en voer op het einde uit
            //in het beste scenario staat alles op volgorde, in het slechtste staat mijn parent steeds op het einde
            while(toAddParent.Count > 0)
            {
                Tuple<string, CustTreeItems> it = toAddParent.Dequeue();
                CustTreeItems child = it.Item2;
                if (itemsDictionary.ContainsKey(it.Item1))
                {
                    CustTreeItems parent = itemsDictionary[it.Item1];
                    child.Parent = parent;
                    int count = parent.Items.Where(x => x.Name == child.Name).Count();
                    if (count == 0)
                        parent.Items.Add(child);
                    itemsDictionary.TryAdd(it.Item1, child);
                }
                else
                {
                    toAddParent.Enqueue(it);
                }

            }


            //foreach(MenuItem menuitem in dbmi)
            //{
            //    CustTreeItems cti;
            //    if (itemsDictionary.ContainsKey(menuitem.Name))
            //    {
            //        cti = itemsDictionary[menuitem.Name] ?? new CustTreeItems(menuitem.Name);
            //    }
            //    else
            //    {
            //        cti = new CustTreeItems(menuitem.Name);
            //    }
            //    cti.HasAccess = false;
            //    if (!string.IsNullOrEmpty(menuitem.PermissionName))
            //    {
            //        cti.HasPermissions = true;
            //        cti.Options.Add(new Permission(cti, menuitem.PermissionName, menuitem.PermissionDescription, false));
            //    }
            //    if (cti.HasParent = !string.IsNullOrEmpty(menuitem.ParentName))
            //    {
            //        CustTreeItems parent;
            //        if (itemsDictionary.TryGetValue(menuitem.ParentName, out parent))
            //        {
            //            parent.Items.Add(cti);
            //            cti.Parent = parent;
            //        }
            //        else
            //        {
            //            toAddParent.Add( Tuple.Create(menuitem.ParentName, cti) );
            //        }
            //    }
            //    foreach(var tuple in toAddParent)
            //    {
            //        if(tuple.Item1 == menuitem.Name)
            //        {
            //            CustTreeItems child = tuple.Item2;
            //            child.Parent = cti;
            //            cti.Items.Add(child);
            //            break;
            //        }
            //    }
            //    itemsDictionary.TryAdd(menuitem.Name, cti);
            //}

            ////itemsDictionary.Values.Where(treeitem => !treeitem.HasParent).ToList().ForEach(x => mu.Items.Add(x));
            foreach (var v in itemsDictionary)
            {
                if (!v.Value.HasParent)
                {
                    mu.Items.Add(v.Value);
                }
            }
            return mu;
        }

        public static MenuItem FillMenuItem(IDataReader rdr)
        {
            MenuItem mi = new MenuItem
            {
                Name = rdr.GetString(0),
                ParentName = rdr.IsDBNull(1) ? "" : rdr.GetString(1),
                PermissionName = rdr.IsDBNull(2) ? "" : rdr.GetString(2),
                PermissionDescription = rdr.IsDBNull(3) ? "" : rdr.GetString(3),
                items = new List<MenuItem>()
            };
            
            return mi;
        }

        public static Permissions GetPermissionsByUserID(int id)
        {
            if (id > -1)
            {
                Permissions perms = new Permissions();
                int errorCode;
                IList<UserControlPermission> controlPermissions = DAL.DAL.ExecuteDataReader("S_User_ControlPermissions", FillUserControlPermission, out errorCode,
                        DAL.DAL.Parameter("UserID", id));
                //Executable actions
                IList<UserPermission> permissions = DAL.DAL.ExecuteDataReader("S_User_Permissions", FillUserPermission, out errorCode,
                    DAL.DAL.Parameter("UserID", id));
                if (permissions.Count > 0)
                {
                    //Permission heeft altijd een control, zonder permissie geen access en geen notie
                    //getpermissions true = allow false = deny
                    foreach (UserControlPermission perm in controlPermissions)
                        perms.addPermission(perm.Control, perm.AccessValue);
                    foreach (UserPermission perm in permissions)
                        perms.addPermission(perm.Control, perm.AccessValue, perm.Permission);
                }
                return perms;
            }
            else
            {
                throw new Exception("User ID nog niet ingesteld");
            }
        }
        #endregion
        

        #region Insert
        //INSERT METHODS
        public static void SetPermissionByUserID(int userID, UserPermission ucp)
        {
            byte Value = (byte)(ucp.AccessValue ? 1 : 0);
            DAL.DAL.ExecuteNonQuery("I_User_ControlPermission_ByControlName_ByPermissionName",
                DAL.DAL.Parameter("UserID", userID), 
                DAL.DAL.Parameter("ControlName", ucp.Control), 
                DAL.DAL.Parameter("PermissionName", ucp.Permission),
                DAL.DAL.Parameter("Value", Value)
            );
        }

        public static void SetPermissionByUserID(int userID, UserControlPermission ucp)
        {
            byte Value = (byte)(ucp.AccessValue ? 1 : 0);
            DAL.DAL.ExecuteNonQuery("I_User_ControlPermission_ByControlName",
                DAL.DAL.Parameter("UserID", userID),
                DAL.DAL.Parameter("ControlName", ucp.Control),
                DAL.DAL.Parameter("Value", Value)
            );
        }

        #endregion


        #region Delete
        //DELETE METHODS
        public static void DeleteControlAccess(int userID, string controlName)
        {
            DAL.DAL.ExecuteNonQuery("D_UserAccessPermission_ByUserID_ByControlName",
                                    DAL.DAL.Parameter("UserID", userID),
                                    DAL.DAL.Parameter("ControlName", controlName)
                                    );
        }

        public static void DeleteControlPermissions(int userID, string controlName)
        {
            DAL.DAL.ExecuteNonQuery("D_UserAccessPermission_ClearByControlName",
                                    DAL.DAL.Parameter("UserID", userID),
                                    DAL.DAL.Parameter("ControlName", controlName)
                                    );
        }

        public static void DeleteControlPermissionAccess(int userID, UserPermission ucp)
        {
            DAL.DAL.ExecuteNonQuery("D_UserAccessPermission_ByUserID_ByControlName_ByPermissionName",
                                    DAL.DAL.Parameter("UserID", userID),
                                    DAL.DAL.Parameter("ControlName", ucp.Control),
                                    DAL.DAL.Parameter("PermissionName", ucp.Permission)

                                    );
        }
        #endregion

        public static UserControlPermission FillUserControlPermission(IDataReader arg)
        {
            string output = "";
            output += arg.FieldCount + " ";
            

            UserControlPermission ucp = new UserControlPermission();
            ucp.Control = arg.GetString(0);
            ucp.AccessValue = arg.GetByte(1) == (byte)1 ? true : false;

            return ucp;
        }


        public static UserPermission FillUserPermission(IDataReader sq)
        {
            string output = sq.FieldCount.ToString();
            UserPermission perm = new UserPermission();
            perm.Control = sq.GetString(0);
            perm.AccessValue = sq.GetByte(1) == (byte)1 ? true : false;
            if (sq.FieldCount > 2)
            perm.Permission = sq.IsDBNull(2) ? "" : sq.GetString(2);
            return perm;
        }
    }
    
}
