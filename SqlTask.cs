using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_UnrealizedGainsOrLosses
{
    public class SqlTask
    {
        static string sqlSet = "Data Source = .; Initial Catalog = ESMP; Integrated Security = True;";
        SqlConnection sqlConn = new SqlConnection(sqlSet);
        public void qtype_detail(object o)
        {
            root SearchElement = o as root;
            try
            {
                sqlConn.Open();
                SqlCommand command = new SqlCommand("Select TDATE from dbo.TCNUD where BHNO = @BHNO AND CSEQ = @CSEQ", sqlConn);
                command.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                command.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                int result = command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}", reader["TDATE"]));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
        }
    }
}
