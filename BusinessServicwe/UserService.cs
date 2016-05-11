using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Microsoft.Xrm.Sdk;

namespace BusinessService
{
    public class UserService
    {
        public User CheckUser(string userName, string passWord)
        {
            ConvertClass<User> convertClass = new ConvertClass<User>();
            User user = null;

            SqlParameter[] sps = new SqlParameter[2];
            sps[0] = new SqlParameter("@loginname", userName);
            sps[1] = new SqlParameter("@password", passWord);
            DataSet st = new DataSet();
            st = SqlHelper.ExecuteDataset(SqlConnect.CRM_ADDON_ConnectString, CommandType.StoredProcedure,
                "usp_protal_checkuser", sps);
            if (st.Tables[0].Rows.Count > 0)
            {
                user = convertClass.ToT(st.Tables[0].Rows[0]);
            }

            return user;

            //DataSet ds = SqlHelper.ExecuteDataset(SqlConnect.CRM_MSCRM_ConnectString, CommandType.Text,
            //"SELECT * from account where getdate() between expires_from and expires_to");

            //return userName == "admin" && passWord == "123456";
        }


        public User GetUser(string userId)
        {
            ConvertClass<User> convertClass = new ConvertClass<User>();
            User user = null;


            DataSet st = SqlHelper.ExecuteDataset(SqlConnect.CRM_ADDON_ConnectString, CommandType.StoredProcedure,
                "usp_protal_GetUserInfo", new SqlParameter("@userId", userId));
            if (st.Tables[0].Rows.Count > 0)
            {
                user = convertClass.ToT(st.Tables[0].Rows[0]);
            }

            return user;
        }


        public SickPeople GetSickPeople(string userId)
        {
            ConvertClass<SickPeople> convertClass = new ConvertClass<SickPeople>();
            SickPeople sickPeople = null;


            DataSet st = SqlHelper.ExecuteDataset(SqlConnect.CRM_ADDON_ConnectString, CommandType.StoredProcedure,
                "usp_protal_GetSickPeople", new SqlParameter("@userId", userId));
            if (st.Tables[0].Rows.Count > 0)
            {
                sickPeople = convertClass.ToT(st.Tables[0].Rows[0]);
            }

            return sickPeople;
        }

        public string ChangePassword(string userId,string phone)
        {
            Entity letter=new Entity("letter");
            letter["slc_tonumber"] = phone;
            letter["to"] = new EntityReference("contact", new Guid(userId));
            letter["slc_messagetype"] = new OptionSetValue(4);
            letter["subject"] = "修改密码通知短信";
            Random rad = new Random();//实例化随机数产生器rad；
            int value = rad.Next(1000, 10000);//用rad生成大于等于1000，小于等于9999的随机数；
            letter["slc_content"] = value.ToString(CultureInfo.InvariantCulture);
            CrmService.OrgService.Create(letter);
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
