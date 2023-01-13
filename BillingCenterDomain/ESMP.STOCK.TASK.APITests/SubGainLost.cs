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
    public class SubGainLost:GainLost
    {
        public static List<unoffset_qtype_sum> SubSearchSum(List<TCNUD> TCNUDList, Dictionary<string, List<Symbol>> Quote_Dic)
        {
            return SearchSum(TCNUDList, Quote_Dic);
        }
        public static List<unoffset_qtype_sum> SubSearchSum(List<TCRUD> TCRUDList, Dictionary<string, List<Symbol>> Quote_Dic)
        {
            return SearchSum(TCRUDList, Quote_Dic);
        }
        public static List<unoffset_qtype_sum> SubSearchSum(List<TDBUD> TDBUDList, Dictionary<string, List<Symbol>> Quote_Dic)
        {
            return SearchSum(TDBUDList, Quote_Dic);
        }
        public static unoffset_qtype_accsum SubSearchAccSum(List<unoffset_qtype_sum> sumList)
        {
            return SearchAccSum(sumList);
        }
    }
}
