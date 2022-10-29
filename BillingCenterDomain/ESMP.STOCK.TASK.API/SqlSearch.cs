using ESMP.STOCK.DB.TABLE.API;
using ESMP.STOCK.FORMAT.API;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.API
{
    public class SqlSearch
    {
        static string sqlSet = "Data Source = .; Initial Catalog = ESMP; Integrated Security = True;";
        SqlConnection sqlConn = new SqlConnection(sqlSet);
        //未實現損益
        List<unoffset_qtype_detail> lst = new List<unoffset_qtype_detail>();
        //已實現損益
        List<profit_detail_out> lst_detail = new List<profit_detail_out>();
        List<profit_detail> lst_detailB = new List<profit_detail>();
        //對帳單查詢
        List<profile> lst_profile = new List<profile>();

        //----------------------------------------------------------------------------------
        // function selectTCNUD() - 查詢 TCNUD TABLE 個股明細取得8個欄位值
        //----------------------------------------------------------------------------------
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

        //----------------------------------------------------------------------------------
        // function selectMSTMB() - 查詢 MSTMB TABLE 個股明細取得1個欄位值
        //----------------------------------------------------------------------------------
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

        //----------------------------------------------------------------------------------
        // function selectStockName() - 查詢 MSTMB TABLE 股票名稱值
        //----------------------------------------------------------------------------------
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

        //----------------------------------------------------------------------------------
        // function selectHCNRH() - 查詢 HCNRH TABLE 個股明細資料(賣出)取得13個欄位值
        //----------------------------------------------------------------------------------
        public List<HCNRH> selectHCNRH(object o)
        {
            root SearchElement = o as root;

            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT BHNO, TDATE, RDATE, CSEQ, BDSEQ, BDNO, SDSEQ, SDNO, STOCK, CQTY, BPRICE, BFEE, SPRICE, SFEE, TAX, INCOME, COST, PROFIT, ADJDATE, WTYPE, BQTY, SQTY, STINTAX, IOFLAG, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                    FROM dbo.HCNRH
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                sqlCmd.Parameters.AddWithValue("@SDATE", SearchElement.sdate);
                sqlCmd.Parameters.AddWithValue("@EDATE", SearchElement.edate);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("沒有沖銷資料");
                    }
                    while (reader.Read())
                    {
                        //ADJDATE, WTYPE, BQTY, SQTY, STINTAX, IOFLAG, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                        var row = new HCNRH();
                        row.BHNO = reader.GetString(0);
                        row.TDATE = reader.GetString(1);
                        row.RDATE = reader.GetString(2);
                        row.CSEQ = reader.GetString(3);
                        row.BDSEQ = reader.GetString(4);
                        row.BDNO = reader.GetString(5);
                        row.SDSEQ = reader.GetString(6);
                        row.SDNO = reader.GetString(7);
                        row.STOCK = reader.GetString(8);
                        row.CQTY = reader.GetDecimal(9);
                        row.BPRICE = reader.GetDecimal(10);
                        row.BFEE = reader.GetDecimal(11);
                        row.SPRICE = reader.GetDecimal(12);
                        row.SFEE = reader.GetDecimal(13);
                        row.TAX = reader.GetDecimal(14);
                        row.INCOME = reader.GetDecimal(15);
                        row.COST = reader.GetDecimal(16);
                        row.PROFIT = reader.GetDecimal(17);

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

        //----------------------------------------------------------------------------------
        // function selectHCNTD() - 查詢 HCNTD TABLE 個股明細資料(賣出)取得12個欄位值
        //----------------------------------------------------------------------------------
        public List<profit_detail_out> selectHCNTD(object o)
        {
            root SearchElement = o as root;

            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, TDATE, SDSEQ, SDNO, SQTY, CQTY, SPRICE, COST, INCOME, SFEE, TAX, PROFIT
                                    FROM dbo.HCNTD
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                sqlCmd.Parameters.AddWithValue("@SDATE", SearchElement.sdate);
                sqlCmd.Parameters.AddWithValue("@EDATE", SearchElement.edate);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("沒有當沖資料");
                    }
                    while (reader.Read())
                    {
                        var row = new profit_detail_out();
                        row.stock = reader.GetString(0);
                        row.tdate = reader.GetString(1);
                        row.dseq = reader.GetString(2);
                        row.dno = reader.GetString(3);
                        row.mqty = reader.GetDecimal(4);
                        row.cqty = reader.GetDecimal(5);
                        row.mprice = reader.GetDecimal(6).ToString();
                        row.cost = reader.GetDecimal(7);
                        row.income = reader.GetDecimal(8);
                        row.netamt = reader.GetDecimal(8);
                        row.fee = reader.GetDecimal(9);
                        row.tax = reader.GetDecimal(10);
                        row.profit = reader.GetDecimal(11);
                        row.wtype = "0";
                        row.ttypename2 = "賣沖";
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

        //----------------------------------------------------------------------------------
        // function selectHCNRH_B() - 查詢 HCNRH TABLE 個股明細資料(買入)取得17個欄位值
        //----------------------------------------------------------------------------------
        public List<profit_detail> selectHCNRH_B(object o)
        {
            root SearchElement = o as root;

            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, RDATE, BDSEQ, BDNO, BQTY, CQTY, BPRICE, COST, INCOME, BFEE, ADJDATE, WTYPE, PROFIT, IOFLAG, SDSEQ, SDNO, TDATE
                                    FROM dbo.HCNRH
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                sqlCmd.Parameters.AddWithValue("@SDATE", SearchElement.sdate);
                sqlCmd.Parameters.AddWithValue("@EDATE", SearchElement.edate);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("沒有沖銷資料");
                    }
                    while (reader.Read())
                    {
                        var row = new profit_detail();
                        row.stock = reader.GetString(0);
                        row.tdate = reader.GetString(1);
                        row.dseq = reader.GetString(2);
                        row.dno = reader.GetString(3);
                        row.mqty = reader.GetDecimal(4);
                        row.cqty = reader.GetDecimal(5);
                        row.mprice = reader.GetDecimal(6).ToString();
                        row.cost = reader.GetDecimal(7);
                        row.income = reader.GetDecimal(8);
                        row.netamt = reader.GetDecimal(7) * -1;
                        row.fee = reader.GetDecimal(9);
                        row.adjdate = reader.GetString(10);
                        row.wtype = reader.GetString(11);
                        row.profit = reader.GetDecimal(12);
                        if (reader.IsDBNull(13))
                            row.ioflag = " ";
                        row.sdseq = reader.GetString(14);
                        row.sdno = reader.GetString(15);
                        row.sdate = reader.GetString(16);
                        lst_detailB.Add(row);
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
            return lst_detailB;
        }

        //----------------------------------------------------------------------------------
        // function selectHCNTD_B() - 查詢 HCNTD TABLE 個股明細資料(買入)取得13個欄位值
        //----------------------------------------------------------------------------------
        public List<profit_detail> selectHCNTD_B(object o)
        {
            root SearchElement = o as root;

            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, TDATE, BDSEQ, BDNO, BQTY, CQTY, BPRICE, COST, INCOME, BFEE, PROFIT, SDSEQ, SDNO
                                    FROM dbo.HCNTD
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                sqlCmd.Parameters.AddWithValue("@SDATE", SearchElement.sdate);
                sqlCmd.Parameters.AddWithValue("@EDATE", SearchElement.edate);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("沒有當沖資料");
                    }
                    while (reader.Read())
                    {
                        var row = new profit_detail();
                        row.stock = reader.GetString(0);
                        row.tdate = reader.GetString(1);
                        row.dseq = reader.GetString(2);
                        row.dno = reader.GetString(3);
                        row.mqty = reader.GetDecimal(4);
                        row.cqty = reader.GetDecimal(5);
                        row.mprice = reader.GetDecimal(6).ToString();
                        row.cost = reader.GetDecimal(7);
                        row.income = reader.GetDecimal(8);
                        row.netamt = reader.GetDecimal(8) * -1;
                        row.fee = reader.GetDecimal(9);
                        row.adjdate = "";
                        row.wtype = "0";
                        row.profit = reader.GetDecimal(10);
                        row.ioflag = "0";
                        row.sdseq = reader.GetString(11);
                        row.sdno = reader.GetString(12);
                        row.sdate = reader.GetString(1);
                        lst_detailB.Add(row);
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
            return lst_detailB;
        }

        //----------------------------------------------------------------------------------
        // function selectHCMIO() - 查詢 HCMIO TABLE 對帳單明細資料 取得13個欄位值
        //----------------------------------------------------------------------------------
        public List<profile> selectHCMIO(object o)
        {
            root SearchElement = o as root;

            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, TDATE, DSEQ, DNO, TTYPE, BSTYPE, ETYPE, PRICE, QTY, AMT, FEE, TAX, NETAMT
                                    FROM dbo.HCMIO
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
                        Console.WriteLine("沒有歷史對帳單明細資料");
                    }
                    while (reader.Read())
                    {
                        var row = new profile();
                        row.stock = reader.GetString(0);
                        row.mdate = reader.GetString(1);
                        row.dseq = reader.GetString(2);
                        row.dno = reader.GetString(3);
                        row.ttype = reader.GetString(4);
                        if (reader.GetString(4) == "0" && reader.GetString(5) == "B")
                            row.ttypename = "現買";
                        else if (reader.GetString(4) == "0" && reader.GetString(5) == "S")
                            row.ttypename = "現賣";
                        row.bstype = reader.GetString(5);
                        if (reader.GetString(5) == "B")
                            row.bstypename = "買";
                        else if (reader.GetString(5) == "S")
                            row.bstypename = "賣";
                        row.etype = reader.GetString(6);
                        row.mprice = reader.GetDecimal(7);
                        row.mqty = reader.GetDecimal(8);
                        row.mamt = reader.GetDecimal(9);
                        row.fee = reader.GetDecimal(10);
                        row.tax = reader.GetDecimal(11);
                        row.netamt = reader.GetDecimal(12);
                        lst_profile.Add(row);
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
            return lst_profile;
        }

        //----------------------------------------------------------------------------------
        // function selectTMHIO() - 查詢 TMHIO TABLE 對帳單明細資料 取得9個欄位值
        //----------------------------------------------------------------------------------
        public List<profile> selectTMHIO(object o)
        {
            root SearchElement = o as root;

            try
            {
                sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, TDATE, DSEQ, JRNUM, TTYPE, BSTYPE, ETYPE, PRICE, QTY
                                    FROM dbo.TMHIO
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
                        Console.WriteLine("沒有當日交易明細資料");
                    }
                    while (reader.Read())
                    {
                        var row = new profile();
                        row.stock = reader.GetString(0);
                        row.mdate = reader.GetString(1);
                        row.dseq = reader.GetString(2);
                        row.dno = reader.GetString(3);
                        row.ttype = reader.GetString(4);

                        if (reader.GetString(4) == "0" && reader.GetString(5) == "B" && reader.GetString(6) == "0")
                            row.ttypename = "現買";
                        else if (reader.GetString(4) == "0" && reader.GetString(5) == "B" && reader.GetString(6) == "2")
                            row.ttypename = "盤後零買";
                        else if (reader.GetString(4) == "0" && reader.GetString(5) == "B" && reader.GetString(6) == "5")
                            row.ttypename = "盤中零買";
                        else if (reader.GetString(4) == "0" && reader.GetString(5) == "S" && reader.GetString(6) == "0")
                            row.ttypename = "現賣";
                        else if (reader.GetString(4) == "0" && reader.GetString(5) == "S" && reader.GetString(6) == "2")
                            row.ttypename = "盤後零賣";
                        else if (reader.GetString(4) == "0" && reader.GetString(5) == "S" && reader.GetString(6) == "5")
                            row.ttypename = "盤中零賣";

                        row.bstype = reader.GetString(5);

                        if (reader.GetString(5) == "B")
                            row.bstypename = "買";
                        else if (reader.GetString(5) == "S")
                            row.bstypename = "賣";

                        if (reader.GetString(6) == "2")
                            row.etype = "1";
                        else
                            row.etype = "0";

                        row.mprice = reader.GetDecimal(7);
                        row.mqty = reader.GetDecimal(8);
                        row.mamt = decimal.Truncate(reader.GetDecimal(7) * reader.GetDecimal(8));

                        if (decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(reader.GetDecimal(7) * reader.GetDecimal(8)) * 0.001425)) < 20)
                            row.fee = 20;
                        else
                            row.fee = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(reader.GetDecimal(7) * reader.GetDecimal(8)) * 0.001425));
                        if(reader.GetString(5) == "S")
                            row.tax = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(reader.GetDecimal(7) * reader.GetDecimal(8)) * 0.003));
                        lst_profile.Add(row);
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
            return lst_profile;
        }
    }
}
