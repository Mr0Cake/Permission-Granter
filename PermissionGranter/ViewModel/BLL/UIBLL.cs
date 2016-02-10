using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.BLL
{
    public class UIBLL
    {

        #region Select
        public static void InsertSubItems(CustTreeItems parent)
        {
            foreach(CustTreeItems cti in 
                DAL.DAL.ExecuteDataReader("S_Control_ByParentName",FillCustTreeItems,
                            DAL.DAL.Parameter("ParentName", parent.Name)))
            {
                cti.Parent = parent;
                if (cti.HasPermissions)
                {
                    AddPermissionsToControl(cti);
                }
                else
                {
                    InsertSubItems(cti);
                }
                parent.Items.Add(cti);
            }
        }

        public static void AddPermissionsToControl(CustTreeItems cti)
        {
            foreach(Permission p in DAL.DAL.ExecuteDataReader("S_Permissions_ByControlName", fillPermission,
                DAL.DAL.Parameter("ControlName", cti.Name)))
            {
                p.ParentItem = cti;
                cti.Options.Add(p);
            }
        }

        private static Permission fillPermission(IDataReader arg)
        {
            Permission p = new Permission();
            p.Name = arg.GetString(0);
            p.Description = arg.GetString(1);
            
            return p;
        }

        //Fill menuitems
        public static void GetAllItems(MenuItems menu)
        {
            //select all controls that have no parent
            foreach(CustTreeItems cti in DAL.DAL.ExecuteDataReader("S_AllTopControls_Name", FillCustTreeItems))
            {
                //if cti has permissions : add permissions
                if (cti.HasPermissions)
                {
                    AddPermissionsToControl(cti);
                }
                else
                {
                    //no permissions check if there are subItems
                    InsertSubItems(cti);
                }
                //finally add the toplevel item
                menu.Items.Add(cti);
            }
        }

        public static CustTreeItems FillCustTreeItems(System.Data.IDataReader r)
        {
            CustTreeItems cti = new CustTreeItems();
            cti.Name = r.GetString(0);
            cti.HasPermissions = Convert.ToBoolean(r.GetByte(1));
            return cti;
        }

        #endregion

        #region Update

        #endregion

        #region Insert
        #endregion

        #region Delete
        #endregion
    }
}
