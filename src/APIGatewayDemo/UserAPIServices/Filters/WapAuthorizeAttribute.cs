using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace UserAPIServices.Filters
{
    public class WapAuthorizeAttribute : AuthorizeAttribute
    {
        public WapAuthorizeAttribute()
        {

        }
 
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool exist = actionContext.Request.Headers.TryGetValues("Token", out IEnumerable<string> tokens);
            if (exist)
            {
                string token = tokens.SingleOrDefault();

                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}