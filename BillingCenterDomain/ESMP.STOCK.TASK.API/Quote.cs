﻿using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
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
        public static Dictionary<string, List<Symbol>> _Quote_Dic = null;
        /// <summary>
        /// 依照給定股票代號列表 查詢Quote站台資料
        /// </summary>
        /// <param name="stockNo">股票代號列表</param>
        /// <returns> Quote_Dic </returns>
        //回傳Symbol
        public static Dictionary<string, List<Symbol>> Quote_Dic(string[] stockNo)
        {
            List<Symbol> SymbolList = new List<Symbol>();
            string strUrl = "http://10.10.56.182:8080/Quote/Stock.jsp?stock=";
            strUrl += string.Join("," , stockNo);
            string content = SearchQuote(strUrl);
            if (string.IsNullOrEmpty(content))
            {
                Console.WriteLine("Quote站台 url回應為空");
            }
            else
            {
                Symbols symbols = new Symbols();
                XmlSerializer ser = new XmlSerializer(typeof(Symbols));
                Symbols obj = (Symbols)ser.Deserialize(new StringReader(content));
                SymbolList = obj.Symbol;
            }
            _Quote_Dic = SymbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            return _Quote_Dic;
        }

        /// <summary>
        /// 取得要求的url回應
        /// </summary>
        /// <param name="strUrl">請求的url</param>
        /// <returns>傳回來自url的回應(xml格式)</returns>
        private static string SearchQuote(string strUrl)
        {
            string responseBody = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);

                // Set some reasonable limits on resources used by this request
                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                // Set credentials to use for this request.
                request.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Console.WriteLine("Content length is {0}", response.ContentLength);
                Console.WriteLine("Content type is {0}", response.ContentType);

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format.
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                responseBody = readStream.ReadToEnd();
                Console.WriteLine("Response stream received.");
                Console.WriteLine(readStream.ReadToEnd());
                response.Close();
                readStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return responseBody;
        }
    }
}
