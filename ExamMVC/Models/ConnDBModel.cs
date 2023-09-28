using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ExamMVC.Models
{
    public class ConnDBModel : IDisposable
    {
        void IDisposable.Dispose() { }
        public void Dispose() { }

        public string s_Key { get; set; }
        public string s_ConnectionString { get; set; }
        public SqlConnection s_Conn { get; set; }
        public SqlTransaction s_Trans { get; set; }
        public SqlCommand s_Command { get; set; }

        public ConnDBModel()
        {
            s_Key = "ConnDB";
            s_Conn = GetConnection_SQL(s_Key);
        }

        public ConnDBModel(string strKeyName)
        {
            s_Key = strKeyName;
            s_Conn = GetConnection_SQL(s_Key);
        }

        public ConnDBModel(SqlConnection conn)
        {
            s_Conn = conn;
        }

        public string GetConnectionString(string strKeyName)
        {
            //return ConfigurationManager.ConnectionStrings[strKeyName].ConnectionString;
            s_ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[strKeyName].ConnectionString;
            return s_ConnectionString;
        }

        public SqlConnection GetConnection_SQL()
        {
            return s_Conn;
        }

        public SqlConnection GetConnection_SQL(string strKeyName)
        {
            s_Conn = new SqlConnection(GetConnectionString(strKeyName));
            return s_Conn;
        }

        public SqlTransaction GetTransaction_SQL(ref SqlConnection strConnection)
        {
            s_Trans = strConnection.BeginTransaction();
            return s_Trans;
        }

        public SqlTransaction GetTransaction_SQL(string strKeyName)
        {
            s_Trans = GetConnection_SQL(strKeyName).BeginTransaction();
            return s_Trans;
        }

        /// <summary>
        /// get Config Value
        /// </summary>
        /// <returns></returns>
        public static string getConfigValue(string strKey)
        {
            try
            {
                //return System.Configuration.ConfigurationManager.AppSettings[strKey];
                return System.Web.Configuration.WebConfigurationManager.AppSettings[strKey];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="strSQL">SQL Command</param>
        /// <param name="parameters">條件參數</param>
        /// <param name="Conn">Connection</param>
        /// <returns>回傳Data</returns>
        public static DataTable ExecuteQuery(string strSQL, Dictionary<string, object> parameters, SqlConnection Conn)
        {
            try
            {
                if (Conn == null) throw new Exception();

                using (SqlCommand Cmd = new SqlCommand())
                {
                    if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
                    Cmd.Connection = Conn;
                    Cmd.CommandTimeout = 2000;
                    Cmd.CommandType = CommandType.Text;
                    Cmd.CommandText = strSQL;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> cPar in parameters)
                        {
                            Cmd.Parameters.Add(new SqlParameter(cPar.Key, cPar.Value)).Direction = ParameterDirection.InputOutput;
                        }
                    }

                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = Cmd;

                    //DataTable dt = new DataTable
                    //{
                    //    Locale = CultureInfo.InvariantCulture
                    //};

                    DataTable dt = new DataTable();
                    dt.Locale = CultureInfo.InvariantCulture;

                    adp.Fill(dt);
                    DataTable dtResult = dt;
                    return dtResult;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        /// <summary>
        /// 查詢(含Transaction)
        /// </summary>
        /// <param name="strSQL">SQL Command</param>
        /// <param name="parameters">條件參數</param>
        /// <param name="Conn">Connection</param>
        /// <returns>回傳Data</returns>
        public static DataTable ExecuteQueryTrans(string strSQL, Dictionary<string, object> parameters, ConnDBModel p_clsCB)
        {
            try
            {
                if (p_clsCB.s_Conn == null) throw new Exception();

                using (SqlCommand Cmd = new SqlCommand())
                {
                    if (p_clsCB.s_Conn.State == ConnectionState.Closed) { p_clsCB.s_Conn.Open(); }
                    Cmd.Connection = p_clsCB.s_Conn;
                    Cmd.CommandTimeout = 2000;
                    Cmd.Transaction = p_clsCB.s_Trans;
                    Cmd.CommandType = CommandType.Text;
                    Cmd.CommandText = strSQL;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> cPar in parameters)
                        {
                            Cmd.Parameters.Add(new SqlParameter(cPar.Key, cPar.Value)).Direction = ParameterDirection.InputOutput;
                        }
                    }

                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = Cmd;

                    //DataTable dt = new DataTable
                    //{
                    //    Locale = CultureInfo.InvariantCulture
                    //};

                    DataTable dt = new DataTable();
                    dt.Locale = CultureInfo.InvariantCulture;

                    adp.Fill(dt);
                    DataTable dtResult = dt;
                    return dtResult;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 新增修改刪除資料
        /// </summary>
        /// <param name="strSQL">SQL Command</param>
        /// <param name="parameters">條件參數</param>
        /// <param name="Conn">SqlConnection</param>
        /// <param name="Trans">SqlTransaction</param>
        /// <returns></returns>
        //public static int ExecuteSQL(string strSQL, Dictionary<string, object> parameters, SqlConnection Conn, SqlTransaction Trans)
        public static int ExecuteSQL(string strSQL, Dictionary<string, object> parameters, SqlConnection Conn)
        {
            try
            {
                if (Conn == null) throw new Exception();
                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.Connection = Conn;
                    Cmd.CommandTimeout = 2000;
                    //Cmd.Transaction = Trans;
                    Cmd.CommandType = CommandType.Text;
                    Cmd.CommandText = strSQL;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> cPar in parameters)
                        {
                            Cmd.Parameters.Add(new SqlParameter(cPar.Key, cPar.Value)).Direction = ParameterDirection.InputOutput;
                        }
                    }

                    return Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        /// <summary>
        /// 新增修改刪除資料
        /// </summary>
        /// <param name="strSQL">SQL Command</param>
        /// <param name="parameters">條件參數</param>
        /// <param name="Conn">Connection</param>
        /// <returns>執行結果值</returns>
        //public static int ExecuteSQL(string strSQL, Dictionary<string, object> parameters, SqlConnection Conn, SqlTransaction p_trans)
        public static int ExecuteSQL(string strSQL, Dictionary<string, object> parameters, ConnDBModel p_clsCB)
        {
            try
            {
                //if (Conn == null) throw new Exception();
                if (p_clsCB.s_Conn == null) throw new Exception();

                using (SqlCommand Cmd = new SqlCommand())
                {
                    //Cmd.Connection = Conn;
                    //Cmd.Transaction = p_trans;
                    Cmd.Connection = p_clsCB.s_Conn;
                    Cmd.CommandTimeout = 2000;
                    Cmd.Transaction = p_clsCB.s_Trans;
                    Cmd.CommandType = CommandType.Text;
                    Cmd.CommandText = strSQL;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> cPar in parameters)
                        {
                            Cmd.Parameters.Add(new SqlParameter(cPar.Key, cPar.Value)).Direction = ParameterDirection.InputOutput;
                        }
                    }

                    return Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 執行Store Procedure
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="parameters"></param>
        /// <param name="p_clsCB"></param>
        /// <returns></returns>
        public static string ExecuteProcedure(string strSQL, Dictionary<string, object> parameters, Dictionary<string, object> outparameters, SqlConnection Conn)
        {
            try
            {
                if (Conn == null) throw new Exception();

                //using (SqlCommand Cmd = new SqlCommand())
                //{
                //    //Cmd.Connection = Conn;
                //    //Cmd.Transaction = p_trans;
                //    Cmd.Connection = p_clsCB.s_Conn;
                //    Cmd.CommandTimeout = 2000;
                //    Cmd.Transaction = p_clsCB.s_Trans;
                //    //Cmd.CommandType = CommandType.StoredProcedure;
                //    Cmd.CommandType = CommandType.Text;
                //    Cmd.CommandText = strSQL;


                //    if (parameters != null)
                //    {
                //        foreach (KeyValuePair<string, object> cPar in parameters)
                //        {
                //            Cmd.Parameters.Add(new SqlParameter(cPar.Key, cPar.Value)).Direction = ParameterDirection.InputOutput;
                //        }
                //    }

                //    //SqlParameter retValParam = Cmd.Parameters.Add("@OutputData", SqlDbType.VarChar, 250);
                //    //retValParam.Direction = ParameterDirection.Output;

                //    return Cmd.ExecuteNonQuery();
                //    //return retValParam;
                //}



                using (SqlCommand Cmd = new SqlCommand())
                {
                    if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
                    Cmd.Connection = Conn;
                    Cmd.CommandTimeout = 2000;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.CommandText = strSQL;

                    //Cmd.Connection = p_clsCB.s_Conn;
                    //Cmd.CommandTimeout = 2000;
                    //Cmd.Transaction = p_clsCB.s_Trans;
                    //Cmd.CommandType = CommandType.StoredProcedure;
                    //Cmd.CommandText = strSQL;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> cPar in parameters)
                        {
                            Cmd.Parameters.Add(new SqlParameter(cPar.Key, cPar.Value)).Direction = ParameterDirection.Input;
                        }
                    }

                    SqlParameter retValParam = new SqlParameter();
                    if (outparameters != null)
                    {
                        foreach (KeyValuePair<string, object> cPar in outparameters)
                        {
                            retValParam = Cmd.Parameters.Add(new SqlParameter(cPar.Key, SqlDbType.VarChar, 10, cPar.Value.ToString()));
                            retValParam.Direction = ParameterDirection.Output;
                        }

                        //retValParam = Cmd.Parameters.Add(outparameters.Keys, outparameters.Values).Direction = ParameterDirection.Output;
                        //retValParam.Direction = ParameterDirection.Output;
                    }

                    ////SqlParameter retValParam = Cmd.Parameters.Add("@OutputData", SqlDbType.VarChar, 250);
                    //SqlParameter retValParam = Cmd.Parameters.Add("@Count", SqlDbType.Int, 10);
                    //retValParam.Direction = ParameterDirection.Output;

                    Cmd.ExecuteNonQuery();
                    return retValParam.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                //throw (ex);
                return "Error";
            }
        }

        /// <summary>
        /// 執行Store Procedure
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="parameters"></param>
        /// <param name="p_clsCB"></param>
        /// <returns></returns>
        public static int ExecuteProcedure(string strSQL, Dictionary<string, object> parameters, SqlConnection Conn)
        {
            try
            {
                if (Conn == null) throw new Exception();

                //using (SqlCommand Cmd = new SqlCommand())
                //{
                //    //Cmd.Connection = Conn;
                //    //Cmd.Transaction = p_trans;
                //    Cmd.Connection = p_clsCB.s_Conn;
                //    Cmd.CommandTimeout = 2000;
                //    Cmd.Transaction = p_clsCB.s_Trans;
                //    //Cmd.CommandType = CommandType.StoredProcedure;
                //    Cmd.CommandType = CommandType.Text;
                //    Cmd.CommandText = strSQL;

                //    if (parameters != null)
                //    {
                //        foreach (KeyValuePair<string, object> cPar in parameters)
                //        {
                //            Cmd.Parameters.Add(new SqlParameter(cPar.Key, cPar.Value)).Direction = ParameterDirection.InputOutput;
                //        }
                //    }

                //    //SqlParameter retValParam = Cmd.Parameters.Add("@OutputData", SqlDbType.VarChar, 250);
                //    //retValParam.Direction = ParameterDirection.Output;

                //    return Cmd.ExecuteNonQuery();
                //    //return retValParam;
                //}


                using (SqlCommand Cmd = new SqlCommand())
                {
                    if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
                    Cmd.Connection = Conn;
                    Cmd.CommandTimeout = 2000;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.CommandText = strSQL;

                    //Cmd.Connection = p_clsCB.s_Conn;
                    //Cmd.CommandTimeout = 2000;
                    //Cmd.Transaction = p_clsCB.s_Trans;
                    //Cmd.CommandType = CommandType.StoredProcedure;
                    //Cmd.CommandText = strSQL;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> cPar in parameters)
                        {
                            Cmd.Parameters.Add(new SqlParameter(cPar.Key, cPar.Value)).Direction = ParameterDirection.Input;
                        }
                    }


                    return Cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


    }
}