using System;
namespace Application {
    /// <summary>
    /// Enumeration for set the type of log
    /// Message is for information messages,
    /// Error is for storing exception message or fails,
    /// Warning is for storing caution messages.
    /// </summary>
    public enum LogType : short {
        Message = 1,
        Error = 2,
        Warning = 3
    }
}
