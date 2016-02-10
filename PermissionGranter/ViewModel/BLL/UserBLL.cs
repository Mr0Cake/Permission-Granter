using PermissionGranter.Model;
using PermissionGranter.ViewModel.BLL.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PermissionGranter.ViewModel.Extensions;

namespace PermissionGranter.ViewModel.BLL
{
    public class UserBLL
    {
        /*
            StoredProceduresName                                            Expects                                                             Returns
            I_User                                                          Email, FirstName, LastName, Function, Password                      @ReturnValue, Identity
            D_User                                                          UserID
            S_User_PasswordHash                                             Email                                                               @ReturnValue
            S_User                                                          Email                                                               @ReturnValue
            S_User_ControlPermissions                                       UserID                                                              @ReturnValue
            S_User_Permissions                                              UserID                                                              @ReturnValue
        */


        public static bool SelectUser(out User user, int userID)
        {
            user = null;
            try
            {
                int returnvalue;
                user = DAL.DAL.ExecuteDataReader("S_User",FillUser, out returnvalue,userID.SetParameter("UserID")).FirstOrDefault();
                user.OwnedPermissions = PermissionsBLL.GetPermissionsByUserID(userID);
                user.UserGroupPermissions = GroupBLL.GetGroupsByUserID(userID);
            }
            catch (Exception)
            {

            }
            return false;
        }

        

