using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace APIGatewayDemo.Common.Middlewares
{
    /// <summary>
    /// 【Middleware】Api接口签名验证
    /// </summary>
    public class WapSignatureValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public WapSignatureValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string sign = context.Request.Headers["sign"];
            if (string.IsNullOrEmpty(sign))
            {
                context.Response.StatusCode = 403; //禁止访问

                return;
            }

            await _next(context);
        }
    }
}
