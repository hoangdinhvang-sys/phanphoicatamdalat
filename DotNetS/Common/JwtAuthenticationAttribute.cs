using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace DotNetS.Common
{
    public class JwtAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        private EStatusLogin Status { get; set; }
        private bool isLoginPage { get; set; }

        public JwtAuthenticationAttribute(bool isLoginPage = false)
        {
            this.isLoginPage = isLoginPage;
        }
        //check
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            string accessKey = filterContext.HttpContext.Request.Cookies[Cookies.ACCESS_KEY]?.Value;
            if (string.IsNullOrEmpty(accessKey))
            {
                Status = EStatusLogin.WrongAccessKey;
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                if (!isLoginPage)
                {
                    var token = filterContext.HttpContext.Request.Cookies[Cookies.TOKEN]?.Value;
                    if (string.IsNullOrEmpty(token))
                    {
                        Status = EStatusLogin.NotLoggedIn;
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                    else
                    {
                        Status = ValidateToken(token, accessKey);
                        if (Status != EStatusLogin.Correct)
                        {
                            filterContext.Result = new HttpUnauthorizedResult();
                        }
                    }
                }
            }
        }

        //redirect
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                if (Status == EStatusLogin.WrongAccessKey)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        CreateResponseUnauthorizedResult(ref filterContext);
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("/Account/Login");
                    }
                }
                else if (Status == EStatusLogin.NotLoggedIn)
                {
                    if (!isLoginPage)
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            CreateResponseUnauthorizedResult(ref filterContext);
                            //throw new Exception(Message.NotLogin);
                            filterContext.Result = new Http401Result();
                        }
                        else
                        {
                            string returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                            string redirectResult = "";
                            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/")
                                redirectResult = "/account/login";
                            else
                                redirectResult = "/account/login?returnUrl=" + returnUrl;
                            filterContext.Result = new RedirectResult(redirectResult);
                        }
                    }
                }
            }

        }

        private static void CreateResponseUnauthorizedResult(ref AuthenticationChallengeContext filterContext)
        {
            filterContext.HttpContext.Response.ContentType = "application/json";
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(new { status = "401", messsage = "Cannot find the page" });
            filterContext.HttpContext.Response.Write(json);
            filterContext.HttpContext.Response.End();
        }

        private static EStatusLogin ValidateToken(string token, string accessKey)
        {
            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated)
                return EStatusLogin.NotLoggedIn;


            var accessKeyClaim = identity.FindFirst(ClaimTypeConst.ACCESS_KEY);

            if (string.IsNullOrEmpty(accessKeyClaim?.Value) || accessKey != accessKeyClaim?.Value)
                return EStatusLogin.WrongAccessKey;

            var userIdClaim = identity.FindFirst(ClaimTypeConst.USER_ID);
            if (string.IsNullOrEmpty(userIdClaim?.Value))
                return EStatusLogin.NotLoggedIn;

            return EStatusLogin.Correct;
        }
    }
}