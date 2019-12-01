using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MathSubject
{
    public class DBContext : IDisposable
    {
        DbConnection dbConn;
        DbTransaction dbTran;
        DbProviderFactory dbFactory;
        public DBContext()
        {
            var connectionConfig = System.Configuration.ConfigurationManager.ConnectionStrings["default"];

            dbFactory = DbProviderFactories.GetFactory(connectionConfig.ProviderName);
            dbConn = dbFactory.CreateConnection();
            dbConn.ConnectionString = connectionConfig.ConnectionString;
        }

        public void Open()
        {
            dbConn.Open();
        }
        public void Close()
        {
            if (dbConn != null)
            {
                dbConn.Close();
                dbConn.Dispose();
            }
        }

        public void BeginTransaction()
        {
            dbTran = dbConn.BeginTransaction();
        }
        public void Commit()
        {
            dbTran.Commit();
        }
        public void Rollback()
        {
            dbTran.Rollback();
        }


        public int Execute(string sql, Dictionary<string, object> Parameters)
        {
            var cmd = CreateCommand(sql, Parameters);
            return cmd.ExecuteNonQuery();
        }
        public DataTable Query(string sql, Dictionary<string, object> Parameters)
        {
            var cmd = CreateCommand(sql, Parameters);
            var dt = new DataTable();
            var da = dbFactory.CreateDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            return dt;
        }
        public object ExecuteScalar(string sql, Dictionary<string, object> Parameters)
        {
            var cmd = CreateCommand(sql, Parameters);
            return cmd.ExecuteScalar();
        }

        private DbCommand CreateCommand(string sql, Dictionary<string, object> Parameters)
        {
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            foreach (var item in Parameters)
            {
                var parameter = dbFactory.CreateParameter();
                parameter.ParameterName = item.Key;
                parameter.Value = item.Value;
                cmd.Parameters.Add(parameter);
            }
            if (dbTran != null)
            {
                cmd.Transaction = dbTran;
            }
            return cmd;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
