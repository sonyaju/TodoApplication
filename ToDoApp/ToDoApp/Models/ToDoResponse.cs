using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoApp.Models;

namespace ToDoApp.Models
{
    /// <summary>
    /// To do response body
    /// </summary>
    public class ResponseBody
    {
        public List<ToDo> todo { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class TodoResponse
    {
        public ResponseHeader responseHeader { get; set; }
        public ResponseBody responseBody { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}