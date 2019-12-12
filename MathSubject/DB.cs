using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MathSubject
{
    /// <summary>
    /// 数据库操作上下文
    /// </summary>
    public class DBContext : IDisposable
    {
        DbConnection dbConn;
        DbTransaction dbTran;
        DbProviderFactory dbFactory;
        /// <summary>
        /// 数据库操作构造函数
        /// </summary>
        public DBContext()
        {
            var connectionConfig = System.Configuration.ConfigurationManager.ConnectionStrings["default"];

            dbFactory = DbProviderFactories.GetFactory(connectionConfig.ProviderName);
            dbConn = dbFactory.CreateConnection();
            dbConn.ConnectionString = connectionConfig.ConnectionString;
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            dbConn.Open();
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (dbConn != null)
            {
                dbConn.Close();
                dbConn.Dispose();
            }
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            dbTran = dbConn.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            dbTran.Commit();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            dbTran.Rollback();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public int Execute(string sql, Dictionary<string, object> Parameters)
        {
            var cmd = CreateCommand(sql, Parameters);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataTable Query(string sql, Dictionary<string, object> Parameters)
        {
            var cmd = CreateCommand(sql, Parameters);
            var dt = new DataTable();
            var da = dbFactory.CreateDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            return dt;
        }
        /// <summary>
        /// 执行查询，返回第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
