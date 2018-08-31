using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using ToDoApp.Utility;

namespace ToDoApp.WebApi_Exception
{
    public class TodoCustomExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// To Do custom exception for unhandled exceptions
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            var res = actionExecutedContext.Exception.Message;
            Logger.WriteLog(res + ": Stack Trace :" + string.Format(actionExecutedContext.Exception.StackTrace));
            //Check the Exception Type
            if (actionExecutedContext.Exception is TodoCustomException)
            {
                //The Response Message Set by the Action During Execution
                processException(actionExecutedContext, res);
            }
            else
            {
                //unhandled exception
                processException(actionExecutedContext, res);
            }
        }

        /// <summary>
        /// Method to process the exception and add the paramteres to the error response
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="res"></param>
        private static void processException(HttpActionExecutedContext actionExecutedContext, string res)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(res),
                ReasonPhrase = res + " Please contact system admin"
            };
            actionExecutedContext.Response = response;
        }
    }
}