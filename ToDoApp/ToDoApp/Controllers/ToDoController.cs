using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ToDoApp.Models;
using ToDoApp.Constants;
using ToDoApp.Entities;
using WebApi.OutputCache.V2;
using ToDoApp.Filters;
using ToDoApp.Utility;
using ToDoApp.WebApi_Exception;


namespace ToDoApp.Controllers
{
    public class ToDoController : ApiController
    {
        private ITodoRepository _repository;

        /// <summary>
        /// Constructor injection for dependent object
        /// </summary>
        /// <param name="repository"></param>
        public ToDoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        /* //Comment/Uncomment this block to enable in memory cache for 10 seconds
        [CacheOutput(ClientTimeSpan = 10, ServerTimeSpan = 10)]
        */
        /// <summary>
        /// GET Api method to return all the todo items from database
        /// </summary>
        /// <returns>TodoResponse objct</returns>
        /// GET: api/ToDo 
        public TodoResponse GetToDoes()
        {
            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();
            List<ToDo> lstTodo = new List<ToDo>();
            List<Error> errorlist = new List<Error>();
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string statusMessage = AppConstants.Success;
            try
            {
                 lstTodo = _repository.getAllTodo();
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.NotModified;
                statusMessage = AppConstants.Error;
                string strErrorMsg = AppConstants.Error_Message + ex.Message;                
                errorlist.Add(new Error(strErrorMsg, AppConstants.SYSTEM_EXEPTION));
                Logger.WriteLog(strErrorMsg + AppConstants.Stack_Trace + ex.StackTrace);
            }            

            responseHeader.statusMessage = statusMessage;
            responseHeader.statusCode = statusCode;
            responseHeader.error = errorlist;
            responseBody.todo = lstTodo;
            todoResponse.responseHeader = responseHeader;
            todoResponse.responseBody = responseBody;
            return todoResponse;
        }


        /* //Comment/Uncomment this to enable in memory cache for 10 seconds
         [CacheOutput(ClientTimeSpan = 10, ServerTimeSpan = 10)]
         */
        /// <summary>
        /// GET api method to return a particular ToDo item from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// GET: api/ToDo/5
        [ResponseType(typeof(ToDo))]
        public TodoResponse GetToDo(int id)
        {
            
            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();
            List<Error> errorlist = new List<Error>();
            string statusMessage = string.Empty;
            HttpStatusCode statusCode;

            try
            {
                ToDo toDo = _repository.getTodoById(id);
                if (toDo == null)
                {
                    statusMessage = AppConstants.Error;
                    statusCode = HttpStatusCode.NotFound;
                    // multiple values can go into the error message depending on different conditions                   
                    errorlist.Add(new Error(string.Format(AppConstants.TODO_NOT_FOUND_MSG,id), AppConstants.TODO_NOT_FOUND));
                }
                else
                {
                    statusMessage = AppConstants.Success;
                    statusCode = HttpStatusCode.OK;
                    List<ToDo> lstToDo = new List<ToDo>();
                    lstToDo.Add(toDo);
                    responseBody.todo = lstToDo;
                }
            }
            catch (Exception Ex)
            {
                statusCode = HttpStatusCode.NotModified;
                statusMessage = AppConstants.Error;
                string strErrorMsg = AppConstants.Error_Message + Ex.Message;
                Logger.WriteLog(strErrorMsg + AppConstants.Stack_Trace+ Ex.StackTrace);
                errorlist.Add(new Error(strErrorMsg, AppConstants.SYSTEM_EXEPTION));                
            }
            //Write to log file if error is present
            if (errorlist.Count > 0)
            {
                //logging error messaages
                errorlist.ForEach(error => Logger.WriteLog(AppConstants.Error + "in GetToDo() for ID : " + id + AppConstants.Error_Code + error.errorCode + AppConstants.Error_Message + error.errorMessage));
                //foreach (var error in errorlist)
                //{
                //    Logger.WriteLog(AppConstants.Error + "in GetToDo() for ID : " + id + AppConstants.Error_Code + error.errorCode + AppConstants.Error_Message + error.errorMessage);
                //}
            }
            responseHeader.statusCode = statusCode;
            responseHeader.statusMessage = statusMessage;
            responseHeader.error = errorlist;
            todoResponse.responseHeader = responseHeader;
            todoResponse.responseBody = responseBody;          
            return todoResponse;
        }        

       /// <summary>
        /// PUT api method to edit a particular ToDo item in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toDo"></param>
        /// <returns>TodoResponse</returns>
        /// PUT: api/ToDo/5
        [ResponseType(typeof(ToDo))]
        public TodoResponse PutToDo(int id, ToDo toDo)
        {
            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();
            List<Error> errorlist = new List<Error>();
            HttpStatusCode statusCode;
            string statusMessage = string.Empty;
            errorlist = ValidateUtility.ValidateInput(toDo);

            try
            {
                if (!ModelState.IsValid)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    statusMessage = AppConstants.Error;
                    errorlist.Add(new Error(AppConstants.MODAL_STATE_INVALID, AppConstants.BAD_REQUEST));
                }

                //Check if any of the input parameters are empty
                else if (errorlist.Count > 0)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    statusMessage = AppConstants.Error;
                    //add mulitple error if needed to errorlist
                    errorlist.ForEach(e => Logger.WriteLog(e.errorMessage));
                }