        public static void CreateUser(User AddUser)
        {
            try {
                int returnCode;
                List<SqlParameter> para = GetUserParameters(AddUser);
                para.Add(DAL.DAL.Identity());
                //CreateUser
                List<object> outputParams =
                DAL.DAL.ExecuteNonQuery("I_User", out returnCode, para.ToArray());

                int UserID = (int)outputParams[0];
                AddUser.UserID = UserID;
                //CreatePermissions
                // - allow: als een gebruiker allow permissies heeft dan worden parent controls automatisch aangevinkt.
                PermissionsListToDatabase(UserID, AddUser.OwnedPermissions.AllowPermissions, true);
                // - Deny: deny kan ook op een venster zijn, deny wordt niet doorgegeven aan de parent
                PermissionsListToDatabase(UserID, AddUser.OwnedPermissions.DenyPermissions, false);
                //setgroups
                foreach(UserGroup ug in AddUser.UserGroupPermissions)
                {
                    if (ug.GroupID > -1)
                    {
                        GroupBLL.CreateUserGroup(ug);
                    }
                    else
                    {
                        AddUserToGroup(AddUser, ug);
                    }

                }
                

                //Bind
            }catch(InvalidCastException ce)
            {
                throw new Exception("Database heeft geen UserID teruggegeven\r\n" + ce.Message);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static IList<User> SelectUsersByGroupName(string name)
        {
            return DAL.DAL.ExecuteDataReader("S_Users_ByGroupName", FillUser, name.SetParameter("GroupName"));
        }

        public static void DeleteUser(User u)
        {
            DAL.DAL.ExecuteNonQuery("D_User", DAL.DAL.Parameter("UserID", u.UserID));
        }

        public static void UpdateUser(User u)
        {
            if(u.UserID > -1)
            {
                DAL.DAL.ExecuteNonQuery("U_User", DAL.DAL.Parameter("UserID", u.UserID));
                PermissionsBLL.CompareChangesAndUpdate(u.OwnedPermissions, u);
            }
            else
            {
                CreateUser(u);
            }
        }

        private static List<SqlParameter> GetUserParameters(User addUser)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                            {
                                DAL.DAL.Parameter("Email", addUser.Email),
                                DAL.DAL.Parameter("FirstName", addUser.FirstName),
                                DAL.DAL.Parameter("LastName", addUser.LastName),
                                DAL.DAL.Parameter("Password", addUser.Password)
                            };

            return parameters;
        }

        public static bool ComparePasswords(out User loggedUser, string password, string email)
        {
            //retrieve hash, salt, count
            //DAL.Parameter("Email", email)
            int errorCode;
            int returnvalue = 0;
            loggedUser = null;
            HashSaltCount hash;
            try
            {
                hash = DAL.DAL.ExecuteDataReader("S_User_PasswordHash", FillStruct, out errorCode,
                DAL.DAL.Parameter("Email", email), DAL.DAL.Parameter("@Returnvalue", returnvalue))[0];
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            if (PasswordEncryption.PasswordMatch(password, hash.Hash, hash.Salt, hash.count))
            {
                //GetUser
                loggedUser = DAL.DAL.ExecuteDataReader("S_User_ByMail", FillUser, out errorCode, DAL.DAL.Parameter("Email", email))[0];

                //UserPermissions
                IList<UserControlPermission> controlPermissions = DAL.DAL.ExecuteDataReader("S_User_ControlPermissions", PermissionsBLL.FillUserControlPermission, out errorCode,
                    DAL.DAL.Parameter("UserID", loggedUser.UserID));
                //Executable actions
                IList<UserPermission> permissions = DAL.DAL.ExecuteDataReader("S_User_Permissions", PermissionsBLL.FillUserPermission, out errorCode,
                    DAL.DAL.Parameter("UserID", loggedUser.UserID));

                if (permissions.Count > 0)
                {
                    //Permission heeft altijd een control, zonder permissie geen access en geen notie
                    //getpermissions true = allow false = deny
                    foreach (UserControlPermission perm in controlPermissions)
                        loggedUser.OwnedPermissions.addPermission(perm.Control, perm.AccessValue);
                    foreach (UserPermission perm in permissions)
                        loggedUser.OwnedPermissions.addPermission(perm.Control, perm.AccessValue, perm.Permission);

                    //getGroups
                }
                loggedUser.Email = email;

                return true;
            }
            return false;
        }
        

        /// <summary>
        /// Send a permission dictionary of a user to the database
        /// In case of a deny permission: no permissions -> block the control
        ///                                 permissions > 0 -> block only the permissions
        /// </summary>
        /// <param name="UserID">int userid</param>
        /// <param name="inputPermissions">list of permissions</param>
        /// <param name="AllowOrDeny">true = Allow, false = Deny</param>
        public static void PermissionsListToDatabase(int UserID, Dictionary<string, HashSet<string>> inputPermissions, bool AllowOrDeny)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter user = DAL.DAL.Parameter("UserID", UserID);
            SqlParameter allowordeny = DAL.DAL.Parameter("Value", AllowOrDeny);
            //int errorCode = -1;
            foreach (var e in inputPermissions)
            {
                string controlName = e.Key;
                SqlParameter control = DAL.DAL.Parameter("ControlName", e.Key);
                DAL.DAL.ExecuteNonQuery("I_User_ControlPermission", user, control, allowordeny);
                if (e.Value.Count > 0 && AllowOrDeny)
                {
                    foreach (string permission in e.Value)
                    {
                        SqlParameter perm = DAL.DAL.Parameter("Permission", permission);
                        DAL.DAL.ExecuteNonQuery("I_User_Permission", user, control, perm, allowordeny);
                    }
                }
            }
        }

        public static void AddUserToGroup(User u, UserGroup g)
        {
            DAL.DAL.ExecuteNonQuery("I_GroupMember", DAL.DAL.Parameter("GroupID", g.GroupID), DAL.DAL.Parameter("UserID", u.UserID));
        }

        public static void DeleteUserFromGroup(User u, UserGroup g)
        {
            DAL.DAL.ExecuteNonQuery("D_GroupMember_ByGroupID_ByUserID", DAL.DAL.Parameter("GroupID", g.GroupID), DAL.DAL.Parameter("UserID", u.UserID));
        }

        public struct BLLUserGroup
        {
            public int GroupID;
            public int DummyUserID;
            public string GroupName;
            public string Description;
        }

        public static BLLUserGroup FillUserGroup(IDataReader dr)
        {
            BLLUserGroup ug = new BLLUserGroup();
            ug.GroupID = dr.GetInt32(0);
            ug.GroupName = dr.GetString(1);
            ug.DummyUserID = dr.GetInt32(2);
            ug.Description = dr.GetString(3);
            return ug;
        }

        public static UserGroup FillUserGroupPermissions(BLLUserGroup group)
        {
            UserGroup ug = new UserGroup();
            ug.GroupName = group.GroupName;
            ug.GroupID = group.GroupID;
            ug.Description = group.Description;
            ug.DummyUser = group.DummyUserID;
            User Dummy = null;
            if (SelectUser(out Dummy, group.DummyUserID))
            {
                ug.OwnedPermissions = Dummy.OwnedPermissions;
            }

            return ug;
        }

        public static bool SelectUserGroupByName(out UserGroup ug, string groupName)
        {
            ug = null;
            try {
                
                BLLUserGroup group =
                DAL.DAL.ExecuteDataReader("S_UserGroup_ByName", FillUserGroup, DAL.DAL.Parameter("GroupName", groupName)).FirstOrDefault();
                ug = FillUserGroupPermissions(group);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static void InsertUserGroup(UserGroup g)
        {
            //identity teruggeven!
            DAL.DAL.ExecuteNonQuery("I_UserGroup", DAL.DAL.Parameter("GroupName", g.GroupName), DAL.DAL.Parameter("Description", g.Description));
        }

        public static HashSaltCount FillStruct(IDataReader sq)
        {
            HashSaltCount hsc = new HashSaltCount();
            hsc.Hash = sq.GetString(3);
            hsc.Salt = sq.GetString(4);
            hsc.count = sq.GetInt32(5);
            return hsc;
        }

        public static User GetUserByMail(string email)
        {
            int errorCode;
            User u = DAL.DAL.ExecuteDataReader("S_User", FillUser, out errorCode, DAL.DAL.Parameter("Email", email))[0];
            u.OwnedPermissions = PermissionsBLL.GetPermissionsByUserID(u.UserID);
            u.UserGroupPermissions = GroupBLL.GetGroupsByUserID(u.UserID);
            return u;
        }


        public static User FillUser(IDataReader sq)
        {
            User u = new User();
            u.UserID = sq.GetInt32(0);
            u.FirstName = sq.IsDBNull(1) ? "" : sq.GetString(1);
            u.LastName = sq.IsDBNull(2) ? "" : sq.GetString(2);
            u.Password = sq.IsDBNull(3) ? "" : sq.GetString(3);

            return u;
        }

        public static List<User> AllUsers()
        {
            return DAL.DAL.ExecuteDataReader("S_AllUsers", FillUser).ToList();
        }
    }
}
