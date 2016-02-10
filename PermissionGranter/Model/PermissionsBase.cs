using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public class PermissionsBase : INotifyPropertyChanged, CanCopy<PermissionsBase>
    {

        private Permissions _OwnedPermissions = new Permissions();

        /// <summary>
        /// Permissions die gezet zijn specifiek voor de gebruiker/groep
        /// </summary>
        public virtual Permissions OwnedPermissions
        {
            get { return _OwnedPermissions; }
            set
            {
                NotifyPropertyChanged("OwnedPermissions");
                _OwnedPermissions = value;
            }
        }

        private bool _Changed;

        public virtual bool Changed
        {
            get { return _Changed && OwnedPermissions.Changed; }
            set { _Changed = value; }
        }

        public Guid InstanceID { get; protected set; }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                Changed = true;
            }
        }

        public virtual PermissionsBase GetCopy()
        {
            PermissionsBase u = new PermissionsBase();
            u.OwnedPermissions = this.OwnedPermissions.GetCopy();
            u.InstanceID = this.InstanceID;
            return u;
        }

        public override bool Equals(object obj)
        {
            if (obj is PermissionsBase)
                return (obj as PermissionsBase).InstanceID.Equals(InstanceID);
            return false;
        }

        public override int GetHashCode()
        {
            return InstanceID.GetHashCode();
        }



        public event PropertyChangedEventHandler PropertyChanged;

    }
}
