

namespace ToDoApp.Constants
{
    /// <summary>
    /// This is the File to save all the constants
    /// </summary>
    public static class AppConstants
    {
        //Messages
        public static string Error = "Error";
        public static string Success = "Success";
        public static string Error_Message = "Error Message :: ";
        public static string Stack_Trace = "Stack trace ::";
        public static string Error_Code = ": Error Code : ";
        public static string Inner_Exception = "::: Inner Exception : ";
        public static string Soure = "::: Source: ";
        public static string Log_Path = "C:/ToDoApp/Logs/";

        //Error Codes
        public static string TODO_NOT_FOUND = "TODO_NOT_FOUND";
        public static string TODO_ID_MISMATCH = "TODO_ID_MISMATCH";
        public static string TODO_ALREADY_EXIST = "TODO_ALREADY_EXIST";
        public static string TODO_DELETE_ERROR = "TODO_DELETE_ERROR";
        public static string BAD_REQUEST = "BAD_REQUEST";
        public static string SAVE_FAILED = "SAVE_FAILED";
        public static string MODAL_STATE_INVALID = "Modal State Invalid";
        public static string INTERNAL_SERVER_ERROR = "INTERNAL_SERVER_ERROR";
        public static string INSERT_FAILED = "ADD_FAILED";
        public static string DELETE_FAILED = "DELETE_FAILED";
        public static string INPUT_EMPTY = "INPUT_INVALID";
        public static string ITEM_EMPTY = "ITEM_EMPTY";
        public static string DESCRIPTION_EMPTY = "DESCRIPTION_EMPTY";
        public static string SYSTEM_EXEPTION = "SYSTEM_EXEPTION";

        //Error Messages
        public static string TODO_NOT_FOUND_MSG = "No item found for the todo id: {0}";
        public static string TODO_ID_MISMATCH_MSG = "Mismatch between To Do list Id and Passed Id";
        public static string SAVE_FAILED_MSG = "Save failed, Please retry after sometime";
        public static string INTERNAL_SERVER_ERROR_MSG = "Internal Server Error";
        public static string TODO_ALREADY_EXIST_MSG = "Item already exist";
        public static string INSERT_FAILED_MSG = "Adding new item failed, Please retry after sometime";
        public static string DELETE_FAILED_MSG = "Delete failed, Please retry after sometime";
        public static string INPUT_EMPTY_MSG = "Please enter value for ";
        public static string ITEM_EMPTY_MSG = "Item cannot be empty";
        public static string DESCRIPTION_EMPTY_MSG = "Description cannot be empty";
    }
}