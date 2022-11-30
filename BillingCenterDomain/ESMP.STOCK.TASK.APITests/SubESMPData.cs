using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.APITests
{
    public class SubESMPData:ESMPData
    {
        public static List<HCMIO> SubGetHCMIO(List<TCSIO> TCSIOList, List<TMHIO> TMHIOList)
        {
            return GetHCMIO(TCSIOList, TMHIOList);
        }
        public static (List<HCNRH>, List<TCNUD>) SubCurrentStockSell(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            return CurrentStockSell(TCNUDList, HCMIOList);
        }
        public static List<TCNUD> SubAddTCSIO(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            return AddTCSIO(TCNUDList, HCMIOList);
        }
        public static (List<HCMIO>, List<HCNTD>) SubDayTrade(List<HCMIO> HCMIOList)
        {
            return DayTrade(HCMIOList);
        }
        public static List<HCNTD> SubCalculateHCNTD(List<HCNTD> HCNTDList)
        {
            return CalculateHCNTD(HCNTDList);
        }
        public static List<TCNUD> SubAddTMHIOBuy(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            return AddTMHIOBuy(TCNUDList, HCMIOList);
        }
    }
}
