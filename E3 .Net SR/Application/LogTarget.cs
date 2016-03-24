
namespace Application {
    /// <summary>
    /// This enumeration especify where the log will be generated.
    /// File is for creating or updating a plain text, 1 line per message
    /// Console is for displaying on the console windows
    /// Database is for storing the log message in Sql Server
    /// </summary>
    public enum LogTarget {
        File,
        Console,
        Database
    }
}
