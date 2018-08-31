using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTest.ToDoApp.Mock;
using System.Linq;
using ToDoApp.Models;
using ToDoApp.Entities;
using ToDoApp.Controllers;
using UnitTest.ToDoApp.Constants;
using ToDoApp.Constants;
using System.Net;
using ToDoApp.Utility;
using System.Data.SqlClient;

namespace UnitTest.ToDoApp
{
    /// <summary>
    /// This test class is used to test the ToDoController of the ToDoApp
    /// </summary>
    [TestClass]
    public class ToDoControllerTest
    {
        /// <summary>
        /// Our Mock ToDo Repository for testing
        /// </summary>
        public readonly ITodoRepository MockRepository;

        /// <summary>
        /// Constructor method to Initialize the data and Mock the db functions: Testing Using moq
        /// </summary>
        public ToDoControllerTest()
        {            
            List<ToDo> testToDo = TodoTestData.GetMockAllToDo();

            Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
            //Mocking the Respone
            //Return all To Do items
            mockRepository.Setup(mr => mr.getAllTodo()).Returns(testToDo);
            
            //Return To Do item by Id
            mockRepository.Setup(mr => mr.getTodoById(It.IsAny<int>())).Returns((int i) => testToDo.Where(x => x.SlNo == i).Single());

            // Complete the setup and save our Mock To Do Repository
            this.MockRepository = mockRepository.Object;
        }


        /// <summary>
        /// Test method to test GetToDoes method for All data; Using Dependency injection
        /// </summary>
        [TestMethod]
        public void Test_GetToDoes_All()
        {
            MockTodoRepository mockToDoObj = new MockTodoRepository();
            ToDoController tc = new ToDoController(mockToDoObj); // injecting mock repo into controller
            TodoResponse apiToDoResponse = tc.GetToDoes();            
            List<ToDo> ltsToDo = mockToDoObj.getAllTodo().ToList();
            int iCountApi = apiToDoResponse.responseBody.todo.Count;
            int iCountMock = ltsToDo.Count;
            Assert.AreEqual(iCountApi, iCountMock);
            for (int i = 0; i < iCountApi; i++)
            {
                Assert.AreEqual(apiToDoResponse.responseBody.todo[i].Item, ltsToDo[i].Item);
                Assert.AreEqual(apiToDoResponse.responseBody.todo[i].Description, ltsToDo[i].Description);
                Assert.AreEqual(apiToDoResponse.responseBody.todo[i].SlNo, ltsToDo[i].SlNo);
            }            
        }



        /// <summary>
        /// Test method to test the GetToDo method for particular id; Using moq testing 
        /// </summary>
        [TestMethod]
        public void Test_GetToDo_By_Id()
        {
            ToDo mockToDo = this.MockRepository.getTodoById(2);
            ToDoController tc = new ToDoController(MockRepository);// injecting mock repository into conrtoller
            List<ToDo> apiToDo = tc.GetToDo(2).responseBody.todo;           
            Assert.AreEqual(mockToDo, apiToDo.FirstOrDefault());
        }

        /// <summary>
        /// Test method to check exception flow 
        /// </summary>
        [TestMethod]
        public void Test_GetToDo_By_Id_Throws_eception()
        {
            Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
            mockRepository.Setup(mr => mr.getTodoById(It.IsAny<int>())).Throws(new Exception());

            ToDoController tc = new ToDoController(mockRepository.Object);
            TodoResponse response = tc.GetToDo(2);
            Assert.AreEqual(response.responseHeader.error[0].errorCode, AppConstants.SYSTEM_EXEPTION);
            Assert.IsTrue(response.responseHeader.error[0].errorMessage.Contains("System.Exception"));
        }


        /// <summary>
        /// This method is used to test the PutToDo of the controller which is used to edit existing item
        /// </summary>
        [TestMethod]
        public void Test_PutToDo_Edit()
        {
            MockTodoRepository mockToDoObj = new MockTodoRepository();
            ToDoController tc = new ToDoController(mockToDoObj);
            ToDo toDoITem = new ToDo { SlNo = 0, Item = "Edit Test", Description ="Test the Put api"};
            TodoResponse apiToDoResponse = tc.PutToDo(0, toDoITem);    
            // If edit took place, header will have success message
            Assert.AreEqual(AppConstants.Success, apiToDoResponse.responseHeader.statusMessage);
            Assert.AreEqual(HttpStatusCode.OK, apiToDoResponse.responseHeader.statusCode);
        }

