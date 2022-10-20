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
        List<profit_detail_out> lst_detail = new List<profit_detail_out>();
        //Dictionary<string, string> dic = new Dictionary<string, string>();

        //------------------------------------------------------------------------
        // function selectTCNUD() - 查詢 TCNUD TABLE 個股明細取得8個欄位值
        //------------------------------------------------------------------------
        public List<unoffset_qtype_detail> selectTCNUD(object o)
        {
            root SearchElement = o as root;
            
            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, TDATE, DSEQ, DNO, BQTY, PRICE, FEE, COST
                                    FROM dbo.TCNUD 
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
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
                        row.stock = reader.GetString(0);
                        row.tdate = reader.GetString(1);
                        row.dseq = reader.GetString(2);
                        row.dno = reader.GetString(3);
                        row.bqty = reader.GetDecimal(4);
                        row.mprice = reader.GetDecimal(5);
                        row.fee = reader.GetDecimal(6);
                        row.cost = reader.GetDecimal(7);
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
                                    WHERE M.STOCK = T.STOCK AND BHNO = @BHNO AND CSEQ = @CSEQ
                                    ORDER BY T.BHNO, T.CSEQ, T.STOCK, T.TDATE";
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

        //------------------------------------------------------------------------
        // function selectStockName() - 查詢 MSTMB TABLE 股票名稱值
        //------------------------------------------------------------------------
        public string selectStockName(string stockNo)
        {
            string result = "";
            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT CNAME
                                    FROM MSTMB
                                    WHERE STOCK = @STOCK";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
                sqlCmd.Parameters.AddWithValue("@STOCK", stockNo);
                SqlDataReader Reader = sqlCmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    if (Reader.Read())
                    {
                        result = Reader["CNAME"].ToString();
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
            return result;
        }

        //------------------------------------------------------------------------
        // function selectHCNRH() - 查詢 HCNRH TABLE 個股明細資料(賣出)取得12個欄位值
        //------------------------------------------------------------------------
        public List<profit_detail_out> selectHCNRH(object o)
        {
            root SearchElement = o as root;

            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT TDATE, SDSEQ, SDNO, SQTY, CQTY, SPRICE, COST, INCOME, SFEE, TAX, WTYPE, PROFIT
                                    FROM dbo.HCNRH
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                sqlCmd.Parameters.AddWithValue("@SDATE", SearchElement.sdate);
                sqlCmd.Parameters.AddWithValue("@EDATE", SearchElement.edate);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("reader has no rows");
                    }
                    while (reader.Read())
                    {
                        var row = new profit_detail_out();
                        row.tdate = reader.GetString(0);
                        row.dseq = reader.GetString(1);
                        row.dno = reader.GetString(2);
                        row.mqty = reader.GetDecimal(3);
                        row.cqty = reader.GetDecimal(4);
                        row.mprice = reader.GetDecimal(5).ToString();
                        row.cost = reader.GetDecimal(6);
                        row.income = reader.GetDecimal(7);
                        row.fee = reader.GetDecimal(8);
                        row.tax = reader.GetDecimal(9);
                        row.wtype = reader.GetString(10);
                        row.profit = reader.GetDecimal(11);
                        lst_detail.Add(row);
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
            return lst_detail;
        }
    }
}
