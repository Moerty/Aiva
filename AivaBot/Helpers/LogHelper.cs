using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AivaBot {
    class LogHelper {
        public static LogHelper Logger { get; set; } = new LogHelper("log.txt", true, (LogLevel)Convert.ToInt16(Config.General.Config["General"][nameof(LogLevel)]));

        public delegate void LogEventHandler(LogEntry entry);

        /// <summary>
        /// This event is called, when a log entry was created, regardless of the LogFilter level.
        /// </summary>
        public event LogEventHandler OnLog;
        /// <summary>
        /// This event is called, when a log entry was created whose logLevel is higher than the LogFilter level.
        /// </summary>
        public event LogEventHandler OnLogFiltered;

        /// <summary>
        /// the current log level.
        /// If a log-entries logLevel is lower than the logFilter-level, which was set in the constructor,
        /// OnLog will be called but not OnLogFiltered.
        /// This also means that the entry wont be shown in the console, but will be logged to the file. (If file-logging is enabled)
        /// </summary>
        public LogLevel LogFilter;

        private readonly string filePath;
        private readonly bool logToConsole;
        private readonly bool showExecutionPoint;
        private readonly int writeToFileInterval;

        private readonly Thread fileThread;
        private readonly ConcurrentQueue<LogEntry> fileQueue = new ConcurrentQueue<LogEntry>();

        /// <summary>
        /// Creates a new Logger.
        /// The main class of the logging system.
        /// </summary>
        /// <param name="filePath">
        /// The path of the log file. To disable logging to a file, set it to null.
        /// </param>
        /// <param name="logToConsole">
        /// Set it to false, to disable logging with the Console class.
        /// </param>
        /// <param name="logFilter">
        /// All log entries with a log level whose are less than logFilter, wont be shown in the console.
        /// </param>
        /// <param name="showExecutionPoint">
        /// If the class, method and line of execution should be shown in the console.
        /// </param>
        /// <param name="writeToFileInterval">
        /// The write interval in milliseconds.
        /// </param>
        public LogHelper(string filePath, bool logToConsole = true, LogLevel logFilter = LogLevel.Info, bool showExecutionPoint = true, int writeToFileInterval = 200) {
            this.filePath = filePath;
            this.logToConsole = logToConsole;
            this.LogFilter = logFilter;
            this.showExecutionPoint = showExecutionPoint;
            this.writeToFileInterval = writeToFileInterval;

            if (filePath != null) {
                var info = new FileInfo(filePath).Directory;
                info?.Create();
                OnLog += OnLogToFileAsync;
                fileThread = new Thread(StartFileThread) {
                    IsBackground = true
                };
                fileThread.Start();
            }

            if (logToConsole)
                OnLogFiltered += OnLogToConsole;
        }
        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="message">
        /// The message of this log entry.
        /// </param>
        /// <param name="level">
        /// The log level describes the importance of an log entry.
        /// If its lower than the logFilter-level, which was set in the constructor, the log entry wont be shown in the console.
        /// </param>
        /// <param name="memberName">
        /// Filled by the compiler. Ignore this field.
        /// </param>
        /// <param name="sourceFilePath">
        /// Filled by the compiler. Ignore this field.
        /// </param>
        /// <param name="sourceLineNumber">
        /// Filled by the compiler. Ignore this field.
        /// </param>
        public void Log(string message, LogLevel level = LogLevel.Info,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0) {

            var clazz = Path.GetFileNameWithoutExtension(sourceFilePath);

            var entry = new LogEntry(message, level, clazz, memberName, sourceLineNumber);

            OnLog?.Invoke(entry);
            if (level >= LogFilter)
                OnLogFiltered?.Invoke(entry);
        }

        // Log To File isn't filtered, and the detailed string is used
#pragma warning disable CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
        private async void OnLogToFileAsync(LogEntry entry)
#pragma warning restore CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
        {
            fileQueue.Enqueue(entry);
        }

        // Log To Console is filtered & colored, and toConsoleString is used (but you can implement your own)
        private void OnLogToConsole(LogEntry entry) {
            var color = Console.ForegroundColor;
            switch (entry.Level) {
                case LogLevel.Ann:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.WriteLine(entry.ToConsoleString(showExecutionPoint));

            Console.ForegroundColor = color;
        }

        private void StartFileThread() {
            while (true) {
                if (fileQueue.Count > 0) {
                    var sb = new StringBuilder(fileQueue.Count);
                    while (fileQueue.Count > 0) {
                        LogEntry log;
                        var success = fileQueue.TryDequeue(out log);
                        if (success) {
                            sb.AppendLine(log.ToString());
                        }
                        else {
                            Log("Couldn't dequeue log entry", LogLevel.Error);
                        }
                    }
                    using (var writer = File.AppendText(filePath)) {
                        writer.Write(sb.ToString());
                    }
                }
                Thread.Sleep(writeToFileInterval);
            }
        }

        /* TODO use this
        private static void WriteToFile(string value, string path)
        {
            using (var writer = new StreamWriter(path, true))
            using (var syncWriter = TextWriter.Synchronized(writer))
                syncWriter.WriteLine(value);
        }
        */


        public enum LogLevel {
            Dev,    // Development
            Debug,
            Info,
            Ann,    // Announcement
            Warn,   // Warning
            Error
        }

        public struct LogEntry {

            /// <summary>
            /// The content of this log entry.
            /// In most cases this is the message of this log entry.
            /// </summary>
            public string Content { get; private set; }
            /// <summary>
            /// The log level of this log entry.
            /// </summary>
            public LogLevel Level { get; private set; }

            /// <summary>
            /// The class which created the log entry.
            /// </summary>
            public string Class { get; private set; }
            /// <summary>
            /// The method which created the log entry.
            /// </summary>
            public string Method { get; private set; }
            /// <summary>
            /// The line in which the log entry was created.
            /// </summary>
            public int LineNumber { get; private set; }

            /// <summary>
            /// The time of creation of this log entry.
            /// </summary>
            public DateTime Timestamp { get; private set; }

            /// <summary>
            /// Creates a new log entry.
            /// </summary>
            /// <param name="message">
            /// The log message of this log entry.
            /// </param>
            /// <param name="level">
            /// The log level of this log entry.
            /// </param>
            /// <param name="clazz">
            /// Filled by the compiler. Ignore this field.
            /// </param>
            /// <param name="method">
            /// Filled by the compiler. Ignore this field.
            /// </param>
            /// <param name="lineNumber">
            /// Filled by the compiler. Ignore this field.
            /// </param>
            public LogEntry(string message, LogLevel level = LogLevel.Info, string clazz = "", string method = "", int lineNumber = 0) {
                Content = message;
                Level = level;

                Class = clazz;
                Method = method;
                LineNumber = lineNumber;

                Timestamp = DateTime.Now;
            }

            /// <summary>
            /// Returns the string which is later logged to the console.
            /// This string doesnt't contain as much information as the string which is logged to the log-file.
            /// </summary>
            /// <param name="showExecutionPoint">
            /// Wether the class, the method and the line of execution should be included in this string or not.
            /// </param>
            /// <returns>
            /// A string which contains the most important informations about this log entry.
            /// </returns>
            public string ToConsoleString(bool showExecutionPoint = true) {
                var execPoint = $"{Class}.{Method}:{LineNumber} ";
                return $"{Timestamp.ToShortTimeString()} ({Level}) {(showExecutionPoint ? execPoint : "")}{Content}";
            }

            /// <summary>
            /// Returns the string which is later logged to the log file.
            /// This string contains detailed informations about the log entry.
            /// </summary>
            /// <returns>
            /// A string which contains detailed informations about this log entry.
            /// </returns>
            public override string ToString() {
                return $"{Timestamp.ToLongTimeString()} {Timestamp.ToShortDateString()} ({Level}): {Class}.{Method}:{LineNumber} {Content}";
            }
        }
    }
}
