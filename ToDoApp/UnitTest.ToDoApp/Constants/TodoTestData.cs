using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace UnitTest.ToDoApp.Constants
{
    public static class TodoTestData
    {
        public static List<ToDo> GetMockAllToDo()
        {
            List<ToDo> testToDo = new List<ToDo>();
            testToDo.Add(new ToDo { SlNo = 1, Item = "Item1", Description = "Descc Item1" });
            testToDo.Add(new ToDo { SlNo = 2, Item = "Item2", Description = "Descc Item2" });
            testToDo.Add(new ToDo { SlNo = 3, Item = "Item3", Description = "Descc Item3" });
            testToDo.Add(new ToDo { SlNo = 4, Item = "Item4", Description = "Descc Item4" });
            return testToDo;
        }
    }
}
