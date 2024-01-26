using NLog;
using NLog.Config;
using NLog.Targets;
using RevitConduitTable.Constants;
using System.IO;
using System.Reflection;

/// <summary>
/// Initialize Nlog scheme which place .log file near dll executing assembly
/// </summary>
internal static class NlogSchemaInitialize
{
    public static void InitializeLogger()
    {
        string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

        string logDirectory = Path.Combine(Path.GetDirectoryName(thisAssemblyPath), FileConstants.LoggerFolder);

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        var config = new LoggingConfiguration();

        var logfile = new FileTarget("logfile")
        {
            FileName = Path.Combine(logDirectory, "${shortdate}.log"),
            Layout = "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=toString,StackTrace}"
        };

        config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);

        LogManager.Configuration = config;
        LogManager.ReconfigExistingLoggers();
    }
}