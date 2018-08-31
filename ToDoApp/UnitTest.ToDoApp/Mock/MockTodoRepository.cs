using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Entities;


namespace UnitTest.ToDoApp.Mock
{
    class MockTodoRepository : ITodoRepository
    {
        public void dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This list will be returned when getAllTodo method is invoked from Test class
        /// </summary>
        /// <returns></returns>
        public List<ToDo> getAllTodo()
        {
            var testToDo = new List<ToDo>();
            testToDo.Add(new ToDo { SlNo = 1, Item = "Item1", Description = "Desc Item1" });
            testToDo.Add(new ToDo { SlNo = 2, Item = "Item2", Description = "Desc Item2" });
            testToDo.Add(new ToDo { SlNo = 3, Item = "Item3", Description = "Desc Item3" });
            testToDo.Add(new ToDo { SlNo = 4, Item = "Item4", Description = "Desc Item4" });

            return testToDo;
        }

        public ToDo getTodoById(int id)
        {
           return new ToDo { SlNo = 2, Item = "Item2", Description = "Desc Item2" };
        }

        public int insert(ToDo todo)
        {
           return 1;
        }

        public int remove(ToDo todo)
        {
            return 1; 
        }

        public int save(ToDo todo)
        {
            return 1;
        }
    }
}
