using System.Collections.Generic;
using System.Linq;
using ToDoApp.Constants;
using ToDoApp.Models;

namespace ToDoApp.Utility
{
    public static class ValidateUtility
    {

        /// <summary>
        /// Method to check if item already exist in the To Do list for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if id exist, false if value doesnt exist</returns>
        public static bool ToDoIdExists(List<ToDo> toDo, int id)
        {
            return toDo.Count(e => e.SlNo == id) > 0;
        }

        /// <summary>
        /// Method to check if item and same description already exist in the database
        /// </summary>
        /// <param name="toDoList"></param>
        /// <param name="todo"></param>
        /// <returns>returns True if item exists</returns>
        public static bool ToDoExists(List<ToDo> toDoList, ToDo todo)
        {
            bool isExist = false;
            foreach(var lstToDo in toDoList)
            {
                if (lstToDo.Item.Equals(todo.Item)&&lstToDo.Description.Equals(todo.Description))
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }

        /// <summary>
        /// Validate the input to see if it is empty or not
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        public static List<Error> ValidateInput(ToDo todo)
        {
            List<Error> errorList = new List<Error>();
            
            //check if  item is null
             if (null == todo || string.IsNullOrWhiteSpace(todo.Item))
            {
                errorList.Add(new Error(AppConstants.ITEM_EMPTY_MSG, AppConstants.ITEM_EMPTY));
            }

            //check if description is null
            if (null == todo || string.IsNullOrWhiteSpace(todo.Description))
            {
                errorList.Add(new Error(AppConstants.DESCRIPTION_EMPTY_MSG, AppConstants.DESCRIPTION_EMPTY));
            }
            return errorList;
        }
    }
}