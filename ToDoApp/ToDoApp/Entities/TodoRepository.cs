using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ToDoApp.Models;

namespace ToDoApp.Entities
{
    public class TodoRepository : ITodoRepository
    {
        private DBEntities db; 

        public TodoRepository(DBEntities dbEntity)
        {
            db = dbEntity;
        }

        List<ToDo> ITodoRepository.getAllTodo()
        {
            List<ToDo> lstTodo = db.ToDoes.ToList();
            return lstTodo;
        }

        ToDo ITodoRepository.getTodoById(int id)
        {
            ToDo toDo = db.ToDoes.Find(id);
            return toDo;
        }

        int ITodoRepository.save(ToDo todo)
        {
            db.Entry(todo).State = EntityState.Modified;
            return db.SaveChanges();
        }

        int ITodoRepository.insert(ToDo todo)
        {
            db.ToDoes.Add(todo);
            return db.SaveChanges();
        }

        int ITodoRepository.remove(ToDo todo)
        {
            db.ToDoes.Remove(todo);
            return db.SaveChanges();
        }


        public void dispose()
        {
            db.Dispose();
        }


    }
}