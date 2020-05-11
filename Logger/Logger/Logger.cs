using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    static public class Logger
    {
        static private readonly String DefaultFileName = String.Concat("trace_", DateTime.Now.ToString("dd-MM-yyyy"), ".log");
        private static readonly String DefaultFileDir = ".";
        static private String currLogFileName = GetDefaultLogFileName();
        static private String logFileName = GetDefaultLogFileName();
        static private String logFileDir = GetDefaultLogFileDir();
        static private String logFileNameErrors = GetDefaultLogFileNameErrors();
        static private String logFilePath;
        static private List<String> currentSessionActivities = new List<String>();
        static private Boolean logCurrentSessionActivities = false;
        private static readonly Int32 MaxCharsInActivity = 10_000;

        private static string CurrLogFileName
        {
            get => currLogFileName;
            set => currLogFileName = value;
        }
        public static bool LogCurrentSessionActivities
        {
            get => logCurrentSessionActivities;
            set => logCurrentSessionActivities = value;
        }
        public static string LogFileDir
        {
            get => logFileDir;
            set
            {
                CheckIfDirExists(value);
                logFileDir = value;
            }
        }
        public static string LogFileName
        {
            get => logFileName;
            set
            {
                CreateLogFileIfNotExists(value);
                logFileName = value;
            }
        }
        public static string LogFileNameErrors
        {
            get => logFileNameErrors;
            set
            {
                CreateLogFileIfNotExists(value);
                logFileNameErrors = value;
            }
        }
        public static string LogFilePath
        {
            get => logFilePath;
            private set { logFilePath = value; }
        }
        public static List<string> CurrentSessionActivities
        {
            get => currentSessionActivities;
        }
        static public String GetDefaultLogFileName()
        {
            String logFileName = String.IsNullOrEmpty(GetAppSetting("LogFileName"))
                ? DefaultFileName
                : GetAppSetting("LogFileName");

            return logFileName;
        }
        static public String GetDefaultLogFileNameErrors()
        {
            String logFileName = String.IsNullOrEmpty(GetAppSetting("LogFileNameErrors"))
                ? DefaultFileName
                : GetAppSetting("LogFileNameErrors");

            return logFileName;
        }
        static public String GetDefaultLogFileDir()
        {
            String logFileName = String.IsNullOrEmpty(GetAppSetting("LogFileDir"))
                ? DefaultFileDir
                : GetAppSetting("LogFileDir");

            return logFileName;
        }
        static public void ResetLogFilePaths()
        {
            LogFileDir = GetDefaultLogFileDir();
            LogFileName = GetDefaultLogFileName();
            LogFileNameErrors = GetDefaultLogFileNameErrors();
        }
        static public String GetCurrentSessionActivitiesAsString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (String activity in CurrentSessionActivities)
            {
                stringBuilder.Append(activity);
            }

            return stringBuilder.ToString();
        }
        static private void CreateLogFileIfNotExists(String fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException();
            }

            CheckIfDirExists(LogFileDir);

            String filePath = Path.Combine(LogFileDir, Path.GetFileName(fileName));
            LogFilePath = filePath;

            if (!File.Exists(filePath))
            {
                try
                {
                    File.Create(filePath).Close();
                }
                catch (Exception e)
                {
                    throw new LogFileCreateException(e.ToString());
                }
            }
        }
        static private void CheckIfDirExists(String dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException("The directory '" + dir + "' does not exist!");
            }
        }
        static public void LogActivity(String activity)
        {
            if (activity == null)
            {
                activity = "";
            }

            if (activity.Length > MaxCharsInActivity)
            {
                throw new ArgumentOutOfRangeException("The activity is too long!");
            }

            String activityLine = DateTime.Now.ToString() + ": " + activity + "\n";

            if (LogCurrentSessionActivities)
            {
                CurrentSessionActivities.Add(activityLine);
            }

            CurrLogFileName = logFileName;

            AppendToFile(activityLine);
        }
        static public void LogError(String errMsg)
        {
            if (errMsg == null)
            {
                errMsg = "";
            }

            String errLine = DateTime.Now.ToString() + ": Error: " + errMsg + "\n";

            if (errLine.Length > MaxCharsInActivity)
            {
                throw new ArgumentOutOfRangeException("The error is too long!");
            }

            CurrLogFileName = logFileNameErrors;

            AppendToFile(errLine);
        }
        static private void AppendToFile(String line)
        {
            CreateLogFileIfNotExists(CurrLogFileName);
            try
            {
                File.AppendAllText(LogFilePath, line);
            } catch (Exception e)
            {
                throw new LogFileWriteException("Failed to write to log file '" + LogFilePath + "': " + e.ToString());
            }
        }
        static public void PrintLogFileToConsole()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(LogFilePath);
                String line;

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    Console.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                throw new LogFileReadException("Failed to read from log file '" + LogFilePath + "': " + e.ToString());
            }
            finally
            {
                sr.Close();
            }
        }
        static private String GetAppSetting(String key)
        {
            String value = ConfigurationManager.AppSettings[key];

            if (!String.IsNullOrEmpty(value))
            {
                return value;
            }

            return String.Empty;
        }
    }
}
