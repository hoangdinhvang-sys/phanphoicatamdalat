using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetS.Common
{
    public class CookieProvider
    {
        public static void Set(string key, string value, DateTime expires)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[key] ?? new HttpCookie(key);
            cookie.Value = value;

            cookie.Expires = expires;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void Set(string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[key] ?? new HttpCookie(key);
            cookie.Value = value;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string Get(string key, HttpContext context = null)
        {
            if (context == null)
                context = HttpContext.Current;
            if (context != null && context.Handler != null)
            {
                return context.Request.Cookies[key] != null ? context.Request.Cookies[key].Value : null;
                //return context.Request.Cookies[key] != null ? context.Request.Cookies[key][key] : null;
            }
            return null;
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                HttpCookie cookie = new HttpCookie(key);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void RemoveAlls()
        {
            foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
            {
                Remove(key);
            }
        }
    }
}