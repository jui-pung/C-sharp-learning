using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.APITests
{
    public class SubGainPay:GainPay
    {
        public static List<profit_sum> SubSearchSum(List<HCRRH> HCRRH, string BHNO, string CSEQ)
        {
            return SearchSum(HCRRH, BHNO, CSEQ);
        }
        public static List<profit_sum> SubSearchSum(List<HDBRH> HDBRH, string BHNO, string CSEQ)
        {
            return SearchSum(HDBRH, BHNO, CSEQ);
        }
        public static List<profit_sum> SubSearchSum(List<HCDTD> HCDTD, string BHNO, string CSEQ)
        {
            return SearchSum(HCDTD, BHNO, CSEQ);
        }
        public static List<profit_accsum> SubSearchAccSum(List<profit_sum> sumList)
        {
            return SearchAccSum(sumList);
        }
    }
}
