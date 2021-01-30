using DotNetS.Common;
using DotNetS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetS.Controllers
{
    public class BaseController : Controller
    {
        protected User UserInfo { get { return UserProvider.GetCurrentUser(HttpContext); } }

        protected string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DotNetSConn"].ConnectionString;
            }
        }
    }
}