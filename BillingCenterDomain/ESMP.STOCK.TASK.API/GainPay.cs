using ESMP.STOCK.FORMAT.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.TASK.API
{
    public class GainPay
    {
        string QTYPE, BHNO, CSEQ, SDATE, EDATE;                             //使用者於Form輸入之欄位值
        SqlSearch sqlSearch = new SqlSearch();                              //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)                                           
        List<profit_sum> sumList = new List<profit_sum>();                  //自訂profit_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用
        List<profit_accsum> accsumList = new List<profit_accsum>();         //自訂profit_accsum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

        //--------------------------------------------------------------------------------------------
        //function getFormField() - 取得使用者於Form1輸入的欄位值 - comboBoxQTYPE, txtBHNO, txtCSEQ,
        //                                                          txtSDATE, txtEDATE
        //--------------------------------------------------------------------------------------------
        public void getFormField(string comboBoxQTYPE, string txtBHNO, string txtCSEQ, string txtSDATE, string txtEDATE)
        {
            QTYPE = comboBoxQTYPE;
            BHNO = txtBHNO;
            CSEQ = txtCSEQ;
            SDATE = txtSDATE;
            EDATE = txtEDATE;
        }

        //--------------------------------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //--------------------------------------------------------------------------------------------
        public string searchSerilizer(int type)
        {
            var root = new root()
            {
                qtype = QTYPE,
                bhno = BHNO,
                cseq = CSEQ,
                sdate = SDATE,
                edate = EDATE
            };
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(root));
                    serializer.Serialize(stringwriter, root);
                    return stringwriter.ToString();
                }
            }
            else if (type == 1)
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(root, settings);
                return jsonString;
            }
            return "";
        }

        //--------------------------------------------------------------------------------------------
        // function GetElement() - 取得xml格式字串 Element
        //--------------------------------------------------------------------------------------------
        public object GetElement(string Content, int type)
        {
            root root = new root();
            if (type == 0)
            {
                //建立serializer物件,並指定反序列化物件的型別(root)
                XmlSerializer ser = new XmlSerializer(typeof(root));
                //反序列化XML(obj為反序列化的型別的物件變數)
                root obj = (root)ser.Deserialize(new StringReader(Content));
                return obj;
            }
            else if (type == 1)
            {
                root obj = JsonConvert.DeserializeObject<root>(Content);
                return obj;
            }
            return root;
        }

        //--------------------------------------------------------------------------------------------
        // function searchDetails() - 計算取得 查詢回復階層三的個股明細資料 (賣出)
        //--------------------------------------------------------------------------------------------
        public List<profit_detail_out> searchDetails(List<profit_detail_out> detailList)
        {
            //判斷TDATE、SDSEQ、SDNO欄位是否相同 相同->資料加總
            var lst_detail = detailList.GroupBy(d => new { d.tdate, d.dseq, d.dno }).Select(
            g => new profit_detail_out
            {
                stock = g.First().stock,
                tdate = g.First().tdate,
                dseq = g.First().dseq,
                dno = g.First().dno,
                mqty = g.First().mqty,
                cqty = g.Sum(s => s.cqty),
                mprice = g.First().mprice,
                cost = g.Sum(s => s.cost),
                income = g.Sum(s => s.income),
                netamt = g.Sum(s => s.netamt),
                fee = g.Sum(s => s.fee),
                tax = g.Sum(s => s.tax),
                wtype = g.First().wtype,
                profit = g.Sum(s => s.profit),
                ttypename2 = g.First().ttypename2
            }).ToList();

            //計算計算明細資料(賣出)的成交價金、報酬率
            lst_detail.ForEach(x => x.mamt = (x.cqty * Convert.ToDecimal(x.mprice)).ToString());
            lst_detail.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");

            return lst_detail;
        }

        //--------------------------------------------------------------------------------------------
        // function searchDetails_B() - 計算取得 查詢回復階層三的個股明細資料 (買入)
        //--------------------------------------------------------------------------------------------
        public List<profit_detail> searchDetails_B(List<profit_detail> detailList)
        {
            //計算計算明細資料(買入)的成交價金、報酬率
            detailList.ForEach(x => x.mamt = (x.cqty * Convert.ToDecimal(x.mprice)).ToString());
            detailList.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
            return detailList;
        }

        //--------------------------------------------------------------------------------------------
        // function searchSum() - 計算取得 查詢回復階層二 個股已實現損益
        //--------------------------------------------------------------------------------------------
        public List<profit_sum> searchSum(List<profit_detail_out> detailList)
        {
            //從個股明細資料 (賣出)List取得
            foreach (var item in detailList)
            {
                var row = new profit_sum();
                row.tdate = item.tdate;
                row.dseq = item.dseq;
                row.dno = item.dno;
                row.stock = item.stock;
                row.stocknm = sqlSearch.selectStockName(item.stock);
                row.cqty = item.cqty;
                row.mprice = item.mprice;
                row.fee = item.fee;
                row.tax = item.tax;
                row.cost = item.cost;
                row.income = item.income;
                row.profit = item.profit;
                row.ttypename2 = item.ttypename2;
                row.profit_detail_out = item;
                sumList.Add(row);
            }

            sumList.ForEach(x => x.bhno = BHNO);
            sumList.ForEach(x => x.cseq = CSEQ);
            sumList.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");

            return sumList;
        }

        //--------------------------------------------------------------------------------------------
        // function searchAccSum() - 計算取得 查詢回復階層一 帳戶已實現損益
        //--------------------------------------------------------------------------------------------
        public List<profit_accsum> searchAccSum(List<profit_sum> sumList)
        {
            var row = new profit_accsum();
            row.cqty = sumList.Sum(x => x.cqty);
            row.cost = sumList.Sum(x => x.cost);
            row.income = sumList.Sum(x => x.income);
            row.profit = sumList.Sum(x => x.profit);
            row.fee = sumList.Sum(x => x.fee);
            row.tax = sumList.Sum(x => x.tax);
            accsumList.Add(row);

            accsumList.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");

            //依照股票代號、賣委託書號 排序sumList 並將List內容存放到accsumList的List<profit_sum> profit_sum
            List<profit_sum> sortedList = sumList.OrderBy(x => x.stock).ThenBy(n => n.dseq).ThenBy(n => n.dno).ToList();
            accsumList.ForEach(x => x.profit_sum = sortedList);

            return accsumList;
        }

        //--------------------------------------------------------------------------------------------
        //function resultListSerilizer() - 將QTYPE"0002"查詢結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        public string resultListSerilizer(List<profit_detail> detailList, List<profit_detail_out> detailListOut, int type)
        {
            //依照股票代號、賣委託書號 排序detailList
            List<profit_detail> sortedList = detailList.OrderBy(x => x.stock).ThenBy(n => n.sdseq).ThenBy(n => n.sdno).ToList();
            foreach (var item in accsumList)
            {
                //處理第三階層反序列內容 
                int index = 0;
                for (int i = 0; i < item.profit_sum.Count; i++)
                {
                    
                    //初始化 profit_sum[].profit_detail 清單長度為detaillist長度
                    List<profit_detail> initialization = new List<profit_detail>();
                    int count = 0;
                    while (count < sortedList.Count)
                    {
                        initialization.Add(null);
                        count++;
                    }
                    item.profit_sum[i].profit_detail = initialization;

                    for (int j = index; j < sortedList.Count; j++)
                    {
                        //合併第三階層相同賣出委託書號、賣出分單號、交易日期 到第二階層
                        if (item.profit_sum[i].dseq.Equals(sortedList[j].sdseq) && item.profit_sum[i].dno.Equals(sortedList[j].sdno) && item.profit_sum[i].tdate.Equals(sortedList[j].sdate))
                        {
                            item.profit_sum[i].profit_detail[j] = sortedList[j];
                            Console.WriteLine("profit_sum[" + i + "].profit_detail[" + j + "] Add  " + item.profit_sum[i].dseq + " " + item.profit_sum[i].dno);
                            Console.WriteLine("detailList[" + j + "] Add  " + sortedList[j].sdseq + " " + sortedList[j].sdno);
                        }
                        else
                        {
                            index = j;
                            break;
                        }
                    }

                    item.profit_sum[i].profit_detail.RemoveAll(s => s == null);
                }

                //透過profit_accsum自訂類別反序列 -> accsumSer
                var accsumSer = new profit_accsum()
                {
                    errcode = "0000",
                    errmsg = "成功",
                    cqty = item.cqty,
                    cost = item.cost,
                    income = item.income,
                    profit = item.profit,
                    pl_ratio = item.pl_ratio,
                    fee = item.fee,
                    tax = item.tax,
                    profit_sum = item.profit_sum
                };
                //序列化為xml格式字串
                if (type == 0)
                {
                    using (var stringwriter = new System.IO.StringWriter())
                    {
                        var serializer = new XmlSerializer(typeof(profit_accsum));
                        serializer.Serialize(stringwriter, accsumSer);
                        return stringwriter.ToString();
                    }
                }
                //序列化為json格式字串
                else if (type == 1)
                {
                    var settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    string jsonString = JsonConvert.SerializeObject(accsumSer, settings);
                    return jsonString.ToString();
                }
            }
            return string.Empty;
        }

        //--------------------------------------------------------------------------------------------
        //function resultErrListSerilizer() - 將查詢失敗結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        public string resultErrListSerilizer(int type)
        {
            //透過profit_accsum自訂類別反序列 -> accsumSer
            var accsumSer = new profit_accsum()
            {
                errcode = "0001",
                errmsg = "查詢失敗",
            };
            //序列化為xml格式字串
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(unoffset_qtype_accsum));
                    serializer.Serialize(stringwriter, accsumSer);
                    return stringwriter.ToString();
                }
            }
            //序列化為json格式字串
            else if (type == 1)
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(accsumSer, settings);
                return jsonString.ToString();
            }
            return string.Empty;
        }
    }
}
