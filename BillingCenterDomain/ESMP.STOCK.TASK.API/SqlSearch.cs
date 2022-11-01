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
        static string _sqlSet = "Data Source = .; Initial Catalog = ESMP; Integrated Security = True;";
        SqlConnection _sqlConn = new SqlConnection(_sqlSet);

        //----------------------------------------------------------------------------------
        // function selectTCNUD() - 查詢 TCNUD TABLE
        //----------------------------------------------------------------------------------
        public List<TCNUD> selectTCNUD(object o)
        {
            root SearchElement = o as root;
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, TDATE, DSEQ, DNO, BQTY, PRICE, FEE, COST
                                    FROM dbo.TCNUD 
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
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
                        var row = new TCNUD();
                        row.STOCK = reader["STOCK"].ToString();
                        row.TDATE = reader["TDATE"].ToString();
                        row.DSEQ = reader["DSEQ"].ToString();
                        row.DNO = reader["DNO"].ToString();
                        row.BQTY = Convert.ToDecimal(reader["BQTY"].ToString());
                        row.PRICE = Convert.ToDecimal(reader["PRICE"].ToString());
                        row.FEE = Convert.ToDecimal(reader["FEE"].ToString());
                        row.COST = Convert.ToDecimal(reader["COST"].ToString());
                        dbTCNUD.Add(row);
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
                _sqlConn.Close();
            }
            return dbTCNUD;
        }


        //----------------------------------------------------------------------------------
        // function selectMSTMB() - 查詢 MSTMB TABLE的STOCK,CPRICE
        //----------------------------------------------------------------------------------
        public List<MSTMB> selectMSTMB()
        {
            List<MSTMB> dbMSTMB = new List<MSTMB>();
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, CPRICE
                                    FROM MSTMB";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("reader has no rows");
                    }
                    while (reader.Read())
                    {
                        var row = new MSTMB();
                        row.STOCK = reader["STOCK"].ToString();
                        if (reader.IsDBNull(1))
                            row.CPRICE = 0;
                        else
                            row.CPRICE = Convert.ToDecimal(reader["CPRICE"].ToString());
                        dbMSTMB.Add(row);
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
                _sqlConn.Close();
            }
            return dbMSTMB;
        }
        #region selectCPRICE 舊的寫法
        //----------------------------------------------------------------------------------
        // function selectCPRICE() - 查詢 MSTMB TABLE的CPRICE
        //----------------------------------------------------------------------------------
        public List<unoffset_qtype_detail> selectCPRICE(object o)
        {
            root SearchElement = o as root;
            //未實現損益
            List<unoffset_qtype_detail> lst = new List<unoffset_qtype_detail>();
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT CPRICE
                                    FROM MSTMB M, TCNUD T 
                                    WHERE M.STOCK = T.STOCK AND BHNO = @BHNO AND CSEQ = @CSEQ
                                    ORDER BY T.BHNO, T.CSEQ, T.STOCK, T.TDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
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
                _sqlConn.Close();
            }
            return lst;
        }
        #endregion
        //----------------------------------------------------------------------------------
        // function selectStockCprice() - 查詢 MSTMB TABLE 股票現價
        //----------------------------------------------------------------------------------
        public string selectStockCprice(string stockNo)
        {
            string result = "";
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT CPRICE
                                    FROM MSTMB
                                    WHERE STOCK = @STOCK";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                sqlCmd.Parameters.AddWithValue("@STOCK", stockNo);
                SqlDataReader Reader = sqlCmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    if (Reader.Read())
                    {
                        result = Reader["CPRICE"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _sqlConn.Close();
            }
            return result;
        }
        //----------------------------------------------------------------------------------
        // function selectStockName() - 查詢 MSTMB TABLE 股票名稱值
        //----------------------------------------------------------------------------------
        public string selectStockName(string stockNo)
        {
            string result = "";
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT CNAME
                                    FROM MSTMB
                                    WHERE STOCK = @STOCK";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
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
                _sqlConn.Close();
            }
            return result;
        }

        //----------------------------------------------------------------------------------
        // function selectHCNRH() - 查詢 HCNRH TABLE 個股明細資料(沖銷)
        //----------------------------------------------------------------------------------
        public List<HCNRH> selectHCNRH(object o)
        {
            root SearchElement = o as root;
            List<HCNRH> dbHCNRH = new List<HCNRH>();

            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT BHNO, TDATE, RDATE, CSEQ, BDSEQ, BDNO, SDSEQ, SDNO, STOCK, CQTY, BPRICE, BFEE, SPRICE, SFEE, TAX, INCOME, COST, PROFIT, ADJDATE, WTYPE, BQTY, SQTY, STINTAX, IOFLAG, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                    FROM dbo.HCNRH
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
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
                        var row = new HCNRH();
                        row.BHNO = reader["BHNO"].ToString();
                        row.TDATE = reader["TDATE"].ToString();
                        row.RDATE = reader["RDATE"].ToString();
                        row.CSEQ = reader["CSEQ"].ToString();
                        row.BDSEQ = reader["BDSEQ"].ToString();
                        row.BDNO = reader["BDNO"].ToString();
                        row.SDSEQ = reader["SDSEQ"].ToString();
                        row.SDNO = reader["SDNO"].ToString();
                        row.STOCK = reader["STOCK"].ToString();
                        row.CQTY = Convert.ToDecimal(reader["CQTY"].ToString());
                        row.BPRICE = Convert.ToDecimal(reader["BPRICE"].ToString());
                        row.BFEE = Convert.ToDecimal(reader["BFEE"].ToString());
                        row.SPRICE = Convert.ToDecimal(reader["SPRICE"].ToString());
                        row.SFEE = Convert.ToDecimal(reader["SFEE"].ToString());
                        row.TAX = Convert.ToDecimal(reader["TAX"].ToString());
                        row.INCOME = Convert.ToDecimal(reader["INCOME"].ToString());
                        row.COST = Convert.ToDecimal(reader["COST"].ToString());
                        row.PROFIT = Convert.ToDecimal(reader["PROFIT"].ToString());
                        row.ADJDATE = reader["ADJDATE"].ToString();
                        row.WTYPE = reader["WTYPE"].ToString();
                        row.BQTY = Convert.ToDecimal(reader["BQTY"].ToString());
                        row.SQTY = Convert.ToDecimal(reader["SQTY"].ToString());
                        row.STINTAX = Convert.ToDecimal(reader["STINTAX"].ToString());
                        row.IOFLAG = reader["IOFLAG"].ToString();
                        row.TRDATE = reader["TRDATE"].ToString();
                        row.TRTIME = reader["TRTIME"].ToString();
                        row.MODDATE = reader["MODDATE"].ToString();
                        row.MODTIME = reader["MODTIME"].ToString();
                        row.MODUSER = reader["MODUSER"].ToString();

                        dbHCNRH.Add(row);
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
                _sqlConn.Close();
            }
            return dbHCNRH;
        }

        //----------------------------------------------------------------------------------
        // function selectHCNTD() - 查詢 HCNTD TABLE 個股明細資料(當沖)
        //----------------------------------------------------------------------------------
        public List<HCNTD> selectHCNTD(object o)
        {
            root SearchElement = o as root;
            List<HCNTD> dbHCNTD = new List<HCNTD>();

            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT BHNO, TDATE, CSEQ, BDSEQ, BDNO, SDSEQ, SDNO, STOCK, CQTY, BPRICE, BFEE, SPRICE, SFEE, TAX, INCOME, COST, PROFIT, BQTY, SQTY, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                    FROM dbo.HCNTD
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
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
                        var row = new HCNTD();
                        row.BHNO = reader["BHNO"].ToString();
                        row.TDATE = reader["TDATE"].ToString();
                        row.CSEQ = reader["CSEQ"].ToString();
                        row.BDSEQ = reader["BDSEQ"].ToString();
                        row.BDNO = reader["BDNO"].ToString();
                        row.SDSEQ = reader["SDSEQ"].ToString();
                        row.SDNO = reader["SDNO"].ToString();
                        row.STOCK = reader["STOCK"].ToString();
                        row.CQTY = Convert.ToDecimal(reader["CQTY"].ToString());
                        row.BPRICE = Convert.ToDecimal(reader["BPRICE"].ToString());
                        row.BFEE = Convert.ToDecimal(reader["BFEE"].ToString());
                        row.SPRICE = Convert.ToDecimal(reader["SPRICE"].ToString());
                        row.SFEE = Convert.ToDecimal(reader["SFEE"].ToString());
                        row.TAX = Convert.ToDecimal(reader["TAX"].ToString());
                        row.INCOME = Convert.ToDecimal(reader["INCOME"].ToString());
                        row.COST = Convert.ToDecimal(reader["COST"].ToString());
                        row.PROFIT = Convert.ToDecimal(reader["PROFIT"].ToString());
                        row.BQTY = Convert.ToDecimal(reader["BQTY"].ToString());
                        row.SQTY = Convert.ToDecimal(reader["SQTY"].ToString());
                        row.TRDATE = reader["TRDATE"].ToString();
                        row.TRTIME = reader["TRTIME"].ToString();
                        row.MODDATE = reader["MODDATE"].ToString();
                        row.MODTIME = reader["MODTIME"].ToString();
                        row.MODUSER = reader["MODUSER"].ToString();

                        dbHCNTD.Add(row);
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
                _sqlConn.Close();
            }
            return dbHCNTD;
        }

        //----------------------------------------------------------------------------------
        // function selectHCMIO() - 查詢 HCMIO TABLE 對帳單明細資料
        //----------------------------------------------------------------------------------
        public List<HCMIO> selectHCMIO(object o)
        {
            root SearchElement = o as root;
            List<HCMIO> dbHCMIO = new List<HCMIO>();
            try
            {
                _sqlConn.Open();
                if(SearchElement.stockSymbol.Length > 1)
                {
                    string sqlQuery = @"SELECT STOCK, TDATE, DSEQ, DNO, TTYPE, BSTYPE, ETYPE, PRICE, QTY, AMT, FEE, TAX, NETAMT
                                    FROM dbo.HCMIO
                                    WHERE STOCK = @STOCK AND BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                    sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                    sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                    sqlCmd.Parameters.AddWithValue("@SDATE", SearchElement.sdate);
                    sqlCmd.Parameters.AddWithValue("@EDATE", SearchElement.edate);
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("沒有歷史對帳單明細資料");
                        }
                        while (reader.Read())
                        {
                            var row = new HCMIO();
                            row.STOCK = reader["STOCK"].ToString();
                            row.TDATE = reader["TDATE"].ToString();
                            row.DSEQ = reader["DSEQ"].ToString();
                            row.DNO = reader["DNO"].ToString();
                            row.TTYPE = reader["TTYPE"].ToString();
                            row.BSTYPE = reader["BSTYPE"].ToString();
                            row.ETYPE = reader["ETYPE"].ToString();
                            row.PRICE = Convert.ToDecimal(reader["PRICE"].ToString());
                            row.QTY = Convert.ToDecimal(reader["QTY"].ToString());
                            row.AMT = Convert.ToDecimal(reader["AMT"].ToString());
                            row.FEE = Convert.ToDecimal(reader["FEE"].ToString());
                            row.TAX = Convert.ToDecimal(reader["TAX"].ToString());
                            row.NETAMT = Convert.ToDecimal(reader["NETAMT"].ToString());
                            dbHCMIO.Add(row);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    string sqlQuery = @"SELECT STOCK, TDATE, DSEQ, DNO, TTYPE, BSTYPE, ETYPE, PRICE, QTY, AMT, FEE, TAX, NETAMT
                                    FROM dbo.HCMIO
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
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
                            var row = new HCMIO();
                            row.STOCK = reader["STOCK"].ToString();
                            row.TDATE = reader["TDATE"].ToString();
                            row.DSEQ = reader["DSEQ"].ToString();
                            row.DNO = reader["DNO"].ToString();
                            row.TTYPE = reader["TTYPE"].ToString();
                            row.BSTYPE = reader["BSTYPE"].ToString();
                            row.ETYPE = reader["ETYPE"].ToString();
                            row.PRICE = Convert.ToDecimal(reader["PRICE"].ToString());
                            row.QTY = Convert.ToDecimal(reader["QTY"].ToString());
                            row.AMT = Convert.ToDecimal(reader["AMT"].ToString());
                            row.FEE = Convert.ToDecimal(reader["FEE"].ToString());
                            row.TAX = Convert.ToDecimal(reader["TAX"].ToString());
                            row.NETAMT = Convert.ToDecimal(reader["NETAMT"].ToString());
                            dbHCMIO.Add(row);
                        }
                        reader.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _sqlConn.Close();
            }
            return dbHCMIO;
        }

        //----------------------------------------------------------------------------------
        // function selectTMHIO() - 查詢 TMHIO TABLE 對帳單明細資料
        //----------------------------------------------------------------------------------
        public List<TMHIO> selectTMHIO(object o)
        {
            root SearchElement = o as root;
            List<TMHIO> dbTMHIO = new List<TMHIO>();
            try
            {
                _sqlConn.Open();
                if (SearchElement.stockSymbol.Length > 1)
                {
                    string sqlQuery = @"SELECT STOCK, TDATE, DSEQ, JRNUM, TTYPE, BSTYPE, ETYPE, PRICE, QTY
                                    FROM dbo.TMHIO
                                    WHERE STOCK = @STOCK AND BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                    sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                    sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                    sqlCmd.Parameters.AddWithValue("@SDATE", SearchElement.sdate);
                    sqlCmd.Parameters.AddWithValue("@EDATE", SearchElement.edate);
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("沒有當日此股票對帳單明細資料");
                        }
                        while (reader.Read())
                        {
                            var row = new TMHIO();
                            row.STOCK = reader["STOCK"].ToString();
                            row.TDATE = reader["TDATE"].ToString();
                            row.DSEQ = reader["DSEQ"].ToString();
                            row.JRNUM = reader["JRNUM"].ToString();
                            row.TTYPE = reader["TTYPE"].ToString();
                            row.BSTYPE = reader["BSTYPE"].ToString();
                            row.ETYPE = reader["ETYPE"].ToString();
                            row.PRICE = Convert.ToDecimal(reader["PRICE"].ToString());
                            row.QTY = Convert.ToDecimal(reader["QTY"].ToString());
                            dbTMHIO.Add(row);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    string sqlQuery = @"SELECT STOCK, TDATE, DSEQ, JRNUM, TTYPE, BSTYPE, ETYPE, PRICE, QTY
                                    FROM dbo.TMHIO
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                    sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                    sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                    sqlCmd.Parameters.AddWithValue("@SDATE", SearchElement.sdate);
                    sqlCmd.Parameters.AddWithValue("@EDATE", SearchElement.edate);

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("沒有當日對帳單明細資料");
                        }
                        while (reader.Read())
                        {
                            var row = new TMHIO();
                            row.STOCK = reader["STOCK"].ToString();
                            row.TDATE = reader["TDATE"].ToString();
                            row.DSEQ = reader["DSEQ"].ToString();
                            row.JRNUM = reader["JRNUM"].ToString();
                            row.TTYPE = reader["TTYPE"].ToString();
                            row.BSTYPE = reader["BSTYPE"].ToString();
                            row.ETYPE = reader["ETYPE"].ToString();
                            row.PRICE = Convert.ToDecimal(reader["PRICE"].ToString());
                            row.QTY = Convert.ToDecimal(reader["QTY"].ToString());
                            dbTMHIO.Add(row);
                        }
                        reader.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _sqlConn.Close();
            }
            return dbTMHIO;
        }  
    }
}
