using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Services.Utilities
{
    public static class DataBaseHelper
    {
        // public static string ConnectionString { get; } = new SqlConnectionStringBuilder
        // {
        //     DataSource = "LOCALHOST", UserID = "sa", Password = "", InitialCatalog = ""
        // }.ToString();
        private static string ConnectionString { get; } =
            new JsonConfigHelper("../../../../Resource/Config.json").Config.SqlConnectionString;

        #region Public method

        public static int GetMaxId(string fieldName, string tableName)
        {
            var strSql = "select max(" + fieldName + ")+1 from " + tableName;
            var obj = GetSingle(strSql);
            return obj == null ? 1 : int.Parse(obj.ToString() ?? string.Empty);
        }

        public static bool Exists(string strSql, params SqlParameter[] cmdParams)
        {
            var obj = GetSingle(strSql, cmdParams);
            int cmdResult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdResult = 0;
            }
            else
            {
                cmdResult = int.Parse(obj.ToString() ?? string.Empty);
            }

            return cmdResult != 0;
        }

        #endregion

        #region Execute SQL

        public static int ExecuteSql(string sqlString)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sqlString, connection);
            try
            {
                connection.Open();
                var rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (SqlException ex)
            {
                connection.Close();
                throw new Exception(ex.Message);
            }
        }

        public static void ExecuteSqlTran(ArrayList sqlStringList)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            var cmd = new SqlCommand {Connection = conn};
            var tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                foreach (var t in sqlStringList)
                {
                    var sqlString = t.ToString();
                    if (sqlString != null && sqlString.Trim().Length <= 1) continue;
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch (SqlException ex)
            {
                tx.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public static int ExecuteSql(string sqlString, string content)
        {
            using var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(sqlString, connection);
            var myParameter =
                new SqlParameter("@content", SqlDbType.NText) {Value = content};
            cmd.Parameters.Add(myParameter);
            try
            {
                connection.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
            }
        }

        public static int ExecuteSqlInsertImg(string sqlString, byte[] fs)
        {
            using var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(sqlString, connection);
            var myParameter =
                new SqlParameter("@fs", SqlDbType.Image) {Value = fs};
            cmd.Parameters.Add(myParameter);
            try
            {
                connection.Open();
                var rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
            }
        }

        private static object GetSingle(string sqlString)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sqlString, connection);
            try
            {
                connection.Open();
                var obj = cmd.ExecuteScalar();
                if (Equals(obj, null) || Equals(obj, DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch (SqlException ex)
            {
                connection.Close();
                throw new Exception(ex.Message);
            }
        }

        public static SqlDataReader ExecuteReader(string sqlString)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlString, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataSet Query(string sqlString)
        {
            using var connection = new SqlConnection(ConnectionString);
            var ds = new DataSet();
            try
            {
                connection.Open();
                var command = new SqlDataAdapter(sqlString, connection);
                command.Fill(ds, "ds");
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        #endregion

        #region Execute SQl with Param

        public static int ExecuteSql(string sqlString, params SqlParameter[] cmdParams)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, sqlString, cmdParams);
                int rows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rows;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void ExecuteSqlTran(Hashtable sqlStringList)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();
            var cmd = new SqlCommand();
            try
            {
                foreach (DictionaryEntry de in sqlStringList)
                {
                    var cmdText = de.Key.ToString();
                    var cmdParams = (SqlParameter[]) de.Value;
                    PrepareCommand(cmd, conn, trans, cmdText, cmdParams);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    trans.Commit();
                }
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        private static object GetSingle(string sqlString, params SqlParameter[] cmdParams)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, sqlString, cmdParams);
                object obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                if (Equals(obj, null) || Equals(obj, DBNull.Value))
                {
                    return null;
                }

                return obj;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SqlDataReader ExecuteReader(string sqlString, params SqlParameter[] cmdParams)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, sqlString, cmdParams);
                var myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataSet Query(string sqlString, params SqlParameter[] cmdParams)
        {
            using var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, sqlString, cmdParams);
            using var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            try
            {
                da.Fill(ds, "ds");
                cmd.Parameters.Clear();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }


        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText,
            SqlParameter[] cmdParams)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text; //cmdType;
            if (cmdParams == null) return;
            foreach (var param in cmdParams)
                cmd.Parameters.Add(param);
        }

        #endregion

        #region Run StoredProceduce

        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            var returnReader = command.ExecuteReader();
            return returnReader;
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            var sqlDa = new SqlDataAdapter();
            using SqlConnection connection = new SqlConnection(ConnectionString);
            var dataSet = new DataSet();
            connection.Open();
            sqlDa.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDa.Fill(dataSet, tableName);
            connection.Close();
            return dataSet;
        }

        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName,
            IDataParameter[] parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var command = new SqlCommand(storedProcName, connection) {CommandType = CommandType.StoredProcedure};
            foreach (var dataParameter in parameters)
            {
                var parameter = (SqlParameter) dataParameter;
                command.Parameters.Add(parameter);
            }

            return command;
        }

        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var command = BuildIntCommand(connection, storedProcName, parameters);
            rowsAffected = command.ExecuteNonQuery();
            var result = (int) command.Parameters["ReturnValue"].Value;
            //Connection.Close();
            return result;
        }

        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName,
            IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        #endregion
    }
}