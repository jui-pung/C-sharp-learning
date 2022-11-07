using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.API
{
    public class Ioflagname
    {
        static string _sqlSet = "Data Source = .; Initial Catalog = ESMP; Integrated Security = True;";
        SqlConnection _sqlConn = new SqlConnection(_sqlSet);
        //建立ioflagname字典
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
