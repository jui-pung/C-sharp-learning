using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.API
{
    public sealed class BasicData
    {
        static string _sqlSet = "Data Source = .; Initial Catalog = ESMP; Integrated Security = True;";
        private static object chekLock = new object();
        private static Dictionary<string, string> _MsysDict = null;
        private BasicData()
        {

        }
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
    }
}
