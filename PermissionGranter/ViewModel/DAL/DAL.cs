using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.DAL
{
    public static class DAL
    {

        public static readonly Dictionary<int, string> ErrorMessage =
            new Dictionary<int, string> {
                {1,"OK"}, 
                {2,"De opgegeven waarde is veranderd voordat u uw bewerking kon uitvoeren."},
                {3, "Gebruiker niet gevonden" },
                {4, "Onvoorziene fout" },
                {5, "Gebruiker bestond reeds" }
            };


        private static void SetConnectionString(this SqlConnection sq)
        {
            SqlConnectionStringBuilder sqlb = new SqlConnectionStringBuilder();
            sqlb.IntegratedSecurity = true;
            sqlb.PersistSecurityInfo = false;
            sqlb.InitialCatalog = Properties.Resources.InitialCatalog;
            sqlb.DataSource = Properties.Resources.DataSource;
            sq.ConnectionString = sqlb.ConnectionString;
        }

        public static SqlParameter Identity()
        {
            
            return Parameter("@Identity", SqlDbType.Int);
        }

        public static SqlParameter Parameter(string parameterName, object parameterValue)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = parameterValue;
            param.IsNullable = true;
            return param;
        }

        public static SqlParameter Parameter(string parameterName, SqlDbType parameterType)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.SqlDbType = parameterType;
            param.Direction = ParameterDirection.Output;
            param.IsNullable = true;
            return param;
        }

        

        /// <summary>
        /// Execute SQL Datareader with no errorCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">StoredProcedure in database</param>
        /// <param name="fillMethod">The method to fill the object, must have a IDataReader as parameter</param>
        /// <param name="arrParameter">All the Parameters required for the stored procedure</param>
        /// <returns></returns>
        public static IList<T> ExecuteDataReader<T>
                                                    (
                                                    string storedProcedure,
                                                    Func<IDataReader, T> fillMethod,
                                                    params SqlParameter[] arrParameter
                                                    )
                                                    where T : new()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                if (arrParameter != null)
                {
                    foreach (SqlParameter sqlpara in arrParameter)
                    {
                        sqlCommand.Parameters.Add(sqlpara);
                    }
                }

                IList<T> dataList = new List<T>();

                using (SqlConnection sq = new SqlConnection())
                {
                    try
                    {
                        sq.SetConnectionString();
                        sqlCommand.Connection = sq;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storedProcedure;
                        sq.Open();
                        using (SqlDataReader da = sqlCommand.ExecuteReader())
                        {
                            while (da.Read())
                            {
                                dataList.Add(fillMethod(da));
                            }
                        }
                        return dataList;
                    }
                    catch (SqlException sqlException)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("Database returned following errors:");
                        foreach (SqlError sql in sqlException.Errors)
                        {
                            stringBuilder.Append(sql.Number.ToString())
                                .Append(": ")
                                .Append(sql.Message)
                                .Append(Environment.NewLine);
                        }
                        throw new Exception(stringBuilder.ToString());
                    }
                }
            }


        }

        /// <summary>
        /// Execute SQL Datareader with ReturnValue returned to errorCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">StoredProcedure in database</param>
        /// <param name="fillMethod">The method to fill the object, must have a IDataReader as parameter</param>
        /// <param name="errorCode">Errorcode that will be returned</param>
        /// <param name="arrParameter">All the Parameters required for the stored procedure</param>
        /// <returns></returns>
        public static IList<T> ExecuteDataReader<T>
                                                    (
                                                    string storedProcedure, 
                                                    Func<IDataReader, T> fillMethod, 
                                                    out int errorCode, 
                                                    params SqlParameter[] arrParameter
                                                    )
                                                    where T : new()
        {
            bool hasReturnValue = false;
            errorCode = 0;
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                if (arrParameter != null) {
                    foreach (SqlParameter sqlpara in arrParameter)
                    {
                        sqlCommand.Parameters.Add(sqlpara);
                    }
                    sqlCommand.Parameters.Add(DAL.Parameter("@ReturnValue", SqlDbType.Int));
                }
                IList<T> dataList = new List<T>();
                string controlValue = string.Empty;
                string myconnection = string.Empty;
            
                using (SqlConnection sq = new SqlConnection())
                {
                    try
                    {
                        sq.SetConnectionString();
                        sqlCommand.Connection = sq;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storedProcedure;
                        sq.Open();
                        using (SqlDataReader da = sqlCommand.ExecuteReader())
                        {
                            if (hasReturnValue)
                            {
                                errorCode = (int) sqlCommand.Parameters["@ReturnValue"].Value;
                                string errorMessage = string.Empty;
                                ErrorMessage.TryGetValue(errorCode, out errorMessage);
                                if(errorCode != 1)
                                throw new Exception(errorMessage);
                            }
                            while (da.Read())
                            {
                                dataList.Add(fillMethod(da));
                            }
                        }
                        return dataList;
                    }
                    catch (SqlException sqlException)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("Database returned following errors:");
                        foreach (SqlError sql in sqlException.Errors)
                        {
                            stringBuilder.Append(sql.Number.ToString())
                                .Append(": ")
                                .Append(sql.Message)
                                .Append(Environment.NewLine);
                        }
                        throw new Exception(stringBuilder.ToString());
                    }
                }
            }

            
        }

        /// <summary>
        /// Execute SQL NonQuery with ReturnValue returned to errorCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">StoredProcedure in database</param>
        /// <param name="fillMethod">The method to fill the object, must have a IDataReader as parameter</param>
        /// <param name="errorCode">Errorcode that will be returned</param>
        /// <param name="arrParameter">All the Parameters required for the stored procedure</param>
        /// <returns></returns>
        public static List<object> ExecuteNonQuery(string storedProcedure, out int errorCode, params SqlParameter[] parameters)
        {
            errorCode = 0;
            List<object> outputList = new List<object>();

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                if (parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters);
                }
                sqlCommand.Parameters.Add(DAL.Parameter("@ReturnValue", SqlDbType.Int));
                using (SqlConnection sq = new SqlConnection())
                {
                    try
                    {
                        sq.SetConnectionString();
                        sqlCommand.Connection = sq;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storedProcedure;
                        sq.Open();
                        //execute query
                        sqlCommand.ExecuteNonQuery();
                        errorCode = (int)sqlCommand.Parameters["@ReturnValue"].Value;
                        string errorMessage = string.Empty;
                        ErrorMessage.TryGetValue(errorCode, out errorMessage);
                        if (errorCode != 1)
                                    throw new Exception(errorMessage);
                        //return a list of output objects
                        foreach(SqlParameter para in parameters)
                        {
                            if(para.Direction == ParameterDirection.Output)
                            {
                                outputList.Add(sqlCommand.Parameters[para.ParameterName].Value);
                            }
                        }
                        return outputList;
                        
                    }
                    catch (SqlException sqlException)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("Database returned following errors:");
                        foreach (SqlError sql in sqlException.Errors)
                        {
                            stringBuilder.Append(sql.Number.ToString())
                                .Append(": ")
                                .Append(sql.Message)
                                .Append(Environment.NewLine);
                        }
                        throw new Exception(stringBuilder.ToString());
                    }
                }
            }


        }

        /// <summary>
        /// Execute SQL NonQuery, output parameters will be returned in a List of object
        /// </summary>
        /// <param name="storedProcedure">StoredProcedure in database</param>
        /// <param name="arrParameter">All the Parameters required for the stored procedure</param>
        /// <returns>List of output objects</returns>
        public static List<object> ExecuteNonQuery(string storedProcedure, params SqlParameter[] parameters)
        {
            List<object> outputList = new List<object>();

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                if (parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters);
                }
                using (SqlConnection sq = new SqlConnection())
                {
                    try
                    {
                        sq.SetConnectionString();
                        sqlCommand.Connection = sq;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storedProcedure;
                        sq.Open();
                        //execute query
                        sqlCommand.ExecuteNonQuery();
                        string errorMessage = string.Empty;
                        //return a list of output objects
                        foreach (SqlParameter para in parameters)
                        {
                            if (para.Direction == ParameterDirection.Output)
                            {
                                outputList.Add(sqlCommand.Parameters[para.ParameterName].Value);
                            }
                        }
                        return outputList;

                    }
                    catch (SqlException sqlException)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("Database returned following errors:");
                        foreach (SqlError sql in sqlException.Errors)
                        {
                            stringBuilder.Append(sql.Number.ToString())
                                .Append(": ")
                                .Append(sql.Message)
                                .Append(Environment.NewLine);
                        }
                        throw new Exception(stringBuilder.ToString());
                    }
                }
            }


        }

        //public static T GetObjectFromDataReader<T>(string storedProcedure, Func<IDataReader, T> fillFunc)
        //    where T : new()
        //{

        //    T retrieveObject = default(T);
        //    using (SqlCommand sqlCommand = new SqlCommand())
        //    {
        //        using (SqlConnection sq = new SqlConnection())
        //        {
        //            try
        //            {
        //                sq.SetConnectionString();
        //                sqlCommand.Connection = sq;
        //                sqlCommand.CommandType = CommandType.StoredProcedure;
        //                sqlCommand.CommandText = storedProcedure;
        //                sq.Open();
        //                using (SqlDataReader da = sqlCommand.ExecuteReader())
        //                {
        //                    while (da.Read())
        //                    {
        //                        retrieveObject = fillFunc(da);
        //                    }
        //                }
        //                return retrieveObject;
        //            }
        //            catch (SqlException sqlException)
        //            {
        //                StringBuilder stringBuilder = new StringBuilder();
        //                stringBuilder.AppendLine("Database returned following errors:");
        //                foreach (SqlError sql in sqlException.Errors)
        //                {
        //                    stringBuilder.Append(sql.Number.ToString())
        //                        .Append(": ")
        //                        .Append(sql.Message)
        //                        .Append(Environment.NewLine);
        //                }
        //                throw new Exception(stringBuilder.ToString());
        //            }
        //        }
        //    }
        //}

        //public static dynamic ExecuteDynamicDataReader(string storedProcedure)
        //{
        //    string controlValue = string.Empty;
        //    SqlDataReader da = null;
        //    SqlCommand sqlCommand = new SqlCommand();
        //    SqlConnection sq = new SqlConnection();
        //    sq.SetConnectionString();
        //    sqlCommand.Connection = sq;
        //    sqlCommand.CommandType = CommandType.StoredProcedure;
        //    sqlCommand.CommandText = storedProcedure;
        //    da = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        //    return new DynamicDataReader(da);
        //}

        //public static dynamic ExecuteDataReader(string storedProcedure, out int errorCode, params SqlParameter[] arrParameter)
        //{
        //    bool hasReturnValue = false;
        //    errorCode = 0;
        //    SqlCommand sqlCommand = new SqlCommand();
        //    SqlConnection sq = new SqlConnection();

        //    if (arrParameter != null)
        //        foreach (SqlParameter sqlpara in arrParameter)
        //        {
        //            if (sqlpara.ParameterName.Equals("@ReturnValue"))
        //            {
        //                hasReturnValue = true;
        //                sqlpara.Direction = ParameterDirection.Output;
        //            }
        //            sqlCommand.Parameters.Add(sqlpara);

        //        }

        //    sq.SetConnectionString();
        //    sqlCommand.Connection = sq;
        //    sqlCommand.CommandType = CommandType.StoredProcedure;
        //    sqlCommand.CommandText = storedProcedure;
        //    SqlDataReader da = null;

        //    try
        //    {
        //        sq.Open();
        //        da = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        //        if (hasReturnValue)
        //        {
        //            errorCode = (int) sqlCommand.Parameters["@ReturnValue"].Value;
        //            string errorMessage = string.Empty;
        //            ErrorMessage.TryGetValue(errorCode, out errorMessage);
        //            throw new Exception(errorMessage);
        //        }
        //        return new DynamicDataReader(da);
        //    }
        //    catch (SqlException se)
        //    {
        //        SqlErrorCollection sqlError = se.Errors;
        //        StringBuilder stringBuilder = new StringBuilder();
        //        stringBuilder.AppendLine("Database returned following errors:");
        //        foreach (SqlError sql in sqlError)
        //        {
        //            stringBuilder.Append(sql.Number.ToString())
        //                .Append(": ")
        //                .Append(sql.Message)
        //                .Append(Environment.NewLine);
        //        }
        //        errorCode = 996;
        //        throw new Exception(stringBuilder.ToString());
        //    } 




        //}

    }
}
