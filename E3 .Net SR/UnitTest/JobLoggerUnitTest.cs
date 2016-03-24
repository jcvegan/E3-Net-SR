using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application;
using System.Configuration;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class JobLoggerUnitTest
    {
        [TestMethod]
        public void TextFileExistsAfterLog()
        {
            try
            {
                string filePath = ConfigurationManager.AppSettings["Log_FileDirectory"];
                string fecha = DateTime.Today.ToString("yyyyMMdd");
                string path = Path.Combine(filePath, string.Concat("LogFile", fecha, ".txt"));
                if (File.Exists(path))
                    File.Delete(path);
                JobLogger.LogMessage(LogType.Message, LogTarget.Console, "Test 1");
                if (!File.Exists(path))
                    Assert.Fail("No se ha creado el archivo de log");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            
        }
        [TestMethod]
        public void LogMessageIdAppendedInLog()
        {
            try
            {
                int totalLog = 0;
                string filePath = ConfigurationManager.AppSettings["Log_FileDirectory"];
                string fecha = DateTime.Today.ToString("yyyyMMdd");
                string path = Path.Combine(filePath, string.Concat("LogFile", fecha, ".txt"));
                if (File.Exists(path))
                {
                    string[] messages = File.ReadAllLines(path);
                    totalLog = messages.Length;
                }
                JobLogger.LogMessage(LogType.Message,LogTarget.File, "Inserting message log for testing");
                string[] messagesAfterLogging = File.ReadAllLines(path);
                if(totalLog +1  != messagesAfterLogging.Length)
                    Assert.Fail("Not inserting new log record");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }

        }
    }
}
