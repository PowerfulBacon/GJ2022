using System;

public static class Log
{

    private static LogType LogFlags = LogType.LOG_ALL;

    /// <summary>
    /// TODO
    /// </summary>
    public static void WriteLine(object message, LogType logType = LogType.MESSAGE)
    {
        //Ignore this log
        if ((logType & LogFlags) != logType)
            return;
        //Write it
        Console.WriteLine($"[{logType}][{DateTime.Now}] {message.ToString()}");
    }

}
