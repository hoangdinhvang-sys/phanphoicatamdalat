using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetS.Common
{
    public class Constant
    {
        // login
        public const string USER_INFO = "user_info";
        public const string TOKEN = "token";
        public const string ACCESS_KEY = "access_key";
        public const string MOD_CONST = "mod_const";
        public const string REG_HASH_ID = "reg_hash_id";
        public const string LOGIN_FIELD = "login_field";
    }
    public class ClaimTypeConst
    {
        public const string ACCESS_KEY = "access_key";
        public const string USER_ID = "user_id";
        public const string USER_NAME = "user_name";
        public const string USER_FULLNAME = "user_full_name";
        public const string USER_PHONENUMBER = "user_phone_number";
        public const string USER_CREATEDDATE = "user_created_date";
        public const string USER_GROUPID = "user_group_id";
    }
    public enum EStatusLogin
    {
        WrongAccessKey,
        NotLoggedIn,
        Correct
    }
    public class Cookies
    {
        public const string ACCESS_KEY = "access_key";
        public const string TOKEN = "token";
        public const string LOGIN_FIELD = "login_field";
    }
    public class Session
    {
        // login
        public const string USER_INFO = "user_info";
        public const string MOD_CONST = "mod_const";
        public const string USER = "user";
    }
}