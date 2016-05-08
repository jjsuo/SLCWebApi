using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
namespace BusinessService
{
    public class UserService
    {
        public bool CheckUser(string userName, string passWord, out User user)
        {
            user = new User() { userName = userName, passWord = passWord };
            return userName == "admin" && passWord == "123456";
        }
    }
}
