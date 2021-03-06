﻿using Quick.OwinMVC.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Owin;
using Quick.OwinMVC;
using Quick.OwinMVC.Routing;
using SvnManage.Middleware;
using Quick.OwinMVC.Localization;
using System.Threading.Tasks;

namespace SvnManage.Controller
{
    /// <summary>
    /// 登录控制器
    /// </summary>
    [Route("login")]
    public class LoginController : ApiController
    {
        [TextResource]
        public enum Texts
        {
            [Text("用户名密码验证失败!")]
            ERROR_USER_PASSWORD_INCORRECT
        }

        protected override object doGet(IOwinContext context)
        {
            var rep = context.Response;
            context.GetSession().Clear();
            return (ApiResult.Success());
        }

        protected override object doPost(IOwinContext context)
        {
            var req = context.Request;
            var session = context.GetSession();
            var formData = context.GetFormData();
            var arg_password = formData["password"];
            if (arg_password == null)
            {
                return null;
            }
            String arg_password_str = Encoding.UTF8.GetString(Convert.FromBase64String(arg_password));
            String[] args = arg_password_str.Split(':');
            var account = args[0].Trim();
            var password = args[1];

            if (Svn.ApiController.Instance.Check(account, password))
            {
                session[LoginMiddleware.LOGINED_USER_KEY] = account;
                return ApiResult.Success();
            }
            return ApiResult.Error(context.GetText(Texts.ERROR_USER_PASSWORD_INCORRECT));
        }
    }
}
