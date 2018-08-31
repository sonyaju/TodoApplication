using System.Collections.Generic;
using ToDoApp.Models;

namespace ToDoApp.Entities
{
    /// <summary>
    /// Interface method for ToDoRepositry
    /// </summary>
    public interface ITodoRepository
    {
        List<ToDo> getAllTodo();
        ToDo getTodoById(int id);
        int save(ToDo todo);
        int remove(ToDo todo);
        int insert(ToDo todo);
        void dispose();
    }
}