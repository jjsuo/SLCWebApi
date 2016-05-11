using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SqlConnect
    {
        public static string CRM_ADDON_ConnectString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MSCRM_ADDON"].ToString(); 
            }
        }
        public static string CRM_MSCRM_ConnectString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MSCRM_SLC"].ToString();
            }
        }
    }
}
