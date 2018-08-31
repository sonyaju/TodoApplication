using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Constants;
using ToDoApp.Models;
using ToDoApp.Utility;
using UnitTest.ToDoApp.Mock;

namespace UnitTest.ToDoApp.ValidateUtilityTest
{
    /// <summary>
    /// This test class is used to test the methods in the Validate Utility of ToDoApp
    /// </summary>
    [TestClass]
    public class ValidateUtilityTest
    {
        /// <summary>
        /// This method is used to test the ValidateInput method of ValidateUtility for not empty inputs 
        /// </summary>
        [TestMethod]
        public void ValidateInput_Not_Empty()
        {
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "Item1", Description = "Desc Item1" };
            List<Error> lstValidateInput = ValidateUtility.ValidateInput(toDoITem);
            Assert.IsTrue(lstValidateInput.Count == 0);

        }

        /// <summary>
        /// This method is used to test the ValidateInput method of ValidateUtility for empty Item
        /// </summary>
        [TestMethod]
        public void ValidateInput_Item_Empty()
        {
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "", Description = "Desc Item1" };
            List<Error> lstValidateInput = ValidateUtility.ValidateInput(toDoITem);
            Assert.IsTrue(lstValidateInput.Count == 1);
            Assert.AreEqual(AppConstants.ITEM_EMPTY, lstValidateInput[0].errorCode);
            Assert.AreEqual(AppConstants.ITEM_EMPTY_MSG, lstValidateInput[0].errorMessage);            
        }

        /// <summary>
        /// This method is used to test the ValidateInput method of ValidateUtility for empty Description 
        /// </summary>
        [TestMethod]
        public void ValidateInput_Description_Empty()
        {
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "Item1", Description = "" };
            List<Error> lstValidateInput = ValidateUtility.ValidateInput(toDoITem);
            Assert.IsTrue(lstValidateInput.Count == 1);
            Assert.AreEqual(AppConstants.DESCRIPTION_EMPTY, lstValidateInput[0].errorCode);
            Assert.AreEqual(AppConstants.DESCRIPTION_EMPTY_MSG, lstValidateInput[0].errorMessage);
        }

        /// <summary>
        /// This method is used to test the ValidateInput method of ValidateUtility for empty Item and Description 
        /// </summary>
        [TestMethod]
        public void ValidateInput_Item_Desc_Empty()
        {
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "", Description = "" };
            List<Error> lstValidateInput = ValidateUtility.ValidateInput(toDoITem);
            Assert.IsTrue(lstValidateInput.Count == 2);
            Assert.AreEqual(AppConstants.ITEM_EMPTY, lstValidateInput[0].errorCode);
            Assert.AreEqual(AppConstants.ITEM_EMPTY_MSG, lstValidateInput[0].errorMessage);
            Assert.AreEqual(AppConstants.DESCRIPTION_EMPTY, lstValidateInput[1].errorCode);
            Assert.AreEqual(AppConstants.DESCRIPTION_EMPTY_MSG, lstValidateInput[1].errorMessage);
        }

        /// <summary>
        /// This method is used to test the ToDoExists method of ValidateUtility for existing values
        /// </summary>
        [TestMethod]
        public void ToDoExists_True()
        {
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "Item1", Description = "Desc Item1" };
            MockTodoRepository mockObj = new MockTodoRepository();
            List<ToDo> getAllTodo = mockObj.getAllTodo();

            bool isExist = ValidateUtility.ToDoExists(getAllTodo,toDoITem);
            Assert.AreEqual(true,isExist);

        }

        /// <summary>
        /// This method is used to test the ToDoExists method of ValidateUtility 
        /// for non existing values: Item Different
        /// </summary>
        [TestMethod]
        public void ToDoExists_False_1()
        {
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "Item2", Description = "Desc Item1" };
            MockTodoRepository mockObj = new MockTodoRepository();
            List<ToDo> getAllTodo = mockObj.getAllTodo();

            bool isExist = ValidateUtility.ToDoExists(getAllTodo, toDoITem);
            Assert.AreEqual(false, isExist);

        }

        /// <summary>
        /// This method is used to test the ToDoExists method of ValidateUtility 
        /// for non existing values: Item Different
        /// </summary>
        [TestMethod]
        public void ToDoExists_False_2()
        {
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "Item1", Description = "Desc Item3" };
            MockTodoRepository mockObj = new MockTodoRepository();
            List<ToDo> getAllTodo = mockObj.getAllTodo();

            bool isExist = ValidateUtility.ToDoExists(getAllTodo, toDoITem);
            Assert.AreEqual(false, isExist);

        }

        /// <summary>
        /// This method is used to test the ToDoExists method of ValidateUtility for 
        /// non existing values: Item and Description Different
        /// </summary>
        [TestMethod]
        public void ToDoExists_False_3()
        {
            ToDo toDoITem = new ToDo { SlNo = 1, Item = "Item8", Description = "Desc Item8" };
            MockTodoRepository mockObj = new MockTodoRepository();
            List<ToDo> getAllTodo = mockObj.getAllTodo();

            bool isExist = ValidateUtility.ToDoExists(getAllTodo, toDoITem);
            Assert.AreEqual(false, isExist);

        }

        /// <summary>
        /// This method is used to test the ToIdExists method of ValidateUtility for existing id
        /// </summary>
        [TestMethod]
        public void ToIdExists_True()
        {
            MockTodoRepository mockObj = new MockTodoRepository();
            List<ToDo> getAllTodo = mockObj.getAllTodo();
            bool isExist = ValidateUtility.ToDoIdExists(getAllTodo, 1);
            Assert.AreEqual(true, isExist);
        }

        /// <summary>
        /// This method is used to test the ToIdExists method of ValidateUtility for non existing id
        /// </summary>
        [TestMethod]
        public void ToIdExists_False()
        {
            MockTodoRepository mockObj = new MockTodoRepository();
            List<ToDo> getAllTodo = mockObj.getAllTodo();
            bool isExist = ValidateUtility.ToDoIdExists(getAllTodo,12);
            Assert.AreEqual(false, isExist);
        }
    }
    
}
