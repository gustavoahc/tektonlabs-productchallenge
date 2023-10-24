using NLog;

namespace TektonLabs.Presentation.Api.Helpers.Logging
{
    public class Logging : ILogging
    {
        private static NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

        public void LogMessage(string message) => Logger.Info(message);
    }
}
