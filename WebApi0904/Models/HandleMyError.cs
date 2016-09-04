using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace WebApi0904.Models
{
    public class HandleMyErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new MyError()
            {
                Error_Code = 1,
                Error_Message = actionExecutedContext.Exception.Message
            });
        }
    }
}