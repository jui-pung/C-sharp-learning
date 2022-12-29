using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESMP.STOCK.DB.TABLE;

namespace ESMP.STOCK.TASK.API
{
    public sealed class BasicData
    {
        static string _sqlSet = "Data Source = .; Initial Catalog = ESMP; Integrated Security = True;";
        static SqlSearch _sqlSearch;
        private static object chekLock = new object();
        private static Dictionary<string, string> _MsysDict = null;
        static List<MSTMB> _MSTMBList = new List<MSTMB>();                  //自訂MSTMB類別List (dbESMP.STOCK.DB.TABLE.API)
        public static Dictionary<string, List<MSTMB>> _MSTMB_Dic = null;
        static List<MCSRH> _MCSRHList = new List<MCSRH>();                  //自訂MCUMS類別List (dbESMP.STOCK.DB.TABLE.API)
        public static Dictionary<string, List<MCSRH>> _MCSRH_Dic = null;
        static List<MCUMS> _MCUMSList = new List<MCUMS>();                  //自訂MCUMS類別List (dbESMP.STOCK.DB.TABLE.API)
        public static Dictionary<string, List<MCUMS>> _MCUMS_Dic = null;

        public static Dictionary<string, string> MsysDict
        { 
            get
            {
                if(_MsysDict == null)
                {
                    lock (chekLock)
                    {
                        if( _MsysDict == null)
                        {
                            _MsysDict = new Dictionary<string, string>();
                            _MsysDict = createIoflagameDic();
                        }   
                    }
                }
                return _MsysDict;
            } 
        }
        public static Dictionary<string, List<MSTMB>> MSTMB_Dic
        {
            get
            {
                if (_MSTMB_Dic == null)
                {
                    lock (chekLock)
                    {
                        if (_MSTMB_Dic == null)
                        {
                            _sqlSearch = new SqlSearch();
                            _MSTMBList = _sqlSearch.selectMSTMB();
                            //依據STOCK 建立 MSTMB Dictionary
                            _MSTMB_Dic = _MSTMBList.GroupBy(d => d.STOCK).ToDictionary(x => x.Key, x => x.ToList());
                        }
                    }
                }
                return _MSTMB_Dic;
            }
        }
        public static Dictionary<string, List<MCSRH>> MCSRH_Dic
        {
            get
            {
                if (_MCSRH_Dic == null)
                {
                    lock (chekLock)
                    {
                        if (_MCSRH_Dic == null)
                        {
                            _sqlSearch = new SqlSearch();
                            _MCSRHList = _sqlSearch.selectMCSRH();
                            //依據 BHNO CSEQ STOCK 建立 MCSRH Dictionary
                            _MCSRH_Dic = _MCSRHList.GroupBy(d => d.BHNO + d.CSEQ + d.STOCK).ToDictionary(x => x.Key, x => x.ToList());
                        }
                    }
                }
                return _MCSRH_Dic;
            }
        }

        public static Dictionary<string, List<MCUMS>> MCUMS_Dic
        {
            get
            {
                if (_MCUMS_Dic == null)
                {
                    lock (chekLock)
                    {
                        if (_MCUMS_Dic == null)
                        {
                            _sqlSearch = new SqlSearch();
                            _MCUMSList = _sqlSearch.selectMCUMS();
                            //依據 BHNO CSEQ 建立 MCUMS Dictionary
                            _MCUMS_Dic = _MCUMSList.GroupBy(d => d.BHNO + d.CSEQ).ToDictionary(x => x.Key, x => x.ToList());
                        }
                    }
                }
                return _MCUMS_Dic;
            }
        }
        public static string getIoflagName(string ioflag)
        {
            if (ioflag == "0167")
            {
                ioflag = "167";
            }
            if (ioflag == "0256")
            {
                ioflag = "256";
            }
            if (_MsysDict.ContainsKey(ioflag))
            {
                return _MsysDict[ioflag];
            }
            else
                return string.Empty;
        }

        private static Dictionary<string, string> createIoflagameDic()
        {
            SqlConnection sqlConn = new SqlConnection(_sqlSet);
            SqlDataAdapter da;
            DataTable dt_dictionary = new DataTable();
            Dictionary<string, string> ioflagNameDic = new Dictionary<string, string>();
            dt_dictionary.Clear();
            ioflagNameDic.Clear();
            try
            {
                sqlConn.Open();
                SqlCommand command = new SqlCommand("SELECT VARNAME, VALUE FROM dbo.MSYS", sqlConn);
                da = new SqlDataAdapter(command);
                da.Fill(dt_dictionary);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }

            foreach (DataRow dtRow in dt_dictionary.Rows)
            {
                ioflagNameDic.Add(dtRow["VARNAME"].ToString().Substring(6), dtRow["VALUE"].ToString());
            }
            return ioflagNameDic;
        }

        public static void GetDBToXml()
        {
            string dbName = "ESMP";
            string connstr = $"Server=localhost;Integrated security=SSPI;database={dbName}";
            string cmdstr = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
            List<string> tableNames = new List<string>();
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                using (SqlCommand cmd = new SqlCommand(cmdstr, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tableNames.Add((string)reader["TABLE_NAME"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            cmdstr = "";
            foreach (var item in tableNames)
            {
                cmdstr = String.Concat(cmdstr, $"SELECT Top 0 * FROM dbo.{item}; ");
            }
            DataSet dbESMP = new DataSet();
            dbESMP.DataSetName = "dbESMP";
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                using (SqlCommand cmd = new SqlCommand(cmdstr, conn))
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;

                        conn.Open();
                        adapter.Fill(dbESMP);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            for (int i = 0; i < dbESMP.Tables.Count; i++)
            {
                dbESMP.Tables[i].TableName = tableNames[i];
            }
            dbESMP.WriteXml(@"C:\temp\dbESMP.xml", XmlWriteMode.WriteSchema);
        }
    }
}
