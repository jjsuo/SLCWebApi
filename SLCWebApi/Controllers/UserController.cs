using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Security;
using BusinessEntities;
using BusinessService;

namespace SLCWebApi.Controllers
{
   [RoutePrefix("api/user")]

    public class UserController : ApiController
    {
        [HttpGet]
        [Route("Login")]
        public User Login(string loginName, string strPwd)
        {
            try
            {
                int expiration = 0;
                UserService service = new UserService();
                User userData = service.CheckUser(loginName, strPwd);
                if (userData == null)
                    return null;
                if (string.IsNullOrEmpty(loginName))
                    throw new ArgumentNullException("loginName");
                if (string.IsNullOrEmpty(strPwd))
                    throw new ArgumentNullException("strPwd");

                // 1. 把需要保存的用户数据转成一个字符串。
                string data = null;
                data = userData.UserId;

                // 2. 创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。


                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, loginName, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), false, data);

                // 3. 加密Ticket，变成一个加密的字符串。
                string cookieValue = FormsAuthentication.Encrypt(ticket);

                // 4. 根据加密结果创建登录Cookie
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
                cookie.HttpOnly = true;
                cookie.Secure = FormsAuthentication.RequireSSL;
                cookie.Domain = FormsAuthentication.CookieDomain;
                cookie.Path = FormsAuthentication.FormsCookiePath;
                if (expiration > 0)
                    cookie.Expires = DateTime.Now.AddDays(expiration);

                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new InvalidOperationException();

                // 5. 写登录Cookie
                context.Response.Cookies.Remove(cookie.Name);
                context.Response.Cookies.Add(cookie);
                return userData;
            }
            catch (Exception exception)
            {

                throw;
            }
           
          

        }



       //public string ChangePassword()
    }
}
