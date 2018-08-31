using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using ToDoApp.Models;

namespace ToDoApp.Filters
{
    /// <summary>
    /// Converts NotImplementedException exceptions into HTTP status code 501, Not Implemented
    /// </summary>
    //public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute
    //{
    //    public override void OnException(HttpActionExecutedContext context)
    //    {
    //        if (context.Exception is NotImplementedException)
    //        {
    //            TodoResponse todoResponse = new TodoResponse();
    //            ResponseHeader responseHeader = new ResponseHeader();
    //            ResponseBody responseBody = new ResponseBody();
    //            context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
    //        }
    //    }
    //}

    //public class TodoCustomExceptionFilterAttribute : ExceptionFilterAttribute
    //{
    //    public override void OnException(HttpActionExecutedContext context)
    //    {
    //        //if (context.Exception is NotImplementedException)
    //        {
    //            TodoResponse todoResponse = new TodoResponse();
    //            ResponseHeader responseHeader = new ResponseHeader();
    //            ResponseBody responseBody = new ResponseBody();
    //            context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
    //        }
    //    }
    //}
}