                // check if the ids in the request match
                else if (id != toDo.SlNo)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    statusMessage = AppConstants.Error;
                    //add mulitple error if needed to errorlist
                    errorlist.Add(new Error(AppConstants.TODO_ID_MISMATCH_MSG, AppConstants.TODO_ID_MISMATCH));
                }

                // Perform the Edit operation
                else
                {
                    // Database update
                    int  result = _repository.save(toDo);
                    // return value and check the status and handle error
                    if (result > 0)
                    {
                        statusCode = HttpStatusCode.OK;
                        statusMessage = AppConstants.Success;
                    }
                    else
                    {
                        statusCode = HttpStatusCode.InternalServerError;
                        statusMessage = AppConstants.Error;
                        errorlist.Add(new Error(AppConstants.SAVE_FAILED_MSG, AppConstants.SAVE_FAILED));
                    }
                }                
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                // If the Id for Deletion is not present in the database
                if (!ToDoIdExists(id))
                {
                    statusCode = HttpStatusCode.NotFound;
                    statusMessage = AppConstants.Error;
                    string strErrorMsg = AppConstants.Error_Message + dbEx.Message; 
                    errorlist.Add(new Error(strErrorMsg, AppConstants.TODO_NOT_FOUND));

                    Logger.WriteLog(AppConstants.Error_Message + strErrorMsg +
                                    AppConstants.Inner_Exception + dbEx.InnerException +
                                    AppConstants.Stack_Trace + dbEx.StackTrace + AppConstants.Soure + dbEx.Source);
                }
                else
                {
                    // This will be caught and handled in the ToDoCustomException handler
                    throw;
                }
            }

            //Write to log file if error is present
            if (errorlist.Count > 0)
            {
                errorlist.ForEach(error => Logger.WriteLog(AppConstants.Error + " in PutToDo() for ID : " + id + AppConstants.Error_Code
                                   + error.errorCode + AppConstants.Error_Message + error.errorMessage));                
            }
            responseHeader.statusCode = statusCode;
            responseHeader.statusMessage = statusMessage;
            responseHeader.error = errorlist;
            todoResponse.responseHeader = responseHeader;
            responseBody.todo = _repository.getAllTodo();
            todoResponse.responseBody = responseBody;            
            return todoResponse;
        }



        /// <summary>
        /// POST api method to insert a new ToDo item in the database
        /// </summary>
        /// <param name="toDo"></param>
        /// <returns></returns>
        // POST: api/ToDo
        [ResponseType(typeof(ToDo))]
        [HttpPost] // Routing by explicitly specifying the HTTP Action type in the header
        public TodoResponse AddNewToDo(ToDo toDo)
        {
            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();
            HttpStatusCode statusCode;
            List<Error> errorlist = new List<Error>();
            string statusMessage = string.Empty;
            errorlist = ValidateUtility.ValidateInput(toDo);

            try
            {
                if (!ModelState.IsValid)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    statusMessage = AppConstants.Error;
                    errorlist.Add(new Error(AppConstants.MODAL_STATE_INVALID, AppConstants.BAD_REQUEST));
                }

                //Check if any of the input parameters are empty
                else if (errorlist.Count >0)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    statusMessage = AppConstants.Error;
                    errorlist.ForEach(e => Logger.WriteLog(e.errorMessage));
                }

                else
                {
                    // Check if the item with same descrition is already present
                    if (ValidateUtility.ToDoExists(_repository.getAllTodo(),toDo))
                    {
                        statusCode = HttpStatusCode.Conflict;
                        statusMessage = AppConstants.Error;
                        Error error = new Error( AppConstants.TODO_ALREADY_EXIST_MSG, AppConstants.TODO_ALREADY_EXIST);
                        errorlist.Add(error);
                        Logger.WriteLog(AppConstants.TODO_ALREADY_EXIST_MSG);
                    }

                    //If item not present, then insert new data
                    else
                    {                        
                        // return value and check the status and handle error
                        // Database insertion takes place here
                        if (_repository.insert(toDo) > 0)
                        {
                            statusCode = HttpStatusCode.OK;
                            statusMessage = AppConstants.Success;
                        }
                        
                        // If database insertion failed then handle the exception
                        else
                        {
                            statusCode = HttpStatusCode.InternalServerError;
                            statusMessage = AppConstants.Error;
                            errorlist.Add(new Error(AppConstants.INSERT_FAILED_MSG, AppConstants.INSERT_FAILED));
                        }
                    }                    
                }

            }
            catch (DbUpdateException dbEx)
            {
                //If To Do item is already present in database with same id 
                if (ToDoIdExists(toDo.SlNo))
                {
                    statusCode = HttpStatusCode.Conflict;
                    statusMessage = AppConstants.Error;
                    string strErrorMsg = AppConstants.Error_Message + dbEx.Message+ AppConstants.Inner_Exception + dbEx.InnerException +
                                     AppConstants.Stack_Trace + dbEx.StackTrace + AppConstants.Soure + dbEx.Source;
                    Error error = new Error( AppConstants.TODO_ALREADY_EXIST_MSG, AppConstants.TODO_ALREADY_EXIST);
                    errorlist.Add(error);
                    Logger.WriteLog(strErrorMsg);
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    statusMessage = AppConstants.Error;
                    string strErrorMsg = AppConstants.Error_Message + dbEx.Message;
                    errorlist.Add(new Error(AppConstants.INTERNAL_SERVER_ERROR_MSG, AppConstants.INTERNAL_SERVER_ERROR));
                    Logger.WriteLog(AppConstants.Error_Message + strErrorMsg + AppConstants.Inner_Exception + dbEx.InnerException +
                                     AppConstants.Stack_Trace + dbEx.StackTrace + AppConstants.Soure + dbEx.Source);
                }
            }
            
            //Write to log file if error is present
            if (errorlist.Count > 0)
            {
                //foreach (var error in errorlist)
                //{
                //    Logger.WriteLog(AppConstants.Error + " in PostToDo() " +AppConstants.Error_Code
                //                   + error.errorCode + AppConstants.Error_Message + error.errorMessage);
                //}
                errorlist.ForEach(error => Logger.WriteLog(AppConstants.Error + " in PostToDo() " + AppConstants.Error_Code
                                   + error.errorCode + AppConstants.Error_Message + error.errorMessage));
            }
            responseHeader.statusCode = statusCode;
            responseHeader.statusMessage = statusMessage;
            responseHeader.error = errorlist;
            todoResponse.responseHeader = responseHeader;
            responseBody.todo = _repository.getAllTodo();
            todoResponse.responseBody = responseBody;
            return todoResponse;
        }

        /// <summary>
        /// DELETE api method to edit a particular ToDo item in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ToDo/5
        [ResponseType(typeof(ToDo))]
        public TodoResponse DeleteToDo(int id)
        {
            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();
            string statusMessage = string.Empty;
            HttpStatusCode statusCode;
            List<Error> errorlist = new List<Error>();
            try
            {
                ToDo toDo = _repository.getTodoById(id);

                //If the item to delete is not present in database
                if (null == toDo)
                {
                    statusCode = HttpStatusCode.NotFound;
                    statusMessage = AppConstants.Error;
                    Error error = new Error(string.Format(AppConstants.TODO_NOT_FOUND_MSG, id), AppConstants.TODO_NOT_FOUND);                    
                    errorlist.Add(error);
                }
                else
                {
                    // return value and check the status and handle error
                    // Database delete takes place here
                    if (_repository.remove(toDo) > 0)
                    {
                        statusCode = HttpStatusCode.OK;
                        statusMessage = AppConstants.Success;
                    }
                    else
                    {
                        statusCode = HttpStatusCode.InternalServerError;
                        statusMessage = AppConstants.Error;
                        errorlist.Add(new Error(AppConstants.DELETE_FAILED_MSG, AppConstants.DELETE_FAILED));
                    }                   
                }

            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.NotModified;
                statusMessage = AppConstants.Error;
                string strErrorMsg = AppConstants.Error_Message + ex.Message;
                Error error = new Error(strErrorMsg,AppConstants.TODO_DELETE_ERROR);                
                errorlist.Add(error);
                Logger.WriteLog(AppConstants.Error_Message + ex.Message + AppConstants.Inner_Exception + ex.InnerException +
                                    AppConstants.Stack_Trace + ex.StackTrace + AppConstants.Soure + ex.Source);
            }
            //Write to log file if error is present
            if (errorlist.Count > 0)
            {
                errorlist.ForEach(error => Logger.WriteLog(AppConstants.Error + " in DeleteToDo() for ID : " + id + AppConstants.Error_Code
                                    + error.errorCode + AppConstants.Error_Message + error.errorMessage));                
            }
            responseHeader.statusCode = statusCode;
            responseHeader.statusMessage = statusMessage;
            responseHeader.error = errorlist;
            todoResponse.responseHeader = responseHeader;
            responseBody.todo = _repository.getAllTodo();
            todoResponse.responseBody = responseBody;
            return todoResponse;
        }

        /// <summary>
        /// Dispose method to dispose the created objects
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Method to check if item already exist in the database for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if value exist, false if value doesnt exist</returns>
        public bool ToDoIdExists(int id)
        {
            return ValidateUtility.ToDoIdExists(_repository.getAllTodo(), id);
        } 

    }
}