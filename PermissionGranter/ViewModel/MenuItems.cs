using PermissionGranter.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel
{
    public class MenuItems:INotifyPropertyChanged,IEnumerable
    {
        public MenuItems(string name)
        {
            //Name = name;
            Items = new ObservableCollection<CustTreeItems>();
        }

        public MenuItems()
        {
            Items = new ObservableCollection<CustTreeItems>();
        }

        private IList<CustTreeItems> _Items;

        public IList<CustTreeItems> Items
        {
            get { return _Items ; }
            private set { _Items = value; }
        }




        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public void ClearItems()
        {
            foreach (CustTreeItems cti in Items)
                cti.ClearItems();
        }

        public List<CustTreeItems> GetAllItemReferences()
        {
            List<CustTreeItems> tempList = new List<CustTreeItems>();

            foreach (CustTreeItems cti in Items)
            {
                cti.GetChildren(ref tempList);
            }


            return tempList;
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
