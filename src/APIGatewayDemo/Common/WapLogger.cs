using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGatewayDemo.Common.Logging
{
    public class WapLogger : ILogger
    {
        private readonly ILogger _logger;
        public WapLogger(ILogger logger)
        {
            _logger = logger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string msg = $"{logLevel} :: {formatter(state, exception)} :: username :: {DateTime.Now}";

            _logger.LogDebug(msg);

            Console.WriteLine(msg + " - 123");

        }

        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message);

            Console.WriteLine(message + " - 123");
        }
    }
}
