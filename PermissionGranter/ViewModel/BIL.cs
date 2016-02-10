using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PermissionGranter.Model;
using PermissionGranter.ViewModel.DAL;

namespace PermissionGranter.ViewModel
{
    public class BIL
    {
    //    public static bool ComparePasswords(out User loggedUser, string password, string email)
    //    {
    //        //retrieve hash, salt, count
    //        //DAL.Parameter("Email", email)
    //        int errorCode;
    //        int returnvalue = 0;
    //        loggedUser = null;
    //        HashSaltCount hash;
    //        try
    //        {
    //            hash = DAL.DAL.ExecuteDataReader("S_User_PasswordHash", FillStruct, out errorCode,
    //            DAL.DAL.Parameter("Email", email), DAL.DAL.Parameter("@Returnvalue", returnvalue))[0];
    //        }
    //        catch (IndexOutOfRangeException)
    //        {
    //            return false;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }

    //        if (PasswordEncryption.PasswordMatch(password, hash.Hash, hash.Salt, hash.count)) 
    //        {
    //            //GetUser
    //            loggedUser = DAL.DAL.ExecuteDataReader("S_User", FillUser, out errorCode, DAL.DAL.Parameter("Email", email))[0];
                
    //            //UserPermissions
    //            IList<UserControlPermission> controlPermissions = DAL.DAL.ExecuteDataReader("S_User_ControlPermissions", FillUserControlPermission, out errorCode,
    //                DAL.DAL.Parameter("UserID", loggedUser.UserID));
    //            //Executable actions
    //            IList<UserPermission> permissions = DAL.DAL.ExecuteDataReader("S_User_Permissions", FillUserPermission, out errorCode,
    //                DAL.DAL.Parameter("UserID", loggedUser.UserID));

    //            if (permissions.Count > 0)
    //            {
    //                //Permission heeft altijd een control, zonder permissie geen access en geen notie
    //                //getpermissions true = allow false = deny
    //                foreach (UserControlPermission perm in controlPermissions)
    //                    loggedUser.OwnedPermissions.addPermission(perm.Control, perm.AccessValue);
    //                foreach (UserPermission perm in permissions)
    //                    loggedUser.OwnedPermissions.addPermission(perm.Control, perm.AccessValue, perm.Permission);

    //                //getGroups
    //            }
    //            loggedUser.Email = email;
                
    //            return true;
    //        }
    //        return false;
    //    }

    //    public static User GetUserByMail(string email)
    //    {
    //        int errorCode;
    //        User u = DAL.DAL.ExecuteDataReader("S_User", FillUser, out errorCode, DAL.DAL.Parameter("Email", email))[0];
    //        u.OwnedPermissions = GetPermissionsByUserID(u.UserID);
    //        u.UserGroupPermissions = GetGroupsByUserID(u.UserID);
    //        return u;
    //    }

    //    public static Permissions GetPermissionsByUserID(int id)
    //    {
    //        Permissions perms = new Permissions();
    //        int errorCode;
    //        IList<UserControlPermission> controlPermissions = DAL.DAL.ExecuteDataReader("S_User_ControlPermissions", FillUserControlPermission, out errorCode,
    //                DAL.DAL.Parameter("UserID", id));
    //        //Executable actions
    //        IList<UserPermission> permissions = DAL.DAL.ExecuteDataReader("S_User_Permissions", FillUserPermission, out errorCode,
    //            DAL.DAL.Parameter("UserID", id));
    //        if (permissions.Count > 0)
    //        {
    //            //Permission heeft altijd een control, zonder permissie geen access en geen notie
    //            //getpermissions true = allow false = deny
    //            foreach (UserControlPermission perm in controlPermissions)
    //                perms.addPermission(perm.Control, perm.AccessValue);
    //            foreach (UserPermission perm in permissions)
    //                perms.addPermission(perm.Control, perm.AccessValue, perm.Permission);
    //        }
    //        return perms;
    //    }

    //    public static List<UserGroup> GetGroupsByUserID(int id)
    //    {
    //        IList<UserGroup> groups = DAL.DAL.ExecuteDataReader("S_UserGroupsByUserID", FillGroup, DAL.DAL.Parameter("UserID", id));
    //        foreach(UserGroup ug in groups)
    //        {
    //            ug.OwnedPermissions = GetPermissionsByUserID(ug.DummyUser);
    //        }
    //        return groups.ToList();
    //    }
        

    //    #region FillMethods

    //    public struct UserControlPermission
    //    {
    //        public string Control;
    //        public bool AccessValue;
    //    }

    //    public struct UserPermission
    //    {
    //        public string Control;
    //        public bool AccessValue;
    //        public string Permission;
    //    }



    //    public struct HashSaltCount
    //    {
    //        public string Hash;
    //        public string Salt;
    //        public int count;
    //    }

    //    public static HashSaltCount FillStruct(IDataReader sq)
    //    {
    //        HashSaltCount hsc = new HashSaltCount();
    //        hsc.Hash = sq.GetString(3);
    //        hsc.Salt = sq.GetString(4);
    //        hsc.count = sq.GetInt32(5);
    //        return hsc;
    //    }

    //    public static UserGroup FillGroup(IDataReader arg)
    //    {
    //        UserGroup ug = new UserGroup();
    //        //UserGroup.GroupID, GroupName, UserGroup.WinUserID, Description
    //        ug.GroupID = arg.GetInt32(0);
    //        ug.GroupName = arg.GetString(1);
    //        ug.DummyUser = arg.GetInt32(2);
    //        ug.Description = arg.GetString(3);
    //        return ug;
    //    }

    //    public static UserControlPermission FillUserControlPermission(IDataReader arg)
    //    {
    //        UserControlPermission ucp = new UserControlPermission();
    //        ucp.Control = arg.GetString(0);
    //        ucp.AccessValue = arg.GetBoolean(1);

    //        return ucp;
    //    }

    //    public static User FillUser(IDataReader sq)
    //    {
    //        User u = new User();
    //        u.UserID = sq.GetInt32(0);
    //        u.FirstName = sq.GetString(1);
    //        u.LastName = sq.GetString(2);
    //        return u;
    //    }

    //    public static UserPermission FillUserPermission(IDataReader sq)
    //    {
    //        UserPermission perm = new UserPermission();
    //        perm.Control = sq.GetString(2);
    //        perm.AccessValue = sq.GetBoolean(3);
    //        perm.Permission = sq.GetString(4);
    //        return perm;
    //    }
    //    #endregion
    }
}
