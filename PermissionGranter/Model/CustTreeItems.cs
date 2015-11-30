using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public class CustTreeItems : INotifyPropertyChanged, IEnumerable
    {
        public CustTreeItems(string name)
        {
            Name = name;
            _Items = new ObservableCollection<CustTreeItems>();
            _Options = new ObservableCollection<Permission>();
        }

        public CustTreeItems(string name, bool? hasAccess)
        {
            Name = name;
            HasAccess = hasAccess;
            _Items = new ObservableCollection<CustTreeItems>();
            Options = new ObservableCollection<Permission>();
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
                    GetChildren(ref tempList);
                }
            }
            else
            {
                tempList.Add(this);
            }
        }

        private bool? _HasAccess = false;

        public bool? HasAccess
        {
            get { return _HasAccess; }
            set
            {
                setChildItems(value);
                _HasAccess = value;
                OnPropertyChanged("HasAccess");
                
            }
        }

        public void ClearItems()
        {
            HasAccess = false;
        }

        public void changeByChild(bool? value)
        {
            _HasAccess = value;
            OnPropertyChanged("HasAccess");
        }

        private void setChildItems(bool? setValue)
        {
            if (Items != null && Items.Count > 0)
            {
                foreach(CustTreeItems cti in Items)
                {
                    cti.HasAccess = setValue;
                }
            }
            else
            {
                if(Options != null && Options.Count > 0)
                {
                    foreach(Permission p in Options)
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

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
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
        private string _Name;
        private bool? _Value;
        public string Name { get { return _Name; } set { OnPropertyChanged("Name"); _Name = value; } }
        public bool changeByParent = false;
        public bool? Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (!changeByParent)
                {
                    ParentItem.changeByChild(new bool?(false));
                }

                
                _Value = value;
                OnPropertyChanged("Value");
            }
        }

        public CustTreeItems ParentItem { get; private set; }
        public Permission(CustTreeItems parent, string name, bool? value)
        {
            Name = name; _Value = value; ParentItem = parent;
        }
        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
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