        ///// <summary>
        ///// This method is used to test the PutToDo of the controller which is used to edit existing item
        ///// </summary>
        //[TestMethod]
        //public void Test_PutToDo_DBException()
        //{
        //    Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
        //    mockRepository.Setup(mr => mr.save(It.IsAny<ToDo>())).Throws(new SystemException());

        //    ToDoController tc = new ToDoController(mockRepository.Object);

        //    ToDo toDoITem = new ToDo { SlNo = 0, Item = "Edit Test", Description = "Test the Put api" };
        //    TodoResponse apiToDoResponse = tc.PutToDo(0, toDoITem);
        //    // If edit took place, header will have success message
        //    Assert.AreEqual(AppConstants.Success, apiToDoResponse.responseHeader.statusMessage);
        //    Assert.AreEqual(HttpStatusCode.OK, apiToDoResponse.responseHeader.statusCode);
        //}

        /// <summary>
        /// This method is used to test the PutToDo of the controller for different id
        /// </summary>
        [TestMethod]
        public void Test_PutToDo_Id_Different()
        {
            Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
            mockRepository.Setup(mr => mr.save(It.IsAny<ToDo>())).Returns(1);

            ToDoController tc = new ToDoController(mockRepository.Object);

            ToDo toDoITem = new ToDo { SlNo = 2, Item = "Edit Test", Description = "Test the Put api" };
            TodoResponse apiToDoResponse = tc.PutToDo(0, toDoITem);
            // If edit took place, header will have success message
            
            Assert.AreEqual(apiToDoResponse.responseHeader.statusMessage, AppConstants.Error);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusCode, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// This method is used to test the PutToDo of the controller for empty input
        /// </summary>
        [TestMethod]
        public void Test_PutToDo_Id_Empty_Input()
        {
            Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
            mockRepository.Setup(mr => mr.save(It.IsAny<ToDo>())).Returns(1);
            ToDoController tc = new ToDoController(mockRepository.Object);

            ToDo toDoITem = new ToDo { SlNo = 2, Item = "", Description = "" };
            TodoResponse apiToDoResponse = tc.PutToDo(2, toDoITem);

            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorCode, AppConstants.ITEM_EMPTY);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorMessage, AppConstants.ITEM_EMPTY_MSG);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[1].errorCode, AppConstants.DESCRIPTION_EMPTY);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[1].errorMessage, AppConstants.DESCRIPTION_EMPTY_MSG);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusMessage, AppConstants.Error);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusCode, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// This method is used to test the PutToDo of the controller for internal server error exception handling
        /// </summary>
        [TestMethod]
        public void Test_PutToDo_Internal_Server_Error()
        {
            Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
            mockRepository.Setup(mr => mr.save(It.IsAny<ToDo>())).Returns(0);
            ToDoController tc = new ToDoController(mockRepository.Object);

            ToDo toDoITem = new ToDo { SlNo = 2, Item = "Item", Description = "Desc" };
            TodoResponse apiToDoResponse = tc.PutToDo(2, toDoITem);

            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorCode, AppConstants.SAVE_FAILED);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorMessage, AppConstants.SAVE_FAILED_MSG);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusMessage, AppConstants.Error);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusCode, HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// This method is used to test the PostToDo of the controller which is used to add new item
        /// </summary>
        [TestMethod]
        public void Test_PostToDo_Add()
        {
            MockTodoRepository mockToDoObj = new MockTodoRepository();
            ToDoController tc = new ToDoController(mockToDoObj);
            ToDo toDoITem = new ToDo { SlNo = 0, Item = "Insert Test", Description = "Test the Post api" };
            TodoResponse apiToDoResponse = tc.PostToDo(toDoITem);
            Assert.AreEqual(AppConstants.Success, apiToDoResponse.responseHeader.statusMessage);
            Assert.AreEqual(HttpStatusCode.OK, apiToDoResponse.responseHeader.statusCode);
        }

        /// <summary>
        /// This method is used to test the PostToDo of the controller for scenario where item already exist
        /// </summary>
        [TestMethod]
        public void Test_PostToDo_Item_Already_Exist()
        {
            Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
            mockRepository.Setup(mr => mr.save(It.IsAny<ToDo>())).Returns(1);

            MockTodoRepository mockObj = new MockTodoRepository();
            List<ToDo> getAllTodo = mockObj.getAllTodo();
            mockRepository.Setup(mr => mr.getAllTodo()).Returns(getAllTodo);

            ToDoController tc = new ToDoController(mockRepository.Object);
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "Item1", Description = "Desc Item1" };
            TodoResponse apiToDoResponse = tc.PostToDo(toDoITem);
            
            Assert.AreEqual(apiToDoResponse.responseHeader.statusMessage, AppConstants.Error);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusCode, HttpStatusCode.Conflict);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorCode, AppConstants.TODO_ALREADY_EXIST);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorMessage, AppConstants.TODO_ALREADY_EXIST_MSG);
        }

        /// <summary>
        /// This method is used to test the PostToDo of the controller for internal server error exception handling
        /// </summary>
        [TestMethod]
        public void Test_PostToDo_Internal_Server_Error()
        {
            Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
            mockRepository.Setup(mr => mr.insert(It.IsAny<ToDo>())).Returns(0);
            ToDoController tc = new ToDoController(mockRepository.Object);

            MockTodoRepository mockObj = new MockTodoRepository();
            List<ToDo> getAllTodo = mockObj.getAllTodo();
            mockRepository.Setup(mr => mr.getAllTodo()).Returns(getAllTodo);

            ToDo toDoITem = new ToDo { SlNo = 2, Item = "Item", Description = "Desc" };
            TodoResponse apiToDoResponse = tc.PostToDo(toDoITem);

            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorCode, AppConstants.INSERT_FAILED);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorMessage, AppConstants.INSERT_FAILED_MSG);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusMessage, AppConstants.Error);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusCode, HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// This method is used to test the PostToDo of the controller for empty input
        /// </summary>
        [TestMethod]
        public void Test_PostToDo_Id_Empty_Input()
        {
            Mock<ITodoRepository> mockRepository = new Mock<ITodoRepository>();
            ToDoController tc = new ToDoController(mockRepository.Object);

            ToDo toDoITem = new ToDo { SlNo = 2, Item = "", Description = "" };
            TodoResponse apiToDoResponse = tc.PostToDo(toDoITem);

            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorCode, AppConstants.ITEM_EMPTY);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[0].errorMessage, AppConstants.ITEM_EMPTY_MSG);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[1].errorCode, AppConstants.DESCRIPTION_EMPTY);
            Assert.AreEqual(apiToDoResponse.responseHeader.error[1].errorMessage, AppConstants.DESCRIPTION_EMPTY_MSG);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusMessage, AppConstants.Error);
            Assert.AreEqual(apiToDoResponse.responseHeader.statusCode, HttpStatusCode.BadRequest);
        }


        /// <summary>
        /// This method is used to test the DeleteToDo of the controller which is used to delete new item
        /// </summary>
        [TestMethod]
        public void Test_DeleteToDo()
        {
            MockTodoRepository mockToDoObj = new MockTodoRepository();
            ToDoController tc = new ToDoController(mockToDoObj);            
            TodoResponse apiToDoResponse = tc.DeleteToDo(2);
            Assert.AreEqual(AppConstants.Success, apiToDoResponse.responseHeader.statusMessage);
            Assert.AreEqual(HttpStatusCode.OK, apiToDoResponse.responseHeader.statusCode);
        }

        /// <summary>
        /// This method is used to test if an item exist in the Todo list; Test scenario-id exist
        /// </summary>
        [TestMethod]
        public void Test_ToDoExists_True()
        {

        
            bool apiResponse = ValidateUtility.ToDoIdExists(TodoTestData.GetMockAllToDo(),2);
            Assert.IsTrue(apiResponse);           
        }

        /// <summary>
        /// This method is used to test if an item exist in the Todo list; Test scenario-id dont exist
        /// </summary>
        [TestMethod]
        public void Test_ToDoExists_False()
        {
            MockTodoRepository mockToDoObj = new MockTodoRepository();
            ToDoController tc = new ToDoController(mockToDoObj);
            bool apiResponse = ValidateUtility.ToDoIdExists(TodoTestData.GetMockAllToDo(), 50);
            Assert.IsFalse(apiResponse);
        }

    }
}
