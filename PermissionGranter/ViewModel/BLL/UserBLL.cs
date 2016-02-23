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
                foreach(UserGroup u in GroupBLL.GetGroupsByUserID(userID))
                    user.UserGroupPermissions.Add(u);
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
                PermissionsBLL.AddPermissions(AddUser);
                //setgroups
                foreach (UserGroup ug in AddUser.UserGroupPermissions)
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
                DAL.DAL.ExecuteNonQuery("U_User",
                                DAL.DAL.Parameter("Email", u.Email),
                                DAL.DAL.Parameter("FirstName", u.FirstName),
                                DAL.DAL.Parameter("LastName", u.LastName),
                                DAL.DAL.Parameter("UserID", u.UserID)
                    );
                PermissionsBLL.ReplacePermissions(u);
                if (u.PasswordChanged)
                {
                    UpdatePassword(u);
                }
            }
            else
            {
                CreateUser(u);
            }
        }

        public static void UpdatePassword(User u)
        {
            if (u.UserID > -1)
            {
                DAL.DAL.ExecuteNonQuery("U_User_ChangePasswordHash",
                    DAL.DAL.Parameter("@UserID", u.ID),
                    DAL.DAL.Parameter("@Hash", u.Password),
                    DAL.DAL.Parameter("@Salt", u.Salt),
                    DAL.DAL.Parameter("@IterationCount", 10));
            }
        }

        private static List<SqlParameter> GetUserParameters(User addUser)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                            {
                                DAL.DAL.Parameter("Email", addUser.Email),
                                DAL.DAL.Parameter("FirstName", addUser.FirstName),
                                DAL.DAL.Parameter("LastName", addUser.LastName),
                                DAL.DAL.Parameter("Hash", addUser.Password),
                                DAL.DAL.Parameter("Salt", addUser.Salt),
                                DAL.DAL.Parameter("IterationCount", 10)
                            };

            return parameters;
        }

        public static bool ComparePasswords(out User loggedUser, string password, string email)
        {
            //retrieve hash, salt, count
            //DAL.Parameter("Email", email)
            int errorCode;
            loggedUser = null;
            User hash;
            try
            {
                hash = DAL.DAL.ExecuteDataReader("S_User_PasswordHash", FillUser, out errorCode,
                DAL.DAL.Parameter("Email", email)).First();
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            if(PasswordEncryption.PasswordMatch(password, hash.Password, hash.Salt, 10))
            {
                loggedUser = hash;
                return true;
            }
            
            return false;
        }
        

        /// <summary>
        /// Send a permission dictionary of a user to the database
        /// In case of a deny permission: no permissions -> block/allow the control
        ///                                 permissions > 0 -> block/allow only the permissions
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
                SqlParameter perm;
                if (e.Value != null && e.Value.Count > 0)
                {
                    foreach (string permission in e.Value)
                    {
                        perm = DAL.DAL.Parameter("Permission", permission);
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
            if(u.ID > -1)
            foreach (UserGroup ug in GroupBLL.GetGroupsByUserID(u.ID))
                u.UserGroupPermissions.Add(ug);
            return u;
        }


        public static User FillUser(IDataReader sq)
        {
            User u = new User();
            u.UserID = sq.GetInt32(0);
            u.FirstName = sq.IsDBNull(1) ? "" : sq.GetString(1);
            u.LastName = sq.IsDBNull(2) ? "" : sq.GetString(2);
            u.Email = sq.IsDBNull(3) ? "" : sq.GetString(3);
            u.SetPassword(sq.IsDBNull(4) ? "" : sq.GetString(4));
            u.Salt = sq.IsDBNull(5) ? "" : sq.GetString(5);
            return u;
        }

        public static List<User> AllUsers()
        {
            IList<User> users =  DAL.DAL.ExecuteDataReader("S_AllUsers", FillUser).ToList();
            users.ToList().ForEach(x => x.OwnedPermissions=PermissionsBLL.GetPermissionsByUserID(x.ID));
            return users.ToList();
        }
    }
}
