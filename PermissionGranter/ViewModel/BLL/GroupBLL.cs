using PermissionGranter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionGranter.ViewModel.DAL;

namespace PermissionGranter.ViewModel.BLL
{
    public class GroupBLL
    {
        //public static List<UserGroup> GetGroupsByDummyUserID(int id)
        //{
        //    IList<UserGroup> groups = DAL.DAL.ExecuteDataReader("S_UserGroupsByUserID", FillGroup, DAL.DAL.Parameter("UserID", id));
        //    foreach (UserGroup ug in groups)
        //    {
        //        ug.OwnedPermissions = PermissionsBLL.GetPermissionsByUserID(ug.DummyUser);
        //    }
        //    return groups.ToList();
        //}

        public static List<UserGroup> GetGroupsByUserID(int id)
        {
            IList<UserGroup> groups = DAL.DAL.ExecuteDataReader("S_UserGroupsByUserID", FillGroup, DAL.DAL.Parameter("UserID", id));
            foreach (UserGroup ug in groups)
            {
                ug.OwnedPermissions = PermissionsBLL.GetPermissionsByUserID(ug.DummyUser);
            }
            return groups.ToList();
        }

        public static List<int> GetUsersByGroupID(int id)
        {
            IList<int> users = DAL.DAL.ExecuteDataReader("S_Users_ByGroupID", ((System.Data.IDataReader ir) => { return ir.GetInt32(0); }), DAL.DAL.Parameter("GroupID", id));
            return users.ToList();
        }

        public static void CreateUserGroup(UserGroup u)
        {
            if (u.GroupID == -1)
            {
                int returnval;
                object userID = DAL.DAL.ExecuteNonQuery("I_DummyUser", out returnval, DAL.DAL.Identity()).First();
                int dummyUserID = (int)userID;
                object ID = DAL.DAL.ExecuteNonQuery("I_UserGroup",
                                        DAL.DAL.Parameter("GroupName", u.GroupName),
                                        DAL.DAL.Parameter("UserID", dummyUserID), DAL.DAL.Parameter("Description", u.Description),
                                        DAL.DAL.Identity()).First();

                //fillpermissions
                
                //UserBLL.PermissionsListToDatabase(dummyUserID, u.OwnedPermissions.AllowPermissions, true);
                //UserBLL.PermissionsListToDatabase(dummyUserID, u.OwnedPermissions.DenyPermissions, false);

                if (ID != DBNull.Value && ID != null)
                {
                    u.GroupID = (int)ID;
                    u.DummyUser = dummyUserID;
                    PermissionsBLL.AddPermissions(u);
                }
                else
                {
                    throw new Exception("GroupID is null");
                }
            }

        }

        /// <summary>
        /// updates the usergroup u to the new values, creates new Usergroup if GroupID == -1
        /// Does not replace the permissions.
        /// </summary>
        /// <param name="u"></param>
        public static void EditUserGroup(UserGroup u)
        {
            if (u.GroupID != -1)
            {
                DAL.DAL.ExecuteNonQuery("U_UserGroup",
                                        DAL.DAL.Parameter("GroupID", u.GroupID), DAL.DAL.Parameter("GroupName", u.GroupName),
                                        DAL.DAL.Parameter("UserID", u.DummyUser), DAL.DAL.Parameter("Description", u.Description));
                PermissionsBLL.ReplacePermissions(u);
            }
            else
            {
                CreateUserGroup(u);
            }
        }

        private static List<UserGroup> _AllGroups;

        public static List<UserGroup> AllGroups
        {
            get
            {
                if(_AllGroups == null)
                {
                    _AllGroups = GetAllGroups();
                }
                return _AllGroups;
            }
            set { _AllGroups = value; }
        }


        public static List<UserGroup> GetAllGroups()
        {
            IList<UserGroup> groups = DAL.DAL.ExecuteDataReader("S_AllUserGroups", FillGroup);
            groups.ToList().ForEach(x => x.OwnedPermissions = PermissionsBLL.GetPermissionsByUserID(x.DummyUser));
            
            return groups.ToList();
        }

        public static void DeleteUserGroup(UserGroup u)
        {
            DAL.DAL.ExecuteNonQuery("D_Group_ByGroupID", DAL.DAL.Parameter("GroupID", u.GroupID), DAL.DAL.Parameter("UserID", u.DummyUser));
        }

        public static UserGroup FillGroup(System.Data.IDataReader arg)
        {
            UserGroup ug = new UserGroup();
            //UserGroup.GroupID, GroupName, UserGroup.WinUserID, Description
            ug.GroupID = arg.GetInt32(0);
            ug.GroupName = arg.GetString(1);
            ug.DummyUser = arg.IsDBNull(2) ? -1 : arg.GetInt32(2);
            ug.Description = arg.GetString(3);
            return ug;
        }
    }
}
