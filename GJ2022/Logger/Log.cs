﻿using System;

public static class Log
{

    private static object consoleLock = new object();

    private static LogType LogFlags = LogType.LOG_ALL;

    /// <summary>
    /// TODO
    /// </summary>
    public static void WriteLine(object message, LogType logType = LogType.MESSAGE)
    {
        //Ignore this log
        if ((logType & LogFlags) != logType)
            return;
        lock (consoleLock)
        {
            //Write it
            SetConsoleColor(logType);
            Console.Write($"[{logType}][{DateTime.Now}]");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($" {message ?? "null"}");
        }
    }

    private static void SetConsoleColor(LogType logType)
    {
        switch (logType)
        {
            case LogType.DEBUG:
                Console.ForegroundColor = ConsoleColor.Magenta;
                return;
            case LogType.ERROR:
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                return;
            case LogType.WARNING:
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                return;
            case LogType.MESSAGE:
                Console.ForegroundColor = ConsoleColor.Green;
                return;
            case LogType.LOG:
                Console.ForegroundColor = ConsoleColor.Cyan;
                return;
            case LogType.TEMP:
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                return;
        }
    }

}
