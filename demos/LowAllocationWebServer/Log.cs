using System.IO;

namespace System.Diagnostics
{
    public abstract class Log
    {
        public Level CurrentLevel;
        public bool IsVerbose { get { return CurrentLevel >= Level.Verbose; } }

        public abstract void LogMessage(Level level, string message);
        public virtual void LogMessage(Level level, string format, params object[] args)
        {
            LogMessage(level, String.Format(format, args));
        }

        public void LogVerbose(string message)
        {
            if (CurrentLevel >= Level.Verbose)
            {
                LogMessage(Level.Verbose, message);
            }
        }
        public void LogWarning(string message)
        {
            if (CurrentLevel >= Level.Warning)
            {
                LogMessage(Level.Warning, message);
            }
        }
        public void LogError(string message)
        {
            if (CurrentLevel >= Level.Error)
            {
                LogMessage(Level.Error, message);
            }
        }

        public enum Level
        {
            Off = -1,
            Error = 0,
            Warning = 10,
            Verbose = 20, 
        }
    }

    public class ConsoleLog : Log
    {
        object s_lock = new object();

        public ConsoleLog(Log.Level level)
        {
            CurrentLevel = level;
        }
        public override void LogMessage(Level level, string message)
        {
            lock(s_lock)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                switch (level)
                {
                    case Level.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("ERROR: ");
                        break;
                    case Level.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("WARNING: ");
                        break;
                }

                Console.WriteLine(message);
                Console.ForegroundColor = oldColor;
            }
        }
    }
}
