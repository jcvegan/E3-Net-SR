using System;
using System.Linq;
using System.Text;

namespace Application {
    public static class JobLogger {
        private const string CONST_LogFile_Prefix = "LogFile";
        /// <summary>
        /// Method for logging, calling this method will store the message on the three log target (File, Console and Database)
        /// </summary>
        /// <param name="logType">Specify the type of the message (Message, Warining or Error)</param>
        /// <param name="message">Specify the message of the log.</param>
        public static void LogMessage(LogType logType, string message) {
            LogMessage(logType, LogTarget.Console | LogTarget.Database | LogTarget.File, message);
        }
        /// <summary>
        /// Method for logging, calling this method will store the message on the specific target
        /// </summary>
        /// <param name="logType">Specify the type of the message (Message, Warining or Error)</param>
        /// <param name="target">Specify where the record will be stored</param>
        /// <param name="message">Specify the message of the log.</param>
        public static void LogMessage(LogType logType, LogTarget target, string message) {
            if (string.IsNullOrEmpty(message))
                return;
            message = message.Trim();
            if (target.HasFlag(LogTarget.Console))
            {
                switch (logType)
                {
                    case LogType.Message:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogType.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogType.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                Console.WriteLine(string.Format("{0} {1}", DateTime.Today.ToString("yyyyMMdd"), message));
            }
            if (target.HasFlag(LogTarget.File))
            {
                try
                {
                    string path = System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["Log_FileDirectory"], string.Concat(CONST_LogFile_Prefix, DateTime.Today.ToString("yyyyMMdd"), ".txt"));
                    string content = string.Empty;
                    if (System.IO.File.Exists(path))
                        content = System.IO.File.ReadAllText(path);
                    StringBuilder sBuilder = new StringBuilder(content);
                    sBuilder.AppendLine(string.Format("{0} {1}", DateTime.Today.ToString("yyyyMMdd"), message));
                    System.IO.File.WriteAllText(path, sBuilder.ToString());
                }
                catch (System.IO.IOException ioExc)
                {
                    throw ioExc;
                }
            }
            if (target.HasFlag(LogTarget.Database))
            {
                System.Data.SqlClient.SqlConnection connection = null;
                using (connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    try
                    {
                        System.Data.SqlClient.SqlCommand command = connection.CreateCommand();
                        command.CommandText = "exec InsertintoLogValues '" + message + "'," + Convert.ToString(logType) + "";
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException sqlExc)
                    {
                        throw sqlExc;
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }
                }
            }
        }
    }
}