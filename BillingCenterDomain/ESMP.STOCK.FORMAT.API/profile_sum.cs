using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.FORMAT.API
{
    public class profile_sum
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public decimal netamt { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; }
        public decimal mqty { get; set; }
        public decimal mamt { get; set; }
        public billSum billSum { get; set; }
        public List<profile> profile { get; set; }
    }
}
