using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.Model
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Tries to add a Keyvaluepair to a dictionary, returns false on failure to add.
        /// returns true on success
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="target"></param>
        /// <param name="o"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool TryAdd<K,V>(this Dictionary<K,V> target,K o, V o2){
            if (o == null)
                throw new ArgumentNullException("Key");
            if (o2 == null)
                throw new ArgumentNullException("Value");
            if (target == null)
                throw new ArgumentNullException("Target");
            if (target.ContainsKey(o))
            {
                return false;
            }
            else
            {
                target.Add(o, o2);
                return true;
            }
        }

        /// <summary>
        /// Add a range to a HashSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseSet">HashSet base</param>
        /// <param name="addSet">HashSet that will be added</param>
        /// <returns>false on failure, true on success</returns>
        public static bool AddRange<T>(this HashSet<T> baseSet,HashSet<T> addSet)
        {
            if (baseSet == null)
                throw new ArgumentNullException("baseSet null");
            if (addSet == null)
                throw new ArgumentNullException("addSet null");
            foreach (T arg in addSet)
            {
                baseSet.Add(arg);
            }

            return true;
        }

        /// <summary>
        /// Remove a range from HashSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseSet">HashSet base</param>
        /// <param name="removeSet">HashSet that will be added</param>
        /// <returns>false on failure, true on success</returns>
        public static bool RemoveRange<T>(this HashSet<T> baseSet, HashSet<T> removeSet)
        {
            if (baseSet == null)
                throw new ArgumentNullException("baseSet null");
            if (removeSet == null)
                throw new ArgumentNullException("addSet null");
            HashSet<T> tempSet = new HashSet<T>();
            //tempSet = baseSet;
            foreach (T arg in removeSet)
            {
                if(baseSet.Contains(arg))
                baseSet.Remove(arg);
            }

            return true;
        }
    }
}
