using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
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

            TodoResponse _todoResponse = new TodoResponse();
            ResponseHeader _responseHeader = new ResponseHeader();
            ResponseBody _responseBody = new ResponseBody();
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
            //TODO: AUTHENTICATION
            //  IQueryable < ToDo >

            List<ToDo> lstTodo = _repository.getAllTodo();

            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();

            responseHeader.statusMessage = AppConstants.Success;
            responseHeader.statusCode = HttpStatusCode.OK;

            responseBody.todo = lstTodo;
            Logger.WriteLog("First log");
            todoResponse.responseHeader = responseHeader;
            todoResponse.responseBody = responseBody;
          //  throw new SyntaxErrorException();
            return todoResponse;
            //  return db.ToDoes;
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
       /* [ResponseType(typeof(ToDo))]
        public TodoResponse GetToDo(int id)
        {
            //TODO: AUTHENTICATION

            ToDo toDo = _repository.getTodoById(id);
            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();
            List<Error> errorlist;
            try
            {
                if (toDo == null)
                {
                    // Creating error response in response header
                    responseHeader.statusMessage = AppConstants.Error;
                    responseHeader.statusCode = HttpStatusCode.NotFound;
                    //move the error message to config or a common constants
                    // multiple values can go into the error message depending on different conditions
                    Error error = new Error(AppConstants.TODO_NOT_FOUND, AppConstants.TODO_NOT_FOUND_MSG);
                    errorlist = new List<Error>();
                    errorlist.Add(error);
                    responseHeader.error = errorlist;
                   // HttpResponseMessage str = Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
                }
                else
                {
                    responseHeader.statusMessage = AppConstants.Success;
                    responseHeader.statusCode = HttpStatusCode.OK;
                    List<ToDo> lstToDo = new List<ToDo>();
                    lstToDo.Add(toDo);
                    responseBody.todo = lstToDo;
                }
            }
            catch (Exception Ex)
            {
                responseHeader.statusCode = HttpStatusCode.NotModified;
                responseHeader.statusMessage = AppConstants.Error;
                string strErrorMsg = "Error Message: " + Ex.Message + "::: InnerException: " + Ex.InnerException +
                    "::: StackTrace:" + Ex.StackTrace + "::: Source: " + Ex.Source;
                errorlist = createErrorList(AppConstants.TODO_DELETE_ERROR, strErrorMsg);
                responseHeader.error = errorlist;
            }

            todoResponse.responseHeader = responseHeader;
            todoResponse.responseBody = responseBody;
            return todoResponse;
            //return InternalServerError(toDo);
        }
        */

        [ResponseType(typeof(ToDo))]
        public TodoResponse GetToDo(int id)
        {
            //TODO: AUTHENTICATION

            ToDo toDo = _repository.getTodoById(id);
            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();
            List<Error> errorlist;
            try
            {
                if (toDo == null)
                {
                    // Creating error response in response header
                    responseHeader.statusMessage = AppConstants.Error;
                    responseHeader.statusCode = HttpStatusCode.NotFound;
                    //move the error message to config or a common constants
                    // multiple values can go into the error message depending on different conditions
                    Error error = new Error(AppConstants.TODO_NOT_FOUND, AppConstants.TODO_NOT_FOUND_MSG);
                    errorlist = new List<Error>();
                    errorlist.Add(error);
                    // responseHeader.error = errorlist;

                    throw new TodoCustomException(string.Format("your search id is no available {0}", id));
                    // HttpResponseMessage str = Request.CreateErrorResponse(HttpStatusCode.NotFound, "");


                    // HttpResponseMessage str = Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
                }
                else
                {
                    responseHeader.statusMessage = AppConstants.Success;
                    responseHeader.statusCode = HttpStatusCode.OK;
                    List<ToDo> lstToDo = new List<ToDo>();
                    lstToDo.Add(toDo);
                    responseBody.todo = lstToDo;
                }
            }
            catch (Exception Ex)
            {
                if(Ex is TodoCustomException)
                {
                    throw Ex;
                }
                //unhandled exceptions
                responseHeader.statusCode = HttpStatusCode.NotModified;
                responseHeader.statusMessage = AppConstants.Error;
                string strErrorMsg = "Error Message: " + Ex.Message + "::: InnerException: " + Ex.InnerException +
                    "::: StackTrace:" + Ex.StackTrace + "::: Source: " + Ex.Source;
                errorlist = createErrorList(AppConstants.TODO_DELETE_ERROR, strErrorMsg);
                responseHeader.error = errorlist;
            }

            todoResponse.responseHeader = responseHeader;
            todoResponse.responseBody = responseBody;
            return todoResponse;
            //return InternalServerError(toDo);
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strErrorMsg"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private static List<Error> createErrorList(string strErrorMsg, string errorCode)
        {
            // TODO: mmove to utility method
            List<Error> errorlist;
            Error error = new Error(errorCode, strErrorMsg);
            errorlist = new List<Error>();
            errorlist.Add(error);
            return errorlist;
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
            try
            {
                if (!ModelState.IsValid)
                {
                   // responseHeader.statusCode = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    responseHeader.statusCode = HttpStatusCode.BadRequest;
                    // #TODO return BadRequest(ModelState);
                }

                else if (id != toDo.SlNo)
                {
                    responseHeader.statusCode = HttpStatusCode.BadRequest;
                    responseHeader.statusMessage = AppConstants.Error;
                    Error error = new Error(AppConstants.TODO_ID_MISMATCH, AppConstants.TODO_ID_MISMATCH_MSG);
                    List<Error> errorlist = new List<Error>();
                    errorlist.Add(error);
                    responseHeader.error = errorlist;
                    //return BadRequest();
                }

                else
                {
                    // return value and check the status and handle error
                    _repository.save(toDo);
                    // #TODO No content changed to ok responseHeader.statusCode = HttpStatusCode.NoContent;
                    responseHeader.statusCode = HttpStatusCode.OK;
                    responseHeader.statusMessage = AppConstants.Success;
                }
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                if (!ToDoExists(id))
                {
                    responseHeader.statusCode = HttpStatusCode.NotFound;
                    responseHeader.statusMessage = AppConstants.Error;

                    string strErrorMsg = "Error Message: " + dbEx.Message + "::: InnerException: " + dbEx.InnerException +
                        "::: StackTrace:" + dbEx.StackTrace + "::: Source: " + dbEx.Source;
                    Error error = new Error(AppConstants.TODO_NOT_FOUND, strErrorMsg);
                    List<Error> errorlist = new List<Error>();
                    errorlist.Add(error);
                    responseHeader.error = errorlist;

                    // return NotFound();
                }
                else
                {
                    throw;
                }
            }
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
        public TodoResponse PostToDo(ToDo toDo)
        {
            TodoResponse todoResponse = new TodoResponse();
            ResponseHeader responseHeader = new ResponseHeader();
            ResponseBody responseBody = new ResponseBody();

            try
            {
                if (!ModelState.IsValid)
                {
                    responseHeader.statusCode = HttpStatusCode.BadRequest;
                    // #TODO return BadRequest(ModelState);
                }
                else
                {
                    _repository.insert(toDo);
                    responseHeader.statusCode = HttpStatusCode.OK;
                    responseHeader.statusMessage = AppConstants.Success;
                }

            }
            catch (DbUpdateException dbEx)
            {
                //If TO Do already present with same id 
                if (ToDoExists(toDo.SlNo))
                {
                    responseHeader.statusCode = HttpStatusCode.Conflict;
                    responseHeader.statusMessage = AppConstants.Error;
                    string strErrorMsg = "Error Message: " + dbEx.Message + "::: InnerException: " + dbEx.InnerException +
                        "::: StackTrace:" + dbEx.StackTrace + "::: Source: " + dbEx.Source;
                    Error error = new Error(AppConstants.TODO_ALREADY_EXIST, strErrorMsg);
                    List<Error> errorlist = new List<Error>();
                    errorlist.Add(error);
                    responseHeader.error = errorlist;
                    //  return Conflict();
                }
                else
                {
                    responseHeader.statusCode = HttpStatusCode.UnsupportedMediaType;
                    responseHeader.statusMessage = AppConstants.Error;

                    string strErrorMsg = "Error Message: " + dbEx.Message + "::: InnerException: " + dbEx.InnerException +
                        "::: StackTrace:" + dbEx.StackTrace + "::: Source: " + dbEx.Source;
                    Error error = new Error(AppConstants.TODO_ALREADY_EXIST, strErrorMsg);
                    List<Error> errorlist = new List<Error>();
                    errorlist.Add(error);
                    responseHeader.error = errorlist;
                }
            }

            todoResponse.responseHeader = responseHeader;
            responseBody.todo = _repository.getAllTodo();
            todoResponse.responseBody = responseBody;
            return todoResponse;
            // IHttpActionResult iht = CreatedAtRoute("DefaultApi", new { id = toDo.SlNo }, toDo);
            //  #TODO return CreatedAtRoute("DefaultApi", new { id = toDo.SlNo }, toDo);
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

            try
            {
                ToDo toDo = _repository.getTodoById(id);
                if (null == toDo)
                {
                    responseHeader.statusCode = HttpStatusCode.NotFound;
                    responseHeader.statusMessage = AppConstants.Error;
                    Error error = new Error(AppConstants.TODO_NOT_FOUND, "To Do item not found");
                    List<Error> errorlist = new List<Error>();
                    errorlist.Add(error);
                    responseHeader.error = errorlist;
                    //return NotFound();
                }
                else
                {
                    _repository.remove(toDo);
                    responseHeader.statusCode = HttpStatusCode.OK;
                    responseHeader.statusMessage = AppConstants.Success;
                }

            }
            catch (Exception ex)
            {
                responseHeader.statusCode = HttpStatusCode.NotModified;
                responseHeader.statusMessage = AppConstants.Error;
                string strErrorMsg = "Error Message: " + ex.Message + "::: InnerException: " + ex.InnerException +
                    "::: StackTrace:" + ex.StackTrace + "::: Source: " + ex.Source;
                Error error = new Error(AppConstants.TODO_DELETE_ERROR, strErrorMsg);
                List<Error> errorlist = new List<Error>();
                errorlist.Add(error);
                responseHeader.error = errorlist;
            }

            todoResponse.responseHeader = responseHeader;
            responseBody.todo = _repository.getAllTodo();
            todoResponse.responseBody = responseBody;
            return todoResponse;
            //  return Ok(toDo);
        }

        /// <summary>
        /// Dispose method
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
        /// Method to check if and item already existin the To Do list for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if value exist, false if value doesnt exist</returns>
        public bool ToDoExists(int id)
        {
            return _repository.getAllTodo().Count(e => e.SlNo == id) > 0;
        }
    }
}