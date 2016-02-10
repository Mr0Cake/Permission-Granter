using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel
{
    /// <summary>
    /// This class provides an easy way to use object.property
    /// syntax with a DataReader by wrapping a DataReader into
    /// a dynamic object.
    /// 
    /// The class also automatically fixes up DbNull values
    /// (null into .NET and DbNUll)
    /// ------ Nederlands ------
    /// Deze klasse gaat een IDataReader converteren naar een dynamisch object,
    /// Hierdoor kunnen we volgende code uitvoeren
    /// SqlDataReader sqlreader = SqlCommand.ExecuteReader....
    /// dynamic reader = new DynamicDataReader(sqlReader);
    /// while(sqlreader.Read())
    /// {
    ///     string Naam = reader.Naam;     //is gelijk aan sqlreader["Naam"] as string
    ///     string Postcode = reader.Postcode; //is gelijk aan sqlreader["Postcode"] as string
    /// }
    /// Dus alle oproepen naar een Property worden dynamisch omgezet naar de code van sqlreader[Property]
    /// -----------------------
    /// It's important to understand that what you're seeing is a dynamic type, not a strongly typed .NET type. 
    /// Dynamic means you get to use object.property syntax in this case, and you get automatic casting, 
    /// but you do not get strong typing and so no compiler type checking or Intellisense on those properties. 
    /// They are dynamic and so essentially syntactic sugar around dynamic invocation and Reflection implemented through the Dynamic Language Runtime (DLR).
    /// </summary>
    public class DynamicDataReader : DynamicObject
    {
        /// <summary>
        /// Cached Instance of DataReader passed in
        /// </summary>
        IDataReader DataReader;

        /// <summary>
        /// Pass in a loaded DataReader
        /// </summary>
        /// <param name="dataReader">DataReader instance to work off</param>
        public DynamicDataReader(IDataReader dataReader)
        {
            DataReader = dataReader;
            
        }

        /// <summary>
        /// Returns a value from the current DataReader record
        /// If the field doesn't exist null is returned.
        /// DbNull values are turned into .NET nulls.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            // 'Implement' common reader properties directly
            if (binder.Name == "IsClosed")
                result = DataReader.IsClosed;
            else if (binder.Name == "RecordsAffected")
                result = DataReader.RecordsAffected;
            // lookup column names as fields
            else
            {
                try
                {
                    result = DataReader[binder.Name];
                    if (result == DBNull.Value)
                        result = null;
                }
                catch
                {
                    result = null;
                    return false;
                }
            }

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {

            // Implement most commonly used method
            if (binder.Name == "Read")
                result = DataReader.Read();
            else if (binder.Name == "Close")
            {
                DataReader.Close();
                result = null;
            }
            else
                // call other DataReader methods using Reflection (slow - not recommended)
                // recommend you use full DataReader instance
                result = DataReader.GetType().GetMethod(binder.Name).Invoke(DataReader, args);
                //result = ReflectionUtils.CallMethod(DataReader, binder.Name, args);

            return true;
        }
    }
}
