using System;

namespace HTAlt
{
    /// <summary>
    /// HTAlt Static Logger System.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// An <see cref="Array"/> of <seealso cref="HTLog"/>s.
        /// <para></para>
        /// NOTE: This property requires <seealso cref="Init()"/> before any usage.
        /// </summary>
        public static HTLog[] Logs { get; internal set; }

        /// <summary>
        /// Location of the folder storing logs.
        /// </summary>
        public static string LogFolder { get; set; }

        /// <summary>
        /// The <see cref="System.Text.Encoding"/> method used to write text on logs.
        /// </summary>
        public static System.Text.Encoding Encoding { get; set; } = System.Text.Encoding.UTF8;

        /// <summary>
        /// The Date & time format used to determine the date and time of the logs.
        /// </summary>
        public static string DateTimeFormat { get; set; } = "dd-MM-yyyyy-HH-mm-ss-FFFFFF";

        /// <summary>
        /// Disables HTAlt information on startup.
        /// </summary>
        public static bool DisableHTAltInfo { get; set; } = false;

        /// <summary>
        /// Determines if the initialization is done or not.
        /// </summary>
        public static bool isInitDone { get; internal set; } = false;

        /// <summary>
        /// Append a log to the current log.
        /// <para></para>
        /// NOTE: This property requires <seealso cref="Init()"/> before any usage.
        /// </summary>
        /// <param name="info">Message to write on.</param>
        /// <param name="level">Level of the log.</param>
        public static void Append(string info, LogLevel level = LogLevel.None)
        {
            if (isInitDone)
            {
                CurrentSession.Append(info, level);
            }
            else
            {
                throw new Exception("Logger is not initialized. Start Initialization before using this method.");
            }
        }

        /// <summary>
        /// Gets the current session.
        /// <para></para>
        /// NOTE: This property requires <seealso cref="Init()"/> before any usage.
        /// </summary>
        public static HTLog CurrentSession { get; set; }

        /// <summary>
        /// Initializes the entire system.
        /// </summary>
        public static void Init()
        {
            CurrentSession = new HTLog() { Location = System.IO.Path.Combine(LogFolder, DateTime.Now.ToString(DateTimeFormat) + ".log") };
            Logs = new HTLog[] { CurrentSession };
            isInitDone = true;
            CurrentSession.Append("[HTAlt] Logger Init successful.", LogLevel.Info);
            if (!DisableHTAltInfo)
            {
                CurrentSession.Append("[HTAlt] HTAlt Standart Library ver. " + HTInfo.ProjectVersion + " [" + HTInfo.ProjectCodeName + "]", LogLevel.Info);
                CurrentSession.Append("[HTAlt] HTAlt Current Log System: Static (HTAlt.Standart/Logger.cs)", LogLevel.Info);
            }
        }

        /// <summary>
        /// Loads the old logs
        /// <para></para>
        /// NOTE: This property requires <seealso cref="Init()"/> before any usage.
        /// </summary>
        public static void LoadOldLogs()
        {
            if (isInitDone)
            {
                var files = System.IO.Directory.GetFiles(LogFolder, "*.log", System.IO.SearchOption.TopDirectoryOnly);
                var _l = Logs;
                for (int i = 0; i < files.Length; i++)
                {
                    HTLog log = new HTLog() { Location = files[i] };
                    Array.Resize(ref _l, _l.Length + 1);
                    _l[_l.Length + 1] = log;
                }
                Logs = _l;
            }
            else
            {
                throw new Exception("Logger is not initialized. Start Initialization before using this method.");
            }
        }

        /// <summary>
        /// Unloads the old logs.
        /// </summary>
        /// z<param name="doGC">Collects garbage afterwards.</param>
        public static void UnloadOldLogs(bool doGC = false)
        {
            if (isInitDone)
            {
                Logs = new HTLog[] { CurrentSession };
                if (doGC) { GC.Collect(); }
            }
            else
            {
                throw new Exception("Logger is not initialized. Start Initialization before using this method.");
            }
        }
    }

    /// <summary>
    /// Static Log System Item.
    /// </summary>
    public class HTLog
    {
        /// <summary>
        /// Location of the this log file.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The entire content of the log file.
        /// </summary>
        public string Log { get => Tools.ReadFile(Location, Logger.Encoding); set => Tools.WriteFile(Location, value, Logger.Encoding); }

        /// <summary>
        /// Appneds a log into this file.
        /// </summary>
        /// <param name="log">Log to append to.</param>
        /// <param name="level">Level of the log.</param>
        public void Append(string log, LogLevel level = LogLevel.None)
        {
            switch (level)
            {
                case LogLevel.Hidden:
                    break;

                default:
                case LogLevel.None:
                    log = "[" + DateTime.Now.ToString(Logger.DateTimeFormat) + "] " + log;
                    break;

                case LogLevel.Info:
                    log = "[I] [" + DateTime.Now.ToString(Logger.DateTimeFormat) + "] " + log;
                    break;

                case LogLevel.Warning:
                    log = "[W] [" + DateTime.Now.ToString(Logger.DateTimeFormat) + "] " + log;
                    break;

                case LogLevel.Error:
                    log = "[E] [" + DateTime.Now.ToString(Logger.DateTimeFormat) + "] " + log;
                    break;

                case LogLevel.Critical:
                    log = "[C] [" + DateTime.Now.ToString(Logger.DateTimeFormat) + "] " + log;
                    break;
            }
            Console.WriteLine(log);
            Log += log + Environment.NewLine;
        }

        /// <summary>
        /// Gets all the logs in a level.
        /// </summary>
        /// <param name="level">Level of the logs.</param>
        /// <returns>An <see cref="Array"/> of logs in that level.</returns>
        public string[] GetLogsByLevel(LogLevel level)
        {
            string[] found = new string[] { };
            using (System.IO.StringReader reader = new System.IO.StringReader(Log))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    switch (level)
                    {
                        case LogLevel.Hidden:
                            if (!line.ToLowerEnglish().StartsWith("["))
                            {
                                Array.Resize(ref found, found.Length + 1);
                                found[found.Length - 1] = line;
                            }
                            break;

                        case LogLevel.None:
                            break;

                        default:
                            char type = ' ';
                            switch (level)
                            {
                                case LogLevel.Info:
                                    type = 'i';
                                    break;

                                case LogLevel.Warning:
                                    type = 'w';
                                    break;

                                case LogLevel.Error:
                                    type = 'w';
                                    break;

                                case LogLevel.Critical:
                                    type = 'c';
                                    break;
                            }
                            if (line.ToLowerEnglish().StartsWith("[" + type + "]"))
                            {
                                Array.Resize(ref found, found.Length + 1);
                                found[found.Length - 1] = line;
                            }
                            break;
                    }
                }
            }
            return found;
        }

        /// <summary>
        /// Gets all log levels in this order:
        /// <para>Hidden</para>
        /// <para>None</para>
        /// <para>Information</para>
        /// <para>Warning</para>
        /// <para>Error</para>
        /// <para>Critical</para>
        /// </summary>
        public string[][] AllLogLevels => new string[][] { GetLogsByLevel(LogLevel.Hidden), GetLogsByLevel(LogLevel.None), GetLogsByLevel(LogLevel.Info), GetLogsByLevel(LogLevel.Warning), GetLogsByLevel(LogLevel.Error), GetLogsByLevel(LogLevel.Critical) };
    }
}