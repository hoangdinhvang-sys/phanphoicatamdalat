using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetS.Common
{
    public class SessionProvider
    {
        public static void Set(string key, object value)
        {
            HttpContext.Current.Session.Add(key, value);
        }

        public static T Get<T>(string key, HttpContext context = null) where T : class
        {
            if (context == null)
                context = HttpContext.Current;
            if (context != null && context.Handler != null)
            {
                return (T)context.Session[key];
            }
            return null;
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session[key] = null;
            }
        }

        public static void RemoveAlls()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}