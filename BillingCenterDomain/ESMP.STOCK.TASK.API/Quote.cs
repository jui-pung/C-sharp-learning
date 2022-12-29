using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ESMP.STOCK.TASK.API
{
    public class Quote
    {
        /// <summary>
        /// 依照給定股票代號列表 查詢Quote站台資料
        /// </summary>
        /// <param name="stocks">股票代號列表</param>
        /// <returns> Dictionary<string, List<Symbol>> </returns>
        public static Dictionary<string, List<Symbol>> GetQuoteDic(string[] stocks)
        {
            int maxStockQty = 250;                  //允許查詢股票參數數量最大值(2048-47(strUrl.Length)/7(股票6位加逗號))
            int currStockQty = stocks.Length;       //目前輸入查詢股票參數數量
            string[] quoteResponse = new string[currStockQty / maxStockQty + 1];        //查詢結果xml字串陣列 (陣列大小為分批查詢次數)
            Dictionary<string, List<Symbol>> dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            string strUrl = "http://10.10.56.182:8080/Quote/Stock.jsp?stock=";
            
            //一次查詢完成
            if (currStockQty <= maxStockQty)
            {
                string stockQuery = string.Empty;
                stockQuery = string.Join(",", stocks);
                Console.WriteLine(strUrl.Length + stockQuery.Length);
                quoteResponse[0] = SearchQuote(strUrl + stockQuery);
            }
            //分次查詢 (處理url長度限制問題)
            else
            {
                int index = 0;
                for (int i = 0; i < currStockQty; i = i + maxStockQty)
                {
                    string stockQuery = string.Empty;
                    if (i + maxStockQty < currStockQty)
                        stockQuery = string.Join(",", stocks, i, maxStockQty);
                    else
                        stockQuery = string.Join(",", stocks, i, currStockQty - i);
                    quoteResponse[index] = SearchQuote(strUrl + stockQuery);
                    index++;
                }
            }
            foreach (var item in quoteResponse)
            {
                List<Symbol> currSymbolList = new List<Symbol>();
                if (string.IsNullOrEmpty(item))
                {
                    Console.WriteLine("Quote站台 url回應為空");
                }
                else
                {
                    Symbols symbols = new Symbols();
                    XmlSerializer ser = new XmlSerializer(typeof(Symbols));
                    Symbols obj = (Symbols)ser.Deserialize(new StringReader(item));
                    currSymbolList = obj.Symbol;
                }
                symbolList = symbolList.Concat(currSymbolList).ToList();
            }
            dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            return dic;
        }

        /// <summary>
        /// 取得要求的url回應
        /// </summary>
        /// <param name="strUrl">請求的url</param>
        /// <returns>傳回來自url的回應(xml格式)</returns>
        private static string SearchQuote(string strUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 30000;

            string result = "";
            //取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                Console.WriteLine("Content length is {0}", response.ContentLength);
                Console.WriteLine("Content type is {0}", response.ContentType);
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                response.Close();
            }
            return result;
        }
    }
}
