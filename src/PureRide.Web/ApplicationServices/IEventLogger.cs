using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace PureRide.Web.ApplicationServices
{
    public class Log4NetEventLogger : IEventLogger
    {
        private readonly ILog _logger;

        public Log4NetEventLogger()
        {
            _logger = LogManager.GetLogger("PureRide");
        }

        public void LogException(Exception ex)
        {
            _logger.Error(ex.Message,ex);
        }
    }

    public interface IEventLogger
    {
        void LogException(Exception ex);
    }
}
