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
        public static List<UserGroup> GetGroupsByUserID(int id)
        {
            IList<UserGroup> groups = DAL.DAL.ExecuteDataReader("S_UserGroupsByUserID", FillGroup, DAL.DAL.Parameter("UserID", id));
            foreach (UserGroup ug in groups)
            {
                ug.OwnedPermissions = PermissionsBLL.GetPermissionsByUserID(ug.DummyUser);
            }
            return groups.ToList();
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

                UserBLL.PermissionsListToDatabase(dummyUserID, u.OwnedPermissions.AllowPermissions, true);
                UserBLL.PermissionsListToDatabase(dummyUserID, u.OwnedPermissions.DenyPermissions, false);

                if (ID != DBNull.Value && ID != null)
                {
                    u.GroupID = (int)ID;
                }
                else
                {
                    throw new Exception("GroupID is null");
                }
            }

        }

        public static void EditUserGroup(UserGroup u)
        {
            DAL.DAL.ExecuteNonQuery("U_UserGroup",
                                    DAL.DAL.Parameter("GroupID", u.GroupID), DAL.DAL.Parameter("GroupName", u.GroupName),
                                    DAL.DAL.Parameter("UserID", u.DummyUser), DAL.DAL.Parameter("Description", u.Description));
            PermissionsBLL.CompareChangesAndUpdate(u.OwnedPermissions, u);

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
            foreach(UserGroup g in groups)
            {
                g.OwnedPermissions = PermissionsBLL.GetPermissionsByUserID(g.DummyUser);
            }
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
