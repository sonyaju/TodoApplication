using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ToDoApp.Constants;

namespace ToDoApp.Utility
{
    public static class Logger
    {
        public static void VerifyDir(string path)
        {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }          
        }

        public static void WriteLog(string strMessage)
        {
            try
            {
                string path = AppConstants.Log_Path;
                VerifyDir(path);
                string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + 
                                  DateTime.Now.Year.ToString() + "_Logs.txt";
                StreamWriter file = new StreamWriter(path + fileName, true);
                file.WriteLine(DateTime.Now.ToString() + "  :  " + strMessage);
                file.Close();
            }
            catch (Exception){}
        }

    }
}