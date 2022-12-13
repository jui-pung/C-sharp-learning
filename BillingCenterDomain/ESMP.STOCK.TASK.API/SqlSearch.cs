using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.API
{
    public class SqlSearch
    {
        static string _sqlSet = "Data Source = .; Initial Catalog = ESMP; Integrated Security = True;";
        static int _dateDiff = -56;             //當日交易明細測試使用 資料庫當日資料為2022/10/17
        SqlConnection _sqlConn = new SqlConnection(_sqlSet);

        //----------------------------------------------------------------------------------
        // function selectTCNUD() - 查詢 TCNUD TABLE
        //----------------------------------------------------------------------------------
        public List<TCNUD> selectTCNUD(object o)
        {
            root SearchElement = o as root;
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    //加入查詢股票代號
                    sqlQuery = @"SELECT BHNO, CSEQ, STOCK, TDATE, DSEQ, DNO, QTY, BQTY, PRICE, FEE, COST
                                FROM dbo.TCNUD 
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND STOCK = @STOCK
                                ORDER BY BHNO, CSEQ, STOCK, TDATE, WTYPE, DNO";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT BHNO, CSEQ, STOCK, TDATE, DSEQ, DNO, QTY, BQTY, PRICE, FEE, COST
                                FROM dbo.TCNUD
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ
                                ORDER BY BHNO, CSEQ, STOCK, TDATE, WTYPE, DNO";
                }
                sqlCmd.CommandText = sqlQuery;
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
                        row.BHNO = reader["BHNO"].ToString();
                        row.CSEQ = reader["CSEQ"].ToString();
                        row.STOCK = reader["STOCK"].ToString();
                        row.TDATE = reader["TDATE"].ToString();
                        row.DSEQ = reader["DSEQ"].ToString();
                        row.DNO = reader["DNO"].ToString();
                        row.QTY = Convert.ToDecimal(reader["QTY"].ToString());
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
        // function selectTCRUD() - 查詢 TCRUD TABLE
        //----------------------------------------------------------------------------------
        public List<TCRUD> selectTCRUD(object o)
        {
            root SearchElement = o as root;
            List<TCRUD> dbTCRUD = new List<TCRUD>();
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    //加入查詢股票代號
                    sqlQuery = @"SELECT TDATE, BHNO, DSEQ, PRICE, DNO, CSEQ, STOCK, QTY, CRAMT, PAMT, BQTY, BCRAMT, CRINT, SFCODE, CRRATIO, ASFAMT, FEE, COST, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                FROM dbo.TCRUD
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND STOCK = @STOCK
                                ORDER BY BHNO, CSEQ, STOCK";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT TDATE, BHNO, DSEQ, PRICE, DNO, CSEQ, STOCK, QTY, CRAMT, PAMT, BQTY, BCRAMT, CRINT, SFCODE, CRRATIO, ASFAMT, FEE, COST, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                FROM dbo.TCRUD
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ
                                ORDER BY BHNO, CSEQ, STOCK";
                }
                sqlCmd.CommandText = sqlQuery;
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
                        var row = new TCRUD();
                        row.TDATE = reader.IsDBNull(0) ? " " : reader["TDATE"].ToString();
                        row.BHNO = reader.IsDBNull(1) ? " " : reader["BHNO"].ToString();
                        row.DSEQ = reader.IsDBNull(2) ? " " : reader["DSEQ"].ToString();
                        row.PRICE = reader.IsDBNull(3) ? 0 : Convert.ToDecimal(reader["PRICE"].ToString());
                        row.DNO = reader.IsDBNull(4) ? " " : reader["DNO"].ToString();
                        row.CSEQ = reader.IsDBNull(5) ? " " : reader["CSEQ"].ToString();
                        row.STOCK = reader.IsDBNull(6) ? " " : reader["STOCK"].ToString();
                        row.QTY = reader.IsDBNull(7) ? 0 : Convert.ToDecimal(reader["QTY"].ToString());
                        row.CRAMT = reader.IsDBNull(8) ? 0 : Convert.ToDecimal(reader["CRAMT"].ToString());
                        row.PAMT = reader.IsDBNull(9) ? 0 : Convert.ToDecimal(reader["PAMT"].ToString());
                        row.BQTY = reader.IsDBNull(10) ? 0 : Convert.ToDecimal(reader["BQTY"].ToString());
                        row.BCRAMT = reader.IsDBNull(11) ? 0 : Convert.ToDecimal(reader["BCRAMT"].ToString());
                        row.CRINT = reader.IsDBNull(12) ? 0 : Convert.ToDecimal(reader["CRINT"].ToString());
                        row.SFCODE = reader.IsDBNull(13) ? " " : reader["SFCODE"].ToString();
                        row.CRRATIO = reader.IsDBNull(14) ? 0 : Convert.ToDecimal(reader["CRRATIO"].ToString());
                        row.ASFAMT = reader.IsDBNull(15) ? 0 : Convert.ToDecimal(reader["ASFAMT"].ToString());
                        row.FEE = reader.IsDBNull(16) ? 0 : Convert.ToDecimal(reader["FEE"].ToString());
                        row.COST = reader.IsDBNull(17) ? 0 : Convert.ToDecimal(reader["COST"].ToString());
                        row.TRDATE = reader.IsDBNull(18) ? " " : reader["TRDATE"].ToString();
                        row.TRTIME = reader.IsDBNull(19) ? " " : reader["TRTIME"].ToString();
                        row.MODDATE = reader.IsDBNull(20) ? " " : reader["MODDATE"].ToString();
                        row.MODTIME = reader.IsDBNull(21) ? " " : reader["MODTIME"].ToString();
                        row.MODUSER = reader.IsDBNull(22) ? " " : reader["MODUSER"].ToString();
                        dbTCRUD.Add(row);
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
            return dbTCRUD;
        }

        //----------------------------------------------------------------------------------
        // function selectTDBUD() - 查詢 TDBUD TABLE
        //----------------------------------------------------------------------------------
        public List<TDBUD> selectTDBUD(object o)
        {
            root SearchElement = o as root;
            List<TDBUD> dbTDBUD = new List<TDBUD>();
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    //加入查詢股票代號
                    sqlQuery = @"SELECT TDATE, BHNO, DSEQ, DNO, CSEQ, STOCK, PRICE, QTY, DBAMT, GTAMT, DNAMT, BQTY, BDBAMT, BGTAMT, BDNAMT, GTINT, DNINT, DLFEE, DLINT, DBRATIO, SFCODE, AGTAMT, FEE, TAX, DBFEE, COST, STINTAX, HEALTHFEE, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                FROM dbo.TDBUD
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND STOCK = @STOCK
                                ORDER BY BHNO, CSEQ, STOCK";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT TDATE, BHNO, DSEQ, DNO, CSEQ, STOCK, PRICE, QTY, DBAMT, GTAMT, DNAMT, BQTY, BDBAMT, BGTAMT, BDNAMT, GTINT, DNINT, DLFEE, DLINT, DBRATIO, SFCODE, AGTAMT, FEE, TAX, DBFEE, COST, STINTAX, HEALTHFEE, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                FROM dbo.TDBUD
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ
                                ORDER BY BHNO, CSEQ, STOCK";
                }
                sqlCmd.CommandText = sqlQuery;
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
                        var row = new TDBUD();
                        row.TDATE = reader.IsDBNull(0) ? " " : reader["TDATE"].ToString();
                        row.BHNO = reader.IsDBNull(1) ? " " : reader["BHNO"].ToString();
                        row.DSEQ = reader.IsDBNull(2) ? " " : reader["DSEQ"].ToString();
                        row.DNO = reader.IsDBNull(3) ? " " : reader["DNO"].ToString();
                        row.CSEQ = reader.IsDBNull(4) ? " " : reader["CSEQ"].ToString();
                        row.STOCK = reader.IsDBNull(5) ? " " : reader["STOCK"].ToString();
                        row.PRICE = reader.IsDBNull(6) ? 0 : Convert.ToDecimal(reader["PRICE"].ToString());
                        row.QTY = reader.IsDBNull(7) ? 0 : Convert.ToDecimal(reader["QTY"].ToString());
                        row.DBAMT = reader.IsDBNull(8) ? 0 : Convert.ToDecimal(reader["DBAMT"].ToString());
                        row.GTAMT = reader.IsDBNull(9) ? 0 : Convert.ToDecimal(reader["GTAMT"].ToString());
                        row.DNAMT = reader.IsDBNull(10) ? 0 : Convert.ToDecimal(reader["DNAMT"].ToString());
                        row.BQTY = reader.IsDBNull(11) ? 0 : Convert.ToDecimal(reader["BQTY"].ToString());
                        row.BDBAMT = reader.IsDBNull(12) ? 0 : Convert.ToDecimal(reader["BDBAMT"].ToString());
                        row.BGTAMT = reader.IsDBNull(13) ? 0 : Convert.ToDecimal(reader["BGTAMT"].ToString());
                        row.BDNAMT = reader.IsDBNull(14) ? 0 : Convert.ToDecimal(reader["BDNAMT"].ToString());
                        row.GTINT = reader.IsDBNull(15) ? 0 : Convert.ToDecimal(reader["GTINT"].ToString());
                        row.DNINT = reader.IsDBNull(16) ? 0 : Convert.ToDecimal(reader["DNINT"].ToString());
                        row.DLFEE = reader.IsDBNull(17) ? 0 : Convert.ToDecimal(reader["DLFEE"].ToString());
                        row.DLINT = reader.IsDBNull(18) ? 0 : Convert.ToDecimal(reader["DLINT"].ToString());
                        row.DBRATIO = reader.IsDBNull(19) ? 0 : Convert.ToDecimal(reader["DBRATIO"].ToString());
                        row.SFCODE = reader.IsDBNull(20) ? " " : reader["SFCODE"].ToString();
                        row.AGTAMT = reader.IsDBNull(21) ? 0 : Convert.ToDecimal(reader["AGTAMT"].ToString());
                        row.FEE = reader.IsDBNull(22) ? 0 : Convert.ToDecimal(reader["FEE"].ToString());
                        row.TAX = reader.IsDBNull(23) ? 0 : Convert.ToDecimal(reader["TAX"].ToString());
                        row.DBFEE = reader.IsDBNull(24) ? 0 : Convert.ToDecimal(reader["DBFEE"].ToString());
                        row.COST = reader.IsDBNull(25) ? 0 : Convert.ToDecimal(reader["COST"].ToString());
                        row.STINTAX = reader.IsDBNull(26) ? 0 : Convert.ToDecimal(reader["STINTAX"].ToString());
                        row.HEALTHFEE = reader.IsDBNull(27) ? 0 : Convert.ToDecimal(reader["HEALTHFEE"].ToString());
                        row.TRDATE = reader.IsDBNull(28) ? " " : reader["TRDATE"].ToString();
                        row.TRTIME = reader.IsDBNull(29) ? " " : reader["TRTIME"].ToString();
                        row.MODDATE = reader.IsDBNull(30) ? " " : reader["MODDATE"].ToString();
                        row.MODTIME = reader.IsDBNull(31) ? " " : reader["MODTIME"].ToString();
                        row.MODUSER = reader.IsDBNull(32) ? " " : reader["MODUSER"].ToString();
                        dbTDBUD.Add(row);
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
            return dbTDBUD;
        }

        # region selectMSTMB
        //----------------------------------------------------------------------------------
        // function selectMSTMB() - 查詢 MSTMB TABLE
        //----------------------------------------------------------------------------------
        public List<MSTMB> selectMSTMB()
        {
            List<MSTMB> dbMSTMB = new List<MSTMB>();
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT STOCK, CNAME, ENAME, MTYPE, STYPE, SCLASS, TSDATE, TEDATE, CLDATE, CPRICE, TPRICE, BPRICE, TSTATUS, BRKNO, IDATE, IRATE, IDAY, CURRENCY, COUNTRY, SHARE, WARNING, TMARK, MFLAG, WMARK, TAXTYPE, PTYPE, DRDATE, PDRDATE, CDIV, SDIV, CNTDTYPE, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
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
                        row.CNAME = reader["CNAME"].ToString();
                        row.ENAME = reader["ENAME"].ToString();
                        row.MTYPE = reader["MTYPE"].ToString();
                        row.STYPE = reader["STYPE"].ToString();
                        row.SCLASS = reader["SCLASS"].ToString();
                        row.TSDATE = reader["TSDATE"].ToString();
                        row.TEDATE = reader["TEDATE"].ToString();
                        row.CLDATE = reader["CLDATE"].ToString();
                        row.CPRICE = reader.IsDBNull(9) ? 10 : Convert.ToDecimal(reader["CPRICE"].ToString());      //現價如果是空值, 假設現價為10
                        row.TPRICE = reader.IsDBNull(10) ? 0 : Convert.ToDecimal(reader["TPRICE"].ToString());
                        row.BPRICE = reader.IsDBNull(11) ? 0 : Convert.ToDecimal(reader["BPRICE"].ToString());
                        row.TSTATUS = reader["TSTATUS"].ToString();
                        row.BRKNO = reader["BRKNO"].ToString();
                        row.IDATE = reader["IDATE"].ToString();
                        row.IRATE = reader.IsDBNull(15) ? 0 : Convert.ToDecimal(reader["IRATE"].ToString());
                        row.IDAY = reader.IsDBNull(16) ? 0 : Convert.ToDecimal(reader["IDAY"].ToString());
                        row.CURRENCY = reader["CURRENCY"].ToString();
                        row.COUNTRY = reader["COUNTRY"].ToString();
                        row.SHARE = reader.IsDBNull(19) ? 0 : Convert.ToDecimal(reader["SHARE"].ToString());
                        row.WARNING = reader["WARNING"].ToString();
                        row.TMARK = reader["TMARK"].ToString();
                        row.MFLAG = reader["MFLAG"].ToString();
                        row.WMARK = reader["WMARK"].ToString();
                        row.TAXTYPE = reader["TAXTYPE"].ToString();
                        row.PTYPE = reader["PTYPE"].ToString();
                        row.DRDATE = reader["DRDATE"].ToString();
                        row.PDRDATE = reader["PDRDATE"].ToString();
                        row.CDIV = reader.IsDBNull(28) ? 0 : Convert.ToDecimal(reader["CDIV"].ToString());
                        row.SDIV = reader.IsDBNull(29) ? 0 : Convert.ToDecimal(reader["SDIV"].ToString());
                        if (!reader.IsDBNull(30))
                            row.CNTDTYPE = reader["CNTDTYPE"].ToString();
                        else
                            row.CNTDTYPE = "N";          //沖銷資格如果是空值, 假設賣單只能沖銷昨日餘額(N)
                        row.TRDATE = reader["TRDATE"].ToString();
                        row.TRTIME = reader["TRTIME"].ToString();
                        row.MODDATE = reader["MODDATE"].ToString();
                        row.MODTIME = reader["MODTIME"].ToString();
                        row.MODUSER = reader["MODUSER"].ToString();
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
        #endregion
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
        public decimal selectStockCprice(string stockNo)
        {
            decimal result = 0;
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
                        if (!Reader.IsDBNull(0))
                            result = Convert.ToDecimal(Reader["CPRICE"].ToString());
                        else
                            result = 10;          //現價如果是空值, 假設現價為10
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
        // function selectStockCNTDTYPE() - 查詢股票 MSTMB TABLE 當沖資格
        //----------------------------------------------------------------------------------
        public string selectStockCNTDTYPE(string stockNo)
        {
            string result = "";
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT CNTDTYPE
                                    FROM MSTMB
                                    WHERE STOCK = @STOCK";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                sqlCmd.Parameters.AddWithValue("@STOCK", stockNo);
                SqlDataReader Reader = sqlCmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    if (Reader.Read())
                    {
                        if (!Reader.IsDBNull(0))
                            result = Reader["CNTDTYPE"].ToString();
                        else
                            result = "N";          //沖銷資格如果是空值, 假設賣單只能沖銷昨日餘額(N)
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
        // function selectMCUMS() - 查詢 MCUMS TABLE
        //----------------------------------------------------------------------------------
        public List<MCUMS> selectMCUMS()
        {
            List<MCUMS> dbMCUMS = new List<MCUMS>();
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT BHNO, CSEQ, BTYPE, IDNO, ACCOUNTTYPE, DCBFLAG, SALES, OTYPE, ODATE, FDATE, DSTATUS, CNACNO, SFCODE, DAYSDATE, DAYRDATE, CONTRA, CRLEVEL, DBLEVEL, RATIO, CRCREDIT, DBCREDIT, CNACOTYPE, CALCN, SELFQUR, EMAIL, NOLMTMINFEE, FEECRFREE, FEECOUNT, KIND, SBHNO, IBNO, CALSIT, CALHF, CNTDTYPE, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                    FROM MCUMS";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("reader has no rows");
                    }
                    while (reader.Read())
                    {
                        var row = new MCUMS();
                        row.BHNO = reader["BHNO"].ToString();
                        row.CSEQ = reader["CSEQ"].ToString();
                        row.BTYPE = reader["BTYPE"].ToString();
                        row.IDNO = reader["IDNO"].ToString();
                        row.ACCOUNTTYPE = reader["ACCOUNTTYPE"].ToString();
                        row.DCBFLAG = reader["DCBFLAG"].ToString();
                        row.SALES = reader["SALES"].ToString();
                        row.OTYPE = reader["OTYPE"].ToString();
                        row.ODATE = reader["ODATE"].ToString();
                        row.FDATE = reader["FDATE"].ToString();
                        row.DSTATUS = reader["DSTATUS"].ToString();
                        row.CNACNO = reader["CNACNO"].ToString();
                        row.SFCODE = reader["SFCODE"].ToString();
                        row.DAYSDATE = reader["DAYSDATE"].ToString();
                        row.DAYRDATE = reader["DAYRDATE"].ToString();
                        row.CONTRA = reader["CONTRA"].ToString();
                        row.CRLEVEL = reader["CRLEVEL"].ToString();
                        row.DBLEVEL = reader["DBLEVEL"].ToString();
                        row.RATIO = reader.IsDBNull(18) ? 0 : Convert.ToDecimal(reader["RATIO"].ToString());
                        row.CRCREDIT = reader.IsDBNull(19) ? 0 : Convert.ToDecimal(reader["CRCREDIT"].ToString());
                        row.DBCREDIT = reader.IsDBNull(20) ? 0 : Convert.ToDecimal(reader["DBCREDIT"].ToString());
                        row.CNACOTYPE = reader["CNACOTYPE"].ToString();
                        row.CALCN = reader["CALCN"].ToString();
                        row.SELFQUR = reader["SELFQUR"].ToString();
                        row.EMAIL = reader["EMAIL"].ToString();
                        row.NOLMTMINFEE = reader["NOLMTMINFEE"].ToString();
                        row.FEECRFREE = reader["FEECRFREE"].ToString();
                        row.FEECOUNT = reader.IsDBNull(27) ? 0 : Convert.ToDecimal(reader["FEECOUNT"].ToString());
                        row.KIND = reader["KIND"].ToString();
                        row.SBHNO = reader["SBHNO"].ToString();
                        row.IBNO = reader["IBNO"].ToString();
                        row.CALSIT = reader["CALSIT"].ToString();
                        row.CALHF = reader["CALHF"].ToString();
                        row.CNTDTYPE = reader["CNTDTYPE"].ToString();
                        row.TRDATE = reader["TRDATE"].ToString();
                        row.TRTIME = reader["TRTIME"].ToString();
                        row.MODDATE = reader["MODDATE"].ToString();
                        row.MODTIME = reader["MODTIME"].ToString();
                        row.MODUSER = reader["MODUSER"].ToString();
                        dbMCUMS.Add(row);
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
            return dbMCUMS;
        }

        //----------------------------------------------------------------------------------
        // function selectMCSRH() - 查詢 MCSRH TABLE
        //----------------------------------------------------------------------------------
        public List<MCSRH> selectMCSRH()
        {
            List<MCSRH> dbMCSRH = new List<MCSRH>();
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT BHNO, CSEQ, STOCK, CNQBAL, CRAQTY, CROQTY, DBAQTY, DBOQTY, CLDATE, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER, CNQBAL_DE
                                    FROM MCSRH";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("reader has no rows");
                    }
                    while (reader.Read())
                    {
                        var row = new MCSRH();
                        row.BHNO = reader["BHNO"].ToString();
                        row.CSEQ = reader["CSEQ"].ToString();
                        row.STOCK = reader["STOCK"].ToString();
                        row.CNQBAL = reader.IsDBNull(3) ? 0 : Convert.ToDecimal(reader["CNQBAL"].ToString());
                        row.CRAQTY = reader.IsDBNull(4) ? 0 : Convert.ToDecimal(reader["CRAQTY"].ToString());
                        row.CROQTY = reader.IsDBNull(5) ? 0 : Convert.ToDecimal(reader["CROQTY"].ToString());
                        row.DBAQTY = reader.IsDBNull(6) ? 0 : Convert.ToDecimal(reader["DBAQTY"].ToString());
                        row.DBOQTY = reader.IsDBNull(7) ? 0 : Convert.ToDecimal(reader["DBOQTY"].ToString());
                        row.CLDATE = reader["CLDATE"].ToString();
                        row.TRDATE = reader["TRDATE"].ToString();
                        row.TRTIME = reader["TRTIME"].ToString();
                        row.MODDATE = reader["MODDATE"].ToString();
                        row.MODTIME = reader["MODTIME"].ToString();
                        row.MODUSER = reader["MODUSER"].ToString();
                        row.CNQBAL_DE = reader.IsDBNull(14) ? 0 : Convert.ToDecimal(reader["CNQBAL_DE"].ToString());
                        dbMCSRH.Add(row);
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
            return dbMCSRH;
        }

        //----------------------------------------------------------------------------------
        // function selectCustomerCNTDTYPE() - 查詢客戶 MCUMS TABLE 當沖資格
        //----------------------------------------------------------------------------------
        public string selectCustomerCNTDTYPE(string BHNONo, string CSEQNo)
        {
            string result = "";
            try
            {
                _sqlConn.Open();
                string sqlQuery = @"SELECT CNTDTYPE
                                    FROM MCUMS
                                    WHERE BHNO = @BHNO AND CSEQ = CSEQ";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery, _sqlConn);
                sqlCmd.Parameters.AddWithValue("@BHNO", BHNONo);
                sqlCmd.Parameters.AddWithValue("@CSEQ", CSEQNo);
                SqlDataReader Reader = sqlCmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    if (Reader.Read())
                    {
                        if (!Reader.IsDBNull(0))
                            result = Reader["CNTDTYPE"].ToString();
                        else
                            result = "N";          //沖銷資格如果是空值, 假設賣單只能沖銷昨日餘額(N)
                    }
                }
                else
                    result = "Y";       //如果查不到客戶的沖銷資格, 假設賣單可以沖銷比該筆早買進的資料(Y)
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
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    sqlQuery = @"SELECT BHNO, TDATE, RDATE, CSEQ, BDSEQ, BDNO, SDSEQ, SDNO, STOCK, CQTY, BPRICE, BFEE, SPRICE, SFEE, TAX, INCOME, COST, PROFIT, ADJDATE, WTYPE, BQTY, SQTY, STINTAX, IOFLAG, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                    FROM dbo.HCNRH
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND STOCK = @STOCK AND TDATE BETWEEN @SDATE AND @EDATE
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT BHNO, TDATE, RDATE, CSEQ, BDSEQ, BDNO, SDSEQ, SDNO, STOCK, CQTY, BPRICE, BFEE, SPRICE, SFEE, TAX, INCOME, COST, PROFIT, ADJDATE, WTYPE, BQTY, SQTY, STINTAX, IOFLAG, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                    FROM dbo.HCNRH
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE
                                    ORDER BY BHNO, CSEQ, STOCK, TDATE";
                }

                sqlCmd.CommandText = sqlQuery;
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
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    sqlQuery = @"SELECT BHNO, TDATE, CSEQ, BDSEQ, BDNO, SDSEQ, SDNO, STOCK, CQTY, BPRICE, BFEE, SPRICE, SFEE, TAX, INCOME, COST, PROFIT, BQTY, SQTY, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                FROM dbo.HCNTD
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND STOCK = @STOCK AND TDATE BETWEEN @SDATE AND @EDATE
                                ORDER BY BHNO, CSEQ, STOCK, TDATE";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT BHNO, TDATE, CSEQ, BDSEQ, BDNO, SDSEQ, SDNO, STOCK, CQTY, BPRICE, BFEE, SPRICE, SFEE, TAX, INCOME, COST, PROFIT, BQTY, SQTY, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                FROM dbo.HCNTD
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE
                                ORDER BY BHNO, CSEQ, STOCK, TDATE";
                }
                sqlCmd.CommandText = sqlQuery;
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
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    sqlQuery = @"SELECT STOCK, TDATE, DSEQ, DNO, TTYPE, BSTYPE, ETYPE, PRICE, QTY, AMT, FEE, TAX, NETAMT
                                FROM dbo.HCMIO
                                WHERE STOCK = @STOCK AND BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT STOCK, TDATE, DSEQ, DNO, TTYPE, BSTYPE, ETYPE, PRICE, QTY, AMT, FEE, TAX, NETAMT
                                FROM dbo.HCMIO
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE BETWEEN @SDATE AND @EDATE";
                }
                sqlCmd.CommandText = sqlQuery;
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
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    sqlQuery = @"SELECT TDATE, BHNO, DSEQ, JRNUM, MTYPE, CSEQ, TTYPE, ETYPE, BSTYPE, STOCK, QTY, PRICE, SALES, ORGIN, MTIME, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER																						
                                FROM dbo.TMHIO
                                WHERE STOCK = @STOCK AND BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE = CONVERT(varchar,(DATEADD(dd, @DATEDIFF, GETDATE())), 112)";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT TDATE, BHNO, DSEQ, JRNUM, MTYPE, CSEQ, TTYPE, ETYPE, BSTYPE, STOCK, QTY, PRICE, SALES, ORGIN, MTIME, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER																						
                                FROM dbo.TMHIO
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE = CONVERT(varchar,(DATEADD(dd, @DATEDIFF, GETDATE())), 112)";
                }
                sqlCmd.CommandText = sqlQuery;
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                sqlCmd.Parameters.AddWithValue("@DATEDIFF", _dateDiff);
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("沒有當日對帳單明細資料");
                    }
                    while (reader.Read())
                    {
                        var row = new TMHIO();
                        row.TDATE = reader["TDATE"].ToString();
                        row.BHNO = reader["BHNO"].ToString();
                        row.CSEQ = reader["CSEQ"].ToString();
                        row.DSEQ = reader["DSEQ"].ToString();
                        row.JRNUM = reader["JRNUM"].ToString();
                        row.MTYPE = reader["MTYPE"].ToString();
                        row.TTYPE = reader["TTYPE"].ToString();
                        row.ETYPE = reader["ETYPE"].ToString();
                        row.BSTYPE = reader["BSTYPE"].ToString();
                        row.STOCK = reader["STOCK"].ToString();
                        row.QTY = Convert.ToDecimal(reader["QTY"].ToString());
                        row.PRICE = Convert.ToDecimal(reader["PRICE"].ToString());
                        row.SALES = reader["SALES"].ToString();
                        row.ORGIN = reader["ORGIN"].ToString();
                        row.MTIME = reader["MTIME"].ToString();
                        row.TRDATE = reader["TRDATE"].ToString();
                        row.TRTIME = reader["TRTIME"].ToString();
                        row.MODDATE = reader["MODDATE"].ToString();
                        row.MODTIME = reader["MODTIME"].ToString();
                        row.MODUSER = reader["MODUSER"].ToString();
                        dbTMHIO.Add(row);
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
            return dbTMHIO;
        }

        //----------------------------------------------------------------------------------
        // function selectTCSIO() - 查詢 TCSIO TABLE 當日現股匯撥檔資料
        //----------------------------------------------------------------------------------
        public List<TCSIO> selectTCSIO(object o)
        {
            root SearchElement = o as root;
            List<TCSIO> dbTCSIO = new List<TCSIO>();
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    sqlQuery = @"SELECT TDATE, BHNO, DSEQ, DNO, CSEQ, STOCK, BSTYPE, QTY, IOFLAG, REMARK, JRNUM, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                FROM dbo.TCSIO
                                WHERE STOCK = @STOCK AND BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE = CONVERT(varchar,(DATEADD(dd, @DATEDIFF, GETDATE())), 112)";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT TDATE, BHNO, DSEQ, DNO, CSEQ, STOCK, BSTYPE, QTY, IOFLAG, REMARK, JRNUM, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER
                                FROM dbo.TCSIO
                                WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND TDATE = CONVERT(varchar,(DATEADD(dd, @DATEDIFF, GETDATE())), 112)";
                }
                sqlCmd.CommandText = sqlQuery;
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);
                sqlCmd.Parameters.AddWithValue("@DATEDIFF", _dateDiff);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("沒有當日現股匯撥檔資料");
                    }
                    while (reader.Read())
                    {
                        var row = new TCSIO();
                        row.TDATE = reader["TDATE"].ToString();
                        row.BHNO = reader["BHNO"].ToString();
                        row.DSEQ = reader["DSEQ"].ToString();
                        row.DNO = reader["DNO"].ToString();
                        row.CSEQ = reader["CSEQ"].ToString();
                        row.STOCK = reader["STOCK"].ToString();
                        row.BSTYPE = reader["BSTYPE"].ToString();
                        row.QTY = Convert.ToDecimal(reader["QTY"].ToString());
                        row.IOFLAG = reader["IOFLAG"].ToString();
                        row.REMARK = reader["REMARK"].ToString();
                        row.JRNUM = reader["JRNUM"].ToString();
                        row.TRDATE = reader["TRDATE"].ToString();
                        row.TRTIME = reader["TRTIME"].ToString();
                        row.MODDATE = reader["MODDATE"].ToString();
                        row.MODTIME = reader["MODTIME"].ToString();
                        row.MODUSER = reader["MODUSER"].ToString();
                        dbTCSIO.Add(row);
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
            return dbTCSIO;
        }

        /// <summary>
        /// 查詢 TCNTD TABLE
        /// </summary>
        /// <param name="o">查詢元素</param>
        /// <returns>TCNTD List</returns>
        public List<TCNTD> selectTCNTD(object o)
        {
            root SearchElement = o as root;
            List<TCNTD> dbTCNTD = new List<TCNTD>();
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    sqlQuery = @"SELECT TDATE, BHNO, BDSEQ, BDNO, SDSEQ, SDNO, CSEQ, STOCK, CQTY, BPRICE, BQTY, BFEE, SPRICE, SQTY, SFEE, TAX, INCOME, COST, PROFIT, STINTAX, JRNUM, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER																
                                    FROM dbo.TCNTD
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ AND STOCK = @STOCK";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT TDATE, BHNO, BDSEQ, BDNO, SDSEQ, SDNO, CSEQ, STOCK, CQTY, BPRICE, BQTY, BFEE, SPRICE, SQTY, SFEE, TAX, INCOME, COST, PROFIT, STINTAX, JRNUM, TRDATE, TRTIME, MODDATE, MODTIME, MODUSER																
                                    FROM dbo.TCNTD
                                    WHERE BHNO = @BHNO AND CSEQ = @CSEQ";
                }

                sqlCmd.CommandText = sqlQuery;
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("沒有現股當沖指定沖銷資料");
                    }
                    while (reader.Read())
                    {
                        var row = new TCNTD();
                        row.TDATE = reader["TDATE"].ToString();
                        row.BHNO = reader["BHNO"].ToString();
                        row.BDSEQ = reader["BDSEQ"].ToString();
                        row.BDNO = reader["BDNO"].ToString();
                        row.SDSEQ = reader["SDSEQ"].ToString();
                        row.SDNO = reader["SDNO"].ToString();
                        row.CSEQ = reader["CSEQ"].ToString();
                        row.STOCK = reader["STOCK"].ToString();
                        row.CQTY = Convert.ToDecimal(reader["CQTY"].ToString());
                        row.BPRICE = Convert.ToDecimal(reader["BPRICE"].ToString());
                        row.BQTY = Convert.ToDecimal(reader["BQTY"].ToString());
                        row.BFEE = Convert.ToDecimal(reader["BFEE"].ToString());
                        row.SPRICE = Convert.ToDecimal(reader["SPRICE"].ToString());
                        row.SQTY = Convert.ToDecimal(reader["SQTY"].ToString());
                        row.SFEE = Convert.ToDecimal(reader["SFEE"].ToString());
                        row.TAX = Convert.ToDecimal(reader["TAX"].ToString());
                        row.INCOME = Convert.ToDecimal(reader["INCOME"].ToString());
                        row.COST = Convert.ToDecimal(reader["COST"].ToString());
                        row.PROFIT = Convert.ToDecimal(reader["PROFIT"].ToString());
                        row.STINTAX = Convert.ToDecimal(reader["STINTAX"].ToString());
                        row.JRNUM = reader["JRNUM"].ToString();
                        row.TRDATE = reader["TRDATE"].ToString();
                        row.TRTIME = reader["TRTIME"].ToString();
                        row.MODDATE = reader["MODDATE"].ToString();
                        row.MODTIME = reader["MODTIME"].ToString();
                        row.MODUSER = reader["MODUSER"].ToString();

                        dbTCNTD.Add(row);
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
            return dbTCNTD;
        }

        /// <summary>
        /// 查詢 T210 Table
        /// </summary>
        /// <param name="o">查詢元素</param>
        /// <returns>T210 List</returns>
        public List<T210> selectT210(object o)
        {
            root SearchElement = o as root;
            List<T210> dbT210 = new List<T210>();
            string sqlQuery = "";
            try
            {
                _sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = _sqlConn;
                if (!string.IsNullOrWhiteSpace(SearchElement.stockSymbol))
                {
                    sqlQuery = @"SELECT TDate, BhNo, CSeq, DSEQ, Stock, Price, TrDate, TrTime, ModDate, ModTime, ModUser																
                                    FROM dbo.T210
                                    WHERE BhNo = @BHNO AND CSeq = @CSEQ AND Stock = @STOCK";
                    sqlCmd.Parameters.AddWithValue("@STOCK", SearchElement.stockSymbol);
                }
                else
                {
                    sqlQuery = @"SELECT TDate, BhNo, CSeq, DSEQ, Stock, Price, TrDate, TrTime, ModDate, ModTime, ModUser																
                                    FROM dbo.T210
                                    WHERE BhNo = @BHNO AND CSeq = @CSEQ";
                }

                sqlCmd.CommandText = sqlQuery;
                sqlCmd.Parameters.AddWithValue("@BHNO", SearchElement.bhno);
                sqlCmd.Parameters.AddWithValue("@CSEQ", SearchElement.cseq);

                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("沒有現股當沖指定沖銷資料");
                    }
                    while (reader.Read())
                    {
                        var row = new T210();
                        row.TDate = reader["TDate"].ToString();
                        row.BhNo = reader["BhNo"].ToString();
                        row.CSeq = reader["CSeq"].ToString();
                        row.DSEQ = reader["DSEQ"].ToString();
                        row.Stock = reader["Stock"].ToString();
                        row.Price = Convert.ToDecimal(reader["Price"].ToString());
                        row.TrDate = reader["TrDate"].ToString();
                        row.TrTime = reader["TrTime"].ToString();
                        row.ModDate = reader["ModDate"].ToString();
                        row.ModTime = reader["ModTime"].ToString();
                        row.ModUser = reader["ModUser"].ToString();
                        dbT210.Add(row);
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
            return dbT210;
        }
    }
}
