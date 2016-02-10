using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Utility
{
    public class Memento<T> where T : CanCopy<T>
    {

        private T _SavedCopy;

        public T SavedCopy
        {
            get { return _SavedCopy; }
        }


        public Memento(T obj)
        {
            _SavedCopy = obj.GetCopy();
        }

    }
}
