using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Application {
    public static class JobLogger {
        private const string CONST_LogFile_Prefix = "LogFile";
        public static void LogMessage(LogType logType, string message) {
            LogMessage(logType, LogTarget.Console | LogTarget.Database | LogTarget.File, message);
        }
        
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
                Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), message));
            }
            if (target.HasFlag(LogTarget.File))
            {
                try
                {
                    string path = System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["Log_FileDirectory"], string.Concat(CONST_LogFile_Prefix, DateTime.Now.ToShortDateString(), ".txt"));
                    string content = string.Empty;
                    if (System.IO.File.Exists(path))
                        content = System.IO.File.ReadAllText(path);
                    StringBuilder sBuilder = new StringBuilder(content);
                    sBuilder.AppendLine(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), message));
                    System.IO.File.WriteAllText(path, sBuilder.ToString());
                }
                catch (System.IO.IOException ioExc)
                {
                    throw ioExc;
                }
            }
            if (target.HasFlag(LogTarget.Database))
            {
                SqlConnection connection = null;
                using (connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    try
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "InsertintoLogValues('" + message + "'," + Convert.ToString(logType) + ")";
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException sqlExc)
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