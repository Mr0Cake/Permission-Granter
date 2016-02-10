using PermissionGranter.ViewModel.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PermissionGranter.View;

namespace PermissionGranter.ViewModel
{
    public class CustTreeItems : INotifyPropertyChanged, IEnumerable
    {
        
        public ICommand Command
        {
            get; set;
        }

        private bool CanOpenWindow(object obj)
        {
            return HasAccess == true ? true : false;
        }

        private void OpenWindow(object obj)
        {
            List<string> options = new List<string>();
            if(Options.Count > 0)
            {
                foreach(var v in Options)
                {
                    if(v.Value == true)
                    options.Add(v.Name);
                }
            }
            MenuItemWindow miw = new MenuItemWindow(options.ToArray());
            miw.ShowDialog();
        }

        private bool _HasParent = false;

        public bool HasParent
        {
            get { return _HasParent; }
            set
            { 
                _HasParent = value;
                OnPropertyChanged("HasParent");
            }
        }

        public CustTreeItems Parent { get; set; }

        public CustTreeItems()
        {
            _Items = new ObservableCollection<CustTreeItems>();
            _Options = new ObservableCollection<Permission>();
            Command = new CustomCommand(OpenWindow, CanOpenWindow);
        }

        public CustTreeItems(string name) : this()
        {
            Name = name;
        }

        public CustTreeItems(string name, bool? hasAccess) : this(name)
        {
            HasAccess = hasAccess;
        }

        public CustTreeItems(bool hasParent, string name, bool? hasAccess) : this(name,hasAccess)
        {
            HasParent = hasParent;
            
        }

        public CustTreeItems(CustTreeItems Parent, bool hasParent, string name, bool? hasAccess) :this(hasParent, name, hasAccess)
        {
            this.Parent = Parent;
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
                
            }
        }

        /// <summary>
        /// Get a List of all the Items and subitems
        /// In this case:
        /// if this custTreeItem has items: Add this item and check children items
        /// if this custTreeItem doesn't have items: Add this item
        /// </summary>
        /// <returns></returns>
        public void GetChildren(ref List<CustTreeItems> tempList)
        {
            if(Items != null && Items.Count > 0)
            {
                tempList.Add(this);
                foreach(CustTreeItems cti in Items)
                {
                    cti.GetChildren(ref tempList);
                }
            }
            else
            {
                tempList.Add(this);
            }
        }

        //public bool AddToParent(string parentName)
        //{
        //    return false;
        //}

        public void setAccess(bool? value)
        {
            _HasAccess = value;
            OnPropertyChanged("HasAccess");
        }

        private bool? _HasAccess = false;

        public bool? HasAccess
        {
            get 
            {
                return _HasAccess; 
            }
            set
            {
                setChildItems(value);
                //if (value == true && HasParent && Parent != null)
                if (HasParent && Parent != null)
                    Parent.changeByChild(value);
                _HasAccess = value;  
                OnPropertyChanged("HasAccess");
                
            }
        }

        public void ClearItems()
        {
            HasAccess = false;
            if(Options.Count > 0)
            {
                Options.ToList().ForEach(perm => perm.Value = false);
            }
        }

        public void changeByChild(bool? value)
        {
            if (value == true)
            {
                _HasAccess = value;
                if (Parent != null)
                {
                    Parent.changeByChild(value);
                }
            }
            else
            {
                bool accessFound = false;
                foreach (var o in Options)
                {
                    if (o.Value == true)
                    {
                        accessFound = true;
                        break;
                    }
                }
                foreach (var o in Items)
                {
                    if (HasAccess == true)
                    {
                        accessFound = true;
                        break;
                    }
                }
                if (!accessFound)
                    _HasAccess = false;

            }
            OnPropertyChanged("HasAccess");
        }

        private void setChildItems(bool? setValue)
        {
            if (Items != null && Items.Count > 0)
            {
                if(setValue != true)
                foreach(CustTreeItems cti in Items)
                {
                    cti.HasAccess = setValue;
                }
            }
            else
            {
                //SetOptions
                if (Options != null && Options.Count > 0)
                {
                    foreach (Permission p in Options)
                    {
                        p.bulkChangeValue(setValue);
                    }
                }
            }
        }


        private IList<CustTreeItems> _Items;

        public IList<CustTreeItems> Items
        {
            get { return _Items; }
            private set { _Items = value; }
        }

        private bool _HasPermissions;

        public bool HasPermissions
        {
            get { return _HasPermissions; }
            set
            {
                if (_HasPermissions == value)
                    return;

                _HasPermissions = value;
                OnPropertyChanged("HasPermissions");
            }
        }


        public ObservableCollection<Permission> _Options;

        public ObservableCollection<Permission> Options
        {
            get
            {
                return _Options;
            }
            set
            {
                _Options = value;
            }
        }

        private bool _Changed = false;

        public bool Changed
        {
            get { return _Changed; }
            set
            {
                _Changed = value;
                if (!value)
                {
                    Items.ToList().ForEach(x => x.Changed = false);
                    Options.ToList().ForEach(x => x.Changed = false);
                }
                else
                {
                    if(HasParent && Parent!= null)
                    {
                        Parent.Changed = true;
                    }
                }
            }
        }


        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
                Changed = true;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }

    public class Permission: INotifyPropertyChanged
    {
        public Permission(CustTreeItems parent, string name, bool? value)
        {
            Name = name; _Value = value; ParentItem = parent;
        }

        public Permission(CustTreeItems parent, string name, string description, bool? value)
        {
            Name = name; _Value = value; ParentItem = parent; Description = description;
        }

        public Permission()
        {
        }

        private string _Name;
        private bool? _Value;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }
        private string _Description;

        public string Description
        {
            get { return _Description??_Name; }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        public void setAccess(bool? value)
        {
            _Value = value;
            OnPropertyChanged("Value");
        }

        public bool? Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                OnPropertyChanged("Value");
                ParentItem.changeByChild(value);
            }
        }

        public CustTreeItems ParentItem { get; set; }

        private bool _Changed = false;

        public bool Changed
        {
            get { return _Changed; }
            set
            {
                _Changed = value;
                if (value)
                {
                    ParentItem.Changed = true;
                }
            }
        }


        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
                Changed = true;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        //fix looping
        public void bulkChangeValue(bool? value)
        {
            _Value = value;
            OnPropertyChanged("Value");
        }
    }
}
