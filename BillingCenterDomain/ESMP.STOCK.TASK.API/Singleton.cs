using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.API
{
    public class Singleton
    {
        static string _sqlSet = "Data Source = .; Initial Catalog = ESMP; Integrated Security = True;";
        SqlConnection _sqlConn = new SqlConnection(_sqlSet);
        private static Singleton _instance = null;
        private static object chekLock = new object();
        private Singleton(){ }
        public static Singleton Instance
        {
            get
            {
                lock (chekLock)
                {
                    if (_instance == null)
                        _instance = new Singleton();
                    return _instance;
                }
            }
        }

        private static Dictionary<string, string> _MsysDict = null;
        public Dictionary<string, string> MsysDict
        { get
            {
                if(_MsysDict == null)
                {
                    lock (chekLock)
                    {
                        if( _MsysDict == null)
                        {
                            _MsysDict = new Dictionary<string, string>();
                        }   
                    }
                }
                return _MsysDict;
            } 
        }

        public static string getIoflagName(string ioflag)
        {
            if(ioflag == "0167")
            {
                ioflag = "167";
            }
            if(_MsysDict.ContainsKey(ioflag))
            {
                return _MsysDict[ioflag];
            }
            else 
                return string.Empty;
        }


        public Dictionary<string, string> createIoflagameDic()
        {
            SqlDataAdapter da;
            DataTable dt_dictionary = new DataTable();
            Dictionary<string, string> ioflagNameDic = new Dictionary<string, string>();
            dt_dictionary.Clear();
            ioflagNameDic.Clear();
            try
            {
                _sqlConn.Open();
                SqlCommand command = new SqlCommand("SELECT VARNAME, VALUE FROM dbo.MSYS", _sqlConn);
                da = new SqlDataAdapter(command);
                da.Fill(dt_dictionary);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _sqlConn.Close();
            }

            foreach (DataRow dtRow in dt_dictionary.Rows)
            {
                ioflagNameDic.Add(dtRow["VARNAME"].ToString().Substring(6), dtRow["VALUE"].ToString());
            }
            return ioflagNameDic;
        }
    }
}
