using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Http;
using ToDoApp.Models;

namespace ToDoApp.WebApi_Exception
{
    [Serializable]
    public class TodoCustomException : Exception
    {
        List<Error> errorlist;

        public TodoCustomException(string message)
        : base(message)
        {

        }

        public TodoCustomException(String message, List<Error> errorlist) : base(message)
        {
            this.errorlist = errorlist;
        }
    }    
}