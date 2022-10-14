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
        List<unoffset_qtype_detail> lst = new List<unoffset_qtype_detail>();
        //Dictionary<string, string> dic = new Dictionary<string, string>();

        //------------------------------------------------------------------------
        // function selectTCNUD() - 查詢 TCNUD TABLE 個股明細取得7個欄位值
        //------------------------------------------------------------------------
        public List<unoffset_qtype_detail> selectTCNUD(object o)
        {
            root SearchElement = o as root;
            
            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT TDATE, DSEQ, DNO, BQTY, PRICE, FEE, COST
                                    FROM dbo.TCNUD 
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("reader has no rows");
                    }
                    while (reader.Read())
                    {
                        var row = new unoffset_qtype_detail();
                        row.tdate = reader.GetString(0);
                        row.dseq = reader.GetString(1);
                        row.dno = reader.GetString(2);
                        row.bqty = reader.GetDecimal(3);
                        row.mprice = reader.GetDecimal(4);
                        row.fee = reader.GetDecimal(5);
                        row.cost = reader.GetDecimal(6);
                        lst.Add(row);
                    } 
                    reader.Close();
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
            return lst;
        }

        //------------------------------------------------------------------------
        // function selectMSTMB() - 查詢 MSTMB TABLE 個股明細取得1個欄位值
        //------------------------------------------------------------------------
        public List<unoffset_qtype_detail> selectMSTMB(object o)
        {
            root SearchElement = o as root;

            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT CPRICE
                                    FROM MSTMB M, TCNUD T 
                                    WHERE M.STOCK = T.STOCK AND BHNO = @BHNO AND CSEQ = @CSEQ";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("reader has no rows");
                    }
                    int index = 0;
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0))
                            lst[index].lastprice = 0;
                        else
                            lst[index].lastprice = reader.GetDecimal(0);
                        index++;
                    }
                    reader.Close();
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
            return lst;
        }
    }
}
