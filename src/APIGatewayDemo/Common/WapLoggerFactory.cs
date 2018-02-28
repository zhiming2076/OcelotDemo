using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGatewayDemo.Common.Logging
{
    public interface IWapLoggerFactory
    {
        ILogger CreateLogger<T>();
    }

    public class WapLoggerFactory : IWapLoggerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public WapLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public ILogger CreateLogger<T>()
        {
            var logger = _loggerFactory.CreateLogger<T>();
            return new WapLogger(logger);
        }
    }
}
