using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.API
{
    public class ToolKit
    {
        public enum Parameters
        {
            BHNO,
            CSEQ
        }
        /// <summary>
        /// 檢核必填欄位是否為空
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        public static void CheckQueryParameter(string value, Parameters parameter)
        {
            if (parameter == Parameters.BHNO && value == "")
            {
                Console.WriteLine("Error");
            }
        }
    }
}
